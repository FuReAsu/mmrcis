// Models/PostingTransaction.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models // IMPORTANT: Replace with your actual project namespace
{
    public class PostingTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public DateTime PDate { get; set; } = DateTime.Now; // PDATE in SQL

        public int? AccountCode { get; set; } // Nullable, references CostRate.AccountCode
        [ForeignKey("AccountCode")] // Requires configuration in DbContext.OnModelCreating
        public CostRate? CostRate { get; set; } // Navigation property

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        public int? OperatorID { get; set; } // Nullable based on your script
        [ForeignKey("OperatorID")]
        public Person? Operator { get; set; } // Navigation property to Person (as Operator)

        public int? CheckedPersonID { get; set; } // Nullable based on your script
        [ForeignKey("CheckedPersonID")]
        public Person? CheckedPerson { get; set; } // Navigation property to Person

        public ICollection<ClinicDocument>? ClinicDocuments { get; set; }
    }
}
