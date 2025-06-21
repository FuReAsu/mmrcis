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
		public class PatientVisitRecordController : Controller
		{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CisDbContext _context;
        private readonly ILogger<PatientVisitRecordController> _logger; 
        private readonly IAuditService _auditService;

        public PatientVisitRecordController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               CisDbContext context,
                               ILogger<PatientVisitRecordController> logger,
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
            string currentController = "PatientVisitRecord";
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
        
        private async Task PopulateDropdowns(PatientVisitRecordViewModel? model = null)
        {
            var patientcheckinoutrecords = await _context.PatientCheckInOuts
                                            .Include(pcio => pcio.Appointment)
                                            .Include(pcio => pcio.Patient)
                                                .ThenInclude(p => p.Person)
                                            .OrderByDescending(pcio => pcio.Appointment.AppointmentDateTime)
                                            .Select(pcio => new {pcio.ID, PatientCheckInOutDetails = $"#{pcio.ID} {pcio.Patient.Person.FullName} {pcio.Appointment.AppointmentDateTime:g}"})
                                            .ToListAsync();
            ViewBag.PatientCheckInOutRecords = new SelectList(patientcheckinoutrecords,"ID", "PatientCheckInOutDetails", model?.PatientCheckInOutID);
        }

        public async Task<IActionResult> Index(int? patientid)
        {
            if (patientid.HasValue)
            {
                var patientvisitrecordswithpatient = await _context.PatientVisitRecords
                                            .Include(pvr => pvr.PatientVitals)
                                            .Include(pvr => pvr.Patient)
                                                .ThenInclude(p => p.Person)
                                            .Include(pvr => pvr.PatientCheckInOut)
                                                .ThenInclude(pcio => pcio.Appointment)
                                            .Include(pvr => pvr.Doctor)
                                            .Where(pvr => pvr.PatientID == patientid)
                                            .OrderByDescending(pvr => pvr.PatientCheckInOut.Appointment.AppointmentDateTime)
                                            .ToListAsync();
                return View(patientvisitrecordswithpatient);
            }
            var patientvisitrecords = await _context.PatientVisitRecords
                                        .Include(pvr => pvr.PatientVitals)
                                        .Include(pvr => pvr.Patient)
                                            .ThenInclude(p => p.Person)
                                        .Include(pvr => pvr.PatientCheckInOut)
                                            .ThenInclude(pcio => pcio.Appointment)
                                        .Include(pvr => pvr.Doctor)
                                        .OrderByDescending(pvr => pvr.PatientCheckInOut.Appointment.AppointmentDateTime)
                                        .ToListAsync();
            return View(patientvisitrecords);
        }

        public async Task<IActionResult> Create(int? appointmentid)
        {
            var model = new PatientVisitRecordViewModel();

            if (appointmentid.HasValue)
            {
                var patientcheckinoutrecord = await _context.PatientCheckInOuts
                                                .Include(pcio => pcio.Appointment)
                                                .FirstOrDefaultAsync(pcio => pcio.AppointmentID == appointmentid);
                if (patientcheckinoutrecord == null)
                    {
                        ModelState.AddModelError("", "Invalid appointment ID: no matching check-in/out record.");
                        await PopulateDropdowns(model);
                        return View(model);
                    }
                model.PatientCheckInOutID = patientcheckinoutrecord.ID;
                model.PatientID = patientcheckinoutrecord.Appointment.PatientID;
                model.DateOfVisit = patientcheckinoutrecord.Appointment.AppointmentDateTime;
                model.DoctorID = patientcheckinoutrecord.Appointment.PersonID;
            }
            await PopulateDropdowns(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientVisitRecordViewModel model)
        {
           if(ModelState.IsValid)
           {
               var newPatientVisitRecord = new PatientVisitRecord()
               {
                   PatientID = model.PatientID,
                   PatientCheckInOutID = model.PatientCheckInOutID,
                   DoctorID = model.DoctorID,
                   DateOfVisit = model.DateOfVisit,
                   Diagnoses = model.Diagnoses,
                   Prescriptions = model.Prescriptions,
                   Remarks = model.Remarks
               };
                
               var patient = await _context.Patients
                                    .Include(p => p.Person)
                                    .FirstOrDefaultAsync(p => p.ID == model.PatientID);
               var doctor = await _context.Persons
                                    .FirstOrDefaultAsync(p => p.ID == model.DoctorID);
               _context.Add(newPatientVisitRecord);
               await _context.SaveChangesAsync();
               TempData["SuccessMessage"] = $"PatientVisitRecord created for {patient.Person.FullName}";
               _logger.LogInformation($"Doctor {doctor.FullName} created PatientVisitRecord for Patient {patient.Person.FullName}");
               string logParameters = $"Patient = {patient.Person.FullName}, Doctor = {doctor.FullName}, DateOfVisit = {model.DateOfVisit}";
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
            var patientvisitrecord = await _context.PatientVisitRecords
                                        .Include(pvr => pvr.Patient)
                                            .ThenInclude(p => p.Person)
                                        .Include(pvr => pvr.Doctor)
                                        .Include(pvr => pvr.PatientCheckInOut)
                                            .ThenInclude(pcio => pcio.Appointment)
                                        .FirstOrDefaultAsync(pvr => pvr.ID == id);

            if (patientvisitrecord == null)
            {
                return NotFound();
            }
            return View(patientvisitrecord);
        }
    

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var patientvisitrecord = await _context.PatientVisitRecords
                                        .Include(pvr => pvr.Patient)
                                            .ThenInclude(p => p.Person)
                                        .Include(pvr => pvr.Doctor)
                                        .Include(pvr => pvr.PatientCheckInOut)
                                            .ThenInclude(pcio => pcio.Appointment)
                                        .FirstOrDefaultAsync(pvr => pvr.ID == id);

            if (patientvisitrecord == null)
            {
                return NotFound();
            }
            return View(patientvisitrecord);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientvisitrecord = await _context.PatientVisitRecords
                                        .Include(pvr => pvr.Patient)
                                            .ThenInclude(p => p.Person)
                                        .Include(pvr => pvr.Doctor)
                                        .Include(pvr => pvr.PatientCheckInOut)
                                            .ThenInclude(pcio => pcio.Appointment)
                                        .FirstOrDefaultAsync(pvr => pvr.ID == id);

            if (patientvisitrecord != null)
            {
                string patient = patientvisitrecord?.Patient.Person.FullName ?? "Anonymous";
                string doctor = patientvisitrecord?.Doctor.FullName ?? "Anonymous";
                int patientcheckinoutid = patientvisitrecord?.PatientCheckInOutID ?? 0;
               
                _context.Remove(patientvisitrecord);
                await _context.SaveChangesAsync(); 
                TempData["SuccessMessage"] = $"Successfully deleted patient visit record for patient {patient}.";
                _logger.LogInformation($"Doctor deleted patient visist record for patient {patient}, doctor {doctor}, Patient Check In Out {patientcheckinoutid}");
                string logParameters = $"PatientCheckInOutID={patientcheckinoutid}, Patient={patient}, Doctor={doctor}";
                await GenerateAuditLog("Delete", logParameters);
            }
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var patientvisitrecord = await _context.PatientVisitRecords
                                        .Include(pvr => pvr.Patient)
                                            .ThenInclude(p => p.Person)
                                        .Include(pvr => pvr.Doctor)
                                        .Include(pvr => pvr.PatientCheckInOut)
                                            .ThenInclude(pcio => pcio.Appointment)
                                        .FirstOrDefaultAsync(pvr => pvr.ID == id);

            if (patientvisitrecord == null)
            {
                return NotFound();
            }
            return View(patientvisitrecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientVisitRecordViewModel model)
        {
            if ( id != model.ID )
            {
                return NotFound();
            }
            var patientvisitrecord = await _context.PatientVisitRecords
                                        .Include(pvr => pvr.Patient)
                                            .ThenInclude(p => p.Person)
                                        .Include(pvr => pvr.Doctor)
                                        .Include(pvr => pvr.PatientCheckInOut)
                                            .ThenInclude(pcio => pcio.Appointment)
                                        .FirstOrDefaultAsync(pvr => pvr.ID == id);

            string patient = patientvisitrecord?.Patient.Person.FullName ?? "Anonymous";
            string doctor = patientvisitrecord?.Doctor.FullName ?? "Anonymous";
            int patientcheckinoutid = patientvisitrecord?.PatientCheckInOutID ?? 0;

            if ( patientvisitrecord != null)
            {
                patientvisitrecord.Prescriptions = model.Prescriptions;
                patientvisitrecord.Diagnoses = model.Diagnoses;
                patientvisitrecord.Remarks = model.Remarks;
            }
            
            _context.Update(patientvisitrecord);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Successfully edited patient visit record for patient {patient}.";
            _logger.LogInformation($"Doctor edited patient visist record for patient {patient}, doctor {doctor}, Patient Check In Out {patientcheckinoutid}");
            string logParameters = $"PatientCheckInOutID={patientcheckinoutid}, Patient={patient}, Doctor={doctor}";
            await GenerateAuditLog("Edit", logParameters);

            return RedirectToAction(nameof(Index));

        }
    }
}
