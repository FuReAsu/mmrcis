// Models/IncomeBillItem.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models // IMPORTANT: Replace with your actual project namespace
{
    public class IncomeBillItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int IncomeBillID { get; set; }
        [ForeignKey("IncomeBillID")]
        public IncomeBill IncomeBill { get; set; } // Navigation property

        public int CostCode { get; set; } // References CostRate.CostCode, not CostRate.ID
        [ForeignKey("CostCode")] // This will require specific configuration in DbContext.OnModelCreating
        public CostRate CostRate { get; set; } // Navigation property

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
    }
}
