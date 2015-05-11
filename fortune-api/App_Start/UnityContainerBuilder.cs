using fortune_api.Models;
using fortune_api.Persistence;
using fortune_api.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.App_Start
{
    public class UnityContainerBuilder
    {
        public static UnityContainer Build()
        {
            UnityContainer container = new UnityContainer();

            //Register Types
            container.RegisterType<FortuneDbContext>(new PerResolveLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager());
            container.RegisterType<IRepo<Location>, Repo<Location>>(new PerResolveLifetimeManager());
            container.RegisterType<IRepo<Trailer>, Repo<Trailer>>(new PerResolveLifetimeManager());
            container.RegisterType<IRepo<Load>, Repo<Load>>(new PerResolveLifetimeManager());
            container.RegisterType<ILocationService, LocationService>();
            container.RegisterType<ITrailerService, TrailerService>();
            container.RegisterType<ILoadService, LoadService>();

            return container;
        }
    }
}