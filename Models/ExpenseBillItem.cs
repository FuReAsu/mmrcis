// Models/ExpenseBillItem.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models // IMPORTANT: Replace with your actual project namespace
{
    public class ExpenseBillItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ExpenseBillID { get; set; }
        [ForeignKey("ExpenseBillID")]
        public ExpenseBill ExpenseBill { get; set; } // Navigation property

        public int CostCode { get; set; } // References CostRate.CostCode, not CostRate.ID
        [ForeignKey("CostCode")] // This will require specific configuration in DbContext.OnModelCreating
        public CostRate CostRate { get; set; } // Navigation property

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
    }
}
