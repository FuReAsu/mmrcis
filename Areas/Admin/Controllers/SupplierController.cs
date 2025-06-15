
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmrcis.Data;
using mmrcis.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic; 

namespace mmrcis.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SuppliersController : Controller
    {
        private readonly CisDbContext _context;

        public SuppliersController(CisDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Suppliers.OrderBy(s => s.Name).ToListAsync());
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,ContactPerson,Phone,Email,Address,IsActive")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.RegisteredSince = DateTime.Now;
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Supplier '{supplier.Name}' created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,ContactPerson,Phone,Email,Address,IsActive,RegisteredSince")] Supplier supplier)
        {
            if (id != supplier.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Supplier '{supplier.Name}' updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.ID))
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
            return View(supplier);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
            }
            
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Supplier '{supplier?.Name}' deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.ID == id);
        }
    }
}
