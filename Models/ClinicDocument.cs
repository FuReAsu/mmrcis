
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models 
{
    public class ClinicDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? TransactionID { get; set; } 
        [ForeignKey("TransactionID")]
        public PostingTransaction? PostingTransaction { get; set; } 

        [Column(TypeName = "varbinary(max)")]
        public byte[] DocumentContent { get; set; } 
    }
}
