// Areas/Admin/Models/StaffListViewModel.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace mmrcis.Areas.Admin.Models
{
    public class StaffListViewModel
    {
        public string UserId { get; set; } // The Identity user ID
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Full Name")]
        public string? FullName { get; set; } // From the Person model

        [Display(Name = "Person Type")]
        public string? PersonType { get; set; } // From the Person model

        [Display(Name = "Roles")]
        public string? Roles { get; set; } // Combined string of roles

        [Display(Name = "Registered Since")]
        [DataType(DataType.Date)]
        public DateTime RegisteredSince { get; set; } // From the Person model
    }
}
