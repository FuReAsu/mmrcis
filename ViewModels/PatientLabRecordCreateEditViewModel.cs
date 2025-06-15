// ViewModels/PatientLabRecordCreateEditViewModel.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mmrcis.Models; // Make sure to use your Models namespace

namespace mmrcis.ViewModels // IMPORTANT: Replace with your actual project namespace
{
    public class PatientLabRecordCreateEditViewModel
    {
        // --- Properties for PatientLabRecord ---
        public int ID { get; set; } // For editing existing records

        [Required(ErrorMessage = "Patient is required.")]
        [Display(Name = "Patient")]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Doctor is required.")]
        [Display(Name = "Ordering Doctor")]
        public int DoctorID { get; set; }

        [Display(Name = "Lab Sample Collected?")]
        public bool IsCollected { get; set; }

        // --- Properties for handling Income Bill Linkage ---
        [Display(Name = "Link to Existing Bill")]
        public int? SelectedIncomeBillID { get; set; } // Nullable, for selecting an existing bill

        [Display(Name = "Create New Bill for this Record")]
        public bool CreateNewIncomeBill { get; set; } // Checkbox to choose between new/existing

        // --- Properties for New Income Bill (only if CreateNewIncomeBill is true) ---
        // For simplicity, we'll only ask for Patient ID here, TotalAmount will be 0 initially
        // You can expand this to include IncomeBillItems in a more complex scenario
        // but let's start simple.

        // These properties are required ONLY if CreateNewIncomeBill is true.
        // We'll handle this with custom validation or logic in the controller.
        [Display(Name = "Bill Total (Optional for New Bill)")]
        [DataType(DataType.Currency)]
        public decimal NewIncomeBillTotalAmount { get; set; } = 0.00m;


        // Constructor to initialize lists if needed
        public PatientLabRecordCreateEditViewModel()
        {
            // You might initialize other collections here if your ViewModel becomes more complex
        }

        // Method to map from Model to ViewModel (for Edit scenario)
        public static PatientLabRecordCreateEditViewModel FromPatientLabRecord(PatientLabRecord record)
        {
            return new PatientLabRecordCreateEditViewModel
            {
                ID = record.ID,
                PatientID = record.PatientID,
                DoctorID = record.DoctorID,
                IsCollected = record.IsCollected,
                SelectedIncomeBillID = record.IncomeBillID, // Pre-select existing bill
                CreateNewIncomeBill = false // Assume not creating new when editing existing
            };
        }
    }
}
