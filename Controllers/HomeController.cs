using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebProfile.Models;

namespace MyWebProfile.Controllers
{
public class HomeController : Controller
{
        private readonly MyWebProfileContext _context;

        public HomeController(MyWebProfileContext context)
    {
            _context = context;
    }

        public async Task<IActionResult> Index()
    {
            // Lấy dữ liệu từ database
            var projects = await _context.Projects.Where(p => p.IsActive).OrderByDescending(p => p.CreatedAt).Take(6).ToListAsync();
            var skills = await _context.Skills.Where(s => s.IsActive).OrderBy(s => s.DisplayOrder).ToListAsync();
            var experiences = await _context.Experiences.Where(e => e.IsActive).OrderByDescending(e => e.StartDate).ToListAsync();

            ViewBag.Projects = projects;
            ViewBag.Skills = skills;
            ViewBag.Experiences = experiences;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
            return View();
        }
    }
}
