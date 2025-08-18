using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebProfile.Models;
using MyWebProfile.Services;

namespace MyWebProfile.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyWebProfileContext _context;
        private readonly IEmailService _emailService;

        public HomeController(MyWebProfileContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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

            // Load theme settings
            var themeSettings = await _context.ThemeSettings.FirstOrDefaultAsync();
            if (themeSettings == null)
            {
                // Create default theme settings if none exist
                themeSettings = new ThemeSettings
                {
                    PrimaryColor = "#007bff",
                    SecondaryColor = "#6c757d",
                    BackgroundColor = "#ffffff",
                    TextColor = "#333333",
                    FontFamily = "'Inter', sans-serif",
                    FontSize = 16,
                    LineHeight = "1.6",
                    LetterSpacing = "0.5px",
                    BorderRadius = "8px",
                    BoxShadow = "0 2px 4px rgba(0,0,0,0.1)",
                    TransitionDuration = "0.3s",
                    TransitionTimingFunction = "ease",
                    PrimaryButtonGradientStart = "#007bff",
                    PrimaryButtonGradientEnd = "#0056b3",
                    SecondaryButtonGradientStart = "#6c757d",
                    SecondaryButtonGradientEnd = "#545b62",
                    OutlineButtonBorderColor = "#007bff",
                    OutlineButtonTextColor = "#007bff",
                    // Page section colors
                    HeaderBackgroundColor = "#ffffff",
                    HeaderTextColor = "#333333",
                    HeroBackgroundColor = "#f8f9fa",
                    HeroTextColor = "#333333",
                    AboutBackgroundColor = "#ffffff",
                    AboutTextColor = "#333333",
                    ExperienceBackgroundColor = "#f8f9fa",
                    ExperienceTextColor = "#333333",
                    ProjectsBackgroundColor = "#ffffff",
                    ProjectsTextColor = "#333333",
                    ContactBackgroundColor = "#f8f9fa",
                    ContactTextColor = "#333333",
                    FooterBackgroundColor = "#343a40",
                    FooterTextColor = "#ffffff",
                    // Hover effects
                    LinkHoverColor = "#0056b3",
                    ButtonHoverTransform = "translateY(-2px)",
                    ButtonHoverShadow = "0 4px 8px rgba(0,0,0,0.2)",
                    CardHoverTransform = "translateY(-4px)",
                    CardHoverShadow = "0 8px 16px rgba(0,0,0,0.15)",
                    ImageHoverTransform = "scale(1.05)",
                    ImageHoverShadow = "0 6px 12px rgba(0,0,0,0.2)",
                    // Page loader settings
                    EnablePageLoader = true,
                    LoaderType = "spinner",
                    LoaderColor = "#007bff",
                    LoaderBackgroundColor = "#ffffff",
                    LoaderSize = "40px",
                    LoaderAnimationDuration = "1s",
                    LoaderFadeOutDuration = "0.5s",
                    LoaderShowOnNavigation = true,
                    LoaderShowOnAjax = true
                };
                _context.ThemeSettings.Add(themeSettings);
                await _context.SaveChangesAsync();
            }

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
            ViewBag.ThemeSettings = themeSettings;

            // Set ContentSettings for layout
            ViewData["ContentSettings"] = contentSettings;
            ViewData["ThemeSettings"] = themeSettings;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendContactMessage([FromBody] ContactMessage contactMessage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors = errors });
                }

                // Tạo tin nhắn mới
                var message = new ContactMessage
                {
                    Name = contactMessage.Name,
                    Email = contactMessage.Email,
                    Phone = contactMessage.Phone,
                    Subject = contactMessage.Subject,
                    Message = contactMessage.Message,
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    IsDeleted = false
                };

                _context.ContactMessages.Add(message);
                await _context.SaveChangesAsync();

                // Gửi email thông báo
                await _emailService.SendContactNotificationAsync(message);

                return Json(new { success = true, message = "Tin nhắn đã được gửi thành công! Chúng tôi sẽ liên hệ lại sớm nhất." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi gửi tin nhắn. Vui lòng thử lại sau." });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
