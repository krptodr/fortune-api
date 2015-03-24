using load_board_api.Models;
using load_board_api.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace load_board_api.Tests.Test_Start
{
    static class TestUtil
    {
        public static readonly IUnitOfWork UNIT_OF_WORK;
        public static readonly IEnumerable<Value> VALUES;

        static TestUtil()
        {
            //Test data
            VALUES = new List<Value>
            {
                new Value {
                    Id = Guid.NewGuid(),
                    Name = "TEST 1"
                },
                new Value {
                    Id = Guid.NewGuid(),
                    Name = "TEST 2"
                }
            };

            //Mock Repos
            Mock<IRepo<Value>> mockValueRepo = new Mock<IRepo<Value>>();

            //Exists
            mockValueRepo.Setup(x => x.Exists(It.IsIn<Guid>(new List<Guid> { VALUES.ElementAt(0).Id, VALUES.ElementAt(1).Id }))).Returns(true);
            mockValueRepo.Setup(x => x.Exists(It.Is<Guid>(y => y == Guid.Empty))).Returns(false);

            //Query
            mockValueRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<Value, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Func<IQueryable<Value>, IOrderedQueryable<Value>>>(),
                It.IsAny<string>()
            )).Returns(VALUES);

            //Get
            mockValueRepo.Setup(x => x.Get(It.Is<Guid>(y => y == VALUES.ElementAt(0).Id))).Returns(VALUES.ElementAt(0));
            mockValueRepo.Setup(x => x.Get(It.Is<Guid>(y => y == VALUES.ElementAt(1).Id))).Returns(VALUES.ElementAt(1));
            mockValueRepo.Setup(x => x.Get(It.Is<Guid>(y => y == Guid.Empty))).Returns<Value>(null);

            //LoadBoardDbContext
            Mock<LoadBoardDbContext> mockContext = new Mock<LoadBoardDbContext>();

            //Unit of work
            UNIT_OF_WORK = new UnitOfWork(
                mockContext.Object,
                mockValueRepo.Object
            );
        }
    }
}
