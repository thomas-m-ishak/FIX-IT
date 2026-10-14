using Microsoft.AspNetCore.Mvc;
using FIX_IT.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FIX_IT.Controllers
{
    public class HomeController : Controller
    {
        private readonly FixItDBContext _context;

        public HomeController(FixItDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return View("Welcome");
            }

            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.TotalTechnicians = await _context.Technicians.CountAsync();
            ViewBag.TotalIssues = await _context.Bookings.CountAsync();
            ViewBag.OpenIssues = await _context.Bookings.CountAsync(i => i.IsClosed == false);

            if (User.IsInRole("Admin"))
            {
                var topTechnicians = await _context.Bookings
                    .Where(i => i.AssignedTechnician != null)
                    .GroupBy(i => i.AssignedTechnician!.Name)
                    .Select(g => new { Name = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(5)
                    .ToListAsync();

                ViewBag.TopTechnicians = topTechnicians;
                return View("AdminDashboard");
            }
            else if (User.IsInRole("Technician"))
            {
                return View("TechnicianHome");
            }
            else
            {
                var services = await _context.Categories.ToListAsync();
                return View(services);
            }
        }
    }
}
