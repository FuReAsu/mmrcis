using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using mmrcis.Data;
using mmrcis.Models;
using mmrcis.Areas.Operator.Models;
using mmrcis.Services;

namespace mmrcis.Areas.Operator.Controllers
{
		[Area("Operator")]
		[Authorize(Policy = "RequireOperatorRole")]
		public class PatientController : Controller
		{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CisDbContext _context;
        private readonly ILogger<PatientController> _logger; 
        private readonly IAuditService _auditService;

        public PatientController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               CisDbContext context,
                               ILogger<PatientController> logger,
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
            string currentController = "Patient";
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
        
        public async Task<IActionResult> Index()
        {
            var patients = await _context.Patients
                                        .Include(p => p.Person)
                                        .OrderBy(p => p.Person.ID)
                                        .ToListAsync();
            return View(patients);
        }

        public IActionResult Create()
        {
            var model = new PatientViewModel();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
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

                _logger.LogInformation($"Operator registered new Patient: {model.FullName}");
                
                string logParameters = $"PatientName = {model.FullName}";
                await GenerateAuditLog("Create", logParameters);

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
                var person = await _context.Persons.FindAsync(model.PersonID);
                string oldPatientFullName = person.FullName;
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
                _logger.LogInformation($"Operator edited Patient: {oldPatientFullName}");
               
                string logParameters = $"Edited Patient FullName = {oldPatientFullName}";
                await GenerateAuditLog("Edit", logParameters );

                return RedirectToAction(nameof(Index));
            }
            return View(model);
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
                _logger.LogInformation($"Operator deleted Patient: {patient.Person?.FullName}");

                string logParameters = $"Deleted Patient = {patient.Person?.FullName}";
                await GenerateAuditLog("Delete", logParameters);
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
