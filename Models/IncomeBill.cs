
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models
{
    public class IncomeBill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public int? PatientID { get; set; } 
        [ForeignKey("PatientID")]
        public Patient? Patient { get; set; } 

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        public int OperatorID { get; set; }
        [ForeignKey("OperatorID")]
        public Person Operator { get; set; } = null!; 

        public bool IsPosted { get; set; } = false;
        public bool IsVoided { get; set; } = false;

        public DateTime? PostedDate { get; set; }

        
        public ICollection<IncomeBillItem>? IncomeBillItems { get; set; }
        public ICollection<PatientLabRecord>? PatientLabRecords { get; set; } 
        public ICollection<Payment>? Payments { get; set; } 
    }
}
