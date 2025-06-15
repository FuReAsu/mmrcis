
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; 

namespace mmrcis.ViewModels
{
    public class AppointmentViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Patient is required.")]
        [Display(Name = "Patient")]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Doctor/Staff is required.")]
        [Display(Name = "Doctor / Staff")]
        public int DoctorStaffID { get; set; }

        [Required(ErrorMessage = "Service is required.")]
        [Display(Name = "Service")]
        public int ServiceID { get; set; }

        [Required(ErrorMessage = "Appointment Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Appointment Time is required.")]
        [DataType(DataType.Time)]
        [Display(Name = "Appointment Time")]
        public TimeSpan AppointmentTime { get; set; }

        [StringLength(50)]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Scheduled";

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }

        
        public IEnumerable<SelectListItem>? Patients { get; set; }
        public IEnumerable<SelectListItem>? DoctorStaffMembers { get; set; }
        public IEnumerable<SelectListItem>? Services { get; set; }
    }
}
