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
            // Lấy dữ liệu từ database - chỉ lấy những item không bị xóa và đang active
            var projects = await _context.Projects
                .Where(p => p.IsActive && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .Take(6)
                .ToListAsync();
            
            var skills = await _context.Skills
                .Where(s => s.IsActive && !s.IsDeleted)
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync();
            
            var experiences = await _context.Experiences
                .Where(e => e.IsActive && !e.IsDeleted)
                .OrderByDescending(e => e.StartDate)
                .ToListAsync();
            
            var contentSettings = await _context.ContentSettings
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            ViewBag.Projects = projects;
            ViewBag.Skills = skills;
            ViewBag.Experiences = experiences;
            ViewBag.ContentSettings = contentSettings;

            // Set ContentSettings for layout
            ViewData["ContentSettings"] = contentSettings;

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
