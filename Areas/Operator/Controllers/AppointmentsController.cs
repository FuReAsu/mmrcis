using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using mmrcis.Data; // Adjust this to your actual DbContext namespace
using mmrcis.Models; // Adjust this to your actual Appointment, Patient, Person, Service model namespaces
using mmrcis.ViewModels; // Adjust this to your actual AppointmentViewModel namespace
using Microsoft.AspNetCore.Mvc.Rendering; // Needed for SelectList

namespace mmrcis.Areas.Operator.Controllers
{
    [Area("Operator")] // Specifies this controller belongs to the "Operator" area
    [Authorize(Roles = "Operator,Admin")] // Example: Only operators and admins can access
    public class AppointmentsController : Controller
    {
        private readonly CisDbContext _context; // Your application's database context

        public AppointmentsController(CisDbContext context)
        {
            _context = context;
        }

        // Helper method to populate dropdown lists for Patients, Doctors/Staff, and Services
        // This method now correctly uses `async Task` and populates `ViewBag`
        private async Task PopulateDropdowns(AppointmentViewModel? model = null)
        {
            // Populate Patients dropdown
            var patients = await _context.Patients
                                         .Include(p => p.Person) // Include Person data for FullName
                                         .OrderBy(p => p.Person.FullName)
                                         .Select(p => new { p.ID, FullName = $"{p.Person.FullName} (ID: {p.ID})" })
                                         .ToListAsync();
            ViewBag.Patients = new SelectList(patients, "ID", "FullName", model?.PatientID);

            // Populate Doctor/Staff dropdown (assuming Persons are categorized by PersonType)
            var doctorStaff = await _context.Persons
                                            .Where(p => p.PersonType == "Doctor" ) 
                                            .OrderBy(p => p.FullName)
                                            .Select(p => new { p.ID, FullName = $"{p.FullName} ({p.PersonType})" })
                                            .ToListAsync();
            ViewBag.DoctorStaffMembers = new SelectList(doctorStaff, "ID", "FullName", model?.DoctorStaffID);

            // Populate Services dropdown
            var services = await _context.Services
                                         .Where(s => s.IsActive)
                                         .OrderBy(s => s.ServiceName)
                                         .ToListAsync();
            ViewBag.Services = new SelectList(services, "ID", "ServiceName", model?.ServiceID);
        }

        // GET: Operator/Appointments
        // Displays a list of appointments
        public async Task<IActionResult> Index()
        {
            // Include related entities for display in the index view
            var appointments = await _context.Appointments
                                            .Include(a => a.Patient)
                                                .ThenInclude(p => p.Person) // Include Person details within Patient
                                            .Include(a => a.DoctorStaff) // Assuming DoctorStaff is your Person entity for doctors/staff
                                            .Include(a => a.Service)
                                            .OrderByDescending(a => a.AppointmentDateTime) // Order by latest appointments
                                            .ToListAsync();
            return View(appointments);
        }

