using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.Enums
{
    public enum LoadStatus
    {
        Loading,
        Unloading,
        Loaded,
        Ready,
        InTransit,
        Complete
    }
}