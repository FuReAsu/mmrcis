// ViewModels/PatientViewModel.cs
using System;
using System.ComponentModel.DataAnnotations;
using mmrcis.Models; // Ensure your models namespace is correct

namespace mmrcis.ViewModels
{
    public class PatientViewModel
    {
        // Patient specific properties
        public int ID { get; set; } // Patient ID (will be 0 for new, existing for edit)

        [Display(Name = "Patient Status")]
        [StringLength(50)]
        public string Status { get; set; } = "Active"; // Default status

        // Person properties (for creating/editing the associated Person record)
        public int PersonID { get; set; } // Person ID (will be 0 for new, existing for edit)

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
        public DateTime? DOB { get; set; } // Assuming Person has DOB, add if not

        [StringLength(10)]
        [Display(Name = "Sex")]
        public string? Sex { get; set; }

        [StringLength(20)]
        [Display(Name = "Blood Group")]
        public string? BloodGroup { get; set; }

        // Note: PersonType, Qualification, RegisteredSince are typically managed internally for Patients/Staff,
        // not directly by the operator in a simple patient creation form.
        // If you need them editable, add them here.
    }
}
