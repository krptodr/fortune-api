﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(load_board_api.Startup))]

namespace load_board_api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}