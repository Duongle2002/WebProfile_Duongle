using Microsoft.AspNetCore.Mvc;
using MyWebProfile.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace MyWebProfile.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyWebProfileContext _context;
        public AdminController(MyWebProfileContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var projectCount = await _context.Projects.CountAsync(p => p.IsActive);
            var skillCount = await _context.Skills.CountAsync(s => s.IsActive);
            var experienceCount = await _context.Experiences.CountAsync(e => e.IsActive);

            ViewBag.ProjectCount = projectCount;
            ViewBag.SkillCount = skillCount;
            ViewBag.ExperienceCount = experienceCount;

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu.";
                return View();
            }
            var claims = new List<Claim>
        {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role ?? "Admin")
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize]
        public override ViewResult View(string viewName, object model)
        {
            // Bảo vệ các view admin
            return base.View(viewName, model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> CreateDefaultAdmin()
        {
            var existingAdmin = await _context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
            if (existingAdmin == null)
            {
                var adminUser = new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "Admin"
                };
                _context.Users.Add(adminUser);
                await _context.SaveChangesAsync();
                return Content("Admin user created successfully! Username: admin, Password: admin123");
            }
            return Content("Admin user already exists!");
        }

        public async Task<IActionResult> SeedSampleData()
        {
            // Tạo Projects mẫu
            if (!await _context.Projects.AnyAsync())
            {
                var sampleProjects = new List<Project>
                {
                    new Project
                    {
                Title = "E-commerce Platform",
                Description = "Nền tảng thương mại điện tử với React và .NET Core",
                ImageUrl = "https://images.unsplash.com/photo-1563013544-824ae1b704d3?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&h=400",
                Tags = "React,.NET,SQL Server",
                Price = 2500,
                        Rating = 5.0m,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
            },
            new Project
            {
                Title = "Mobile Banking App",
                Description = "Ứng dụng ngân hàng di động với React Native",
                ImageUrl = "https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&h=400",
                Tags = "React Native,Node.js,MongoDB",
                Price = 3200,
                        Rating = 4.9m,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
            }
        };
                _context.Projects.AddRange(sampleProjects);
            }

            // Tạo Skills mẫu
            if (!await _context.Skills.AnyAsync())
            {
                var sampleSkills = new List<Skill>
        {
                    new Skill
                    {
                        Name = "React",
                        Description = "Frontend Framework",
                        Proficiency = 90,
                        Category = "Frontend",
                        Icon = "fab fa-react",
                        DisplayOrder = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
                    },
                    new Skill
                    {
                        Name = ".NET Core",
                        Description = "Backend Framework",
                        Proficiency = 85,
                        Category = "Backend",
                        Icon = "fas fa-code",
                        DisplayOrder = 2,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
                    },
                    new Skill
                    {
                        Name = "SQL Server",
                        Description = "Database Management",
                        Proficiency = 80,
                        Category = "Database",
                        Icon = "fas fa-database",
                        DisplayOrder = 3,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
                    }
                };
                _context.Skills.AddRange(sampleSkills);
            }

            // Tạo Experiences mẫu
            if (!await _context.Experiences.AnyAsync())
            {
                var sampleExperiences = new List<Experience>
        {
            new Experience
            {
                Title = "Senior Full Stack Developer",
                Company = "Tech Innovate Co.",
                Period = "2022 - Hiện tại",
                Description = "Phát triển và duy trì các ứng dụng web quy mô lớn sử dụng React, .NET Core, và Azure.",
                        StartDate = new DateTime(2022, 1, 1),
                        DisplayOrder = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
            },
            new Experience
            {
                Title = "Full Stack Developer",
                Company = "Digital Solutions Ltd.",
                Period = "2020 - 2022",
                Description = "Thiết kế và phát triển các ứng dụng web responsive cho clients trong nhiều lĩnh vực khác nhau.",
                StartDate = new DateTime(2020, 1, 1),
                        EndDate = new DateTime(2022, 1, 1),
                        DisplayOrder = 2,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
                    }
                };
                _context.Experiences.AddRange(sampleExperiences);
            }

            await _context.SaveChangesAsync();
            return Content("Sample data created successfully!");
        }

        #region Projects Management
        [Authorize]
        public async Task<IActionResult> Projects()
        {
            var projects = await _context.Projects.Where(p => !p.IsDeleted).OrderByDescending(p => p.CreatedAt).ToListAsync();
            return View(projects);
        }

        [Authorize]
        public async Task<IActionResult> ProjectsDeleted()
        {
            var projects = await _context.Projects.Where(p => p.IsDeleted).OrderByDescending(p => p.CreatedAt).ToListAsync();
            return View("ProjectsDeleted", projects);
        }

        [Authorize]
        public IActionResult CreateProject()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProject(Project project)
        {
            if (ModelState.IsValid)
            {
                project.CreatedAt = DateTime.Now;
                project.UpdatedAt = DateTime.Now;
                project.IsActive = true;
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Projects));
            }
            return View(project);
        }

        [Authorize]
        public async Task<IActionResult> EditProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProject(int id, Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    project.UpdatedAt = DateTime.Now;
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Projects));
            }
            return View(project);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            project.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Projects));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RestoreProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            project.IsDeleted = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ProjectsDeleted));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteProjectPermanently(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ProjectsDeleted));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
        #endregion

        #region Skills Management
        [Authorize]
        public async Task<IActionResult> Skills()
        {
            var skills = await _context.Skills.Where(s => !s.IsDeleted).OrderBy(s => s.DisplayOrder).ToListAsync();
            return View(skills);
        }

        [Authorize]
        public async Task<IActionResult> SkillsDeleted()
        {
            var skills = await _context.Skills.Where(s => s.IsDeleted).OrderBy(s => s.DisplayOrder).ToListAsync();
            return View("SkillsDeleted", skills);
        }

        [Authorize]
        public IActionResult CreateSkill()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSkill(Skill skill)
        {
            if (ModelState.IsValid)
            {
                skill.CreatedAt = DateTime.Now;
                skill.UpdatedAt = DateTime.Now;
                skill.IsActive = true;
                _context.Skills.Add(skill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Skills));
            }
            return View(skill);
        }

        [Authorize]
        public async Task<IActionResult> EditSkill(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
            {
                return NotFound();
            }
            return View(skill);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditSkill(int id, Skill skill)
        {
            if (id != skill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    skill.UpdatedAt = DateTime.Now;
                    _context.Update(skill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkillExists(skill.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Skills));
            }
            return View(skill);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
            {
                return NotFound();
            }
            skill.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Skills));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RestoreSkill(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
            {
                return NotFound();
            }
            skill.IsDeleted = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SkillsDeleted));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteSkillPermanently(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
            {
                return NotFound();
            }
            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SkillsDeleted));
        }

        private bool SkillExists(int id)
        {
            return _context.Skills.Any(e => e.Id == id);
        }
        #endregion

        #region Experiences Management
        [Authorize]
        public async Task<IActionResult> Experiences()
        {
            var experiences = await _context.Experiences.Where(e => !e.IsDeleted).OrderByDescending(e => e.StartDate).ToListAsync();
            return View(experiences);
        }

        [Authorize]
        public async Task<IActionResult> ExperiencesDeleted()
        {
            var experiences = await _context.Experiences.Where(e => e.IsDeleted).OrderByDescending(e => e.StartDate).ToListAsync();
            return View("ExperiencesDeleted", experiences);
        }

        [Authorize]
        public IActionResult CreateExperience()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateExperience(Experience experience)
        {
            if (ModelState.IsValid)
            {
                experience.CreatedAt = DateTime.Now;
                experience.UpdatedAt = DateTime.Now;
                experience.IsActive = true;
                _context.Experiences.Add(experience);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Experiences));
            }
            return View(experience);
        }

        [Authorize]
        public async Task<IActionResult> EditExperience(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null)
            {
                return NotFound();
            }
            return View(experience);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditExperience(int id, Experience experience)
        {
            if (id != experience.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    experience.UpdatedAt = DateTime.Now;
                    _context.Update(experience);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExperienceExists(experience.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Experiences));
            }
            return View(experience);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null)
            {
                return NotFound();
            }
            experience.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Experiences));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RestoreExperience(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null)
            {
                return NotFound();
            }
            experience.IsDeleted = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ExperiencesDeleted));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteExperiencePermanently(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null)
            {
                return NotFound();
            }
            _context.Experiences.Remove(experience);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ExperiencesDeleted));
        }

        private bool ExperienceExists(int id)
        {
            return _context.Experiences.Any(e => e.Id == id);
        }
        #endregion

        #region Theme Settings
        public IActionResult ThemeSettings()
        {
            return View(_context.ThemeSettings.FirstOrDefault() ?? new ThemeSettings());
        }

        [HttpPost]
        public async Task<IActionResult> ThemeSettings(ThemeSettings settings)
        {
            if (ModelState.IsValid)
            {
                var existingSettings = await _context.ThemeSettings.FirstOrDefaultAsync();
                if (existingSettings == null)
                {
                    _context.ThemeSettings.Add(settings);
                }
                else
                {
                    existingSettings.PrimaryColor = settings.PrimaryColor;
                    existingSettings.SecondaryColor = settings.SecondaryColor;
                    existingSettings.TextColor = settings.TextColor;
                    existingSettings.BackgroundColor = settings.BackgroundColor;
                    existingSettings.FontFamily = settings.FontFamily;
                    existingSettings.FontSize = settings.FontSize;
                    existingSettings.LineHeight = settings.LineHeight;
                    existingSettings.LetterSpacing = settings.LetterSpacing;
                    existingSettings.BorderRadius = settings.BorderRadius;
                    existingSettings.BoxShadow = settings.BoxShadow;
                    existingSettings.TransitionDuration = settings.TransitionDuration;
                    existingSettings.TransitionTimingFunction = settings.TransitionTimingFunction;
                    existingSettings.TransitionDelay = settings.TransitionDelay;
                    existingSettings.IsActive = settings.IsActive;
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ThemeSettings));
            }
            return View(settings);
        }
        #endregion

        #region Preview
        public IActionResult Preview()
        {
            ViewBag.Projects = _context.Projects.Where(p => p.IsActive).OrderByDescending(p => p.CreatedAt).ToList();
            ViewBag.Skills = _context.Skills.Where(s => s.IsActive).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.Experiences = _context.Experiences.Where(e => e.IsActive).OrderByDescending(e => e.StartDate).ToList();
            ViewBag.ThemeSettings = _context.ThemeSettings.FirstOrDefault() ?? new ThemeSettings();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPreviewData()
        {
            var data = new
            {
                projects = await _context.Projects.Where(p => p.IsActive).OrderByDescending(p => p.CreatedAt).ToListAsync(),
                skills = await _context.Skills.Where(s => s.IsActive).OrderBy(s => s.DisplayOrder).ToListAsync(),
                experiences = await _context.Experiences.Where(e => e.IsActive).OrderByDescending(e => e.StartDate).ToListAsync(),
                themeSettings = await _context.ThemeSettings.FirstOrDefaultAsync() ?? new ThemeSettings()
            };
            return Json(data);
        }
        #endregion
    }
} 