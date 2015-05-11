using AutoMapper;
using fortune_api.App_Start;
using fortune_api.Dtos;
using fortune_api.Exceptions;
using fortune_api.Models;
using fortune_api.Persistence;
using fortune_api.Services;
using fortune_api.Tests.Test_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace fortune_api.Tests.Services
{
    /// <summary>
    /// Summary description for TrailerServiceTest
    /// </summary>
    [TestClass]
    public class TrailerServiceTest
    {
        #region Get Trailer

        [TestMethod]
        public void GetTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                Deleted = false,
                LocationId = testLocation.Id
            };
            TrailerDto testDto = Mapper.Map<TrailerDto>(testTrailer);
            testDto.Location = Mapper.Map<LocationDto>(testLocation);

            //Mock call
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns(testTrailer);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto dto = trailerService.Get(testTrailer.Id);
            TestUtil.Compare(testDto, dto);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void GetNonexistentTrailer()
        {
            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test id
            int testId = 111111;

            //Mock call
            mockTrailerRepo.Setup(x => x.Get(testId)).Returns<Trailer>(null);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto dto = trailerService.Get(testId);
        }

        #endregion

        #region Get Trailers

        [TestMethod]
        public void GetTrailers()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                Name = "TEST"
            };

            //Test trailers
            IEnumerable<Trailer> testTrailers = new List<Trailer>
            {
                new Trailer {
                    Id = 111111,
                    Deleted = false,
                    LocationId = testLocation.Id
                },
                new Trailer {
                    Id = 222222,
                    Deleted = false,
                    LocationId = testLocation.Id
                }
            };
            TrailerDto[] testDtos = Mapper.Map<TrailerDto[]>(testTrailers);
            foreach (TrailerDto testDto in testDtos)
            {
                testDto.Location = Mapper.Map<LocationDto>(testLocation);
            }

            //Mock call
            mockTrailerRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<Trailer, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<Trailer>, IOrderedQueryable<Trailer>>>(),
                ""
            )).Returns(testTrailers);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto[] dtos = trailerService.Get(false);
            TestUtil.Compare(testDtos, dtos);
        }

        [TestMethod]
        public void GetTrailersWithDeleted()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                Name = "TEST"
            };

            //Test trailers
            IEnumerable<Trailer> testTrailers = new List<Trailer>
            {
                new Trailer {
                    Id = 111111,
                    Deleted = false,
                    LocationId = testLocation.Id
                },
                new Trailer {
                    Id = 222222,
                    Deleted = true,
                    LocationId = testLocation.Id
                }
            };
            TrailerDto[] testDtos = Mapper.Map<TrailerDto[]>(testTrailers);
            foreach (TrailerDto testDto in testDtos)
            {
                testDto.Location = Mapper.Map<LocationDto>(testLocation);
            }

            //Mock call
            mockTrailerRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<Trailer, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<Trailer>, IOrderedQueryable<Trailer>>>(),
                ""
            )).Returns(testTrailers);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto[] dtos = trailerService.Get(true);
            TestUtil.Compare(testDtos, dtos);
        }

        #endregion

        #region Add Trailer

        [TestMethod]
        public void AddTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                Deleted = false,
                LocationId = testLocation.Id
            };
            TrailerDto testDto = Mapper.Map<TrailerDto>(testTrailer);
            testDto.Location = Mapper.Map<LocationDto>(testLocation);

            //Mock calls
            mockLocationRepo.Setup(x => x.Exists(It.Is<Guid>(y => y == testLocation.Id))).Returns(true);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns<Trailer>(null);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto dto = trailerService.Add(testDto);
            TestUtil.Compare(testDto, dto);
        }

        [TestMethod]
        public void AddSoftDeletedTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                Deleted = true,
                LocationId = testLocation.Id
            };
            TrailerDto testDto = Mapper.Map<TrailerDto>(testTrailer);
            testDto.Location = Mapper.Map<LocationDto>(testLocation);

            //Mock calls
            mockLocationRepo.Setup(x => x.Exists(It.Is<Guid>(y => y == testLocation.Id))).Returns(true);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns(testTrailer);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto dto = trailerService.Add(testDto);
            TestUtil.Compare(testDto, dto, deletedEqual: false);
        }

        [TestMethod]
        public void AddTrailerWithSoftDeletedLocation()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = true,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                Deleted = false,
                LocationId = testLocation.Id
            };
            TrailerDto testDto = Mapper.Map<TrailerDto>(testTrailer);
            testDto.Location = Mapper.Map<LocationDto>(testLocation);

            //Mock calls
            mockLocationRepo.Setup(x => x.Exists(It.Is<Guid>(y => y == testLocation.Id))).Returns(true);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns<Trailer>(null);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto dto = trailerService.Add(testDto);
            TestUtil.Compare(testDto, dto);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void AddExistingTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                Deleted = false,
                LocationId = testLocation.Id
            };
            TrailerDto testDto = Mapper.Map<TrailerDto>(testTrailer);
            testDto.Location = Mapper.Map<LocationDto>(testLocation);

            //Mock calls
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns(testTrailer);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            trailerService.Add(testDto);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void AddTrailerWithNonexistingLocation()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                Deleted = false,
                LocationId = testLocation.Id
            };
            TrailerDto testDto = Mapper.Map<TrailerDto>(testTrailer);
            testDto.Location = Mapper.Map<LocationDto>(testLocation);

            //Mock calls
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns<Location>(null);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns<Trailer>(null);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            trailerService.Add(testDto);
        }

        #endregion

        #region Update Trailer

        [TestMethod]
        public void UpdateTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                Deleted = false,
                LocationId = testLocation.Id
            };
            TrailerDto testDto = Mapper.Map<TrailerDto>(testTrailer);
            testDto.Location = Mapper.Map<LocationDto>(testLocation);

            //Mock calls
            mockLocationRepo.Setup(x => x.Exists(It.Is<Guid>(y => y == testLocation.Id))).Returns(true);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns(testTrailer);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto dto = trailerService.Update(testDto);
            TestUtil.Compare(testDto, dto);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void UpdateNonexistentTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                Deleted = false,
                LocationId = testLocation.Id
            };
            TrailerDto testDto = Mapper.Map<TrailerDto>(testTrailer);
            testDto.Location = Mapper.Map<LocationDto>(testLocation);

            //Mock calls
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns<Trailer>(null);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            trailerService.Update(testDto);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void UpdateTrailerWithNonexistentTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                Deleted = false,
                LocationId = testLocation.Id
            };
            TrailerDto testDto = Mapper.Map<TrailerDto>(testTrailer);
            testDto.Location = Mapper.Map<LocationDto>(testLocation);

            //Mock calls
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns<Trailer>(null);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto dto = trailerService.Update(testDto);
            TestUtil.Compare(testDto, dto);
        }

        #endregion

        #region Delete Trailer

        [TestMethod]
        public void DeleteTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                Deleted = false,
                LocationId = testLocation.Id
            };
            TrailerDto testDto = Mapper.Map<TrailerDto>(testTrailer);

            //Mock call
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns(testTrailer);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            trailerService.Delete(testTrailer.Id);
        }

        [TestMethod]
        public void DeleteNonexistentTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                Deleted = false,
                LocationId = testLocation.Id
            };
            TrailerDto testDto = Mapper.Map<TrailerDto>(testTrailer);

            //Mock call
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns<Trailer>(null);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            trailerService.Delete(testTrailer.Id);
        }

        #endregion
    }
}
