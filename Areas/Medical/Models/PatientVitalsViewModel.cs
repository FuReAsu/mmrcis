using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Areas.Medical.Models
{
    public class PatientVitalsViewModel
    {
        public int ID { get; set; }
        
        [Display(Name = "Patient Visit Records")]
        public int PatientVisitRecordID { get; set; }
        
        [Display(Name = "Medical Staff")]
        public int MedicalStaffID { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "Body Temperature")]
        public decimal Temperature  { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "Pulse Rate")]
        public decimal PulseRate { get; set; }
        
        [Column(TypeName= "decimal(5,2)")]
        [Display(Name = "Respiratory Rate")]
        public decimal RespiratorRate { get; set; }

        [Column(TypeName= "decimal(5,2)")]
        [Display(Name = "Systolic Blood Pressure")]
        public decimal BloodPressureSystolic { get; set; }

        [Column(TypeName= "decimal(5,2)")]
        [Display(Name = "Diastolic Blood Pressure")]
        public decimal BloodPressureDiastolic { get; set; }
 
        [Column(TypeName= "decimal(5,2)")]
        [Display(Name = "Oxygen Saturation")]
        public decimal OxygenSaturation { get; set; }

        [Display(Name = "Notes")]
        public string? Notes { get; set; }
    }
}        
