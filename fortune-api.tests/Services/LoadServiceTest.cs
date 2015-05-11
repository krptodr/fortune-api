using AutoMapper;
using fortune_api.App_Start;
using fortune_api.Dtos;
using fortune_api.Enums;
using fortune_api.Exceptions;
using fortune_api.Models;
using fortune_api.Persistence;
using fortune_api.Services;
using fortune_api.Tests.Test_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace fortune_api.Tests.Services
{
    [TestClass]
    public class LoadServiceTest
    {
        #region Get

        [TestMethod]
        public void GetLoad()
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
            Location testLocation = new Location {
                Deleted = false,
                Id = Guid.NewGuid(),
                Name = "TEST LOCATION"
            };
            LocationDto testLocationDto = Mapper.Map<LocationDto>(testLocation);

            //Test Trailer
            Trailer testTrailer = new Trailer
            {
                Deleted = false,
                Id = 1,
                LocationId = testLocation.Id 
            };
            TrailerDto testTrailerDto = Mapper.Map<TrailerDto>(testTrailer);
            testTrailerDto.Location = testLocationDto;

            //Test Load
            Load testLoad = new Load {
                Appointment = DateTime.UtcNow,
                ArrivalTime = DateTime.UtcNow,
                CfNum = 1,
                Deleted = false,
                DepartureTime = DateTime.UtcNow,
                DestinationId = testLocation.Id,
                Id = Guid.NewGuid(),
                OriginId = testLocation.Id,
                PuNum = 1,
                Status = LoadStatus.InTransit,
                TrailerId = testTrailer.Id,
                Type = LoadType.Inbound
            };
            LoadDto testLoadDto = Mapper.Map<LoadDto>(testLoad);
            testLoadDto.Trailer = testTrailerDto;
            testLoadDto.Origin = testLocationDto;
            testLoadDto.Destination = testLocationDto;

            //Mock Calls
            mockLoadRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLoad.Id))).Returns(testLoad);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns(testTrailer);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.LocationRepo).Returns(mockLocationRepo.Object);
            mockUnitOfWork.SetupGet(x => x.TrailerRepo).Returns(mockTrailerRepo.Object);
            mockUnitOfWork.SetupGet(x => x.LoadRepo).Returns(mockLoadRepo.Object);

            //Load service
            LoadService loadService = new LoadService(mockUnitOfWork.Object);

            //Test
            LoadDto dto = loadService.Get(testLoad.Id);
            TestUtil.Compare(testLoadDto, dto);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void GetNonexistentLoad()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Mock Calls
            mockLoadRepo.Setup(x => x.Get(It.IsAny<Guid>())).Returns<Load>(null);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.LoadRepo).Returns(mockLoadRepo.Object);

            //Load service
            LoadService loadService = new LoadService(mockUnitOfWork.Object);

            //Test
            LoadDto dto = loadService.Get(Guid.NewGuid());
        }

        #endregion

        #region Add

        [TestMethod]
        public void AddLoad()
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
                Deleted = false,
                Id = Guid.NewGuid(),
                Name = "TEST LOCATION"
            };
            LocationDto testLocationDto = Mapper.Map<LocationDto>(testLocation);

            //Test Trailer
            Trailer testTrailer = new Trailer
            {
                Deleted = false,
                Id = 1,
                LocationId = testLocation.Id
            };
            TrailerDto testTrailerDto = Mapper.Map<TrailerDto>(testTrailer);
            testTrailerDto.Location = testLocationDto;

            //Test Load
            Load testLoad = new Load
            {
                Appointment = DateTime.UtcNow,
                ArrivalTime = DateTime.UtcNow,
                CfNum = 1,
                Deleted = false,
                DepartureTime = DateTime.UtcNow,
                DestinationId = testLocation.Id,
                Id = Guid.NewGuid(),
                OriginId = testLocation.Id,
                PuNum = 1,
                Status = LoadStatus.InTransit,
                TrailerId = testTrailer.Id,
                Type = LoadType.Inbound
            };
            LoadDto testLoadDto = Mapper.Map<LoadDto>(testLoad);
            testLoadDto.Trailer = testTrailerDto;
            testLoadDto.Origin = testLocationDto;
            testLoadDto.Destination = testLocationDto;

            //Mock Calls
            mockLoadRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLoad.Id))).Returns<Load>(null);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns(testTrailer);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.LocationRepo).Returns(mockLocationRepo.Object);
            mockUnitOfWork.SetupGet(x => x.TrailerRepo).Returns(mockTrailerRepo.Object);
            mockUnitOfWork.SetupGet(x => x.LoadRepo).Returns(mockLoadRepo.Object);

            //Load service
            LoadService loadService = new LoadService(mockUnitOfWork.Object);

            //Test
            LoadDto dto = loadService.Add(testLoadDto);
            TestUtil.Compare(testLoadDto, dto, idEqual:false);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void AddExistingLoad()
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
                Deleted = false,
                Id = Guid.NewGuid(),
                Name = "TEST LOCATION"
            };
            LocationDto testLocationDto = Mapper.Map<LocationDto>(testLocation);

            //Test Trailer
            Trailer testTrailer = new Trailer
            {
                Deleted = false,
                Id = 1,
                LocationId = testLocation.Id
            };
            TrailerDto testTrailerDto = Mapper.Map<TrailerDto>(testTrailer);
            testTrailerDto.Location = testLocationDto;

            //Test Load
            Load testLoad = new Load
            {
                Appointment = DateTime.UtcNow,
                ArrivalTime = DateTime.UtcNow,
                CfNum = 1,
                Deleted = false,
                DepartureTime = DateTime.UtcNow,
                DestinationId = testLocation.Id,
                Id = Guid.NewGuid(),
                OriginId = testLocation.Id,
                PuNum = 1,
                Status = LoadStatus.InTransit,
                TrailerId = testTrailer.Id,
                Type = LoadType.Inbound
            };
            LoadDto testLoadDto = Mapper.Map<LoadDto>(testLoad);
            testLoadDto.Trailer = testTrailerDto;
            testLoadDto.Origin = testLocationDto;
            testLoadDto.Destination = testLocationDto;

            //Mock Calls
            mockLoadRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLoad.Id))).Returns(testLoad);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns(testTrailer);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.LocationRepo).Returns(mockLocationRepo.Object);
            mockUnitOfWork.SetupGet(x => x.TrailerRepo).Returns(mockTrailerRepo.Object);
            mockUnitOfWork.SetupGet(x => x.LoadRepo).Returns(mockLoadRepo.Object);

            //Load service
            LoadService loadService = new LoadService(mockUnitOfWork.Object);

            //Test
            LoadDto dto = loadService.Add(testLoadDto);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void AddLoadWithNonexistingLocation()
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
                Deleted = false,
                Id = Guid.NewGuid(),
                Name = "TEST LOCATION"
            };
            LocationDto testLocationDto = Mapper.Map<LocationDto>(testLocation);

            //Test Trailer
            Trailer testTrailer = new Trailer
            {
                Deleted = false,
                Id = 1,
                LocationId = testLocation.Id
            };
            TrailerDto testTrailerDto = Mapper.Map<TrailerDto>(testTrailer);
            testTrailerDto.Location = testLocationDto;

            //Test Load
            Load testLoad = new Load
            {
                Appointment = DateTime.UtcNow,
                ArrivalTime = DateTime.UtcNow,
                CfNum = 1,
                Deleted = false,
                DepartureTime = DateTime.UtcNow,
                DestinationId = testLocation.Id,
                Id = Guid.NewGuid(),
                OriginId = testLocation.Id,
                PuNum = 1,
                Status = LoadStatus.InTransit,
                TrailerId = testTrailer.Id,
                Type = LoadType.Inbound
            };
            LoadDto testLoadDto = Mapper.Map<LoadDto>(testLoad);
            testLoadDto.Trailer = testTrailerDto;
            testLoadDto.Origin = testLocationDto;
            testLoadDto.Destination = testLocationDto;

            //Mock Calls
            mockLoadRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLoad.Id))).Returns<Load>(null);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns(testTrailer);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns<Location>(null);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.LocationRepo).Returns(mockLocationRepo.Object);
            mockUnitOfWork.SetupGet(x => x.TrailerRepo).Returns(mockTrailerRepo.Object);
            mockUnitOfWork.SetupGet(x => x.LoadRepo).Returns(mockLoadRepo.Object);

            //Load service
            LoadService loadService = new LoadService(mockUnitOfWork.Object);

            //Test
            LoadDto dto = loadService.Add(testLoadDto);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void AddLoadWithNonexistingTrailer()
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
                Deleted = false,
                Id = Guid.NewGuid(),
                Name = "TEST LOCATION"
            };
            LocationDto testLocationDto = Mapper.Map<LocationDto>(testLocation);

            //Test Trailer
            Trailer testTrailer = new Trailer
            {
                Deleted = false,
                Id = 1,
                LocationId = testLocation.Id
            };
            TrailerDto testTrailerDto = Mapper.Map<TrailerDto>(testTrailer);
            testTrailerDto.Location = testLocationDto;

            //Test Load
            Load testLoad = new Load
            {
                Appointment = DateTime.UtcNow,
                ArrivalTime = DateTime.UtcNow,
                CfNum = 1,
                Deleted = false,
                DepartureTime = DateTime.UtcNow,
                DestinationId = testLocation.Id,
                Id = Guid.NewGuid(),
                OriginId = testLocation.Id,
                PuNum = 1,
                Status = LoadStatus.InTransit,
                TrailerId = testTrailer.Id,
                Type = LoadType.Inbound
            };
            LoadDto testLoadDto = Mapper.Map<LoadDto>(testLoad);
            testLoadDto.Trailer = testTrailerDto;
            testLoadDto.Origin = testLocationDto;
            testLoadDto.Destination = testLocationDto;

            //Mock Calls
            mockLoadRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLoad.Id))).Returns<Load>(null);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns<Trailer>(null);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.LocationRepo).Returns(mockLocationRepo.Object);
            mockUnitOfWork.SetupGet(x => x.TrailerRepo).Returns(mockTrailerRepo.Object);
            mockUnitOfWork.SetupGet(x => x.LoadRepo).Returns(mockLoadRepo.Object);

            //Load service
            LoadService loadService = new LoadService(mockUnitOfWork.Object);

            //Test
            LoadDto dto = loadService.Add(testLoadDto);
        }

        #endregion

        #region Update

        [TestMethod]
        public void UpdateLoad()
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
                Deleted = false,
                Id = Guid.NewGuid(),
                Name = "TEST LOCATION"
            };
            LocationDto testLocationDto = Mapper.Map<LocationDto>(testLocation);

            //Test Trailer
            Trailer testTrailer = new Trailer
            {
                Deleted = false,
                Id = 1,
                LocationId = testLocation.Id
            };
            TrailerDto testTrailerDto = Mapper.Map<TrailerDto>(testTrailer);
            testTrailerDto.Location = testLocationDto;

            //Test Load
            Load testLoad = new Load
            {
                Appointment = DateTime.UtcNow,
                ArrivalTime = DateTime.UtcNow,
                CfNum = 1,
                Deleted = false,
                DepartureTime = DateTime.UtcNow,
                DestinationId = testLocation.Id,
                Id = Guid.NewGuid(),
                OriginId = testLocation.Id,
                PuNum = 1,
                Status = LoadStatus.InTransit,
                TrailerId = testTrailer.Id,
                Type = LoadType.Inbound
            };
            LoadDto testLoadDto = Mapper.Map<LoadDto>(testLoad);
            testLoadDto.Trailer = testTrailerDto;
            testLoadDto.Origin = testLocationDto;
            testLoadDto.Destination = testLocationDto;

            //Mock Calls
            mockLoadRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLoad.Id))).Returns(testLoad);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns(testTrailer);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.LocationRepo).Returns(mockLocationRepo.Object);
            mockUnitOfWork.SetupGet(x => x.TrailerRepo).Returns(mockTrailerRepo.Object);
            mockUnitOfWork.SetupGet(x => x.LoadRepo).Returns(mockLoadRepo.Object);

            //Load service
            LoadService loadService = new LoadService(mockUnitOfWork.Object);

            //Test
            LoadDto dto = loadService.Update(testLoadDto);
            TestUtil.Compare(testLoadDto, dto);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void UpdateNonexistantLoad()
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
                Deleted = false,
                Id = Guid.NewGuid(),
                Name = "TEST LOCATION"
            };
            LocationDto testLocationDto = Mapper.Map<LocationDto>(testLocation);

            //Test Trailer
            Trailer testTrailer = new Trailer
            {
                Deleted = false,
                Id = 1,
                LocationId = testLocation.Id
            };
            TrailerDto testTrailerDto = Mapper.Map<TrailerDto>(testTrailer);
            testTrailerDto.Location = testLocationDto;

            //Test Load
            Load testLoad = new Load
            {
                Appointment = DateTime.UtcNow,
                ArrivalTime = DateTime.UtcNow,
                CfNum = 1,
                Deleted = false,
                DepartureTime = DateTime.UtcNow,
                DestinationId = testLocation.Id,
                Id = Guid.NewGuid(),
                OriginId = testLocation.Id,
                PuNum = 1,
                Status = LoadStatus.InTransit,
                TrailerId = testTrailer.Id,
                Type = LoadType.Inbound
            };
            LoadDto testLoadDto = Mapper.Map<LoadDto>(testLoad);
            testLoadDto.Trailer = testTrailerDto;
            testLoadDto.Origin = testLocationDto;
            testLoadDto.Destination = testLocationDto;

            //Mock Calls
            mockLoadRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLoad.Id))).Returns<Load>(null);
            mockTrailerRepo.Setup(x => x.Get(testTrailer.Id)).Returns(testTrailer);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLocation.Id))).Returns(testLocation);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.LocationRepo).Returns(mockLocationRepo.Object);
            mockUnitOfWork.SetupGet(x => x.TrailerRepo).Returns(mockTrailerRepo.Object);
            mockUnitOfWork.SetupGet(x => x.LoadRepo).Returns(mockLoadRepo.Object);

            //Load service
            LoadService loadService = new LoadService(mockUnitOfWork.Object);

            //Test
            LoadDto dto = loadService.Update(testLoadDto);
        }

        #endregion

        #region Delete

        [TestMethod]
        public void DeleteLoad()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Test Load
            Load testLoad = new Load
            {
                Appointment = DateTime.UtcNow,
                ArrivalTime = DateTime.UtcNow,
                CfNum = 1,
                Deleted = false,
                DepartureTime = DateTime.UtcNow,
                DestinationId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                OriginId = Guid.NewGuid(),
                PuNum = 1,
                Status = LoadStatus.InTransit,
                TrailerId = 1,
                Type = LoadType.Inbound
            };


            //Mock Calls
            mockLoadRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testLoad.Id))).Returns(testLoad);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.LoadRepo).Returns(mockLoadRepo.Object);

            //Load service
            LoadService loadService = new LoadService(mockUnitOfWork.Object);

            //Test
            loadService.Delete(testLoad.Id);
        }

        [TestMethod]
        public void DeleteNonexistentLoad()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock context
            Mock<FortuneDbContext> mockContext = new Mock<FortuneDbContext>();

            //Mock repos
            Mock<IRepo<Load>> mockLoadRepo = new Mock<IRepo<Load>>();

            //Mock Calls
            mockLoadRepo.Setup(x => x.Get(It.IsAny<Guid>())).Returns<Load>(null);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.LoadRepo).Returns(mockLoadRepo.Object);

            //Load service
            LoadService loadService = new LoadService(mockUnitOfWork.Object);

            //Test
            loadService.Delete(Guid.NewGuid());
        }

        #endregion
    }
}
