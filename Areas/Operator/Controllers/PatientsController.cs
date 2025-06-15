
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using mmrcis.Data;
using mmrcis.Models;
using mmrcis.ViewModels; 

namespace mmrcis.Areas.Operator.Controllers
{
    [Area("Operator")]
    [Authorize(Roles = "Operator,Admin")] 
    public class PatientsController : Controller
    {
        private readonly CisDbContext _context;

        public PatientsController(CisDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            
            var patients = await _context.Patients
                                         .Include(p => p.Person)
                                         .OrderBy(p => p.Person.FullName)
                                         .ToListAsync();
            return View(patients);
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.Person) 
                .FirstOrDefaultAsync(m => m.ID == id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        
        public IActionResult Create()
        {
            var model = new PatientViewModel();
            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var person = new Person
                {
                    FullName = model.FullName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    DOB = model.DOB,
                    Sex = model.Sex,
                    BloodGroup = model.BloodGroup,
                    RegisteredSince = DateTime.Now,
                    PersonType = "Patient" 
                };

                _context.Persons.Add(person);
                await _context.SaveChangesAsync(); 

                
                var patient = new Patient
                {
                    PersonID = person.ID,
                    Status = model.Status,
                    PatientSince = DateTime.Now
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Patient '{model.FullName}' added successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                                        .Include(p => p.Person)
                                        .FirstOrDefaultAsync(m => m.ID == id);
            if (patient == null)
            {
                return NotFound();
            }

            
            var model = new PatientViewModel
            {
                ID = patient.ID,
                Status = patient.Status,
                PersonID = patient.Person.ID,
                FullName = patient.Person.FullName,
                Address = patient.Person.Address,
                PhoneNumber = patient.Person.PhoneNumber,
                Email = patient.Person.Email,
                DOB = patient.Person.DOB,
                Sex = patient.Person.Sex,
                BloodGroup = patient.Person.BloodGroup
            };
            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientViewModel model)
        {
            if (id != model.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    var person = await _context.Persons.FindAsync(model.PersonID);
                    if (person == null)
                    {
                        return NotFound(); 
                    }

                    
                    person.FullName = model.FullName;
                    person.Address = model.Address;
                    person.PhoneNumber = model.PhoneNumber;
                    person.Email = model.Email;
                    person.DOB = model.DOB;
                    person.Sex = model.Sex;
                    person.BloodGroup = model.BloodGroup;
                    

                    _context.Update(person);

                    
                    var patient = await _context.Patients.FindAsync(model.ID);
                    if (patient == null)
                    {
                        return NotFound(); 
                    }

                    
                    patient.Status = model.Status;
                    

                    _context.Update(patient);

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Patient '{model.FullName}' updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(model.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException ex) 
                {
                    
                    ModelState.AddModelError(string.Empty, $"An error occurred while saving: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError(string.Empty, $"Details: {ex.InnerException.Message}");
                    }
                    return View(model); 
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.Person)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients
                                        .Include(p => p.Person)
                                        .FirstOrDefaultAsync(p => p.ID == id);
            if (patient != null)
            {
                
                
                
                _context.Patients.Remove(patient);
                if (patient.Person != null)
                {
                    _context.Persons.Remove(patient.Person);
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Patient '{patient.Person?.FullName}' deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.ID == id);
        }
    }
}
