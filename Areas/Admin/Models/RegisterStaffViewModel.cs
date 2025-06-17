using System.ComponentModel.DataAnnotations;

namespace mmrcis.Areas.Admin.Models
{
    public class RegisterStaffViewModel
    {
        
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        
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
    }
}
