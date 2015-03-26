using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using load_board_api.Models;

namespace load_board_api.Persistence
{
    public class DbInit : DropCreateDatabaseAlways<LoadBoardDbContext>
    {
        protected override void Seed(LoadBoardDbContext context)
        {
            //Test Objects
            TestObject[] testObjects = new TestObject[] {
                new TestObject 
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Value 1"
                },
                new TestObject 
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Value 2"
                }
            };

            context.TestObjects.AddRange(testObjects);

            //Locations
            Location[] locations = new Location[] {
                new Location {
                    Id = Guid.NewGuid(),
                    Name = "Test Location 1",
                    LastUpdated = DateTime.UtcNow
                },
                new Location {
                    Id = Guid.NewGuid(),
                    Name = "Test Location 2",
                    LastUpdated = DateTime.UtcNow
                }
            };

            context.Locations.AddRange(locations);

            context.SaveChanges();
        }
    }
}