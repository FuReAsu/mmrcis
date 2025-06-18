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
		public class AppointmentController : Controller
		{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CisDbContext _context;
        private readonly ILogger<AppointmentController> _logger; 
        private readonly IAuditService _auditService;

        public AppointmentController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               CisDbContext context,
                               ILogger<AppointmentController> logger,
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
            string currentController = "Appointment";
            string currentParameters = parameters;
            string currentIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
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

        private async Task PopulateDropdowns(AppointmentViewModel? model = null)
        {
            var patients = await _context.Patients
                                .Include(p => p.Person)
                                .OrderBy(p => p.Person.FullName)
                                .Select(p => new { p.ID, FullName = $"{p.Person.FullName} (ID: {p.ID})" })
                                .ToListAsync();
            ViewBag.Patients = new SelectList(patients, "ID", "FullName", model?.PatientID);

            var doctors = await _context.Persons
                                .Where(p => p.PersonType == "Doctor")
                                .OrderBy(p => p.FullName)
                                .Select(p => new { p.ID, p.FullName })
                                .ToListAsync();
            ViewBag.Doctors = new SelectList(doctors, "ID", "FullName", model?.DoctorID);
        }

				public async Task<IActionResult> Index()
				{
						var appointment = await _context.Appointments
															.Include(a => a.Person)
															.Include(a => a.Patient)
																	.ThenInclude(p => p.Person)
															.ToListAsync();
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
            if(ModelState.IsValid)
            {
                var appointment = new Appointment
                {
                    PatientID = model.PatientID,
                    PersonID = model.DoctorID,
                    AppointmentDateTime = model.AppointmentDate.Date.Add(model.AppointmentTime),
                    Status = model.Status,
                    Remarks = model.Remarks
                };
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Appointment for {model.AppointmentDate.ToShortDateString()} at {DateTime.Today.Add(model.AppointmentTime).ToShortTimeString()} created successfully!";
                _logger.LogInformation($"Operator created Appointment for Patient {model.PatientID} and Doctor {model.DoctorID}");
               
                string logParameters = $"PatientID = {model.PatientID},(Person)DoctorID = {model.DoctorID}";
                await GenerateAuditLog("Create", logParameters);

                return RedirectToAction(nameof(Index));
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
						var appointment = await _context.Appointments
															.Include(a => a.Person)
															.Include(a => a.Patient)
																	.ThenInclude(p => p.Person)
                              .FirstOrDefaultAsync(m => m.ID == id);
            if (appointment == null)
            {
                return NotFound();
            }
						return View(appointment);
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
                DoctorID = appointment.PersonID,
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
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment == null)
                {
                    return NotFound();
                }


                appointment.PatientID = model.PatientID;
                appointment.PersonID = model.DoctorID;
                appointment.AppointmentDateTime = model.AppointmentDate.Date.Add(model.AppointmentTime);
                appointment.Status = model.Status;
                appointment.Remarks = model.Remarks;

                _context.Update(appointment);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Appointment for {model.AppointmentDate.ToShortDateString()} at {DateTime.Today.Add(model.AppointmentTime).ToShortTimeString()} updated successfully!";
                _logger.LogInformation($"Operator edited Appointment for Patient {model.PatientID} and Doctor {model.DoctorID}");
                
                string logParameters = $"Operator edited Appointment for Patient {model.PatientID} and Doctor {model.DoctorID}";
                await GenerateAuditLog("Edit", logParameters);
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
															.Include(a => a.Person)
															.Include(a => a.Patient)
																	.ThenInclude(p => p.Person)
															.FirstOrDefaultAsync(m => m.ID == id);
            if (appointment == null)
            {
                return NotFound();
            }
						return View(appointment);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            int deletedPatientID = appointment.PatientID;
            int deletedPersonID = appointment.PersonID;

            if(appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Appointment deleted successfully!";
                string logParameters = $"Operator deleted Appointment for Patient {deletedPatientID} and Doctor {deletedPersonID}";
                await GenerateAuditLog("Delete", logParameters);
            }
            return RedirectToAction(nameof(Index));
        }
  	}
}
