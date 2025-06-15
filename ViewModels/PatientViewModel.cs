
using System;
using System.ComponentModel.DataAnnotations;
using mmrcis.Models; 

namespace mmrcis.ViewModels
{
    public class PatientViewModel
    {
        
        public int ID { get; set; } 

        [Display(Name = "Patient Status")]
        [StringLength(50)]
        public string Status { get; set; } = "Active"; 

        
        public int PersonID { get; set; } 

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [StringLength(255)]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; } 

        [StringLength(10)]
        [Display(Name = "Sex")]
        public string? Sex { get; set; }

        [StringLength(20)]
        [Display(Name = "Blood Group")]
        public string? BloodGroup { get; set; }

        
        
        
    }
}
