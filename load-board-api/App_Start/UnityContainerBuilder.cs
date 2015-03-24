using load_board_api.Persistence;
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

            return container;
        }
    }
}