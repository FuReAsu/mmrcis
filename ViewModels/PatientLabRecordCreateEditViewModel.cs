
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mmrcis.Models; 

namespace mmrcis.ViewModels 
{
    public class PatientLabRecordCreateEditViewModel
    {
        
        public int ID { get; set; } 

        [Required(ErrorMessage = "Patient is required.")]
        [Display(Name = "Patient")]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Doctor is required.")]
        [Display(Name = "Ordering Doctor")]
        public int DoctorID { get; set; }

        [Display(Name = "Lab Sample Collected?")]
        public bool IsCollected { get; set; }

        
        [Display(Name = "Link to Existing Bill")]
        public int? SelectedIncomeBillID { get; set; } 

        [Display(Name = "Create New Bill for this Record")]
        public bool CreateNewIncomeBill { get; set; } 

        
        
        
        

        
        
        [Display(Name = "Bill Total (Optional for New Bill)")]
        [DataType(DataType.Currency)]
        public decimal NewIncomeBillTotalAmount { get; set; } = 0.00m;


        
        public PatientLabRecordCreateEditViewModel()
        {
            
        }

        
        public static PatientLabRecordCreateEditViewModel FromPatientLabRecord(PatientLabRecord record)
        {
            return new PatientLabRecordCreateEditViewModel
            {
                ID = record.ID,
                PatientID = record.PatientID,
                DoctorID = record.DoctorID,
                IsCollected = record.IsCollected,
                SelectedIncomeBillID = record.IncomeBillID, 
                CreateNewIncomeBill = false 
            };
        }
    }
}
