using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using mmrcis.Data; 
using mmrcis.Models; 
using mmrcis.ViewModels; 
using Microsoft.AspNetCore.Mvc.Rendering; 

namespace mmrcis.Areas.Operator.Controllers
{
    [Area("Operator")] 
    [Authorize(Roles = "Operator,Admin")] 
    public class AppointmentsController : Controller
    {
        private readonly CisDbContext _context; 

        public AppointmentsController(CisDbContext context)
        {
            _context = context;
        }

        
        
        private async Task PopulateDropdowns(AppointmentViewModel? model = null)
        {
            
            var patients = await _context.Patients
                                         .Include(p => p.Person) 
                                         .OrderBy(p => p.Person.FullName)
                                         .Select(p => new { p.ID, FullName = $"{p.Person.FullName} (ID: {p.ID})" })
                                         .ToListAsync();
            ViewBag.Patients = new SelectList(patients, "ID", "FullName", model?.PatientID);

            
            var doctorStaff = await _context.Persons
                                            .Where(p => p.PersonType == "Doctor" ) 
                                            .OrderBy(p => p.FullName)
                                            .Select(p => new { p.ID, FullName = $"{p.FullName} ({p.PersonType})" })
                                            .ToListAsync();
            ViewBag.DoctorStaffMembers = new SelectList(doctorStaff, "ID", "FullName", model?.DoctorStaffID);

            
            var services = await _context.Services
                                         .Where(s => s.IsActive)
                                         .OrderBy(s => s.ServiceName)
                                         .ToListAsync();
            ViewBag.Services = new SelectList(services, "ID", "ServiceName", model?.ServiceID);
        }

        
        
        public async Task<IActionResult> Index()
        {
            
            var appointments = await _context.Appointments
                                            .Include(a => a.Patient)
                                                .ThenInclude(p => p.Person) 
                                            .Include(a => a.DoctorStaff) 
                                            .Include(a => a.Service)
                                            .OrderByDescending(a => a.AppointmentDateTime) 
                                            .ToListAsync();
            return View(appointments);
        }

        
        
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
                .FirstOrDefaultAsync(m => m.ID == id); 
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        
        
        public async Task<IActionResult> Create()
        {
            var model = new AppointmentViewModel();
            
            model.AppointmentDate = DateTime.Today;
            model.AppointmentTime = DateTime.Now.TimeOfDay;

            await PopulateDropdowns(model); 
            return View(model);
        }

        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentViewModel model)
        {
            
            

            if (ModelState.IsValid)
            {
                var appointment = new Appointment
                {
                    PatientID = model.PatientID,
                    DoctorStaffID = model.DoctorStaffID,
                    ServiceID = model.ServiceID,
                    
                    AppointmentDateTime = model.AppointmentDate.Date.Add(model.AppointmentTime),
                    Status = model.Status,
                    Remarks = model.Remarks,
                    BookedAt = DateTime.Now 
                };

                _context.Add(appointment);
                await _context.SaveChangesAsync();

                
                TempData["SuccessMessage"] = $"Appointment for {model.AppointmentDate.ToShortDateString()} at {DateTime.Today.Add(model.AppointmentTime).ToShortTimeString()} created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(model); 
            return View(model);
        }

        
        
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
                ID = appointment.ID, 
                PatientID = appointment.PatientID,
                DoctorStaffID = appointment.DoctorStaffID,
                ServiceID = appointment.ServiceID,
                
                AppointmentDate = appointment.AppointmentDateTime.Date, 
                AppointmentTime = appointment.AppointmentDateTime.TimeOfDay, 
                Status = appointment.Status,
                Remarks = appointment.Remarks
            };

            await PopulateDropdowns(model); 
            return View(model);
        }

        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppointmentViewModel model)
        {
            
            if (id != model.ID) 
            {
                return NotFound();
            }

            
            

            if (ModelState.IsValid)
            {
                try
                {
                    
                    var appointment = await _context.Appointments.FindAsync(id); 
                    if (appointment == null)
                    {
                        return NotFound();
                    }

                    
                    appointment.PatientID = model.PatientID;
                    appointment.DoctorStaffID = model.DoctorStaffID;
                    appointment.ServiceID = model.ServiceID;
                    appointment.AppointmentDateTime = model.AppointmentDate.Date.Add(model.AppointmentTime); 
                    appointment.Status = model.Status;
                    appointment.Remarks = model.Remarks;
                    appointment.LastUpdated = DateTime.Now; 

                    _context.Update(appointment);
                    await _context.SaveChangesAsync();

                    
                    TempData["SuccessMessage"] = $"Appointment for {model.AppointmentDate.ToShortDateString()} at {DateTime.Today.Add(model.AppointmentTime).ToShortTimeString()} updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                    if (!AppointmentExists(model.ID)) 
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

            await PopulateDropdowns(model); 
            return View(model);
        }

        
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Patient).ThenInclude(p => p.Person)
                .Include(a => a.DoctorStaff) 
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.ID == id); 
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        
        
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

        
        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.ID == id); 
        }
    }
}
