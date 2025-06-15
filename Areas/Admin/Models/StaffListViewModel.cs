
using System;
using System.ComponentModel.DataAnnotations;

namespace mmrcis.Areas.Admin.Models
{
    public class StaffListViewModel
    {
        public string UserId { get; set; } 
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Full Name")]
        public string? FullName { get; set; } 

        [Display(Name = "Person Type")]
        public string? PersonType { get; set; } 

        [Display(Name = "Roles")]
        public string? Roles { get; set; } 

        [Display(Name = "Registered Since")]
        [DataType(DataType.Date)]
        public DateTime RegisteredSince { get; set; } 
    }
}
