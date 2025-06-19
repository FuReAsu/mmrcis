using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models
{
    public class PatientVisitRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        [Required]
        [Display(Name = "Patient")]
        public int PatientID { get; set; }

        [ForeignKey("PatientID")]
        public Patient Patient { get; set; } = null!;

        [Required]
        [Display(Name = "Patient Check In Out Record")]
        public int PatientCheckInOutID { get; set; }

        [ForeignKey("PatientCheckInOutID")]
        public PatientCheckInOut PatientCheckInOut { get; set; } = null!;
    
        [Required]
        [Display(Name = "Doctor")]
        public int DoctorID { get; set; }

        [ForeignKey("DoctorID")]
        public Person Doctor { get; set; } = null!;

        [Display(Name = "Date of Visit")]
        public DateTime? DateOfVisit { get; set; }

        [Display(Name = "Diagnoses")]
        public string? Diagnoses { get; set; }

        [Display(Name = "Prescriptions")]
        public string? Prescriptions { get; set; }

        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }
        
        public PatientVitals? PatientVitals { get; set; }
    }
}
