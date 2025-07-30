using System.ComponentModel.DataAnnotations;

namespace MyWebProfile.Models
{
    public class ContentSettings
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Tên cài đặt là bắt buộc")]
        public string Key { get; set; } = string.Empty;
        
        public string Value { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public string Category { get; set; } = string.Empty; // Hero, About, Contact, etc.
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
    
    public class ThemeSettings
    {
        public int Id { get; set; }
        
        public string PrimaryColor { get; set; } = "#007bff";
        public string SecondaryColor { get; set; } = "#6c757d";
        public string BackgroundColor { get; set; } = "#ffffff";
        public string TextColor { get; set; } = "#333333";
        public string FontFamily { get; set; } = "Arial, sans-serif";
        public int FontSize { get; set; } = 16;
        public string LineHeight { get; set; } = "1.6";
        public string LetterSpacing { get; set; } = "0.5px";
        public string BorderRadius { get; set; } = "8px";
        public string BoxShadow { get; set; } = "0 2px 4px rgba(0,0,0,0.1)";
        public string TransitionDuration { get; set; } = "0.3s";
        public string TransitionTimingFunction { get; set; } = "ease";
        public string TransitionDelay { get; set; } = "0s";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
} 