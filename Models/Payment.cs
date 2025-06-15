// Models/Payment.cs
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
        public string PaymentMethod { get; set; } = null!; // e.g., "Cash", "Card", "Bank Transfer"

        [Display(Name = "Reference No.")]
        [StringLength(100)]
        public string? ReferenceNo { get; set; } // e.g., transaction ID, check number

        // Foreign Key to IncomeBill
        public int IncomeBillID { get; set; }
        [ForeignKey("IncomeBillID")]
        public IncomeBill IncomeBill { get; set; } = null!; // The bill this payment is for

        // Foreign Key to the Person (Operator) who received the payment
        public int ReceivedByOperatorID { get; set; }
        [ForeignKey("ReceivedByOperatorID")]
        public Person ReceivedByOperator { get; set; } = null!;
    }
}
