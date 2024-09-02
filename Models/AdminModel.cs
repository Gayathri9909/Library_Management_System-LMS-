using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace library.Models
{
    public class AdminModel
    {
        [Required]
        [Display(Name = "Admin ID")]
        public int AdminId { get; set; }

        [Required]
        [Display(Name = "Admin Name")]
        public string AdminName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string AdminPassword { get; set; }
    }
}