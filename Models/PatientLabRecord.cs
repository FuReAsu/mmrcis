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

        public int DoctorID { get; set; }
        [ForeignKey("DoctorID")]
        public Person Doctor { get; set; } // Navigation property to Person (as Doctor)

        public int IncomeBillID { get; set; }
        [ForeignKey("IncomeBillID")]
        public IncomeBill IncomeBill { get; set; } // Navigation property

        public bool IsCollected { get; set; } = false;
    }
}
