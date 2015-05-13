using fortune_api.Enums.LoadBoard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace fortune_api.Models.LoadBoard
{
    public class Load
    {
        //Required
        [Key]
        public Guid Id { get; set; }
        [Required]
        public LoadType Type { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [Required]
        public bool Deleted { get; set; }
        
        //Not Required
        public LoadStatus Status { get; set; }
        public int TrailerId { get; set; }
        public Guid OriginId { get; set; }
        public Guid DestinationId { get; set; }
        public DateTime? Appointment { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public int? CfNum { get; set; }
        public int? PuNum { get; set; }
    }
}