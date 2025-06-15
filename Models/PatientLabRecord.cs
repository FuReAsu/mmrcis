// Models/PatientLabRecord.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models // IMPORTANT: Replace with your actual project namespace
{
    public class PatientLabRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        // Doctor who ordered/reviewed the lab record
        public int DoctorID { get; set; }
        [ForeignKey("DoctorID")]
        public Person Doctor { get; set; } = null!; // Navigation property to Person (as Doctor)

        // Operator who registered/collected the lab record
        public int? OperatorID { get; set; } // Nullable if not every lab record has an associated operator
        [ForeignKey("OperatorID")]
        public Person? Operator { get; set; } // Navigation property to Person (as Operator)

        public int? IncomeBillID { get; set; }
        [ForeignKey("IncomeBillID")]
        public IncomeBill IncomeBill { get; set; } = null!; // Navigation property

        // Link to the Patient model
        public int PatientID { get; set; }
        [ForeignKey("PatientID")]
        public Patient Patient { get; set; } = null!; // Navigation property to Patient

        public bool IsCollected { get; set; } = false;
    }
}
