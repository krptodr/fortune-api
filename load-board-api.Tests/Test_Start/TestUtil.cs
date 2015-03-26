using load_board_api.App_Start;
using load_board_api.Dtos;
using load_board_api.Models;
using load_board_api.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace load_board_api.Tests.Test_Start
{
    static class TestUtil
    {
        public static readonly IUnitOfWork UNIT_OF_WORK;
        public static readonly IEnumerable<TestObject> TEST_OBJECTS;
        public static readonly IEnumerable<Location> LOCATIONS;
        public static readonly IEnumerable<LocationDto> LOCATION_DTOS;

        static TestUtil()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Test data
            TEST_OBJECTS = new List<TestObject>
            {
                new TestObject {
                    Id = Guid.NewGuid(),
                    Name = "TEST 1"
                },
                new TestObject {
                    Id = Guid.NewGuid(),
                    Name = "TEST 2"
                }
            };

            LOCATIONS = new List<Location> {
                new Location {
                    Id = Guid.NewGuid(),
                    Name = "Test Location 1",
                    LastUpdated = DateTime.UtcNow,
                },
                new Location {
                    Id = Guid.NewGuid(),
                    Name = "Test Location 2",
                    LastUpdated = DateTime.UtcNow,
                },
                new Location {
                    Id = Guid.NewGuid(),
                    Name = "Test Deleted Location",
                    Deleted = true,
                    LastUpdated = DateTime.UtcNow
                }
            };
            LOCATION_DTOS = Mapper.Map<IEnumerable<LocationDto>>(LOCATIONS);

            //Mock Repos
            Mock<IRepo<TestObject>> mockTestObjectRepo = new Mock<IRepo<TestObject>>();
            Mock<IRepo<Location>> mockLocationRepo = new Mock<IRepo<Location>>();

            //Exists
            mockTestObjectRepo.Setup(x => x.Exists(It.IsIn<Guid>(new List<Guid> { TEST_OBJECTS.ElementAt(0).Id, TEST_OBJECTS.ElementAt(1).Id }))).Returns(true);
            mockTestObjectRepo.Setup(x => x.Exists(It.Is<Guid>(y => y == Guid.Empty))).Returns(false);
            mockLocationRepo.Setup(x => x.Exists(It.IsIn<Guid>(new List<Guid> { LOCATIONS.ElementAt(0).Id, LOCATIONS.ElementAt(1).Id }))).Returns(true);
            mockLocationRepo.Setup(x => x.Exists(It.Is<Guid>(y => y == Guid.Empty))).Returns(false);

            //Query
            mockTestObjectRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<TestObject, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Func<IQueryable<TestObject>, IOrderedQueryable<TestObject>>>(),
                It.IsAny<string>()
            )).Returns(TEST_OBJECTS);
            mockLocationRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<Location, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Func<IQueryable<Location>, IOrderedQueryable<Location>>>(),
                It.IsAny<string>()
            )).Returns(new List<Location> {LOCATIONS.ElementAt(0)});

            //Get
            mockTestObjectRepo.Setup(x => x.Get(It.Is<Guid>(y => y == TEST_OBJECTS.ElementAt(0).Id))).Returns(TEST_OBJECTS.ElementAt(0));
            mockTestObjectRepo.Setup(x => x.Get(It.Is<Guid>(y => y == TEST_OBJECTS.ElementAt(1).Id))).Returns(TEST_OBJECTS.ElementAt(1));
            mockTestObjectRepo.Setup(x => x.Get(It.Is<Guid>(y => y == Guid.Empty))).Returns<TestObject>(null);
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == LOCATIONS.ElementAt(0).Id))).Returns(LOCATIONS.ElementAt(0));
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == LOCATIONS.ElementAt(1).Id))).Returns(LOCATIONS.ElementAt(1));
            mockLocationRepo.Setup(x => x.Get(It.Is<Guid>(y => y == Guid.Empty))).Returns<Location>(null);

            //LoadBoardDbContext
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Unit of work
            UNIT_OF_WORK = new UnitOfWork(
                mockContext.Object,
                mockTestObjectRepo.Object,
                mockLocationRepo.Object
            );
        }

        public static void AreEqual(LocationDto expected, LocationDto actual) {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, expected.Name);
            Assert.AreEqual(expected.Deleted, actual.Deleted);
            Assert.AreEqual(expected.LastUpdated, actual.LastUpdated);
        }
    }
}
