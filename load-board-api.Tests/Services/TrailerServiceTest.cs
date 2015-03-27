﻿using AutoMapper;
using load_board_api.App_Start;
using load_board_api.Dtos;
using load_board_api.Exceptions;
using load_board_api.Models;
using load_board_api.Persistence;
using load_board_api.Services;
using load_board_api.Tests.Test_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace load_board_api.Tests.Services
{
    /// <summary>
    /// Summary description for TrailerServiceTest
    /// </summary>
    [TestClass]
    public class TrailerServiceTest
    {
        [TestMethod]
        public void GetTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                LastUpdated = DateTime.UtcNow,
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                LastUpdated = DateTime.UtcNow,
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
                mockTrailerRepo.Object
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
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();

            //Test id
            int testId = 111111;

            //Mock call
            mockTrailerRepo.Setup(x => x.Get(testId)).Returns<Trailer>(null);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto dto = trailerService.Get(testId);
        }

        [TestMethod]
        public void GetTrailers()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                LastUpdated = DateTime.UtcNow,
                Deleted = false,
                Name = "TEST"
            };

            //Test trailers
            IEnumerable<Trailer> testTrailers = new List<Trailer>
            {
                new Trailer {
                    Id = 111111,
                    LastUpdated = DateTime.UtcNow,
                    Deleted = false,
                    LocationId = testLocation.Id
                },
                new Trailer {
                    Id = 222222,
                    LastUpdated = DateTime.UtcNow,
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
                mockTrailerRepo.Object
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
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                LastUpdated = DateTime.UtcNow,
                Deleted = false,
                Name = "TEST"
            };

            //Test trailers
            IEnumerable<Trailer> testTrailers = new List<Trailer>
            {
                new Trailer {
                    Id = 111111,
                    LastUpdated = DateTime.UtcNow,
                    Deleted = false,
                    LocationId = testLocation.Id
                },
                new Trailer {
                    Id = 222222,
                    LastUpdated = DateTime.UtcNow,
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
                mockTrailerRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto[] dtos = trailerService.Get(true);
            TestUtil.Compare(testDtos, dtos);
        }

        [TestMethod]
        public void AddTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                LastUpdated = DateTime.UtcNow,
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                LastUpdated = DateTime.UtcNow,
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
                mockTrailerRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto dto = trailerService.Add(testDto);
            TestUtil.Compare(testDto, dto, lastUpdatedEqual: false);
        }

        [TestMethod]
        public void AddSoftDeletedTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                LastUpdated = DateTime.UtcNow,
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                LastUpdated = DateTime.UtcNow,
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
                mockTrailerRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto dto = trailerService.Add(testDto);
            TestUtil.Compare(testDto, dto, deletedEqual: false, lastUpdatedEqual: false);
        }

        [TestMethod]
        public void AddTrailerWithSoftDeletedLocation()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                LastUpdated = DateTime.UtcNow,
                Deleted = true,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                LastUpdated = DateTime.UtcNow,
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
                mockTrailerRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto dto = trailerService.Add(testDto);
            TestUtil.Compare(testDto, dto, lastUpdatedEqual: false);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void AddExistingTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                LastUpdated = DateTime.UtcNow,
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                LastUpdated = DateTime.UtcNow,
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
                mockTrailerRepo.Object
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
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                LastUpdated = DateTime.UtcNow,
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                LastUpdated = DateTime.UtcNow,
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
                mockTrailerRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            trailerService.Add(testDto);
        }

        [TestMethod]
        public void UpdateTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                LastUpdated = DateTime.UtcNow,
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                LastUpdated = DateTime.UtcNow,
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
                mockTrailerRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto dto = trailerService.Update(testDto);
            TestUtil.Compare(testDto, dto, lastUpdatedEqual: false);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void UpdateNonexistentTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                LastUpdated = DateTime.UtcNow,
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                LastUpdated = DateTime.UtcNow,
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
                mockTrailerRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            trailerService.Update(testDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ConflictException))]
        public void UpdateTrailerWithConflict()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                LastUpdated = DateTime.UtcNow,
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                LastUpdated = DateTime.UtcNow,
                Deleted = false,
                LocationId = testLocation.Id
            };
            TrailerDto testDto = Mapper.Map<TrailerDto>(testTrailer);
            testDto.LastUpdated = testDto.LastUpdated.AddMilliseconds(-1);
            testDto.Location = Mapper.Map<LocationDto>(testLocation);

            //Mock calls
            mockLocationRepo.Setup(x => x.Exists(It.Is<Guid>(y => y == testLocation.Id))).Returns(true);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns(testTrailer);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object
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
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                LastUpdated = DateTime.UtcNow,
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                LastUpdated = DateTime.UtcNow,
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
                mockTrailerRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            TrailerDto dto = trailerService.Update(testDto);
            TestUtil.Compare(testDto, dto, lastUpdatedEqual: false);
        }

        [TestMethod]
        public void DeleteTrailer()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                LastUpdated = DateTime.UtcNow,
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                LastUpdated = DateTime.UtcNow,
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
                mockTrailerRepo.Object
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
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                LastUpdated = DateTime.UtcNow,
                Deleted = false,
                Name = "TEST"
            };

            //Test trailer
            Trailer testTrailer = new Trailer
            {
                Id = 111111,
                LastUpdated = DateTime.UtcNow,
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
                mockTrailerRepo.Object
            );

            //Trailer service
            ITrailerService trailerService = new TrailerService(unitOfWork);

            //Test
            trailerService.Delete(testTrailer.Id);
        }
    }
}