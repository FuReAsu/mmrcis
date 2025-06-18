using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mmrcis.Areas.Operator.Models
{
		public class AppointmentViewModel
		{
				public int ID { get; set; }

				[Required]
				[Display(Name = "Patient")]
				public int PatientID { get; set; }

				[Required]
				[Display(Name = "Doctor")]
				public int DoctorID { get; set; }

				[Required]
        [DataType(DataType.Date)]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Appointment Time")]
        public TimeSpan AppointmentTime { get; set; }
				
				[StringLength(20)]
        [Display(Name = "Appointment Status")]
        public string Status { get; set; } = "Schduled";

        [StringLength(200)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; } = "";

        public IEnumerable<SelectListItem>? Patients { get; set; }
        public IEnumerable<SelectListItem>? Doctors { get; set; }
		}
}
