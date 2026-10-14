using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIX_IT.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FIX_IT.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly FixItDBContext _context;

        public BookingsController(FixItDBContext context)
        {
            _context = context;
        }

        // 1.All System Bookings
        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            IQueryable<Booking> query = _context.Bookings
                .Include(b => b.Category)
                .Include(b => b.Reporter)
                .Include(b => b.AssignedTechnician);

            if (User.IsInRole("Admin"))
            {
                ViewData["Title"] = "All System Bookings";
            }
            else if (User.IsInRole("Technician"))
            {
                query = query.Where(b => b.TechnicianId == userId || b.TechnicianId == null);
                ViewData["Title"] = "Assigned & Unassigned Bookings";
            }
            else
            {
                query = query.Where(b => b.ReporterId == userId);
                ViewData["Title"] = "My Bookings";
            }

            return View(await query.ToListAsync());
        }

        // 2.Details Confirmation
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Category)
                .Include(b => b.Reporter)
                .Include(b => b.AssignedTechnician)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // 3.booking
        [HttpGet]
        public IActionResult Create(int? categoryId)
        {
            if (categoryId == null) return RedirectToAction("Index", "Categories");

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", categoryId);

            var booking = new Booking
            {
                CategoryId = categoryId.Value,
                CreatedDate = DateTime.Now
            };

            return View(booking);
        }

        // 4.Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,CategoryId")] Booking booking)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdStr, out int userId))
            {
                booking.ReporterId = userId;
                booking.CreatedDate = DateTime.Now;
                booking.IsClosed = false;

                ModelState.Remove("Reporter");
                ModelState.Remove("Category");
                ModelState.Remove("AssignedTechnician");

                if (ModelState.IsValid)
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = booking.Id });
                }
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", booking.CategoryId);
            return View(booking);
        }

        // 5.Payment
        public async Task<IActionResult> Payment(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // 6.ConfirmPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            return RedirectToAction(nameof(Feedback), new { id = booking.Id });
        }

        // 7.Feedback
        public async Task<IActionResult> Feedback(int? id)
        {
            if (id == null) return NotFound();
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        // 8.Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Feedback(int id, int Rating, string Feedback)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Rating = Rating;
                booking.Feedback = Feedback;
                booking.IsClosed = true;
                _context.Update(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
