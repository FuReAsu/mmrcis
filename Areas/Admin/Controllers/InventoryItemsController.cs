// Areas/Admin/Controllers/InventoryItemsController.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmrcis.Data;
using mmrcis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering; // For SelectList

namespace mmrcis.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class InventoryItemsController : Controller
    {
        private readonly CisDbContext _context;

        public InventoryItemsController(CisDbContext context)
        {
            _context = context;
        }

        // Helper method to populate dropdowns (e.g., Suppliers)
        private async Task PopulateDropdowns(object? selectedSupplier = null)
        {
            var suppliers = await _context.Suppliers
                                        .OrderBy(s => s.Name)
                                        .Select(s => new { s.ID, s.Name })
                                        .ToListAsync();
            ViewBag.SupplierID = new SelectList(suppliers, "ID", "Name", selectedSupplier);
        }

        // GET: Admin/InventoryItems
        public async Task<IActionResult> Index()
        {
            // Eager load the Supplier to display its name
            var inventoryItems = _context.InventoryItems.Include(i => i.Supplier).OrderBy(i => i.Name);
            return View(await inventoryItems.ToListAsync());
        }

        // GET: Admin/InventoryItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryItem = await _context.InventoryItems
                .Include(i => i.Supplier) // Include Supplier for display
                .FirstOrDefaultAsync(m => m.ID == id);
            if (inventoryItem == null)
            {
                return NotFound();
            }

            return View(inventoryItem);
        }

        // GET: Admin/InventoryItems/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns(); // Populate suppliers for the dropdown
            return View();
        }

        // POST: Admin/InventoryItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,UnitOfMeasure,CurrentStock,MinStockLevel,PurchasePrice,Description,SupplierID,IsActive")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                inventoryItem.RegisteredSince = DateTime.Now;
                _context.Add(inventoryItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Inventory Item '{inventoryItem.Name}' created successfully.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns(inventoryItem.SupplierID); // Re-populate dropdown if model state is invalid
            return View(inventoryItem);
        }

        // GET: Admin/InventoryItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryItem = await _context.InventoryItems.FindAsync(id);
            if (inventoryItem == null)
            {
                return NotFound();
            }
            await PopulateDropdowns(inventoryItem.SupplierID); // Populate dropdown with current supplier selected
            return View(inventoryItem);
        }

        // POST: Admin/InventoryItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,UnitOfMeasure,CurrentStock,MinStockLevel,PurchasePrice,Description,SupplierID,IsActive,RegisteredSince")] InventoryItem inventoryItem)
        {
            if (id != inventoryItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventoryItem);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Inventory Item '{inventoryItem.Name}' updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryItemExists(inventoryItem.ID))
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
            await PopulateDropdowns(inventoryItem.SupplierID); // Re-populate dropdown if model state is invalid
            return View(inventoryItem);
        }

        // GET: Admin/InventoryItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryItem = await _context.InventoryItems
                .Include(i => i.Supplier) // Include Supplier for display
                .FirstOrDefaultAsync(m => m.ID == id);
            if (inventoryItem == null)
            {
                return NotFound();
            }

            return View(inventoryItem);
        }

        // POST: Admin/InventoryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventoryItem = await _context.InventoryItems.FindAsync(id);
            if (inventoryItem != null)
            {
                _context.InventoryItems.Remove(inventoryItem);
            }
            
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Inventory Item '{inventoryItem?.Name}' deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryItemExists(int id)
        {
            return _context.InventoryItems.Any(e => e.ID == id);
        }
    }
}
