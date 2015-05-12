using fortune_api.LoadBoard.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.LoadBoard.Dtos
{
    public class LoadDto
    {
        public Guid Id { get; set; }
        public LoadType Type { get; set; }
        public DateTime LastUpdated { get; set; }
        public LoadStatus Status { get; set; }
        public TrailerDto Trailer { get; set; }
        public LocationDto Origin { get; set; }
        public LocationDto Destination { get; set; }
        public DateTime? Appointment { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public int? CfNum { get; set; }
        public int? PuNum { get; set; }
        public bool Deleted { get; set; }
        public byte[] RowVersion { get; set; }
    }
}