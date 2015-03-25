using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using load_board_api.Models;
using load_board_api.Persistence;

namespace load_board_api.Tests.Test_Start
{
    public class DbInit : DropCreateDatabaseAlways<LoadBoardDbContext>
    {
        protected override void Seed(LoadBoardDbContext context)
        {
            TestObject[] values = new TestObject[] {
                new TestObject 
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Value 2"
                },
                new TestObject 
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Value 2"
                }
            };

            context.Values.AddRange(values);

            context.SaveChanges();
        }
    }
}