using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using FIX_IT.Models;

namespace FIX_IT.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TechniciansController : Controller
    {
        private readonly FixItDBContext _context;

        public TechniciansController(FixItDBContext context)
        {
            _context = context;
        }

        // 1.GET Technicians
        public async Task<IActionResult> Index()
        {
            var technicians = _context.Technicians.Include(t => t.Category);
            return View(await technicians.ToListAsync());
        }

        // 2.Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var technician = await _context.Technicians
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (technician == null) return NotFound();

            return View(technician);
        }

        // 3.Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // 4.POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PhoneNumber,IsAvailable,CategoryId")] Technician technician)
        {
            if (ModelState.IsValid)
            {
                _context.Technicians.Add(technician);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", technician.CategoryId);
            return View(technician);
        }

        // 5.Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var technician = await _context.Technicians.FindAsync(id);
            if (technician == null) return NotFound();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", technician.CategoryId);
            return View(technician);
        }

        // 6.Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PhoneNumber,IsAvailable,CategoryId")] Technician technician)
        {
            if (id != technician.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(technician);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TechnicianExists(technician.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", technician.CategoryId);
            return View(technician);
        }

        // 7.Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var technician = await _context.Technicians
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (technician == null) return NotFound();

            return View(technician);
        }

        // 8.Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var technician = await _context.Technicians.FindAsync(id);
            if (technician != null)
            {
                _context.Technicians.Remove(technician);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        private bool TechnicianExists(int id)
        {
            return _context.Technicians.Any(e => e.Id == id);
        }
    }
}
