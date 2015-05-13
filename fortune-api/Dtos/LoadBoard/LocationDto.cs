using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.Dtos.LoadBoard
{
    public class LocationDto
    {
        public Guid Id;
        public string Name;
        public bool Deleted;
        public byte[] RowVersion { get; set; }
    }
}