
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models 
{
    public class IncomeBillItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int IncomeBillID { get; set; }
        [ForeignKey("IncomeBillID")]
        public IncomeBill IncomeBill { get; set; } 

        public int CostCode { get; set; } 
        [ForeignKey("CostCode")] 
        public CostRate CostRate { get; set; } 

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
    }
}
