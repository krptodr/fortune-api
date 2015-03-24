using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace load_board_api.Models
{
    public class Value
    {
        [Key]
        public Guid Id;
        [Required]
        public string Name;
    }
}