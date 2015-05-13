using fortune_api.Models.LoadBoard;
using fortune_api.Persistence;
using fortune_api.Services.LoadBoard;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using fortune_api.Services.Security;
using fortune_api.Models.Auth;
using fortune_api.Services.Auth;

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
            container.RegisterType<IRepo<UserProfile>, Repo<UserProfile>>(new PerResolveLifetimeManager());
            container.RegisterType<IRepo<EmailAuth>, Repo<EmailAuth>>(new PerResolveLifetimeManager());
            container.RegisterType<IRepo<Load>, Repo<Load>>(new PerResolveLifetimeManager());
            container.RegisterType<IRepo<Permission>, Repo<Permission>>();
            container.RegisterType<ILocationService, LocationService>();
            container.RegisterType<ITrailerService, TrailerService>();
            container.RegisterType<ILoadService, LoadService>();
            container.RegisterType<IHasher, CryptSharpHasher>(new ContainerControlledLifetimeManager());
            container.RegisterType<IJwtService, JwtService>();
            container.RegisterType<IAuthService, AuthService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IPermissionService, PermissionService>();

            return container;
        }
    }
}