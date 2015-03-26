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
                },
                new Location {
                    Id = Guid.NewGuid(),
                    Name = "Test Deleted Location",
                    LastUpdated = DateTime.UtcNow,
                    Deleted = true
                }
            };
            context.Locations.AddRange(locations);

            //Trailers
            Trailer[] trailers = new Trailer[] {
                new Trailer {
                    Id = 111111,
                    LocationId = locations[0].Id,
                    LastUpdated = DateTime.UtcNow
                }, new Trailer {
                    Id = 222222,
                    LocationId = locations[0].Id,
                    LastUpdated = DateTime.UtcNow
                }, new Trailer {
                    Id = 333333,
                    LocationId = locations[0].Id,
                    LastUpdated = DateTime.UtcNow,
                    Deleted = true
                }
            };
            context.Trailers.AddRange(trailers);
            

            context.SaveChanges();
        }
    }
}