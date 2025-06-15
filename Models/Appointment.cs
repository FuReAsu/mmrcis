
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models 
{
    public class Appointment
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
        [Display(Name = "Doctor / Staff")]
        public int DoctorStaffID { get; set; } 
        [ForeignKey("DoctorStaffID")]
        public Person DoctorStaff { get; set; } = null!; 

        
        [Required]
        [Display(Name = "Service")]
        public int ServiceID { get; set; }
        [ForeignKey("ServiceID")]
        public Service Service { get; set; } = null!; 
        
        [Required]
        [Display(Name = "Appointment Date & Time")]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDateTime { get; set; }

        [StringLength(50)]
        [Display(Name = "Status")] 
        public string Status { get; set; } = "Scheduled"; 

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }

        [Display(Name = "Booked At")]
        public DateTime BookedAt { get; set; } = DateTime.Now;

        [Display(Name = "Last Updated")]
        public DateTime? LastUpdated { get; set; }
        
    }
}
