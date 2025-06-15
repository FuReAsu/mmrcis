
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; } = null!; 

        [Display(Name = "Reference No.")]
        [StringLength(100)]
        public string? ReferenceNo { get; set; } 

        
        public int IncomeBillID { get; set; }
        [ForeignKey("IncomeBillID")]
        public IncomeBill IncomeBill { get; set; } = null!; 

        
        public int ReceivedByOperatorID { get; set; }
        [ForeignKey("ReceivedByOperatorID")]
        public Person ReceivedByOperator { get; set; } = null!;
    }
}
