
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmrcis.Data;
using mmrcis.Models;
using Microsoft.AspNetCore.Authorization; 

namespace mmrcis.Areas.Admin.Controllers
{
    [Area("Admin")] 
    [Authorize(Roles = "Admin")] 
    public class ServicesController : Controller
    {
        private readonly CisDbContext _context;

        public ServicesController(CisDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Services.ToListAsync());
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.ID == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ServiceName,Description,IsActive")] Service service)
        {
            if (ModelState.IsValid)
            {
                service.RegisteredSince = DateTime.Now; 
                _context.Add(service);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Service '{service.ServiceName}' created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ServiceName,Description,IsActive,RegisteredSince")] Service service)
        {
            if (id != service.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    
                    
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Service '{service.ServiceName}' updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ID))
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
            return View(service);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.ID == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }
            
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Service '{service?.ServiceName}' deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ID == id);
        }
    }
}
