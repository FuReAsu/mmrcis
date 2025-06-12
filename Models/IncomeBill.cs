// Models/IncomeBill.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models // IMPORTANT: Replace with your actual project namespace
{
    public class IncomeBill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public int? PatientID { get; set; } // Nullable if bill might not be directly patient-related
        [ForeignKey("PatientID")]
        public Person? Patient { get; set; } // Navigation property to Person (as Patient)

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        public int OperatorID { get; set; }
        [ForeignKey("OperatorID")]
        public Person Operator { get; set; } // Navigation property to Person (as Operator)

        public bool IsPosted { get; set; } = false; // Corrected default to false (0)
        public bool IsVoided { get; set; } = false; // Corrected default to false (0)

        public DateTime? PostedDate { get; set; }

        // Navigation property for bill items
        public ICollection<IncomeBillItem>? IncomeBillItems { get; set; }
        public ICollection<PatientLabRecord>? PatientLabRecords { get; set; } // Added for consistency
    }
}
