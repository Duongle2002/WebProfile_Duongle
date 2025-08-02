using Microsoft.EntityFrameworkCore;

namespace MyWebProfile.Models
{
    public class MyWebProfileContext : DbContext
    {
        public MyWebProfileContext(DbContextOptions<MyWebProfileContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<ThemeSettings> ThemeSettings { get; set; }
        public DbSet<ContentSettings> ContentSettings { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<EmailSettings> EmailSettings { get; set; }
    }
} 