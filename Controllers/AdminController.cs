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
                        DeployUrl = "https://ecommerce-demo.vercel.app",
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
                        DeployUrl = "", // Không có deploy URL
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
                        DeployUrl = "", // Không có deploy URL
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
            // Log incoming data
            System.Diagnostics.Debug.WriteLine($"=== CreateProject Debug ===");
            System.Diagnostics.Debug.WriteLine($"Title: {project.Title}");
            System.Diagnostics.Debug.WriteLine($"Description: {project.Description}");
            System.Diagnostics.Debug.WriteLine($"Price: {project.Price}");
            System.Diagnostics.Debug.WriteLine($"Rating: {project.Rating}");
            System.Diagnostics.Debug.WriteLine($"ImageUrl: {project.ImageUrl}");
            System.Diagnostics.Debug.WriteLine($"GitHubUrl: {project.GitHubUrl}");
            System.Diagnostics.Debug.WriteLine($"DeployUrl: {project.DeployUrl}");
            System.Diagnostics.Debug.WriteLine($"Tags: {project.Tags}");
            
            // Debug: Log ModelState errors
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine($"=== ModelState Errors ===");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"ModelState Error: {error.ErrorMessage}");
                    }
                }
                
                // Log specific field errors
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state?.Errors.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Field '{key}' has {state.Errors.Count} errors:");
                        foreach (var error in state.Errors)
                        {
                            System.Diagnostics.Debug.WriteLine($"  - {error.ErrorMessage}");
                        }
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"ModelState is VALID");
            }
            
            if (ModelState.IsValid)
            {
                try
            {
                project.CreatedAt = DateTime.Now;
                project.UpdatedAt = DateTime.Now;
                project.IsActive = true;
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine($"Project saved successfully with ID: {project.Id}");
                return RedirectToAction(nameof(Projects));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Database Error: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                    ModelState.AddModelError("", $"Database error: {ex.Message}");
                }
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
            System.Diagnostics.Debug.WriteLine($"=== EditProject Debug ===");
            System.Diagnostics.Debug.WriteLine($"ID: {id}, Project ID: {project.Id}");
            
            if (id != project.Id)
            {
                System.Diagnostics.Debug.WriteLine($"ID mismatch: {id} != {project.Id}");
                return NotFound();
            }

            // Log incoming data
            System.Diagnostics.Debug.WriteLine($"Title: {project.Title}");
            System.Diagnostics.Debug.WriteLine($"Description: {project.Description}");
            System.Diagnostics.Debug.WriteLine($"Price: {project.Price}");
            System.Diagnostics.Debug.WriteLine($"Rating: {project.Rating}");
            System.Diagnostics.Debug.WriteLine($"ImageUrl: {project.ImageUrl ?? "NULL"}");
            System.Diagnostics.Debug.WriteLine($"GitHubUrl: {project.GitHubUrl ?? "NULL"}");
            System.Diagnostics.Debug.WriteLine($"DeployUrl: {project.DeployUrl ?? "NULL"}");
            System.Diagnostics.Debug.WriteLine($"Tags: {project.Tags ?? "NULL"}");

            // Debug: Log ModelState errors
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine($"=== ModelState Errors ===");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"ModelState Error: {error.ErrorMessage}");
                    }
                }
                
                // Log specific field errors
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state?.Errors.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Field '{key}' has {state.Errors.Count} errors:");
                        foreach (var error in state.Errors)
                        {
                            System.Diagnostics.Debug.WriteLine($"  - {error.ErrorMessage}");
                        }
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"ModelState is VALID");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    project.UpdatedAt = DateTime.Now;
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine($"Project updated successfully");
                    return RedirectToAction(nameof(Projects));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Concurrency Error: {ex.Message}");
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Database Error: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                    ModelState.AddModelError("", $"Database error: {ex.Message}");
                }
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
        [Authorize]
        public IActionResult ThemeSettings()
        {
            var settings = _context.ThemeSettings.FirstOrDefault() ?? new ThemeSettings();
            
            // Đảm bảo các giá trị gradient mặc định được thiết lập
            if (string.IsNullOrEmpty(settings.PrimaryButtonGradientStart))
                settings.PrimaryButtonGradientStart = "#007bff";
            if (string.IsNullOrEmpty(settings.PrimaryButtonGradientEnd))
                settings.PrimaryButtonGradientEnd = "#0056b3";
            if (string.IsNullOrEmpty(settings.SecondaryButtonGradientStart))
                settings.SecondaryButtonGradientStart = "#6c757d";
            if (string.IsNullOrEmpty(settings.SecondaryButtonGradientEnd))
                settings.SecondaryButtonGradientEnd = "#545b62";
            if (string.IsNullOrEmpty(settings.OutlineButtonBorderColor))
                settings.OutlineButtonBorderColor = "#007bff";
            if (string.IsNullOrEmpty(settings.OutlineButtonTextColor))
                settings.OutlineButtonTextColor = "#007bff";
            
            // Đảm bảo các giá trị page section mặc định được thiết lập
            if (string.IsNullOrEmpty(settings.HeaderBackgroundColor))
                settings.HeaderBackgroundColor = "#ffffff";
            if (string.IsNullOrEmpty(settings.HeaderTextColor))
                settings.HeaderTextColor = "#333333";
            if (string.IsNullOrEmpty(settings.HeroBackgroundColor))
                settings.HeroBackgroundColor = "#f8f9fa";
            if (string.IsNullOrEmpty(settings.HeroTextColor))
                settings.HeroTextColor = "#333333";
            if (string.IsNullOrEmpty(settings.AboutBackgroundColor))
                settings.AboutBackgroundColor = "#ffffff";
            if (string.IsNullOrEmpty(settings.AboutTextColor))
                settings.AboutTextColor = "#333333";
            if (string.IsNullOrEmpty(settings.ExperienceBackgroundColor))
                settings.ExperienceBackgroundColor = "#f8f9fa";
            if (string.IsNullOrEmpty(settings.ExperienceTextColor))
                settings.ExperienceTextColor = "#333333";
            if (string.IsNullOrEmpty(settings.ProjectsBackgroundColor))
                settings.ProjectsBackgroundColor = "#ffffff";
            if (string.IsNullOrEmpty(settings.ProjectsTextColor))
                settings.ProjectsTextColor = "#333333";
            if (string.IsNullOrEmpty(settings.ContactBackgroundColor))
                settings.ContactBackgroundColor = "#f8f9fa";
            if (string.IsNullOrEmpty(settings.ContactTextColor))
                settings.ContactTextColor = "#333333";
            if (string.IsNullOrEmpty(settings.FooterBackgroundColor))
                settings.FooterBackgroundColor = "#343a40";
            if (string.IsNullOrEmpty(settings.FooterTextColor))
                settings.FooterTextColor = "#ffffff";
            
            // Đảm bảo các giá trị hover mặc định được thiết lập
            if (string.IsNullOrEmpty(settings.LinkHoverColor))
                settings.LinkHoverColor = "#0056b3";
            if (string.IsNullOrEmpty(settings.ButtonHoverTransform))
                settings.ButtonHoverTransform = "translateY(-2px)";
            if (string.IsNullOrEmpty(settings.ButtonHoverShadow))
                settings.ButtonHoverShadow = "0 4px 8px rgba(0,0,0,0.2)";
            if (string.IsNullOrEmpty(settings.CardHoverTransform))
                settings.CardHoverTransform = "translateY(-4px)";
            if (string.IsNullOrEmpty(settings.CardHoverShadow))
                settings.CardHoverShadow = "0 8px 16px rgba(0,0,0,0.15)";
            if (string.IsNullOrEmpty(settings.ImageHoverTransform))
                settings.ImageHoverTransform = "scale(1.05)";
            if (string.IsNullOrEmpty(settings.ImageHoverShadow))
                settings.ImageHoverShadow = "0 6px 12px rgba(0,0,0,0.2)";
            
            // Đảm bảo các giá trị loader mặc định được thiết lập
            if (string.IsNullOrEmpty(settings.LoaderType))
                settings.LoaderType = "spinner";
            if (string.IsNullOrEmpty(settings.LoaderColor))
                settings.LoaderColor = "#007bff";
            if (string.IsNullOrEmpty(settings.LoaderBackgroundColor))
                settings.LoaderBackgroundColor = "#ffffff";
            if (string.IsNullOrEmpty(settings.LoaderSize))
                settings.LoaderSize = "40px";
            if (string.IsNullOrEmpty(settings.LoaderAnimationDuration))
                settings.LoaderAnimationDuration = "1s";
            if (string.IsNullOrEmpty(settings.LoaderFadeOutDuration))
                settings.LoaderFadeOutDuration = "0.5s";
            
            return View(settings);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateThemeSettings(
            // Basic Colors
            string? PrimaryColor, string? SecondaryColor, string? BackgroundColor, string? TextColor,
            // Button Colors
            string? ButtonPrimaryColor, string? ButtonPrimaryHoverColor, string? ButtonPrimaryTextColor,
            string? ButtonSecondaryColor, string? ButtonSecondaryHoverColor, string? ButtonSecondaryTextColor,
            string? ButtonOutlineColor, string? ButtonOutlineHoverColor, string? ButtonOutlineTextColor, string? ButtonOutlineHoverTextColor,
            // Button Gradient Colors
            string? PrimaryButtonGradientStart, string? PrimaryButtonGradientEnd,
            string? SecondaryButtonGradientStart, string? SecondaryButtonGradientEnd,
            string? OutlineButtonBorderColor, string? OutlineButtonTextColor,
            // Gradient Background
            bool UseGradientBackground, string? GradientType, string? GradientDirection, string? GradientStartColor, 
            string? GradientEndColor, string? GradientMiddleColor, string? GradientPosition,
            // Typography
            string? FontFamily, int FontSize, string? LineHeight, string? LetterSpacing, 
            string? HeadingFontFamily, string? HeadingFontWeight,
            // Visual Effects
            string? BorderRadius, string? BoxShadow, string? BorderWidth, string? BorderStyle, string? BorderColor,
            // Animations
            string? TransitionDuration, string? TransitionTimingFunction, bool EnableHoverEffects, bool EnableScrollAnimations,
            // Layout
            string? ContainerMaxWidth, string? SectionPadding, string? CardPadding, string? SpacingUnit,
            // Page Section Colors
            string? HeaderBackgroundColor, string? HeaderTextColor, string? HeroBackgroundColor, string? HeroTextColor,
            string? AboutBackgroundColor, string? AboutTextColor, string? ExperienceBackgroundColor, string? ExperienceTextColor,
            string? ProjectsBackgroundColor, string? ProjectsTextColor, string? ContactBackgroundColor, string? ContactTextColor,
            string? FooterBackgroundColor, string? FooterTextColor,
            // Hover Colors
            string? LinkHoverColor, string? ButtonHoverTransform, string? ButtonHoverShadow,
            string? CardHoverTransform, string? CardHoverShadow, string? ImageHoverTransform, string? ImageHoverShadow,
            // Page Loader Settings
            bool EnablePageLoader, string? LoaderType, string? LoaderColor, string? LoaderBackgroundColor,
            string? LoaderSize, string? LoaderAnimationDuration, string? LoaderFadeOutDuration,
            bool LoaderShowOnNavigation, bool LoaderShowOnAjax,
            // Advanced
            string? CustomCSS)
        {
            try
            {
                var existingSettings = await _context.ThemeSettings.FirstOrDefaultAsync();
                if (existingSettings == null)
                {
                    existingSettings = new ThemeSettings
                    {
                        // Basic Colors
                        PrimaryColor = PrimaryColor ?? "#007bff",
                        SecondaryColor = SecondaryColor ?? "#6c757d",
                        BackgroundColor = BackgroundColor ?? "#ffffff",
                        TextColor = TextColor ?? "#333333",
                        
                        // Button Colors
                        ButtonPrimaryColor = ButtonPrimaryColor ?? "#007bff",
                        ButtonPrimaryHoverColor = ButtonPrimaryHoverColor ?? "#0056b3",
                        ButtonPrimaryTextColor = ButtonPrimaryTextColor ?? "#ffffff",
                        ButtonSecondaryColor = ButtonSecondaryColor ?? "#6c757d",
                        ButtonSecondaryHoverColor = ButtonSecondaryHoverColor ?? "#545b62",
                        ButtonSecondaryTextColor = ButtonSecondaryTextColor ?? "#ffffff",
                        ButtonOutlineColor = ButtonOutlineColor ?? "#007bff",
                        ButtonOutlineHoverColor = ButtonOutlineHoverColor ?? "#007bff",
                        ButtonOutlineTextColor = ButtonOutlineTextColor ?? "#007bff",
                        ButtonOutlineHoverTextColor = ButtonOutlineHoverTextColor ?? "#ffffff",
                        
                        // Button Gradient Colors
                        PrimaryButtonGradientStart = PrimaryButtonGradientStart ?? "#007bff",
                        PrimaryButtonGradientEnd = PrimaryButtonGradientEnd ?? "#0056b3",
                        SecondaryButtonGradientStart = SecondaryButtonGradientStart ?? "#6c757d",
                        SecondaryButtonGradientEnd = SecondaryButtonGradientEnd ?? "#545b62",
                        OutlineButtonBorderColor = OutlineButtonBorderColor ?? "#007bff",
                        OutlineButtonTextColor = OutlineButtonTextColor ?? "#007bff",
                        
                        // Gradient Background
                        UseGradientBackground = UseGradientBackground,
                        GradientType = GradientType ?? "linear",
                        GradientDirection = GradientDirection ?? "to right",
                        GradientStartColor = GradientStartColor ?? "#667eea",
                        GradientEndColor = GradientEndColor ?? "#764ba2",
                        GradientMiddleColor = GradientMiddleColor ?? "",
                        GradientPosition = GradientPosition ?? "center",
                        
                        // Typography
                        FontFamily = FontFamily ?? "Arial, sans-serif",
                        FontSize = FontSize,
                        LineHeight = LineHeight ?? "1.6",
                        LetterSpacing = LetterSpacing ?? "0.5px",
                        HeadingFontFamily = HeadingFontFamily ?? "Arial, sans-serif",
                        HeadingFontWeight = HeadingFontWeight ?? "600",
                        
                        // Visual Effects
                        BorderRadius = BorderRadius ?? "8px",
                        BoxShadow = BoxShadow ?? "0 2px 4px rgba(0,0,0,0.1)",
                        BorderWidth = BorderWidth ?? "1px",
                        BorderStyle = BorderStyle ?? "solid",
                        BorderColor = BorderColor ?? "#dee2e6",
                        
                        // Animations
                        TransitionDuration = TransitionDuration ?? "0.3s",
                        TransitionTimingFunction = TransitionTimingFunction ?? "ease",
                        EnableHoverEffects = EnableHoverEffects,
                        EnableScrollAnimations = EnableScrollAnimations,
                        
                        // Layout
                        ContainerMaxWidth = ContainerMaxWidth ?? "1200px",
                        SectionPadding = SectionPadding ?? "80px 0",
                        CardPadding = CardPadding ?? "20px",
                        SpacingUnit = SpacingUnit ?? "1rem",
                        
                        // Advanced
                        CustomCSS = CustomCSS ?? "",
                        
                        // Page Section Colors
                        HeaderBackgroundColor = HeaderBackgroundColor ?? "#ffffff",
                        HeaderTextColor = HeaderTextColor ?? "#333333",
                        HeroBackgroundColor = HeroBackgroundColor ?? "#f8f9fa",
                        HeroTextColor = HeroTextColor ?? "#333333",
                        AboutBackgroundColor = AboutBackgroundColor ?? "#ffffff",
                        AboutTextColor = AboutTextColor ?? "#333333",
                        ExperienceBackgroundColor = ExperienceBackgroundColor ?? "#f8f9fa",
                        ExperienceTextColor = ExperienceTextColor ?? "#333333",
                        ProjectsBackgroundColor = ProjectsBackgroundColor ?? "#ffffff",
                        ProjectsTextColor = ProjectsTextColor ?? "#333333",
                        ContactBackgroundColor = ContactBackgroundColor ?? "#f8f9fa",
                        ContactTextColor = ContactTextColor ?? "#333333",
                        FooterBackgroundColor = FooterBackgroundColor ?? "#343a40",
                        FooterTextColor = FooterTextColor ?? "#ffffff",
                        
                        // Hover Colors
                        LinkHoverColor = LinkHoverColor ?? "#0056b3",
                        ButtonHoverTransform = ButtonHoverTransform ?? "translateY(-2px)",
                        ButtonHoverShadow = ButtonHoverShadow ?? "0 4px 8px rgba(0,0,0,0.2)",
                        CardHoverTransform = CardHoverTransform ?? "translateY(-4px)",
                        CardHoverShadow = CardHoverShadow ?? "0 8px 16px rgba(0,0,0,0.15)",
                        ImageHoverTransform = ImageHoverTransform ?? "scale(1.05)",
                        ImageHoverShadow = ImageHoverShadow ?? "0 6px 12px rgba(0,0,0,0.2)",
                        
                        // Page Loader Settings
                        EnablePageLoader = EnablePageLoader,
                        LoaderType = LoaderType ?? "spinner",
                        LoaderColor = LoaderColor ?? "#007bff",
                        LoaderBackgroundColor = LoaderBackgroundColor ?? "#ffffff",
                        LoaderSize = LoaderSize ?? "40px",
                        LoaderAnimationDuration = LoaderAnimationDuration ?? "1s",
                        LoaderFadeOutDuration = LoaderFadeOutDuration ?? "0.5s",
                        LoaderShowOnNavigation = LoaderShowOnNavigation,
                        LoaderShowOnAjax = LoaderShowOnAjax,
                        
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _context.ThemeSettings.Add(existingSettings);
                }
                else
                {
                    // Basic Colors
                    existingSettings.PrimaryColor = PrimaryColor ?? existingSettings.PrimaryColor;
                    existingSettings.SecondaryColor = SecondaryColor ?? existingSettings.SecondaryColor;
                    existingSettings.BackgroundColor = BackgroundColor ?? existingSettings.BackgroundColor;
                    existingSettings.TextColor = TextColor ?? existingSettings.TextColor;
                    
                    // Button Colors
                    existingSettings.ButtonPrimaryColor = ButtonPrimaryColor ?? existingSettings.ButtonPrimaryColor;
                    existingSettings.ButtonPrimaryHoverColor = ButtonPrimaryHoverColor ?? existingSettings.ButtonPrimaryHoverColor;
                    existingSettings.ButtonPrimaryTextColor = ButtonPrimaryTextColor ?? existingSettings.ButtonPrimaryTextColor;
                    existingSettings.ButtonSecondaryColor = ButtonSecondaryColor ?? existingSettings.ButtonSecondaryColor;
                    existingSettings.ButtonSecondaryHoverColor = ButtonSecondaryHoverColor ?? existingSettings.ButtonSecondaryHoverColor;
                    existingSettings.ButtonSecondaryTextColor = ButtonSecondaryTextColor ?? existingSettings.ButtonSecondaryTextColor;
                    existingSettings.ButtonOutlineColor = ButtonOutlineColor ?? existingSettings.ButtonOutlineColor;
                    existingSettings.ButtonOutlineHoverColor = ButtonOutlineHoverColor ?? existingSettings.ButtonOutlineHoverColor;
                    existingSettings.ButtonOutlineTextColor = ButtonOutlineTextColor ?? existingSettings.ButtonOutlineTextColor;
                    existingSettings.ButtonOutlineHoverTextColor = ButtonOutlineHoverTextColor ?? existingSettings.ButtonOutlineHoverTextColor;
                    
                    // Button Gradient Colors
                    existingSettings.PrimaryButtonGradientStart = PrimaryButtonGradientStart ?? existingSettings.PrimaryButtonGradientStart;
                    existingSettings.PrimaryButtonGradientEnd = PrimaryButtonGradientEnd ?? existingSettings.PrimaryButtonGradientEnd;
                    existingSettings.SecondaryButtonGradientStart = SecondaryButtonGradientStart ?? existingSettings.SecondaryButtonGradientStart;
                    existingSettings.SecondaryButtonGradientEnd = SecondaryButtonGradientEnd ?? existingSettings.SecondaryButtonGradientEnd;
                    existingSettings.OutlineButtonBorderColor = OutlineButtonBorderColor ?? existingSettings.OutlineButtonBorderColor;
                    existingSettings.OutlineButtonTextColor = OutlineButtonTextColor ?? existingSettings.OutlineButtonTextColor;
                    
                    // Gradient Background
                    existingSettings.UseGradientBackground = UseGradientBackground;
                    existingSettings.GradientType = GradientType ?? existingSettings.GradientType;
                    existingSettings.GradientDirection = GradientDirection ?? existingSettings.GradientDirection;
                    existingSettings.GradientStartColor = GradientStartColor ?? existingSettings.GradientStartColor;
                    existingSettings.GradientEndColor = GradientEndColor ?? existingSettings.GradientEndColor;
                    existingSettings.GradientMiddleColor = GradientMiddleColor ?? existingSettings.GradientMiddleColor;
                    existingSettings.GradientPosition = GradientPosition ?? existingSettings.GradientPosition;
                    
                    // Typography
                    existingSettings.FontFamily = FontFamily ?? existingSettings.FontFamily;
                    existingSettings.FontSize = FontSize;
                    existingSettings.LineHeight = LineHeight ?? existingSettings.LineHeight;
                    existingSettings.LetterSpacing = LetterSpacing ?? existingSettings.LetterSpacing;
                    existingSettings.HeadingFontFamily = HeadingFontFamily ?? existingSettings.HeadingFontFamily;
                    existingSettings.HeadingFontWeight = HeadingFontWeight ?? existingSettings.HeadingFontWeight;
                    
                    // Visual Effects
                    existingSettings.BorderRadius = BorderRadius ?? existingSettings.BorderRadius;
                    existingSettings.BoxShadow = BoxShadow ?? existingSettings.BoxShadow;
                    existingSettings.BorderWidth = BorderWidth ?? existingSettings.BorderWidth;
                    existingSettings.BorderStyle = BorderStyle ?? existingSettings.BorderStyle;
                    existingSettings.BorderColor = BorderColor ?? existingSettings.BorderColor;
                    
                    // Animations
                    existingSettings.TransitionDuration = TransitionDuration ?? existingSettings.TransitionDuration;
                    existingSettings.TransitionTimingFunction = TransitionTimingFunction ?? existingSettings.TransitionTimingFunction;
                    existingSettings.EnableHoverEffects = EnableHoverEffects;
                    existingSettings.EnableScrollAnimations = EnableScrollAnimations;
                    
                    // Layout
                    existingSettings.ContainerMaxWidth = ContainerMaxWidth ?? existingSettings.ContainerMaxWidth;
                    existingSettings.SectionPadding = SectionPadding ?? existingSettings.SectionPadding;
                    existingSettings.CardPadding = CardPadding ?? existingSettings.CardPadding;
                    existingSettings.SpacingUnit = SpacingUnit ?? existingSettings.SpacingUnit;
                    
                    // Advanced
                    existingSettings.CustomCSS = CustomCSS ?? existingSettings.CustomCSS;
                    
                    // Page Section Colors
                    existingSettings.HeaderBackgroundColor = HeaderBackgroundColor ?? existingSettings.HeaderBackgroundColor;
                    existingSettings.HeaderTextColor = HeaderTextColor ?? existingSettings.HeaderTextColor;
                    existingSettings.HeroBackgroundColor = HeroBackgroundColor ?? existingSettings.HeroBackgroundColor;
                    existingSettings.HeroTextColor = HeroTextColor ?? existingSettings.HeroTextColor;
                    existingSettings.AboutBackgroundColor = AboutBackgroundColor ?? existingSettings.AboutBackgroundColor;
                    existingSettings.AboutTextColor = AboutTextColor ?? existingSettings.AboutTextColor;
                    existingSettings.ExperienceBackgroundColor = ExperienceBackgroundColor ?? existingSettings.ExperienceBackgroundColor;
                    existingSettings.ExperienceTextColor = ExperienceTextColor ?? existingSettings.ExperienceTextColor;
                    existingSettings.ProjectsBackgroundColor = ProjectsBackgroundColor ?? existingSettings.ProjectsBackgroundColor;
                    existingSettings.ProjectsTextColor = ProjectsTextColor ?? existingSettings.ProjectsTextColor;
                    existingSettings.ContactBackgroundColor = ContactBackgroundColor ?? existingSettings.ContactBackgroundColor;
                    existingSettings.ContactTextColor = ContactTextColor ?? existingSettings.ContactTextColor;
                    existingSettings.FooterBackgroundColor = FooterBackgroundColor ?? existingSettings.FooterBackgroundColor;
                    existingSettings.FooterTextColor = FooterTextColor ?? existingSettings.FooterTextColor;
                    
                    // Hover Colors
                    existingSettings.LinkHoverColor = LinkHoverColor ?? existingSettings.LinkHoverColor;
                    existingSettings.ButtonHoverTransform = ButtonHoverTransform ?? existingSettings.ButtonHoverTransform;
                    existingSettings.ButtonHoverShadow = ButtonHoverShadow ?? existingSettings.ButtonHoverShadow;
                    existingSettings.CardHoverTransform = CardHoverTransform ?? existingSettings.CardHoverTransform;
                    existingSettings.CardHoverShadow = CardHoverShadow ?? existingSettings.CardHoverShadow;
                    existingSettings.ImageHoverTransform = ImageHoverTransform ?? existingSettings.ImageHoverTransform;
                    existingSettings.ImageHoverShadow = ImageHoverShadow ?? existingSettings.ImageHoverShadow;
                    
                    // Page Loader Settings
                    existingSettings.EnablePageLoader = EnablePageLoader;
                    existingSettings.LoaderType = LoaderType ?? existingSettings.LoaderType;
                    existingSettings.LoaderColor = LoaderColor ?? existingSettings.LoaderColor;
                    existingSettings.LoaderBackgroundColor = LoaderBackgroundColor ?? existingSettings.LoaderBackgroundColor;
                    existingSettings.LoaderSize = LoaderSize ?? existingSettings.LoaderSize;
                    existingSettings.LoaderAnimationDuration = LoaderAnimationDuration ?? existingSettings.LoaderAnimationDuration;
                    existingSettings.LoaderFadeOutDuration = LoaderFadeOutDuration ?? existingSettings.LoaderFadeOutDuration;
                    existingSettings.LoaderShowOnNavigation = LoaderShowOnNavigation;
                    existingSettings.LoaderShowOnAjax = LoaderShowOnAjax;
                    
                    existingSettings.UpdatedAt = DateTime.Now;
                }
                
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cài đặt giao diện đã được lưu thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetThemeSettings()
        {
            try
            {
                var themeSettings = await _context.ThemeSettings.FirstOrDefaultAsync();
                if (themeSettings == null)
                {
                    // Return default settings
                    return Json(new
                    {
                        // Basic Colors
                        primaryColor = "#3b82f6",
                        secondaryColor = "#64748b",
                        backgroundColor = "#ffffff",
                        textColor = "#1e293b",
                        
                        // Button Colors
                        buttonPrimaryColor = "#007bff",
                        buttonPrimaryHoverColor = "#0056b3",
                        buttonPrimaryTextColor = "#ffffff",
                        buttonSecondaryColor = "#6c757d",
                        buttonSecondaryHoverColor = "#545b62",
                        buttonSecondaryTextColor = "#ffffff",
                        buttonOutlineColor = "#007bff",
                        buttonOutlineHoverColor = "#007bff",
                        buttonOutlineTextColor = "#007bff",
                        buttonOutlineHoverTextColor = "#ffffff",
                        
                        // Gradient Background
                        useGradientBackground = false,
                        gradientType = "linear",
                        gradientDirection = "to right",
                        gradientStartColor = "#667eea",
                        gradientEndColor = "#764ba2",
                        gradientMiddleColor = "",
                        gradientPosition = "center",
                        
                        // Typography
                        fontFamily = "'Inter', sans-serif",
                        fontSize = 16,
                        lineHeight = "1.6",
                        letterSpacing = "0.5px",
                        headingFontFamily = "'Inter', sans-serif",
                        headingFontWeight = "600",
                        
                        // Visual Effects
                        borderRadius = "12px",
                        boxShadow = "0 4px 8px rgba(0,0,0,0.15)",
                        borderWidth = "1px",
                        borderStyle = "solid",
                        borderColor = "#dee2e6",
                        
                        // Animations
                        transitionDuration = "0.3s",
                        transitionTimingFunction = "ease",
                        enableHoverEffects = true,
                        enableScrollAnimations = true,
                        
                        // Layout
                        containerMaxWidth = "1200px",
                        sectionPadding = "80px 0",
                        cardPadding = "20px",
                        spacingUnit = "1rem",
                        
                        // Advanced
                        customCSS = ""
                    });
                }

                return Json(new
                {
                    // Basic Colors
                    primaryColor = themeSettings.PrimaryColor,
                    secondaryColor = themeSettings.SecondaryColor,
                    backgroundColor = themeSettings.BackgroundColor,
                    textColor = themeSettings.TextColor,
                    
                    // Button Colors
                    buttonPrimaryColor = themeSettings.ButtonPrimaryColor,
                    buttonPrimaryHoverColor = themeSettings.ButtonPrimaryHoverColor,
                    buttonPrimaryTextColor = themeSettings.ButtonPrimaryTextColor,
                    buttonSecondaryColor = themeSettings.ButtonSecondaryColor,
                    buttonSecondaryHoverColor = themeSettings.ButtonSecondaryHoverColor,
                    buttonSecondaryTextColor = themeSettings.ButtonSecondaryTextColor,
                    buttonOutlineColor = themeSettings.ButtonOutlineColor,
                    buttonOutlineHoverColor = themeSettings.ButtonOutlineHoverColor,
                    buttonOutlineTextColor = themeSettings.ButtonOutlineTextColor,
                    buttonOutlineHoverTextColor = themeSettings.ButtonOutlineHoverTextColor,
                    
                    // Gradient Background
                    useGradientBackground = themeSettings.UseGradientBackground,
                    gradientType = themeSettings.GradientType,
                    gradientDirection = themeSettings.GradientDirection,
                    gradientStartColor = themeSettings.GradientStartColor,
                    gradientEndColor = themeSettings.GradientEndColor,
                    gradientMiddleColor = themeSettings.GradientMiddleColor,
                    gradientPosition = themeSettings.GradientPosition,
                    
                    // Typography
                    fontFamily = themeSettings.FontFamily,
                    fontSize = themeSettings.FontSize,
                    lineHeight = themeSettings.LineHeight,
                    letterSpacing = themeSettings.LetterSpacing,
                    headingFontFamily = themeSettings.HeadingFontFamily,
                    headingFontWeight = themeSettings.HeadingFontWeight,
                    
                    // Visual Effects
                    borderRadius = themeSettings.BorderRadius,
                    boxShadow = themeSettings.BoxShadow,
                    borderWidth = themeSettings.BorderWidth,
                    borderStyle = themeSettings.BorderStyle,
                    borderColor = themeSettings.BorderColor,
                    
                    // Animations
                    transitionDuration = themeSettings.TransitionDuration,
                    transitionTimingFunction = themeSettings.TransitionTimingFunction,
                    enableHoverEffects = themeSettings.EnableHoverEffects,
                    enableScrollAnimations = themeSettings.EnableScrollAnimations,
                    
                    // Layout
                    containerMaxWidth = themeSettings.ContainerMaxWidth,
                    sectionPadding = themeSettings.SectionPadding,
                    cardPadding = themeSettings.CardPadding,
                    spacingUnit = themeSettings.SpacingUnit,
                    
                    // Advanced
                    customCSS = themeSettings.CustomCSS
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
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
            
            // Load display settings
            var displaySettings = contentSettings.Where(c => c.Category == "Display").ToList();
            ViewBag.DisplaySettings = displaySettings;
            
            // Debug: Log display settings
            Console.WriteLine($"=== ContentSettings Action ===");
            Console.WriteLine($"Found {displaySettings.Count} display settings:");
            foreach (var setting in displaySettings)
            {
                Console.WriteLine($"- {setting.Key}: {setting.Value}");
            }
            Console.WriteLine($"=== End ContentSettings Action ===");
            
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

        #region Main Page Content Management
        [Authorize]
        public async Task<IActionResult> MainPageContent()
        {
            var contentSettings = await _context.ContentSettings
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Category)
                .ThenBy(c => c.Key)
                .ToListAsync();

            ViewBag.Categories = new List<string> { "Hero", "About", "Contact", "Navigation", "Footer", "General" };
            return View(contentSettings);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateMainPageContent(string key, string value, string category = "General")
        {
            try
            {
                var existingSetting = await _context.ContentSettings
                    .FirstOrDefaultAsync(c => c.Key == key && !c.IsDeleted);

                if (existingSetting != null)
                {
                    existingSetting.Value = value;
                    existingSetting.Category = category;
                    existingSetting.UpdatedAt = DateTime.Now;
                }
                else
                {
                    var newSetting = new ContentSettings
                    {
                        Key = key,
                        Value = value,
                        Category = category,
                        Description = $"Nội dung cho {key}",
                        IsDeleted = false,
                        UpdatedAt = DateTime.Now
                    };
                    _context.ContentSettings.Add(newSetting);
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        [Authorize]
        public async Task<IActionResult> PreviewMainPage()
        {
            // Lấy dữ liệu như trang chính
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
            ViewData["ContentSettings"] = contentSettings;

            return View("~/Views/Home/Index.cshtml");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateDisplaySettings(bool aboutActive, int aboutOrder, 
            bool experienceActive, int experienceOrder, bool projectsActive, int projectsOrder, 
            bool contactActive, int contactOrder, bool floatingNavActive, bool backToTopActive, 
            bool pageLoaderActive, bool scrollAnimationActive)
        {
            try
            {
                // Debug: Log incoming data
                Console.WriteLine("=== UpdateDisplaySettings Called ===");
                Console.WriteLine($"aboutActive: {aboutActive}, aboutOrder: {aboutOrder}");
                Console.WriteLine($"experienceActive: {experienceActive}, experienceOrder: {experienceOrder}");
                Console.WriteLine($"projectsActive: {projectsActive}, projectsOrder: {projectsOrder}");
                Console.WriteLine($"contactActive: {contactActive}, contactOrder: {contactOrder}");
                Console.WriteLine($"floatingNavActive: {floatingNavActive}, backToTopActive: {backToTopActive}");
                Console.WriteLine($"pageLoaderActive: {pageLoaderActive}, scrollAnimationActive: {scrollAnimationActive}");

                // Lưu cài đặt vào ContentSettings
                var settingsToUpdate = new Dictionary<string, string>
                {
                    { "AboutActive", aboutActive.ToString() },
                    { "AboutOrder", aboutOrder.ToString() },
                    { "ExperienceActive", experienceActive.ToString() },
                    { "ExperienceOrder", experienceOrder.ToString() },
                    { "ProjectsActive", projectsActive.ToString() },
                    { "ProjectsOrder", projectsOrder.ToString() },
                    { "ContactActive", contactActive.ToString() },
                    { "ContactOrder", contactOrder.ToString() },
                    { "FloatingNavActive", floatingNavActive.ToString() },
                    { "BackToTopActive", backToTopActive.ToString() },
                    { "PageLoaderActive", pageLoaderActive.ToString() },
                    { "ScrollAnimationActive", scrollAnimationActive.ToString() }
                };

                foreach (var setting in settingsToUpdate)
                {
                    var existingSetting = await _context.ContentSettings
                        .FirstOrDefaultAsync(c => c.Key == setting.Key && !c.IsDeleted);

                    if (existingSetting != null)
                    {
                        existingSetting.Value = setting.Value;
                        existingSetting.UpdatedAt = DateTime.Now;
                        Console.WriteLine($"Updated: {setting.Key} = {setting.Value}");
                    }
                    else
                    {
                        var newSetting = new ContentSettings
                        {
                            Key = setting.Key,
                            Value = setting.Value,
                            Category = "Display",
                            Description = $"Cài đặt hiển thị cho {setting.Key}",
                            IsDeleted = false,
                            UpdatedAt = DateTime.Now
                        };
                        _context.ContentSettings.Add(newSetting);
                        Console.WriteLine($"Created: {setting.Key} = {setting.Value}");
                    }
                }

                await _context.SaveChangesAsync();
                Console.WriteLine("=== Settings Saved Successfully ===");
                return Json(new { success = true, message = "Cài đặt đã được lưu thành công!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== Error: {ex.Message} ===");
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
        #endregion

        #region Statistics Management
        [Authorize]
        public async Task<IActionResult> Statistics()
        {
            // Lấy dữ liệu thống kê từ database
            var projectsCount = await _context.Projects.Where(p => p.IsActive && !p.IsDeleted).CountAsync();
            var experiencesCount = await _context.Experiences.Where(e => e.IsActive && !e.IsDeleted).CountAsync();
            var skillsCount = await _context.Skills.Where(s => s.IsActive && !s.IsDeleted).CountAsync();
            
            // Lấy settings từ ContentSettings cho tính toán khách hàng
            var contentSettings = await _context.ContentSettings.Where(c => !c.IsDeleted).ToListAsync();
            var baseClients = contentSettings.FirstOrDefault(x => x.Key == "BaseClients")?.Value ?? "30";
            var clientsPerYear = contentSettings.FirstOrDefault(x => x.Key == "ClientsPerYear")?.Value ?? "5";
            
            ViewBag.ProjectsCount = projectsCount;
            ViewBag.ExperiencesCount = experiencesCount;
            ViewBag.SkillsCount = skillsCount;
            ViewBag.BaseClients = baseClients;
            ViewBag.ClientsPerYear = clientsPerYear;
            
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateStatistics(string baseClients, string clientsPerYear)
        {
            try
            {
                // Cập nhật BaseClients
                var baseClientsSetting = await _context.ContentSettings
                    .FirstOrDefaultAsync(c => c.Key == "BaseClients" && !c.IsDeleted);
                
                if (baseClientsSetting != null)
                {
                    baseClientsSetting.Value = baseClients;
                    baseClientsSetting.UpdatedAt = DateTime.Now;
                }
                else
                {
                    var newBaseClients = new ContentSettings
                    {
                        Key = "BaseClients",
                        Value = baseClients,
                        Category = "About",
                        Description = "Số khách hàng cơ bản (mặc định: 30)",
                        IsDeleted = false,
                        UpdatedAt = DateTime.Now
                    };
                    _context.ContentSettings.Add(newBaseClients);
                }

                // Cập nhật ClientsPerYear
                var clientsPerYearSetting = await _context.ContentSettings
                    .FirstOrDefaultAsync(c => c.Key == "ClientsPerYear" && !c.IsDeleted);
                
                if (clientsPerYearSetting != null)
                {
                    clientsPerYearSetting.Value = clientsPerYear;
                    clientsPerYearSetting.UpdatedAt = DateTime.Now;
                }
                else
                {
                    var newClientsPerYear = new ContentSettings
                    {
                        Key = "ClientsPerYear",
                        Value = clientsPerYear,
                        Category = "About",
                        Description = "Số khách hàng tăng mỗi năm (mặc định: 5)",
                        IsDeleted = false,
                        UpdatedAt = DateTime.Now
                    };
                    _context.ContentSettings.Add(newClientsPerYear);
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Thống kê đã được cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
        #endregion

        #region Section Settings Management
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveSectionSettings(string section, string settings)
        {
            try
            {
                // Parse settings JSON
                var sectionSettings = JsonSerializer.Deserialize<Dictionary<string, object>>(settings);
                
                if (sectionSettings == null)
                {
                    return Json(new { success = false, message = "Invalid settings format" });
                }
                
                // Save to database or file system
                // For now, we'll save to a JSON file in wwwroot
                var settingsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "section-settings.json");
                var settingsDir = Path.GetDirectoryName(settingsPath);
                
                if (!string.IsNullOrEmpty(settingsDir) && !Directory.Exists(settingsDir))
                {
                    Directory.CreateDirectory(settingsDir);
                }

                // Load existing settings
                Dictionary<string, object> allSettings = new();
                if (System.IO.File.Exists(settingsPath))
                {
                    var existingJson = await System.IO.File.ReadAllTextAsync(settingsPath);
                    var deserializedSettings = JsonSerializer.Deserialize<Dictionary<string, object>>(existingJson);
                    if (deserializedSettings != null)
                    {
                        allSettings = deserializedSettings;
                    }
                }

                // Update section settings
                allSettings[section] = sectionSettings;

                // Save back to file
                var jsonString = JsonSerializer.Serialize(allSettings, new JsonSerializerOptions { WriteIndented = true });
                await System.IO.File.WriteAllTextAsync(settingsPath, jsonString);

                return Json(new { success = true, message = "Section settings saved successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetSectionSettings(string section)
        {
            try
            {
                var settingsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "section-settings.json");
                
                if (!System.IO.File.Exists(settingsPath))
                {
                    return Json(new { success = false, message = "No settings found" });
                }

                var jsonString = await System.IO.File.ReadAllTextAsync(settingsPath);
                var allSettings = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);

                if (allSettings != null && allSettings.ContainsKey(section))
                {
                    return Json(new { success = true, settings = allSettings[section] });
                }

                return Json(new { success = false, message = "Section not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region Contact Messages Management
        [Authorize]
        public async Task<IActionResult> ContactMessages()
        {
            var messages = await _context.ContactMessages
                .Where(m => !m.IsDeleted)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
            
            ViewBag.Messages = messages;
            ViewBag.UnreadCount = messages.Count(m => !m.IsRead);
            return View();
        }

        [Authorize]
        public async Task<IActionResult> ContactMessagesDeleted()
        {
            var messages = await _context.ContactMessages
                .Where(m => m.IsDeleted)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
            
            ViewBag.Messages = messages;
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message != null)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MarkAsUnread(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message != null)
            {
                message.IsRead = false;
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteContactMessage(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message != null)
            {
                message.IsDeleted = true;
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RestoreContactMessage(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message != null)
            {
                message.IsDeleted = false;
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteContactMessagePermanently(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message != null)
            {
                _context.ContactMessages.Remove(message);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [Authorize]
        public async Task<IActionResult> EmailNotification()
        {
            // Email Notification data
            var unreadMessageCount = await _context.ContactMessages.CountAsync(m => !m.IsRead && !m.IsDeleted);
            var totalMessageCount = await _context.ContactMessages.CountAsync(m => !m.IsDeleted);
            var emailSettings = await _context.EmailSettings.FirstOrDefaultAsync();
            var recentMessages = await _context.ContactMessages
                .Where(m => !m.IsDeleted)
                .OrderByDescending(m => m.CreatedAt)
                .Take(5)
                .ToListAsync();
            
            ViewBag.UnreadMessageCount = unreadMessageCount;
            ViewBag.TotalMessageCount = totalMessageCount;
            ViewBag.EmailNotificationEnabled = emailSettings?.EnableEmailNotification ?? false;
            ViewBag.SmtpServer = emailSettings?.SmtpServer;
            ViewBag.EmailSettings = emailSettings;
            ViewBag.RecentMessages = recentMessages;
            
            return View();
        }

        [Authorize]
        public async Task<IActionResult> EmailSettings()
        {
            var emailSettings = await _context.EmailSettings.FirstOrDefaultAsync();
            if (emailSettings == null)
            {
                emailSettings = new EmailSettings
                {
                    SmtpServer = "smtp.gmail.com",
                    SmtpPort = 587,
                    FromEmail = "",
                    FromName = "",
                    ToEmail = "",
                    EmailPassword = "",
                    EnableSsl = true,
                    EnableEmailNotification = true
                };
                _context.EmailSettings.Add(emailSettings);
                await _context.SaveChangesAsync();
            }
            
            ViewBag.EmailSettings = emailSettings;
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateEmailSettings(EmailSettings emailSettings)
        {
            if (ModelState.IsValid)
            {
                var existingSettings = await _context.EmailSettings.FirstOrDefaultAsync();
                if (existingSettings != null)
                {
                    existingSettings.SmtpServer = emailSettings.SmtpServer;
                    existingSettings.SmtpPort = emailSettings.SmtpPort;
                    existingSettings.FromEmail = emailSettings.FromEmail;
                    existingSettings.FromName = emailSettings.FromName;
                    existingSettings.ToEmail = emailSettings.ToEmail;
                    existingSettings.EmailPassword = emailSettings.EmailPassword;
                    existingSettings.EnableSsl = emailSettings.EnableSsl;
                    existingSettings.EnableEmailNotification = emailSettings.EnableEmailNotification;
                    existingSettings.UpdatedAt = DateTime.Now;
                }
                else
                {
                    emailSettings.CreatedAt = DateTime.Now;
                    emailSettings.UpdatedAt = DateTime.Now;
                    _context.EmailSettings.Add(emailSettings);
                }
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cài đặt email đã được cập nhật thành công!";
                return RedirectToAction("EmailSettings");
            }
            return View("EmailSettings", emailSettings);
        }
        #endregion

        #region Image Upload Management
        [Authorize]
        public IActionResult ImageUpload()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file, string category = "General")
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return Json(new { success = false, message = "Vui lòng chọn file để tải lên!" });
                }

                // Kiểm tra định dạng file
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return Json(new { success = false, message = "Chỉ chấp nhận file hình ảnh: JPG, PNG, GIF, WEBP!" });
                }

                // Kiểm tra kích thước file (tối đa 5MB)
                if (file.Length > 5 * 1024 * 1024)
                {
                    return Json(new { success = false, message = "File quá lớn! Tối đa 5MB." });
                }

                // Tạo tên file unique
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", category.ToLower());
                
                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, fileName);
                
                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Trả về URL của file
                var fileUrl = $"/uploads/{category.ToLower()}/{fileName}";
                
                return Json(new { 
                    success = true, 
                    message = "Tải lên thành công!", 
                    url = fileUrl,
                    fileName = fileName,
                    originalName = file.FileName,
                    size = file.Length,
                    category = category
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUploadedImages(string category = "all")
        {
            try
            {
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                var images = new List<object>();

                if (Directory.Exists(uploadsPath))
                {
                    var directories = category == "all" 
                        ? Directory.GetDirectories(uploadsPath) 
                        : new[] { Path.Combine(uploadsPath, category.ToLower()) };

                    foreach (var dir in directories)
                    {
                        if (Directory.Exists(dir))
                        {
                            var categoryName = Path.GetFileName(dir);
                            var files = Directory.GetFiles(dir, "*.*")
                                .Where(f => new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" }
                                .Contains(Path.GetExtension(f).ToLowerInvariant()))
                                .Select(f => new
                                {
                                    name = Path.GetFileName(f),
                                    url = $"/uploads/{categoryName}/{Path.GetFileName(f)}",
                                    size = new FileInfo(f).Length,
                                    category = categoryName,
                                    uploadDate = System.IO.File.GetCreationTime(f)
                                })
                                .OrderByDescending(f => f.uploadDate);

                            images.AddRange(files);
                        }
                    }
                }

                return Json(new { success = true, images = images });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteImage(string fileName, string category)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", category.ToLower(), fileName);
                
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return Json(new { success = true, message = "Xóa hình ảnh thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy file!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
        #endregion
        #endregion
    }
} 