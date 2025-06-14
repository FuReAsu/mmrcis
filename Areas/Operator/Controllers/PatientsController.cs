// Areas/Operator/Controllers/PatientsController.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using mmrcis.Data;
using mmrcis.Models;
using mmrcis.ViewModels; // Important: Add this using statement

namespace mmrcis.Areas.Operator.Controllers
{
    [Area("Operator")]
    [Authorize(Roles = "Operator,Admin")] // Allow both Operators and Admins to manage patients
    public class PatientsController : Controller
    {
        private readonly CisDbContext _context;

        public PatientsController(CisDbContext context)
        {
            _context = context;
        }

        // GET: Operator/Patients
        public async Task<IActionResult> Index()
        {
            // Eager load Person data along with Patient
            var patients = await _context.Patients
                                         .Include(p => p.Person)
                                         .OrderBy(p => p.Person.FullName)
                                         .ToListAsync();
            return View(patients);
        }

        // GET: Operator/Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.Person) // Eager load Person data
                .FirstOrDefaultAsync(m => m.ID == id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Operator/Patients/Create
        public IActionResult Create()
        {
            var model = new PatientViewModel();
            return View(model);
        }

        // POST: Operator/Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new Person record
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
                    PersonType = "Patient" // Explicitly set PersonType for patients
                };

                _context.Persons.Add(person);
                await _context.SaveChangesAsync(); // Save Person to get its ID

                // Create a new Patient record, linking to the newly created Person
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

        // GET: Operator/Patients/Edit/5
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

            // Map Patient and Person data to the ViewModel for editing
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

        // POST: Operator/Patients/Edit/5
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
                    // Find the existing Person record
                    var person = await _context.Persons.FindAsync(model.PersonID);
                    if (person == null)
                    {
                        return NotFound(); // Person record not found for the given PersonID
                    }

                    // Update Person properties
                    person.FullName = model.FullName;
                    person.Address = model.Address;
                    person.PhoneNumber = model.PhoneNumber;
                    person.Email = model.Email;
                    person.DOB = model.DOB;
                    person.Sex = model.Sex;
                    person.BloodGroup = model.BloodGroup;
                    // PersonType and RegisteredSince remain unchanged for existing Person

                    _context.Update(person);

                    // Find the existing Patient record
                    var patient = await _context.Patients.FindAsync(model.ID);
                    if (patient == null)
                    {
                        return NotFound(); // Patient record not found for the given ID
                    }

                    // Update Patient properties
                    patient.Status = model.Status;
                    // PatientSince remains unchanged for existing Patient

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
                catch (DbUpdateException ex) // Handle potential unique constraint violations, e.g. for email if it becomes unique
                {
                    // This is a generic catch, refine as needed for specific errors like unique email.
                    ModelState.AddModelError(string.Empty, $"An error occurred while saving: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError(string.Empty, $"Details: {ex.InnerException.Message}");
                    }
                    return View(model); // Return to view with error
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Operator/Patients/Delete/5
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

        // POST: Operator/Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients
                                        .Include(p => p.Person)
                                        .FirstOrDefaultAsync(p => p.ID == id);
            if (patient != null)
            {
                // Important: When deleting a Patient, you likely want to delete the associated Person record too
                // UNLESS that Person might be linked to other entities (e.g., if a staff member can also be a patient).
                // For a simple patient management, deleting the person is typical.
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
