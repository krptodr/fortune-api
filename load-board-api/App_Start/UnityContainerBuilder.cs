using load_board_api.Models;
using load_board_api.Persistence;
using load_board_api.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace load_board_api.App_Start
{
    public class UnityContainerBuilder
    {
        public static UnityContainer Build()
        {
            UnityContainer container = new UnityContainer();

            //Register Types
            container.RegisterType<LoadBoardDbContext>(new PerResolveLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager());
            container.RegisterType<IRepo<Location>, Repo<Location>>(new PerResolveLifetimeManager());
            container.RegisterType<IRepo<Trailer>, Repo<Trailer>>(new PerResolveLifetimeManager());
            container.RegisterType<ILocationService, LocationService>();
            container.RegisterType<ITrailerService, TrailerService>();

            return container;
        }
    }
}