
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models
{
    public class PatientCheckinOut
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public int PatientID { get; set; }
        [ForeignKey("PatientID")]
        public Patient Patient { get; set; } = null!; 

        public int? DoctorID { get; set; }
        [ForeignKey("DoctorID")]
        public Person? Doctor { get; set; } 

        public int? PatientVitalID { get; set; }
        [ForeignKey("PatientVitalID")]
        public PatientVital? PatientVital { get; set; } 

        public DateTime? CIN_TIME { get; set; } 
        public DateTime? COUT_TIME { get; set; } 

        public bool? IsServed { get; set; }
        public bool IsBilled { get; set; } = false;
    }
}
