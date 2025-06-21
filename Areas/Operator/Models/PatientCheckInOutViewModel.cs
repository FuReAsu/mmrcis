using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace mmrcis.Areas.Operator.Models
{
		public class PatientCheckInOutViewModel
		{
        [Required]
        public int ID { get; set; }
        
        [Required]
        [Display(Name = "Patient")]
        public int PatientID { get; set; }
        
        [Required]
        [Display(Name = "Appointment")]
        public int AppointmentID { get; set; }
        
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of CheckInOut")]
        public DateTime Date { get; set; }  
        
        [Display(Name = "Remarks")]
        public string? Remarks { get; set; } 
        
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Check In Time")]
        public TimeSpan CheckInTime { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Check Out Time")]
        public TimeSpan CheckOutTime { get; set; }
        
        public IEnumerable<SelectListItem> Appointments { get; set; } = new List<SelectListItem>();
        
        public IEnumerable<SelectListItem> Patients { get; set; } = new List<SelectListItem>();

		}
}
