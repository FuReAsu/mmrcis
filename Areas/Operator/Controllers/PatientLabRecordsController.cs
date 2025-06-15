
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; 
using mmrcis.Data;
using mmrcis.Models;
using mmrcis.ViewModels;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc.Rendering; 

namespace mmrcis.Areas.Operator.Controllers 
{
    [Area("Operator")] 
    [Authorize(Roles = "Operator,Admin")] 
    public class PatientLabRecordsController : Controller
    {
        private readonly CisDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientLabRecordsController(CisDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        
        
        public async Task<IActionResult> Index()
        {
            
            
            var labRecords = await _context.PatientLabRecords
                                            .Include(plr => plr.Patient)
                                                .ThenInclude(p => p.Person) 
                                            .Include(plr => plr.Doctor)      
                                            .Include(plr => plr.Operator)    
                                            .Include(plr => plr.IncomeBill)  
                                            .OrderByDescending(plr => plr.DateTime)
                                            .ToListAsync();

            return View(labRecords);
        }
        public async Task<IActionResult> Create()
        {
            
            ViewBag.PatientID = new SelectList(
                await _context.Patients.Include(p => p.Person).ToListAsync(),
                "ID", "Person.FullName"
            );

            ViewBag.DoctorID = new SelectList(
                await _context.Persons.Where(p => p.PersonType == "Doctor").ToListAsync(),
                "ID", "FullName"
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
                "ID", "DisplayText"
            );

            return View(new PatientLabRecordCreateEditViewModel()); 
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientLabRecordCreateEditViewModel viewModel)
        {
            
            

            
            var currentUser = await _userManager.GetUserAsync(User);
            var operatorPerson = await _context.Persons
                                            .FirstOrDefaultAsync(p => p.ID == currentUser.PersonID && (p.PersonType == "Operator" || p.PersonType == "Admin"));

            if (operatorPerson == null)
            {
                ModelState.AddModelError(string.Empty, "Error: Current user is not a valid operator.");
                
                await PopulateCreateEditDropdowns(viewModel);
                return View(viewModel);
            }

            
            if (!viewModel.CreateNewIncomeBill && !viewModel.SelectedIncomeBillID.HasValue)
            {
                ModelState.AddModelError(string.Empty, "Please either select an existing bill or choose to create a new one.");
            }

            if (ModelState.IsValid)
            {
                IncomeBill? linkedIncomeBill = null;

                if (viewModel.CreateNewIncomeBill)
                {
                    
                    linkedIncomeBill = new IncomeBill
                    {
                        DateTime = DateTime.Now,
                        PatientID = viewModel.PatientID, 
                        TotalAmount = viewModel.NewIncomeBillTotalAmount, 
                        IsPosted = false, 
                        IsVoided = false, 
                        OperatorID = operatorPerson.ID 
                    };
                    _context.Add(linkedIncomeBill);
                    await _context.SaveChangesAsync(); 
                }
                else if (viewModel.SelectedIncomeBillID.HasValue)
                {
                    
                    linkedIncomeBill = await _context.IncomeBills.FindAsync(viewModel.SelectedIncomeBillID.Value);
                    if (linkedIncomeBill == null)
                    {
                        ModelState.AddModelError(string.Empty, "Selected Income Bill not found.");
                        await PopulateCreateEditDropdowns(viewModel);
                        return View(viewModel);
                    }
                }

                
                var patientLabRecord = new PatientLabRecord
                {
                    PatientID = viewModel.PatientID,
                    DoctorID = viewModel.DoctorID,
                    DateTime = DateTime.Now, 
                    IsCollected = viewModel.IsCollected,
                    OperatorID = operatorPerson.ID, 
                    IncomeBillID = linkedIncomeBill?.ID 
                };

                _context.Add(patientLabRecord);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            
            await PopulateCreateEditDropdowns(viewModel);
            return View(viewModel);
        }

        
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

            
            var viewModel = PatientLabRecordCreateEditViewModel.FromPatientLabRecord(patientLabRecord);

            
            await PopulateCreateEditDropdowns(viewModel);

            return View(viewModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientLabRecordCreateEditViewModel viewModel)
        {
            if (id != viewModel.ID)
            {
                return NotFound();
            }

            
            var patientLabRecordToUpdate = await _context.PatientLabRecords.FindAsync(id);
            if (patientLabRecordToUpdate == null)
            {
                return NotFound();
            }

            
            var currentUser = await _userManager.GetUserAsync(User);
            var operatorPerson = await _context.Persons
                                            .FirstOrDefaultAsync(p => p.ID == currentUser.PersonID && p.PersonType == "Operator");

            if (operatorPerson == null)
            {
                ModelState.AddModelError(string.Empty, "Error: Current user is not a valid operator.");
                await PopulateCreateEditDropdowns(viewModel);
                return View(viewModel);
            }

            
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
                        
                        linkedIncomeBill = new IncomeBill
                        {
                            DateTime = DateTime.Now,
                            PatientID = viewModel.PatientID, 
                            TotalAmount = viewModel.NewIncomeBillTotalAmount,
                            IsPosted = false,
                            IsVoided = false,
                            OperatorID = operatorPerson.ID 
                        };
                        _context.Add(linkedIncomeBill);
                        await _context.SaveChangesAsync(); 
                    }
                    else if (viewModel.SelectedIncomeBillID.HasValue)
                    {
                        
                        linkedIncomeBill = await _context.IncomeBills.FindAsync(viewModel.SelectedIncomeBillID.Value);
                        if (linkedIncomeBill == null)
                        {
                            ModelState.AddModelError(string.Empty, "Selected Income Bill not found.");
                            await PopulateCreateEditDropdowns(viewModel);
                            return View(viewModel);
                        }
                    }

                    
                    patientLabRecordToUpdate.PatientID = viewModel.PatientID;
                    patientLabRecordToUpdate.DoctorID = viewModel.DoctorID;
                    patientLabRecordToUpdate.IsCollected = viewModel.IsCollected;
                    patientLabRecordToUpdate.IncomeBillID = linkedIncomeBill?.ID; 

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
            
            await PopulateCreateEditDropdowns(viewModel);
            return View(viewModel);
        }

        
        public async Task<IActionResult> Details(int? id)
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

            return View(patientLabRecord);
        }

        
        public async Task<IActionResult> Delete(int? id)
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

            return View(patientLabRecord);
        }

        
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

