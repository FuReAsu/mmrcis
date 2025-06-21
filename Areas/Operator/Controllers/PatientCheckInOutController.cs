using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using mmrcis.Data;
using mmrcis.Models;
using mmrcis.Areas.Operator.Models;
using mmrcis.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mmrcis.Areas.Operator.Controllers
{
		[Area("Operator")]
		[Authorize(Policy = "RequireOperatorRole")]
		public class PatientCheckInOutController : Controller
		{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CisDbContext _context;
        private readonly ILogger<PatientCheckInOutController> _logger; 
        private readonly IAuditService _auditService;

        public PatientCheckInOutController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               CisDbContext context,
                               ILogger<PatientCheckInOutController> logger,
                               IAuditService auditService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger; 
            _auditService = auditService;
        }

        private async Task GenerateAuditLog(string action, string parameters)
        {
            var currentUser = await _userManager.Users
                                    .Include(u => u.Person)
                                    .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            string currentUserName = currentUser.Person.FullName;
            string currentAction = action;
            string currentController = "PatientCheckInOut";
            string currentParameters = parameters;
            string currentIpAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (string.IsNullOrEmpty(currentIpAddress))
            {
                currentIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            }
            string currentUserAgent = Request.Headers["User-Agent"].ToString();
            await _auditService.LogActionAsync(
                    currentUserName,
                    currentAction,
                    currentController,
                    parameters,
                    currentIpAddress,
                    currentUserAgent
                    );
        }
        
        private async Task PopulateDropdowns(PatientCheckInOutViewModel? model = null)
        {
            var appointments = await _context.Appointments
                                .Where(a => a.Status == "Scheduled")
                                .Include(a => a.Patient)
                                    .ThenInclude(p => p.Person)
                                .Include(a => a.Person)
                                .OrderByDescending(a => a.AppointmentDateTime)
                                .Select(a => new { a.ID, AppointmentDetails = $"#{a.ID} (Patient: {a.Patient.Person.FullName} Doctor: {a.Person.FullName} Time: {a.AppointmentDateTime:g})" })
                                .ToListAsync();
            ViewBag.Appointments = new SelectList(appointments, "ID", "AppointmentDetails", model?.AppointmentID);
           var patients = await _context.Patients
                                .Include(p => p.Person)
                                .Select(p => new { p.ID, FullName = $"{p.Person.FullName} (id: {p.ID})" })
                                .ToListAsync();
           ViewBag.Patients = new SelectList(patients,"ID", "FullName", model?.PatientID );
        }

				public async Task<IActionResult> Index()
				{ 
            var patientcheckinout = await _context.PatientCheckInOuts
                                                .Include(pcio => pcio.Patient)
                                                    .ThenInclude(p => p.Person)
                                                .Include(pcio => pcio.Appointment)
                                                    .ThenInclude(a => a.Person)
                                                    .ToListAsync();
            return View(patientcheckinout);
				}

        public async Task<IActionResult> Create(int? appointmentID)
        {
           var model = new PatientCheckInOutViewModel();

           if (appointmentID.HasValue)
            {
                var appointment = await _context.Appointments.FindAsync(appointmentID);
                model.AppointmentID = appointmentID.Value;
                model.PatientID = appointment.PatientID;
            }
            model.Date = DateTime.Today;
            model.CheckInTime = DateTime.Now.TimeOfDay;

           await PopulateDropdowns(model);
           return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientCheckInOutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newPatientCheckInOut = new PatientCheckInOut()
                {
                    PatientID = model.PatientID,
                    AppointmentID = model.AppointmentID,
                    Date = model.Date,
                    CheckInTime = model.Date.Date.Add(model.CheckInTime),
                    CheckOutTime = model.Date.Date.Add(model.CheckOutTime),
                    Remarks = model.Remarks
                };
                
                _context.Add(newPatientCheckInOut);
                await _context.SaveChangesAsync();
                var patient = await _context.Patients
                            .Include(p => p.Person)
                            .FirstOrDefaultAsync(p => p.ID == model.PatientID);
        
                string patientName = patient?.Person.FullName ?? "Unknown";
                var appointment = await _context.Appointments.FindAsync(model.AppointmentID);
                appointment.Status = "CheckedIn";
                _context.Update(appointment);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"New PatientCheckInOut Record for Patient {patientName} created successfully!\n Status of Appointment {model.AppointmentID} changed to CheckedIn ";
                _logger.LogInformation($"Operator created PatientCheckInOut Record for Patient {patientName}");

                string logParameters = $"Patient = {patientName}, Appointment = {model.AppointmentID}";
                await GenerateAuditLog("Create", logParameters);

                return RedirectToAction(nameof(Index));
            }
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
                }
            }
            await PopulateDropdowns(model);
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientcheckinout = await _context.PatientCheckInOuts
                                        .Include(pcio => pcio.Patient)
                                            .ThenInclude(p => p.Person)
                                        .Include(pcio => pcio.Appointment)
                                            .ThenInclude(a => a.Person)
                                        .FirstOrDefaultAsync(pcio => pcio.ID == id);

            if (patientcheckinout == null)
            {
                return NotFound();
            }
            return View(patientcheckinout);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
               return NotFound();
            }
            var patientcheckinout = await _context.PatientCheckInOuts.FindAsync(id);

            if ( patientcheckinout == null )
            {
                return NotFound();
            }

            var patientcheckinoutViewModel = new PatientCheckInOutViewModel()
            {
                PatientID = patientcheckinout.PatientID,
                AppointmentID = patientcheckinout.AppointmentID,
                Date = patientcheckinout.Date,
                CheckInTime = patientcheckinout.CheckInTime.TimeOfDay,
                CheckOutTime = DateTime.Now.TimeOfDay,
                Remarks = patientcheckinout.Remarks,
            };
            
            await PopulateDropdowns(patientcheckinoutViewModel);
            return View(patientcheckinoutViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientCheckInOutViewModel model)
        {
            if (model.ID != id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                var patientcheckinout = await _context.PatientCheckInOuts
                            .Include(pcio => pcio.Patient)
                                .ThenInclude(p => p.Person)
                            .Include(pcio => pcio.Appointment)
                                .ThenInclude(a => a.Person)
                            .FirstOrDefaultAsync(pcio => pcio.ID == id);
                    
                if ( patientcheckinout == null )
                {
                    return NotFound();
                }
                    
                string patientName = patientcheckinout.Patient.Person.FullName;
                patientcheckinout.CheckOutTime = model.Date.Date.Add(model.CheckOutTime);
                patientcheckinout.Remarks = model.Remarks;

                _context.Update(patientcheckinout);
                await _context.SaveChangesAsync();
                
                var appointment = await _context.Appointments.FindAsync(patientcheckinout.Appointment.ID);
                appointment.Status = "CheckedOut";

                TempData["SuccessMessage"] = $"PatientCheckInOut Record for Patient {patientName} edited successfully!\n And Status of Appointment {model.AppointmentID} changed to CheckedOut";
                _logger.LogInformation($"Operator edited PatientCheckInOut Record for Patient {patientName}");

                string logParameters = $"Patient = {patientName}, Appointment = {model.AppointmentID}";
                await GenerateAuditLog("Edit", logParameters);
            }
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                NotFound();
            }
            var patientcheckinout = await _context.PatientCheckInOuts
                                    .Include(pcio => pcio.Patient)
                                        .ThenInclude(p => p.Person)
                                    .Include(pcio => pcio.Appointment)
                                        .ThenInclude(a => a.Person)
                                    .FirstOrDefaultAsync(pcio => pcio.ID == id);
            
            if (patientcheckinout == null)
            {
                return NotFound();
            }
            return View(patientcheckinout); 
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientcheckinout = await _context.PatientCheckInOuts
                                    .Include(pcio => pcio.Patient)
                                        .ThenInclude(p => p.Person)
                                    .Include(pcio => pcio.Appointment)
                                        .ThenInclude(a => a.Person)
                                    .FirstOrDefaultAsync(pcio => pcio.ID == id);

            if (patientcheckinout != null)
            {
                try
                {
                    string appointmentdatetime = patientcheckinout.Appointment.AppointmentDateTime.ToString("yyyy-MM-dd | HH:mm");  
                    string patientfullname = patientcheckinout.Patient.Person.FullName;
                    string doctorfullname = patientcheckinout.Appointment.Person.FullName;
                    _context.Remove(patientcheckinout);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = $"Successfully deleted the checkinout record for Patient {patientfullname} to Doctor {doctorfullname} at {appointmentdatetime}";
                    _logger.LogInformation($"Operator deleted checkinout record for Patient {patientfullname} to Doctor {doctorfullname} at {appointmentdatetime}");
                    
                    string logParameters = $"Deleted Appointment = {patientfullname} to {doctorfullname} at {appointmentdatetime}";
                    await GenerateAuditLog("Delete", logParameters); 
                }
                catch (DbUpdateException dbEx)
                {
                    TempData["ErrorMessage"] = "CheckInOut record could not be deleted because related visit records exist.";
                    _logger.LogError(dbEx, "Error deleting appointment with ID {id}", id);
                }
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
