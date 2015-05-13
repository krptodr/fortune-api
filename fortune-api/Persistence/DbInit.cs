using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using fortune_api.Models.LoadBoard;
using fortune_api.Enums.LoadBoard;
using fortune_api.Models.Auth;

namespace fortune_api.Persistence
{
    public class DbInit : DropCreateDatabaseAlways<FortuneDbContext>
    {
        protected override void Seed(FortuneDbContext context)
        {
            //Permissions
            Permission[] permissions = new Permission[] {
                new Permission {
                    Id = Guid.NewGuid(),
                    Name = "EditAdmins"
                },
                new Permission {
                    Id = Guid.NewGuid(),
                    Name = "EditPermissions"
                },
                new Permission {
                    Id = Guid.NewGuid(),
                    Name = "EditTrailers"
                },
                new Permission {
                    Id = Guid.NewGuid(),
                    Name = "EditLocations"
                },
                new Permission {
                    Id = Guid.NewGuid(),
                    Name = "EditLoads"
                },
                new Permission {
                    Id = Guid.NewGuid(),
                    Name = "ViewLoads"
                }
            };
            context.Permissions.AddRange(permissions);

            //User Profiles
            UserProfile[] userProfiles = new UserProfile[] {
                new UserProfile {
                    Id = Guid.NewGuid(),
                    FirstName = "Ryan",
                    LastName = "Helmoski",
                    Permissions = permissions.ToList()
                }
            };
            context.UserProfiles.AddRange(userProfiles);

            //Email Auths
            EmailAuth[] emailAuths = new EmailAuth[] {
                new EmailAuth {
                    Id = Guid.NewGuid(),
                    UserId = userProfiles[0].Id,
                    Email = "ryan@fortunetransportation.com",
                    HashedPassword = "$2a$12$qesWZCh6U.aCdxiamaFdKeCXo8MS3cQx4dbxbx19hZHuYDs0uSNUy"
                }
            };
            context.EmailAuths.AddRange(emailAuths);

            //Locations
            Location[] locations = new Location[] {
                new Location {
                    Id = Guid.NewGuid(),
                    Name = "Test Location 1"
                },
                new Location {
                    Id = Guid.NewGuid(),
                    Name = "Test Location 2"
                },
                new Location {
                    Id = Guid.NewGuid(),
                    Name = "Test Deleted Location",
                    Deleted = true
                }
            };
            context.Locations.AddRange(locations);

            //Trailers
            Trailer[] trailers = new Trailer[] {
                new Trailer {
                    Id = 111111,
                    LocationId = locations[0].Id
                }, 
                new Trailer {
                    Id = 222222,
                    LocationId = locations[0].Id
                }, 
                new Trailer {
                    Id = 333333,
                    LocationId = locations[0].Id,
                    Deleted = true
                }
            };
            context.Trailers.AddRange(trailers);

            //Loads
            Load[] loads = new Load[] {
                new Load {
                    Id = Guid.NewGuid(),
                    Type = LoadType.Inbound,
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