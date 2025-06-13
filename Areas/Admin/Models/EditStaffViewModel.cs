// Areas/Admin/Models/EditStaffViewModel.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace mmrcis.Areas.Admin.Models
{
    public class EditStaffViewModel
    {
        // ApplicationUser fields (for ID and Email, Password change handled separately if complex)
        [Required]
        public string UserId { get; set; } // The Identity user ID

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        // Password change fields (optional, only if changing password)
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmNewPassword { get; set; }

        // Person fields
        [Required]
        public int PersonId { get; set; } // The associated Person ID

        [Required]
        [Display(Name = "Full Name")]
        [StringLength(255)]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string SelectedRole { get; set; } // To select from a list of roles

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

        // Property to hold available roles for the dropdown
        public List<string>? AvailableRoles { get; set; }

        // To display current roles for user if needed
        public IList<string>? CurrentRoles { get; set; }
    }
}
