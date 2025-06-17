using System.ComponentModel.DataAnnotations;

namespace mmrcis.Areas.Operator.Models
{
		public class PatientViewModel
		{
				public int ID { get; set; }

				public int PersonID { get; set; }

				[Required]
				[StringLength(100)]
				[Display(Name = "Full Name")]
				public string FullName { get; set; } = string.Empty;

				[StringLength(255)]
				[Display(Name = "Address")]
				public string? Address { get; set; }

				[StringLength(20)]
				[Display(Name = "Phone Number")]
				public string? PhoneNumber { get; set; }

				[StringLength(100)]
				[Display(Name = "Email")]
				public string? Email { get; set; }

				[StringLength(5)]
				[Display(Name = "BloodGroup")]
				public string? BloodGroup { get; set; }
				
				[DataType(DataType.Date)]
				[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
				public DateTime? DOB { get; set; }

				[StringLength(10)]
				[Display(Name = "Sex")]
				public string? Sex { get; set; }

				[StringLength(20)]
				[Display(Name = "Patient Status")]
				public string? Status { get; set; }
		}
}