        // GET: Operator/Appointments/Details/5
        // Shows details for a specific appointment
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Patient).ThenInclude(p => p.Person)
                .Include(a => a.DoctorStaff)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.ID == id); // Using 'ID' for primary key
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Operator/Appointments/Create
        // Displays the form to create a new appointment
        public async Task<IActionResult> Create()
        {
            var model = new AppointmentViewModel();
            // Initialize with current date and time for user convenience
            model.AppointmentDate = DateTime.Today;
            model.AppointmentTime = DateTime.Now.TimeOfDay;

            await PopulateDropdowns(model); // Populate dropdown lists for the form
            return View(model);
        }

        // POST: Operator/Appointments/Create
        // Handles the submission of the new appointment form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentViewModel model)
        {
            // The DateTime.TryParse block for AppointmentDateTime is removed
            // because the ViewModel now uses separate AppointmentDate and AppointmentTime properties.

            if (ModelState.IsValid)
            {
                var appointment = new Appointment
                {
                    PatientID = model.PatientID,
                    DoctorStaffID = model.DoctorStaffID,
                    ServiceID = model.ServiceID,
                    // Combine the separated Date and Time from the ViewModel into the database model's DateTime property
                    AppointmentDateTime = model.AppointmentDate.Date.Add(model.AppointmentTime),
                    Status = model.Status,
                    Remarks = model.Remarks,
                    BookedAt = DateTime.Now // Assuming you have a 'BookedAt' property in your Appointment model
                };

                _context.Add(appointment);
                await _context.SaveChangesAsync();

                // Success message using the new ViewModel properties
                TempData["SuccessMessage"] = $"Appointment for {model.AppointmentDate.ToShortDateString()} at {DateTime.Today.Add(model.AppointmentTime).ToShortTimeString()} created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(model); // If model state is invalid, re-populate dropdowns before returning to view
            return View(model);
        }

        // GET: Operator/Appointments/Edit/5
        // Displays the form to edit an existing appointment
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var model = new AppointmentViewModel
            {
                ID = appointment.ID, // Populate the ViewModel's ID
                PatientID = appointment.PatientID,
                DoctorStaffID = appointment.DoctorStaffID,
                ServiceID = appointment.ServiceID,
                // Split the existing database DateTime into the ViewModel's separate date and time properties
                AppointmentDate = appointment.AppointmentDateTime.Date, // Extracts just the date part
                AppointmentTime = appointment.AppointmentDateTime.TimeOfDay, // Extracts just the time part as a TimeSpan
                Status = appointment.Status,
                Remarks = appointment.Remarks
            };

            await PopulateDropdowns(model); // Populate dropdown lists for the form
            return View(model);
        }

        // POST: Operator/Appointments/Edit/5
        // Handles the submission of the edited appointment form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppointmentViewModel model)
        {
            // Ensure the ID from the URL matches the ID in the submitted form model
            if (id != model.ID) // Using 'ID' for consistency
            {
                return NotFound();
            }

            // The DateTime.TryParse block for AppointmentDateTime is removed
            // because the ViewModel now uses separate AppointmentDate and AppointmentTime properties.

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the original appointment from the database
                    var appointment = await _context.Appointments.FindAsync(id); // Use 'id' from route parameter to find
                    if (appointment == null)
                    {
                        return NotFound();
                    }

                    // Update the database model's properties from the ViewModel
                    appointment.PatientID = model.PatientID;
                    appointment.DoctorStaffID = model.DoctorStaffID;
                    appointment.ServiceID = model.ServiceID;
                    appointment.AppointmentDateTime = model.AppointmentDate.Date.Add(model.AppointmentTime); // Combine
                    appointment.Status = model.Status;
                    appointment.Remarks = model.Remarks;
                    appointment.LastUpdated = DateTime.Now; // Assuming a 'LastUpdated' property

                    _context.Update(appointment);
                    await _context.SaveChangesAsync();

                    // Success message using the new ViewModel properties
                    TempData["SuccessMessage"] = $"Appointment for {model.AppointmentDate.ToShortDateString()} at {DateTime.Today.Add(model.AppointmentTime).ToShortTimeString()} updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency conflicts if another user modified it concurrently
                    if (!AppointmentExists(model.ID)) // Using 'ID' for consistency
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Re-throw if it's another type of concurrency issue
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(model); // If model state is invalid, re-populate dropdowns
            return View(model);
        }

        // GET: Operator/Appointments/Delete/5
        // Displays the confirmation page for deleting an appointment
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Patient).ThenInclude(p => p.Person)
                .Include(a => a.DoctorStaff) // Assuming correct navigation property name
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.ID == id); // Using 'ID' for primary key
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Operator/Appointments/Delete/5
        // Handles the deletion of an appointment
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Appointment deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if an appointment exists
        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.ID == id); // Using 'ID' for consistency
        }
    }
}
