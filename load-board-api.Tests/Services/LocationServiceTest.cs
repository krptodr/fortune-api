using AutoMapper;
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
    [TestClass]
    public class LocationServiceTest
    {
        #region Get Location

        [TestMethod]
        public void GetLocation()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                LastUpdated = DateTime.UtcNow,
                Name = "TEST"
            };
            LocationDto testDto = Mapper.Map<LocationDto>(testLocation);

            //Mock call
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Location Service
            ILocationService locationService = new LocationService(unitOfWork);

            //Test
            LocationDto dto = locationService.Get(testDto.Id);
            TestUtil.Compare(testDto, testDto);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void GetNonexistentLocation()
        {
            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Mock call
            mockLocationRepo.Setup(x => x.Get(It.IsAny<Guid>())).Returns<Location>(null);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Location Service
            ILocationService locationService = new LocationService(unitOfWork);

            //Test
            LocationDto dto = locationService.Get(Guid.NewGuid());
        }

        #endregion

        #region Get Locations

        [TestMethod]
        public void GetLocations()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test locations
            IEnumerable<Location> testLocations = new List<Location> {
                new Location {
                    Id = Guid.NewGuid(),
                    LastUpdated = DateTime.UtcNow,
                    Name = "TEST",
                    Deleted = false
                },
                new Location {
                    Id = Guid.NewGuid(),
                    LastUpdated = DateTime.UtcNow,
                    Name = "TEST",
                    Deleted = false
                }
            };
            LocationDto[] testDtos = Mapper.Map<LocationDto[]>(testLocations);

            //Mock call
            mockLocationRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<Location, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<Location>, IOrderedQueryable<Location>>>(),
                ""
            )).Returns(testLocations);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Location Service
            ILocationService locationService = new LocationService(unitOfWork);

            //Test
            LocationDto[] dtos = locationService.Get(false);
            TestUtil.Compare(testDtos, dtos);
        }

        [TestMethod]
        public void GetLocationsWithDeleted()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test locations
            IEnumerable<Location> testLocations = new List<Location> {
                new Location {
                    Id = Guid.NewGuid(),
                    LastUpdated = DateTime.UtcNow,
                    Name = "TEST",
                    Deleted = false
                },
                new Location {
                    Id = Guid.NewGuid(),
                    LastUpdated = DateTime.UtcNow,
                    Name = "TEST",
                    Deleted = true
                }
            };
            LocationDto[] testDtos = Mapper.Map<LocationDto[]>(testLocations);

            //Mock call
            mockLocationRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<Location, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<Location>, IOrderedQueryable<Location>>>(),
                ""
            )).Returns(testLocations);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Location Service
            ILocationService locationService = new LocationService(unitOfWork);

            //Test
            LocationDto[] dtos = locationService.Get(true);
            TestUtil.Compare(testDtos, dtos);
        }

        #endregion

        #region Add Location

        [TestMethod]
        public void AddLocation()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            LocationDto testDto = new LocationDto
            {
                Id = Guid.Empty,
                Deleted = false,
                LastUpdated = DateTime.UtcNow,
                Name = "TEST"
            };

            //Mock call
            mockLocationRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<Location, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<Location>, IOrderedQueryable<Location>>>(),
                ""
            )).Returns(new List<Location>());

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Location Service
            ILocationService locationService = new LocationService(unitOfWork);

            //Test
            LocationDto dto = locationService.Add(testDto);
            TestUtil.Compare(testDto, dto, idEqual: false);
        }

        [TestMethod]
        public void AddSoftDeletedLocation()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = true,
                LastUpdated = DateTime.UtcNow,
                Name = "TEST"
            };
            LocationDto testDto = Mapper.Map<LocationDto>(testLocation);

            //Mock call
            mockLocationRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<Location, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<Location>, IOrderedQueryable<Location>>>(),
                ""
            )).Returns(new List<Location> { testLocation });

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Location Service
            ILocationService locationService = new LocationService(unitOfWork);

            //Test
            LocationDto dto = locationService.Add(testDto);
            TestUtil.Compare(testDto, dto, deletedEqual: false);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void AddExistingLocation()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                LastUpdated = DateTime.UtcNow,
                Name = "TEST"
            };
            LocationDto testDto = Mapper.Map<LocationDto>(testLocation);

            //Mock call
            mockLocationRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<Location, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<Location>, IOrderedQueryable<Location>>>(),
                ""
            )).Returns(new List<Location> { testLocation });

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Location Service
            ILocationService locationService = new LocationService(unitOfWork);

            //Test
            LocationDto dto = locationService.Add(testDto);
        }

        #endregion

        #region Update location

        [TestMethod]
        public void UpdateLocation()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                LastUpdated = DateTime.UtcNow,
                Name = "TEST"
            };
            LocationDto testDto = Mapper.Map<LocationDto>(testLocation);

            //Mock call
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Location Service
            ILocationService locationService = new LocationService(unitOfWork);

            //Test
            LocationDto dto = locationService.Add(testDto);
            TestUtil.Compare(testDto, dto, idEqual: false);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void UpdateNonexistentLocation()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                LastUpdated = DateTime.UtcNow,
                Name = "TEST"
            };
            LocationDto testDto = Mapper.Map<LocationDto>(testLocation);

            //Mock call
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns<Location>(null);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Location Service
            ILocationService locationService = new LocationService(unitOfWork);

            //Test
            LocationDto dto = locationService.Update(testDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ConflictException))]
        public void UpdateLocationWithConflict()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                LastUpdated = DateTime.UtcNow,
                Name = "TEST"
            };
            LocationDto testDto = Mapper.Map<LocationDto>(testLocation);
            testDto.LastUpdated = testDto.LastUpdated.AddMilliseconds(-1);

            //Mock call
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Location Service
            ILocationService locationService = new LocationService(unitOfWork);

            //Test
            LocationDto dto = locationService.Update(testDto);
        }

        #endregion

        #region Delete Location

        [TestMethod]
        public void DeleteLocation()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test location
            Location testLocation = new Location
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                LastUpdated = DateTime.UtcNow,
                Name = "TEST"
            };

            //Mock call
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Location Service
            ILocationService locationService = new LocationService(unitOfWork);

            //Test
            locationService.Delete(testLocation.Id);
        }

        [TestMethod]
        public void DeleteNonexistentLocation()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Mock repos
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();
            Mock<IRepo<Trailer>> mockTrailerRepo = new Mock<IRepo<Trailer>>();
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test id
            Guid testId = Guid.NewGuid();

            //Mock call
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testId))).Returns<Location>(null);

            //Unit of work
            IUnitOfWork unitOfWork = new UnitOfWork(
                mockContext.Object,
                mockLocationRepo.Object,
                mockTrailerRepo.Object,
                mockLoadRepo.Object
            );

            //Location Service
            ILocationService locationService = new LocationService(unitOfWork);

            //Test
            locationService.Delete(testId);
        }

        #endregion
    }
}
