using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace load_board_api.Dtos
{
    public class TrailerDto
    {
        public Guid Id { get; set; }
        public LocationDto Location { get; set; }
        public bool Deleted { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}