// Models/PatientDocument.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models // IMPORTANT: Replace with your actual project namespace
{
    public class PatientDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int PatientID { get; set; }
        [ForeignKey("PatientID")]
        public Person Patient { get; set; } // Navigation property to Person (as Patient)

        [Column(TypeName = "varbinary(max)")]
        public byte[] DocumentContent { get; set; } // VARBINARY(MAX) maps to byte[]
    }
}
