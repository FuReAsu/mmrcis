
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models 
{
    public class CostRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "int")] 
        public int CostCode { get; set; } 

        [Column(TypeName = "decimal(10,2)")]
        public decimal CostAmount { get; set; }

        [StringLength(12)]
        public string CostType { get; set; } 

        [StringLength(4)]
        public string IorE { get; set; } 

        public int? AccountCode { get; set; } 

        public DateTime RegisteredSince { get; set; } = DateTime.Now;

        
        public ICollection<IncomeBillItem>? IncomeBillItems { get; set; }
        public ICollection<ExpenseBillItem>? ExpenseBillItems { get; set; }
        public ICollection<PostingTransaction>? PostingTransactions { get; set; }
    }
}
