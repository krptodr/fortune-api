using load_board_api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace load_board_api.Models
{
    public class Load
    {
        //Required
        [Key]
        public Guid Id { get; set; }
        [Required]
        public LoadType Type { get; set; }
        [Required]
        public DateTime? LastUpdated { get; set; }
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