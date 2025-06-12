// Models/CostRate.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models // IMPORTANT: Replace with your actual project namespace
{
    public class CostRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "int")] // Ensure int type for CostCode
        public int CostCode { get; set; } // Marked as unique in DbContext

        [Column(TypeName = "decimal(10,2)")]
        public decimal CostAmount { get; set; }

        [StringLength(12)]
        public string CostType { get; set; } // e.g., "Medical", "Admin"

        [StringLength(4)]
        public string IorE { get; set; } // Income or Expense (In/Exp)

        public int? AccountCode { get; set; } // Nullable, marked as unique in DbContext

        public DateTime RegisteredSince { get; set; } = DateTime.Now;

        // Navigation property for related bill items
        public ICollection<IncomeBillItem>? IncomeBillItems { get; set; }
        public ICollection<ExpenseBillItem>? ExpenseBillItems { get; set; }
        public ICollection<PostingTransaction>? PostingTransactions { get; set; }
    }
}
