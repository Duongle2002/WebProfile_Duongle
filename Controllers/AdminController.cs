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
            var projectCount = await _context.Projects.CountAsync(p => p.IsActive && !p.IsDeleted);
            var skillCount = await _context.Skills.CountAsync(s => s.IsActive && !s.IsDeleted);
            var experienceCount = await _context.Experiences.CountAsync(e => e.IsActive && !e.IsDeleted);

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
        public override ViewResult View(string? viewName, object? model)
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
            // Seed Projects
            if (!_context.Projects.Any())
            {
                var projects = new List<Project>
                {
                    new Project
                    {
                        Title = "E-Commerce Website",
                        Description = "A full-featured e-commerce platform built with ASP.NET Core and React",
                        ImageUrl = "https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&h=300",
                        GitHubUrl = "https://github.com/example/ecommerce",
                        Tags = "ASP.NET Core, React, SQL Server, Bootstrap",
                        Price = 2500,
                        Rating = 4.8m,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-30)
                    },
                    new Project
                    {
                        Title = "Portfolio Website",
                        Description = "A modern portfolio website showcasing my work and skills",
                        ImageUrl = "https://images.unsplash.com/photo-1467232004584-a241de8bcf5d?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&h=300",
                        GitHubUrl = "https://github.com/example/portfolio",
                        Tags = "HTML, CSS, JavaScript, Bootstrap",
                        Price = 1500,
                        Rating = 4.9m,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-20)
                    },
                    new Project
                    {
                        Title = "Task Management App",
                        Description = "A collaborative task management application with real-time updates",
                        ImageUrl = "https://images.unsplash.com/photo-1551288049-bebda4e38f71?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&h=300",
                        GitHubUrl = "https://github.com/example/taskmanager",
                        Tags = "Node.js, React, MongoDB, Socket.io",
                        Price = 3000,
                        Rating = 4.7m,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-10)
                    }
                };
                _context.Projects.AddRange(projects);
            }

            // Seed Skills
            if (!_context.Skills.Any())
            {
                var skills = new List<Skill>
                {
                    new Skill { Name = "C#", Description = "Backend Development", Proficiency = 90, Category = "Backend", DisplayOrder = 1, IsActive = true },
                    new Skill { Name = "ASP.NET Core", Description = "Web Framework", Proficiency = 85, Category = "Backend", DisplayOrder = 2, IsActive = true },
                    new Skill { Name = "JavaScript", Description = "Frontend Development", Proficiency = 80, Category = "Frontend", DisplayOrder = 3, IsActive = true },
                    new Skill { Name = "React", Description = "Frontend Framework", Proficiency = 75, Category = "Frontend", DisplayOrder = 4, IsActive = true },
                    new Skill { Name = "SQL Server", Description = "Database Management", Proficiency = 85, Category = "Database", DisplayOrder = 5, IsActive = true },
                    new Skill { Name = "HTML/CSS", Description = "Web Development", Proficiency = 90, Category = "Frontend", DisplayOrder = 6, IsActive = true }
                };
                _context.Skills.AddRange(skills);
            }

            // Seed Experiences
            if (!_context.Experiences.Any())
            {
                var experiences = new List<Experience>
                {
                    new Experience
                    {
                        Company = "Tech Solutions Inc.",
                        Title = "Senior Full Stack Developer",
                        Description = "Led development of enterprise applications using ASP.NET Core and React",
                        Period = "2022 - Hiện tại",
                        StartDate = DateTime.Now.AddYears(-2),
                        EndDate = DateTime.Now,
                        IsActive = true
                    },
                    new Experience
                    {
                        Company = "Digital Innovations",
                        Title = "Full Stack Developer",
                        Description = "Developed web applications and maintained existing systems",
                        Period = "2020 - 2022",
                        StartDate = DateTime.Now.AddYears(-4),
                        EndDate = DateTime.Now.AddYears(-2),
                        IsActive = true
                    }
                };
                _context.Experiences.AddRange(experiences);
            }

            // Seed ContentSettings
            if (!_context.ContentSettings.Any())
            {
                var contentSettings = new List<ContentSettings>
                {
                    new ContentSettings
                    {
                        Key = "HeroName",
                        Value = "Lê Hữu Dương",
                        Description = "Tên hiển thị trong Hero section",
                        Category = "Hero",
                        UpdatedAt = DateTime.Now
                    },
                    new ContentSettings
                    {
                        Key = "HeroLocation",
                        Value = "San Francisco, CA",
                        Description = "Vị trí hiển thị trong Hero section",
                        Category = "Hero",
                        UpdatedAt = DateTime.Now
                    },
                    new ContentSettings
                    {
                        Key = "HeroTitle",
                        Value = "Full Stack Developer & UI/UX Designer",
                        Description = "Chức danh hiển thị trong Hero section",
                        Category = "Hero",
                        UpdatedAt = DateTime.Now
                    },
                    new ContentSettings
                    {
                        Key = "HeroImage",
                        Value = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?ixlib=rb-4.0.3&auto=format&fit=crop&w=200&h=200",
                        Description = "URL ảnh đại diện trong Hero section",
                        Category = "Hero",
                        UpdatedAt = DateTime.Now
                    },
                    new ContentSettings
                    {
                        Key = "HeroProjects",
                        Value = "23",
                        Description = "Số dự án hiển thị trong Hero section",
                        Category = "Hero",
                        UpdatedAt = DateTime.Now
                    },
                    new ContentSettings
                    {
                        Key = "HeroExperience",
                        Value = "5+",
                        Description = "Số năm kinh nghiệm hiển thị trong Hero section",
                        Category = "Hero",
                        UpdatedAt = DateTime.Now
                    },
                    new ContentSettings
                    {
                        Key = "HeroClients",
                        Value = "50+",
                        Description = "Số khách hàng hiển thị trong Hero section",
                        Category = "Hero",
                        UpdatedAt = DateTime.Now
                    },
                    new ContentSettings
                    {
                        Key = "HeroPortfolioButton",
                        Value = "Xem Portfolio",
                        Description = "Text nút Portfolio trong Hero section",
                        Category = "Hero",
                        UpdatedAt = DateTime.Now
                    },
                    new ContentSettings
                    {
                        Key = "HeroContactButton",
                        Value = "Liên hệ ngay",
                        Description = "Text nút Liên hệ trong Hero section",
                        Category = "Hero",
                        UpdatedAt = DateTime.Now
                    },
                    new ContentSettings
                    {
                        Key = "NavBrand",
                        Value = "Duong",
                        Description = "Tên hiển thị trong navigation bar",
                        Category = "Navigation",
                        UpdatedAt = DateTime.Now
                    }
                };
                _context.ContentSettings.AddRange(contentSettings);
            }

            await _context.SaveChangesAsync();
            return Content("Sample data seeded successfully!");
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
            ViewBag.Projects = _context.Projects.Where(p => p.IsActive && !p.IsDeleted).OrderByDescending(p => p.CreatedAt).ToList();
            ViewBag.Skills = _context.Skills.Where(s => s.IsActive && !s.IsDeleted).OrderBy(s => s.DisplayOrder).ToList();
            ViewBag.Experiences = _context.Experiences.Where(e => e.IsActive && !e.IsDeleted).OrderByDescending(e => e.StartDate).ToList();
            ViewBag.ThemeSettings = _context.ThemeSettings.FirstOrDefault() ?? new ThemeSettings();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPreviewData()
        {
            var data = new
            {
                projects = await _context.Projects.Where(p => p.IsActive && !p.IsDeleted).OrderByDescending(p => p.CreatedAt).ToListAsync(),
                skills = await _context.Skills.Where(s => s.IsActive && !s.IsDeleted).OrderBy(s => s.DisplayOrder).ToListAsync(),
                experiences = await _context.Experiences.Where(e => e.IsActive && !e.IsDeleted).OrderByDescending(e => e.StartDate).ToListAsync(),
                themeSettings = await _context.ThemeSettings.FirstOrDefaultAsync() ?? new ThemeSettings()
            };
            return Json(data);
        }
        #endregion

        #region Users Management
        [Authorize]
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.Where(u => !u.IsDeleted).OrderBy(u => u.Username).ToListAsync();
            return View(users);
        }

        [Authorize]
        public async Task<IActionResult> UsersDeleted()
        {
            var users = await _context.Users.Where(u => u.IsDeleted).OrderBy(u => u.Username).ToListAsync();
            return View(users);
        }

        [Authorize]
        public IActionResult CreateUser()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if username already exists
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
                    return View(user);
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                user.Role = user.Role ?? "Admin";
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Users));
            }
            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditUser(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.FindAsync(id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Check if username already exists for other users
                    var duplicateUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username && u.Id != id);
                    if (duplicateUser != null)
                    {
                        ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
                        return View(user);
                    }

                    existingUser.Username = user.Username;
                    existingUser.Role = user.Role;

                    // Only update password if provided
                    if (!string.IsNullOrEmpty(user.PasswordHash))
                    {
                        existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Users));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(user);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Users));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RestoreUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.IsDeleted = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UsersDeleted));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteUserPermanently(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UsersDeleted));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        #endregion

        #region Content Settings
        [Authorize]
        public async Task<IActionResult> ContentSettings()
        {
            var contentSettings = await _context.ContentSettings.Where(c => !c.IsDeleted).OrderBy(c => c.Category).ThenBy(c => c.Key).ToListAsync();
            return View(contentSettings);
        }

        [Authorize]
        public IActionResult CreateContentSetting()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateContentSetting(ContentSettings contentSetting)
        {
            if (ModelState.IsValid)
            {
                contentSetting.UpdatedAt = DateTime.Now;
                _context.ContentSettings.Add(contentSetting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ContentSettings));
            }
            return View(contentSetting);
        }

        [Authorize]
        public async Task<IActionResult> EditContentSetting(int id)
        {
            var contentSetting = await _context.ContentSettings.FindAsync(id);
            if (contentSetting == null)
            {
                return NotFound();
            }
            return View(contentSetting);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditContentSetting(int id, ContentSettings contentSetting)
        {
            if (id != contentSetting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSetting = await _context.ContentSettings.FindAsync(id);
                    if (existingSetting == null)
                    {
                        return NotFound();
                    }

                    existingSetting.Key = contentSetting.Key;
                    existingSetting.Value = contentSetting.Value;
                    existingSetting.Description = contentSetting.Description;
                    existingSetting.Category = contentSetting.Category;
                    existingSetting.UpdatedAt = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ContentSettings));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentSettingExists(contentSetting.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(contentSetting);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteContentSetting(int id)
        {
            var contentSetting = await _context.ContentSettings.FindAsync(id);
            if (contentSetting == null)
            {
                return NotFound();
            }
            contentSetting.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ContentSettings));
        }

        private bool ContentSettingExists(int id)
        {
            return _context.ContentSettings.Any(e => e.Id == id);
        }
        #endregion

        #region Hero Settings
        [Authorize]
        public async Task<IActionResult> HeroSettings()
        {
            var heroSettings = await _context.ContentSettings
                .Where(c => c.Category == "Hero" && !c.IsDeleted)
                .OrderBy(c => c.Key)
                .ToListAsync();

            // Get project count for stats
            var projectCount = await _context.Projects.CountAsync(p => p.IsActive && !p.IsDeleted);
            ViewBag.ProjectCount = projectCount;

            return View(heroSettings);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateHeroSetting(string key, string value)
        {
            var setting = await _context.ContentSettings.FirstOrDefaultAsync(c => c.Key == key && c.Category == "Hero");
            if (setting == null)
            {
                setting = new ContentSettings
                {
                    Key = key,
                    Category = "Hero",
                    UpdatedAt = DateTime.Now
                };
                _context.ContentSettings.Add(setting);
            }

            setting.Value = value;
            setting.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
        #endregion

        #region Debug Actions
        [Authorize]
        public async Task<IActionResult> DebugData()
        {
            var debugInfo = new
            {
                Projects = new
                {
                    Total = await _context.Projects.CountAsync(),
                    Active = await _context.Projects.CountAsync(p => p.IsActive),
                    NotDeleted = await _context.Projects.CountAsync(p => !p.IsDeleted),
                    ActiveAndNotDeleted = await _context.Projects.CountAsync(p => p.IsActive && !p.IsDeleted),
                    Deleted = await _context.Projects.CountAsync(p => p.IsDeleted),
                    Inactive = await _context.Projects.CountAsync(p => !p.IsActive)
                },
                Skills = new
                {
                    Total = await _context.Skills.CountAsync(),
                    Active = await _context.Skills.CountAsync(s => s.IsActive),
                    NotDeleted = await _context.Skills.CountAsync(s => !s.IsDeleted),
                    ActiveAndNotDeleted = await _context.Skills.CountAsync(s => s.IsActive && !s.IsDeleted),
                    Deleted = await _context.Skills.CountAsync(s => s.IsDeleted),
                    Inactive = await _context.Skills.CountAsync(s => !s.IsActive)
                },
                Experiences = new
                {
                    Total = await _context.Experiences.CountAsync(),
                    Active = await _context.Experiences.CountAsync(e => e.IsActive),
                    NotDeleted = await _context.Experiences.CountAsync(e => !e.IsDeleted),
                    ActiveAndNotDeleted = await _context.Experiences.CountAsync(e => e.IsActive && !e.IsDeleted),
                    Deleted = await _context.Experiences.CountAsync(e => e.IsDeleted),
                    Inactive = await _context.Experiences.CountAsync(e => !e.IsActive)
                },
                ContentSettings = new
                {
                    Total = await _context.ContentSettings.CountAsync(),
                    NotDeleted = await _context.ContentSettings.CountAsync(c => !c.IsDeleted),
                    Deleted = await _context.ContentSettings.CountAsync(c => c.IsDeleted)
                }
            };

            return Json(debugInfo);
        }

        [Authorize]
        public async Task<IActionResult> ResetData()
        {
            // Reset all IsDeleted flags to false
            var deletedProjects = await _context.Projects.Where(p => p.IsDeleted).ToListAsync();
            foreach (var project in deletedProjects)
            {
                project.IsDeleted = false;
            }

            var deletedSkills = await _context.Skills.Where(s => s.IsDeleted).ToListAsync();
            foreach (var skill in deletedSkills)
            {
                skill.IsDeleted = false;
            }

            var deletedExperiences = await _context.Experiences.Where(e => e.IsDeleted).ToListAsync();
            foreach (var experience in deletedExperiences)
            {
                experience.IsDeleted = false;
            }

            var deletedContentSettings = await _context.ContentSettings.Where(c => c.IsDeleted).ToListAsync();
            foreach (var setting in deletedContentSettings)
            {
                setting.IsDeleted = false;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion
    }
} 