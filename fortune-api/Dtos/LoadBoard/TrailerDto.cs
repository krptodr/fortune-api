using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.LoadBoard.Dtos
{
    public class TrailerDto
    {
        public int Id { get; set; }
        public LocationDto Location { get; set; }
        public bool Deleted { get; set; }
        public byte[] RowVersion { get; set; }
    }
}