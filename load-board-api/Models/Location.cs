using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace load_board_api.Models
{
    public class Location
    {
        [Key]
        public Guid Id;
        [Required]
        public string Name;
        [Required, DefaultValue(false)]
        public bool Deleted;
        [Required, Column(TypeName = "datetime2")]
        public DateTime LastUpdated;
    }
}