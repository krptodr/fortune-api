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

            context.SaveChanges();
        }
    }
}