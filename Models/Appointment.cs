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
        [Display(Name = "Doctor")]
        public int PersonID { get; set; }

        [ForeignKey("PersonID")]
        public Person Person { get; set; } = null!;

        [Required]
        [Display(Name = "Appointment Date & Time")]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDateTime { get; set; }
        
        [StringLength(20)]
        [Display(Name = "Appointment Status")]
        public string Status { get; set; } = "Schduled";

        [StringLength(200)]
        [Display(Name = "Remarks")]
        public string? Remarks { get; set; } 

        public PatientCheckInOut? PatientCheckInOut { get; set; }
    }
}
