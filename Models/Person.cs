using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string PersonType { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string FullName { get; set; } = null!;

        [StringLength(255)]
        public string? Qualification { get; set; } 

        [StringLength(255)]
        public string? Specialization { get; set; } 

        [Required]
        [StringLength(255)]
        public string Address { get; set; } = null!; 

        public DateTime RegisteredSince { get; set; } = DateTime.Now; 
        
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; } 

        public int? Age { get; set; }

        [StringLength(16)]
        public string? Sex { get; set; }

        [StringLength(8)]
        public string? BloodGroup { get; set; }

        [StringLength(255)]
        public string? Allergy { get; set; }

        [StringLength(255)]
        public string? FatherName { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string? Email { get; set; } 

        public Patient? PatientProfile { get; set; }
    }
}
