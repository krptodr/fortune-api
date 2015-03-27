using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace load_board_api.Models
{
    public class Trailer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }
        [Required, DefaultValue(false)]
        public bool Deleted { get; set; }
        [Required, Column(TypeName="datetime2")]
        public DateTime LastUpdated { get; set; }
    }
}