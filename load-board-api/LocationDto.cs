using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace load_board_api
{
    public class LocationDto
    {
        public Guid Id;
        public string Name;
        public bool Deleted;
        public DateTime LastUpdated;
    }
}