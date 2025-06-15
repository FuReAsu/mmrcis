
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace mmrcis.Areas.Admin.Models
{
    public class EditStaffViewModel
    {
        
        [Required]
        public string UserId { get; set; } 

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmNewPassword { get; set; }

        
        [Required]
        public int PersonId { get; set; } 

        [Required]
        [Display(Name = "Full Name")]
        [StringLength(255)]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string SelectedRole { get; set; } 

        [Display(Name = "Qualification")]
        [StringLength(255)]
        public string? Qualification { get; set; }

        [Display(Name = "Specialization")]
        [StringLength(255)]
        public string? Specialization { get; set; }

        [Display(Name = "Address")]
        [StringLength(255)]
        public string? Address { get; set; }

        [Display(Name = "Phone Number")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        
        public List<string>? AvailableRoles { get; set; }

        
        public IList<string>? CurrentRoles { get; set; }
    }
}
