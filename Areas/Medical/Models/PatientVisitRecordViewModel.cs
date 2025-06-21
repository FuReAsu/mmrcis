using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mmrcis.Areas.Medical.Models
{
		public class PatientVisitRecordViewModel
		{
				[Required]
        public int ID { get; set; }
        
        [Required]
        [Display(Name = "Patient")]
        public int PatientID { get; set; }

        [Required]
        [Display(Name = "Patient Check In Out Record")]
        public int PatientCheckInOutID { get; set; }
        
				[Required]
        [Display(Name = "Doctor")]
        public int DoctorID { get; set; }

        [Display(Name = "Date of Visit")]
        public DateTime? DateOfVisit { get; set; }

        [Display(Name = "Diagnoses")]
        public string? Diagnoses { get; set; }

        [Display(Name = "Prescriptions")]
        public string? Prescriptions { get; set; }

        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }

        public IEnumerable<SelectListItem> PatientCheckInOutRecords { get; set; } = new List<SelectListItem>();

		}
}
