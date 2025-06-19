using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mmrcis.Models
{
    public class PatientCheckInOut
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
        [Display(Name = "Appointment")]
        public int AppointmentID { get; set; }

        [ForeignKey("AppointmentID")]
        public Appointment Appointment { get; set; } = null!;

        [Required]
        [Display(Name = "Date of CheckInOut")]
        public DateTime Date { get; set; }  

        [Required]
        [Display(Name = "Check In Time")]
        public DateTime CheckInTime { get; set; }

        [Display(Name = "Check Out Time")]
        public DateTime? CheckOutTime { get; set; }

        [Display(Name = "Remarks")]
        public string? Remarks { get; set; } 
        
        public IncomeBill? IncomeBill { get; set; } 

        public PatientVisitRecord? PatientVisitRecord { get; set; }
    }
}
