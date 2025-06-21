using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using mmrcis.Data;
using mmrcis.Models;
using mmrcis.Areas.Medical.Models;
using mmrcis.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mmrcis.Areas.Medical.Controllers
{
		[Area("Medical")]
		[Authorize(Policy = "RequireMedicalRole")]
		public class PatientVitalsController : Controller
		{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CisDbContext _context;
        private readonly ILogger<PatientVitalsController> _logger; 
        private readonly IAuditService _auditService;

        public PatientVitalsController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               CisDbContext context,
                               ILogger<PatientVitalsController> logger,
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
            string currentController = "PatientVitals";
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
           var patientvitals = await _context.PatientVitalss
                                                            .Include(pv => pv.PatientVisitRecord)
                                                                .ThenInclude(pvr => pvr.Patient)
                                                                    .ThenInclude(p => p.Person)
                                                            .Include(pv => pv.MedicalStaff)
                                                            .OrderBy(pv => pv.PatientVisitRecord.DateOfVisit)
                                                            .ToListAsync();
           return View(patientvitals);
        }

        public async Task<IActionResult> Create(int? patientvisitrecordid)
        {
            var patientvitals = new PatientVitalsViewModel();
            var currentUser = await _userManager.Users
                                    .Include(u => u.Person)
                                    .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
            if (patientvisitrecordid.HasValue)
            {
                patientvitals.PatientVisitRecordID = patientvisitrecordid.Value;
            }
            if (currentUser != null)
            {
                patientvitals.MedicalStaffID = currentUser.Person.ID;
            }
            else
            {
                return BadRequest(); 
            }
            return View(patientvitals);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientVitalsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patientvisitrecord = await _context.PatientVisitRecords
                                                        .Include(pvr => pvr.Patient) 
                                                            .ThenInclude(p => p.Person)
                                                        .FirstOrDefaultAsync(pvr => pvr.ID == model.PatientVisitRecordID);
                string patient = patientvisitrecord.Patient.Person.FullName;
                string dateofvisit = patientvisitrecord.DateOfVisit.ToString() ?? "";
                var patientvitals = new PatientVitals()
                {
                    PatientVisitRecordID = model.PatientVisitRecordID,
                    MedicalStaffID = model.MedicalStaffID,
                    Temperature = model.Temperature,
                    PulseRate = model.PulseRate,
                    RespiratorRate = model.RespiratorRate,
                    BloodPressureSystolic = model.BloodPressureSystolic,
                    BloodPressureDiastolic = model.BloodPressureDiastolic,
                    OxygenSaturation = model.OxygenSaturation,
                    Notes = model.Notes
                };
                _context.Add(patientvitals);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Successfully created patient vitals record for patient {patient}";
                _logger.LogInformation($"MedicalStaff created patient vitals record for patient {patient}");

                string logParameters = $"Patient= {patient} DateOfVisit= {dateofvisit}";
                await GenerateAuditLog("Create", logParameters);

                return RedirectToAction("Index","PatientVisitRecord");
            }
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var patientvitals = await _context.PatientVitalss
                                            .Include(pv => pv.PatientVisitRecord)
                                            .ThenInclude(pvr => pvr.Patient)
                                                .ThenInclude(p => p.Person)
                                            .Include(pv => pv.MedicalStaff)
                                            .FirstOrDefaultAsync(pv => pv.ID == id);

            if (patientvitals == null)
            {
                return NotFound();
            }
            return View(patientvitals);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var patientvitals = await _context.PatientVitalss
                                            .Include(pv => pv.PatientVisitRecord)
                                            .ThenInclude(pvr => pvr.Patient)
                                                .ThenInclude(p => p.Person)
                                            .Include(pv => pv.MedicalStaff)
                                            .FirstOrDefaultAsync(pv => pv.ID == id);
            if (patientvitals == null)
            {
                return NotFound();
            }

            var model = new PatientVitalsViewModel()
            {
                ID = patientvitals.ID,
                PatientVisitRecordID = patientvitals.PatientVisitRecordID,
                MedicalStaffID = patientvitals.MedicalStaffID,
                Temperature = patientvitals.Temperature,
                PulseRate = patientvitals.PulseRate,
                RespiratorRate = patientvitals.RespiratorRate,
                BloodPressureSystolic = patientvitals.BloodPressureSystolic,
                BloodPressureDiastolic = patientvitals.BloodPressureDiastolic,
                Notes = patientvitals.Notes
            };

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientVitalsViewModel model)
        {
            if (id != model.ID)
            {
                return BadRequest();
            }
            
            var patientvitals = await _context.PatientVitalss
                                            .Include(pv => pv.PatientVisitRecord)
                                                .ThenInclude(pvr => pvr.Patient)
                                                    .ThenInclude(p => p.Person)
                                            .FirstOrDefaultAsync(pv => pv.ID == id);

            if (patientvitals != null)
            {
                string patient = patientvitals.PatientVisitRecord.Patient.Person.FullName;
                string dateofvisit = patientvitals.PatientVisitRecord.DateOfVisit.ToString() ?? "";
                patientvitals.Temperature = model.Temperature;
                patientvitals.RespiratorRate = model.RespiratorRate;
                patientvitals.PulseRate = model.PulseRate;
                patientvitals.BloodPressureSystolic = model.BloodPressureSystolic;
                patientvitals.BloodPressureDiastolic = model.BloodPressureDiastolic;
                patientvitals.OxygenSaturation = model.OxygenSaturation;
                patientvitals.Notes = model.Notes;

                _context.Update(patientvitals);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Successfully edited patient vitals record for patient {patient}";
                _logger.LogInformation($"MedicalStaff edited patient vitals record for patient {patient}");

                string logParameters = $"Patient={patient} DateOfVisit={dateofvisit}";
                await GenerateAuditLog("Edit", logParameters);

            }    
            return RedirectToAction("Index", "PatientVisitRecord");
        }
    }
}

