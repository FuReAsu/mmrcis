using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models
{
    public class PatientVitals 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        [Required]
        [Display(Name = "Patient Visit Records")]
        public int PatientVisitRecordID { get; set; }

        [ForeignKey("PatientVisitRecordID")]
        public PatientVisitRecord PatientVisitRecord { get; set; } = null!;
        
        [Required]
        [Display(Name = "Medical Staff")]
        public int MedicalStaffID { get; set; }

        [ForeignKey("MedicalStaffID")]
        public Person MedicalStaff { get; set; } = null!;

        [Column(TypeName = "decimal(3,2)")]
        [Display(Name = "Body Temperature")]
        public decimal Temperature  { get; set; }
        
        [Column(TypeName = "decimal(3,2)")]
        [Display(Name = "Pulse Rate")]
        public decimal PulseRate { get; set; }
        
        [Column(TypeName= "decimal(3,2)")]
        [Display(Name = "Respiratory Rate")]
        public decimal RespiratorRate { get; set; }

        [Column(TypeName= "decimal(3,2)")]
        [Display(Name = "Systolic Blood Pressure")]
        public decimal BloodPressureSystolic { get; set; }

        [Column(TypeName= "decimal(3,2)")]
        [Display(Name = "Diastolic Blood Pressure")]
        public decimal BloodPressureDiastolic { get; set; }
 
        [Column(TypeName= "decimal(3,2)")]
        [Display(Name = "Oxygen Saturation")]
        public decimal OxygenSaturation { get; set; }

        [Display(Name = "Notes")]
        public string? Notes { get; set; }
    }
}        
