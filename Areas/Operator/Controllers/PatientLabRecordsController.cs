// Areas/Operator/Controllers/PatientLabRecordsController.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // Needed for UserManager
using mmrcis.Data;
using mmrcis.Models;
using mmrcis.ViewModels;
using Microsoft.AspNetCore.Authorization; // If you plan to secure actions
using Microsoft.AspNetCore.Mvc.Rendering; // Needed for SelectList

namespace mmrcis.Areas.Operator.Controllers // IMPORTANT: Ensure this matches your Area's namespace structure
{
    [Area("Operator")] // Specifies this controller belongs to the "Operator" area
    [Authorize(Roles = "Operator,Admin")] // Optional: Restrict access to users with "Operator" role
    public class PatientLabRecordsController : Controller
    {
        private readonly CisDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientLabRecordsController(CisDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Operator/PatientLabRecords
        // This action will display a list of all patient lab records.
        public async Task<IActionResult> Index()
        {
            // Eager load related data: Patient (and its Person), Doctor, Operator, IncomeBill
            // This avoids N+1 query problems in the view.
            var labRecords = await _context.PatientLabRecords
                                            .Include(plr => plr.Patient)
                                                .ThenInclude(p => p.Person) // Patient's associated Person (for FullName, etc.)
                                            .Include(plr => plr.Doctor)      // Doctor is a Person
                                            .Include(plr => plr.Operator)    // Operator is a Person
                                            .Include(plr => plr.IncomeBill)  // The associated IncomeBill
                                            .OrderByDescending(plr => plr.DateTime)
                                            .ToListAsync();

            return View(labRecords);
        }
        public async Task<IActionResult> Create()
        {
            // Populate dropdowns for Patient and Doctor
            ViewBag.PatientID = new SelectList(
                await _context.Patients.Include(p => p.Person).ToListAsync(),
                "ID", "Person.FullName"
            );

            ViewBag.DoctorID = new SelectList(
                await _context.Persons.Where(p => p.PersonType == "Doctor").ToListAsync(),
                "ID", "FullName"
            );

            // Populate dropdown for existing IncomeBills (now optional)
            ViewBag.ExistingIncomeBills = new SelectList(
                await _context.IncomeBills
                              .Include(ib => ib.Patient)
                                  .ThenInclude(p => p.Person)
                              .Select(ib => new {
                                  ID = ib.ID,
                                  // Display the Patient's name along with the Bill ID and Date
                                  DisplayText = $"Bill {ib.ID} - {ib.Patient.Person.FullName} ({ib.DateTime.ToShortDateString()})"
                              })
                              .ToListAsync(),
                "ID", "DisplayText"
            );

            return View(new PatientLabRecordCreateEditViewModel()); // Pass an empty ViewModel to the view
        }

        // POST: Operator/PatientLabRecords/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientLabRecordCreateEditViewModel viewModel)
        {
            // IMPORTANT: If you have required fields for IncomeBillItems,
            // or more complex validation, add it here or in the ViewModel.

            // Get the current logged-in operator's PersonID
            var currentUser = await _userManager.GetUserAsync(User);
            var operatorPerson = await _context.Persons
                                            .FirstOrDefaultAsync(p => p.ID == currentUser.PersonID && (p.PersonType == "Operator" || p.PersonType == "Admin"));

            if (operatorPerson == null)
            {
                ModelState.AddModelError(string.Empty, "Error: Current user is not a valid operator.");
                // Re-populate dropdowns if model state is invalid before returning view
                await PopulateCreateEditDropdowns(viewModel);
                return View(viewModel);
            }

            // Manually add model errors if needed for conditional validation
            if (!viewModel.CreateNewIncomeBill && !viewModel.SelectedIncomeBillID.HasValue)
            {
                ModelState.AddModelError(string.Empty, "Please either select an existing bill or choose to create a new one.");
            }

            if (ModelState.IsValid)
            {
                IncomeBill? linkedIncomeBill = null;

                if (viewModel.CreateNewIncomeBill)
                {
                    // Create a new basic IncomeBill
                    linkedIncomeBill = new IncomeBill
                    {
                        DateTime = DateTime.Now,
                        PatientID = viewModel.PatientID, // Link to the selected patient
                        TotalAmount = viewModel.NewIncomeBillTotalAmount, // Set from ViewModel
                        IsPosted = false, // Default for new bill
                        IsVoided = false, // Default for new bill
                        OperatorID = operatorPerson.ID // Set the current operator as creator
                    };
                    _context.Add(linkedIncomeBill);
                    await _context.SaveChangesAsync(); // Save to get the new IncomeBill ID
                }
                else if (viewModel.SelectedIncomeBillID.HasValue)
                {
                    // Use an existing IncomeBill
                    linkedIncomeBill = await _context.IncomeBills.FindAsync(viewModel.SelectedIncomeBillID.Value);
                    if (linkedIncomeBill == null)
                    {
                        ModelState.AddModelError(string.Empty, "Selected Income Bill not found.");
                        await PopulateCreateEditDropdowns(viewModel);
                        return View(viewModel);
                    }
                }

                // Now create the PatientLabRecord
                var patientLabRecord = new PatientLabRecord
                {
                    PatientID = viewModel.PatientID,
                    DoctorID = viewModel.DoctorID,
                    DateTime = DateTime.Now, // Set current time
                    IsCollected = viewModel.IsCollected,
                    OperatorID = operatorPerson.ID, // Link to the current operator
                    IncomeBillID = linkedIncomeBill?.ID // Link to the newly created or selected IncomeBill
                };

                _context.Add(patientLabRecord);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, re-populate dropdowns and return the view with errors
            await PopulateCreateEditDropdowns(viewModel);
            return View(viewModel);
        }

        // Helper method to populate ViewBag data for Create/Edit forms
        private async Task PopulateCreateEditDropdowns(PatientLabRecordCreateEditViewModel viewModel)
        {
            ViewBag.PatientID = new SelectList(
                await _context.Patients.Include(p => p.Person).ToListAsync(),
                "ID", "Person.FullName", viewModel.PatientID
            );

            ViewBag.DoctorID = new SelectList(
                await _context.Persons.Where(p => p.PersonType == "Doctor").ToListAsync(),
                "ID", "FullName", viewModel.DoctorID
            );

            ViewBag.ExistingIncomeBills = new SelectList(
                await _context.IncomeBills
                              .Include(ib => ib.Patient)
                                  .ThenInclude(p => p.Person)
                              .Select(ib => new {
                                  ID = ib.ID,
                                  DisplayText = $"Bill {ib.ID} - {ib.Patient.Person.FullName} ({ib.DateTime.ToShortDateString()})"
                              })
                              .ToListAsync(),
                "ID", "DisplayText", viewModel.SelectedIncomeBillID
            );
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientLabRecord = await _context.PatientLabRecords
                                                 .Include(plr => plr.Patient)
                                                     .ThenInclude(p => p.Person)
                                                 .Include(plr => plr.Doctor)
                                                 .Include(plr => plr.Operator)
                                                 .Include(plr => plr.IncomeBill)
                                                 .FirstOrDefaultAsync(m => m.ID == id);

            if (patientLabRecord == null)
            {
                return NotFound();
            }

            // Map the model to the ViewModel for editing
            var viewModel = PatientLabRecordCreateEditViewModel.FromPatientLabRecord(patientLabRecord);

            // Populate dropdowns for the view
            await PopulateCreateEditDropdowns(viewModel);

            return View(viewModel);
        }

        // POST: Operator/PatientLabRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientLabRecordCreateEditViewModel viewModel)
        {
            if (id != viewModel.ID)
            {
                return NotFound();
            }

            // Get the existing PatientLabRecord from the database
            var patientLabRecordToUpdate = await _context.PatientLabRecords.FindAsync(id);
            if (patientLabRecordToUpdate == null)
            {
                return NotFound();
            }

            // Get the current logged-in operator's PersonID (for audit, though we don't save it here)
            var currentUser = await _userManager.GetUserAsync(User);
            var operatorPerson = await _context.Persons
                                            .FirstOrDefaultAsync(p => p.ID == currentUser.PersonID && p.PersonType == "Operator");

            if (operatorPerson == null)
            {
                ModelState.AddModelError(string.Empty, "Error: Current user is not a valid operator.");
                await PopulateCreateEditDropdowns(viewModel);
                return View(viewModel);
            }

            // Manually add model errors if needed for conditional validation
            if (!viewModel.CreateNewIncomeBill && !viewModel.SelectedIncomeBillID.HasValue)
            {
                ModelState.AddModelError(string.Empty, "Please either select an existing bill or choose to create a new one.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    IncomeBill? linkedIncomeBill = null;

                    if (viewModel.CreateNewIncomeBill)
                    {
                        // Create a new basic IncomeBill if chosen
                        linkedIncomeBill = new IncomeBill
                        {
                            DateTime = DateTime.Now,
                            PatientID = viewModel.PatientID, // Link to the selected patient
                            TotalAmount = viewModel.NewIncomeBillTotalAmount,
                            IsPosted = false,
                            IsVoided = false,
                            OperatorID = operatorPerson.ID // Set the current operator as creator
                        };
                        _context.Add(linkedIncomeBill);
                        await _context.SaveChangesAsync(); // Save to get the new IncomeBill ID
                    }
                    else if (viewModel.SelectedIncomeBillID.HasValue)
                    {
                        // Use an existing IncomeBill
                        linkedIncomeBill = await _context.IncomeBills.FindAsync(viewModel.SelectedIncomeBillID.Value);
                        if (linkedIncomeBill == null)
                        {
                            ModelState.AddModelError(string.Empty, "Selected Income Bill not found.");
                            await PopulateCreateEditDropdowns(viewModel);
                            return View(viewModel);
                        }
                    }

                    // Update PatientLabRecord properties from ViewModel
                    patientLabRecordToUpdate.PatientID = viewModel.PatientID;
                    patientLabRecordToUpdate.DoctorID = viewModel.DoctorID;
                    patientLabRecordToUpdate.IsCollected = viewModel.IsCollected;
                    patientLabRecordToUpdate.IncomeBillID = linkedIncomeBill?.ID; // Update bill linkage

                    _context.Update(patientLabRecordToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientLabRecordExists(viewModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            // If ModelState is not valid, re-populate dropdowns and return the view with errors
            await PopulateCreateEditDropdowns(viewModel);
            return View(viewModel);
        }

        // GET: Operator/PatientLabRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Eager load all related data for display
            var patientLabRecord = await _context.PatientLabRecords
                                                 .Include(plr => plr.Patient)
                                                     .ThenInclude(p => p.Person)
                                                 .Include(plr => plr.Doctor)
                                                 .Include(plr => plr.Operator)
                                                 .Include(plr => plr.IncomeBill)
                                                 .FirstOrDefaultAsync(m => m.ID == id);

            if (patientLabRecord == null)
            {
                return NotFound();
            }

            return View(patientLabRecord);
        }

        // GET: Operator/PatientLabRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Eager load all related data for confirmation display
            var patientLabRecord = await _context.PatientLabRecords
                                                 .Include(plr => plr.Patient)
                                                     .ThenInclude(p => p.Person)
                                                 .Include(plr => plr.Doctor)
                                                 .Include(plr => plr.Operator)
                                                 .Include(plr => plr.IncomeBill)
                                                 .FirstOrDefaultAsync(m => m.ID == id);

            if (patientLabRecord == null)
            {
                return NotFound();
            }

            return View(patientLabRecord);
        }

        // POST: Operator/PatientLabRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientLabRecord = await _context.PatientLabRecords.FindAsync(id);
            if (patientLabRecord != null)
            {
                _context.PatientLabRecords.Remove(patientLabRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientLabRecordExists(int id)
        {
            return _context.PatientLabRecords.Any(e => e.ID == id);
        }
    }
}

