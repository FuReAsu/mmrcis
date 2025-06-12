// Models/ClinicDocument.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models // IMPORTANT: Replace with your actual project namespace
{
    public class ClinicDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? TransactionID { get; set; } // Nullable based on your script
        [ForeignKey("TransactionID")]
        public PostingTransaction? PostingTransaction { get; set; } // Navigation property

        [Column(TypeName = "varbinary(max)")]
        public byte[] DocumentContent { get; set; } // VARBINARY(MAX) maps to byte[]
    }
}
