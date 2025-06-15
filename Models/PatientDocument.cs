// Models/PatientDocument.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models
{
    public class PatientDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int PatientID { get; set; }
        [ForeignKey("PatientID")]
        public Patient Patient { get; set; } = null!; // Navigation property to Patient

        [Column(TypeName = "varbinary(max)")]
        public byte[] DocumentContent { get; set; } = null!; // Added default to avoid null warnings
    }
}
