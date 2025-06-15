
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models 
{
    public class PostingTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public DateTime PDate { get; set; } = DateTime.Now; 

        public int? AccountCode { get; set; } 
        [ForeignKey("AccountCode")] 
        public CostRate? CostRate { get; set; } 

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        public int? OperatorID { get; set; } 
        [ForeignKey("OperatorID")]
        public Person? Operator { get; set; } 

        public int? CheckedPersonID { get; set; } 
        [ForeignKey("CheckedPersonID")]
        public Person? CheckedPerson { get; set; } 

        public ICollection<ClinicDocument>? ClinicDocuments { get; set; }
    }
}
