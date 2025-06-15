
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace mmrcis.Models
{
    public class PatientVital
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? BP { get; set; } 

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Oximeter { get; set; } 

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Weight { get; set; } 

        [Column(TypeName = "decimal(5,2)")]
        public decimal? RespRate { get; set; } 

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Height { get; set; } 

        public DateTime DateTime { get; set; } = DateTime.Now;

        public int? OperatorID { get; set; } 
        [ForeignKey("OperatorID")]
        public Person? Operator { get; set; } 

        
        public int PatientID { get; set; }
        [ForeignKey("PatientID")]
        public Patient Patient { get; set; } = null!; 

        
        
    }
}
