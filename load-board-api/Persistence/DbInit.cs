using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using load_board_api.Models;
using load_board_api.Enums;

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
                }, 
                new Trailer {
                    Id = 222222,
                    LocationId = locations[0].Id,
                    LastUpdated = DateTime.UtcNow
                }, 
                new Trailer {
                    Id = 333333,
                    LocationId = locations[0].Id,
                    LastUpdated = DateTime.UtcNow,
                    Deleted = true
                }
            };
            context.Trailers.AddRange(trailers);

            //Loads
            Load[] loads = new Load[] {
                new Load {
                    Id = Guid.NewGuid(),
                    Type = LoadType.Inbound,
                    LastUpdated = DateTime.UtcNow,
                    Status = LoadStatus.InTransit,
                    TrailerId = trailers[0].Id,
                    Appointment = null,
                    DepartureTime = DateTime.UtcNow.AddDays(-3),
                    ArrivalTime = DateTime.UtcNow,
                    OriginId = locations[0].Id,
                    DestinationId = locations[1].Id,
                    CfNum = 12345,
                    PuNum = null
                },
                new Load {
                    Id = Guid.NewGuid(),
                    Type = LoadType.Outbound,
                    LastUpdated = DateTime.UtcNow,
                    Status = LoadStatus.Ready,
                    TrailerId = trailers[1].Id,
                    Appointment = DateTime.UtcNow.AddHours(1),
                    DepartureTime = null,
                    ArrivalTime = DateTime.UtcNow,
                    OriginId = locations[1].Id,
                    DestinationId = locations[0].Id,
                    CfNum = 54321,
                    PuNum = 51423 
                },
                new Load {
                    Id = Guid.NewGuid(),
                    Type = LoadType.Intraplant,
                    LastUpdated = DateTime.UtcNow,
                    Status = LoadStatus.Complete,
                    TrailerId = trailers[1].Id,
                    Appointment = null,
                    DepartureTime = null,
                    ArrivalTime = DateTime.UtcNow,
                    OriginId = locations[1].Id,
                    DestinationId = locations[0].Id,
                    CfNum = null,
                    PuNum = null 
                },
                new Load {
                    Id = Guid.NewGuid(),
                    Type = LoadType.LocalDelivery,
                    LastUpdated = DateTime.UtcNow,
                    Status = LoadStatus.InTransit,
                    TrailerId = trailers[1].Id,
                    Appointment = DateTime.UtcNow.AddMinutes(30),
                    DepartureTime = DateTime.UtcNow,
                    ArrivalTime = null,
                    OriginId = locations[1].Id,
                    DestinationId = locations[0].Id,
                    CfNum = null,
                    PuNum = null 
                }
            };
            context.Loads.AddRange(loads);
            
            context.SaveChanges();
        }
    }
}