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

            // Load display settings
            var displaySettings = contentSettings.Where(c => c.Category == "Display").ToList();
            var displayConfig = new Dictionary<string, object>();
            
            foreach (var setting in displaySettings)
            {
                if (setting.Key.Contains("Active"))
                {
                    displayConfig[setting.Key] = setting.Value == "True";
                }
                else if (setting.Key.Contains("Order"))
                {
                    displayConfig[setting.Key] = int.TryParse(setting.Value, out int order) ? order : 1;
                }
            }

            // Tạo danh sách sections theo thứ tự
            var sections = new List<object>();
            
            // Hero section luôn đầu tiên
            sections.Add(new { Type = "Hero", Order = 0, Active = true });
            
            // Các sections khác theo thứ tự từ database
            var aboutOrder = displayConfig.ContainsKey("AboutOrder") ? (int)displayConfig["AboutOrder"] : 1;
            var experienceOrder = displayConfig.ContainsKey("ExperienceOrder") ? (int)displayConfig["ExperienceOrder"] : 2;
            var projectsOrder = displayConfig.ContainsKey("ProjectsOrder") ? (int)displayConfig["ProjectsOrder"] : 3;
            var contactOrder = displayConfig.ContainsKey("ContactOrder") ? (int)displayConfig["ContactOrder"] : 4;
            
            sections.Add(new { Type = "About", Order = aboutOrder, Active = displayConfig.ContainsKey("AboutActive") ? (bool)displayConfig["AboutActive"] : true });
            sections.Add(new { Type = "Experience", Order = experienceOrder, Active = displayConfig.ContainsKey("ExperienceActive") ? (bool)displayConfig["ExperienceActive"] : true });
            sections.Add(new { Type = "Projects", Order = projectsOrder, Active = displayConfig.ContainsKey("ProjectsActive") ? (bool)displayConfig["ProjectsActive"] : true });
            sections.Add(new { Type = "Contact", Order = contactOrder, Active = displayConfig.ContainsKey("ContactActive") ? (bool)displayConfig["ContactActive"] : true });
            
            // Sắp xếp theo thứ tự
            sections = sections.OrderBy(s => ((dynamic)s).Order).ToList();

            // Truyền dữ liệu qua ViewBag
            ViewBag.Projects = projects;
            ViewBag.Skills = skills;
            ViewBag.Experiences = experiences;
            ViewBag.ContentSettings = contentSettings;
            ViewBag.DisplayConfig = displayConfig;
            ViewBag.Sections = sections;

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
