
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models 
{
    public class PatientLabRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        
        public int DoctorID { get; set; }
        [ForeignKey("DoctorID")]
        public Person Doctor { get; set; } = null!; 

        
        public int? OperatorID { get; set; } 
        [ForeignKey("OperatorID")]
        public Person? Operator { get; set; } 

        public int? IncomeBillID { get; set; }
        [ForeignKey("IncomeBillID")]
        public IncomeBill IncomeBill { get; set; } = null!; 

        
        public int PatientID { get; set; }
        [ForeignKey("PatientID")]
        public Patient Patient { get; set; } = null!; 

        public bool IsCollected { get; set; } = false;
    }
}
