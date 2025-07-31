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
        
        // Basic Colors
        public string PrimaryColor { get; set; } = "#007bff";
        public string SecondaryColor { get; set; } = "#6c757d";
        public string BackgroundColor { get; set; } = "#ffffff";
        public string TextColor { get; set; } = "#333333";
        
        // Button Colors
        public string ButtonPrimaryColor { get; set; } = "#007bff";
        public string ButtonPrimaryHoverColor { get; set; } = "#0056b3";
        public string ButtonPrimaryTextColor { get; set; } = "#ffffff";
        public string ButtonSecondaryColor { get; set; } = "#6c757d";
        public string ButtonSecondaryHoverColor { get; set; } = "#545b62";
        public string ButtonSecondaryTextColor { get; set; } = "#ffffff";
        public string ButtonOutlineColor { get; set; } = "#007bff";
        public string ButtonOutlineHoverColor { get; set; } = "#007bff";
        public string ButtonOutlineTextColor { get; set; } = "#007bff";
        public string ButtonOutlineHoverTextColor { get; set; } = "#ffffff";
        
        // Gradient Background
        public bool UseGradientBackground { get; set; } = false;
        public string GradientType { get; set; } = "linear"; // linear, radial, conic
        public string GradientDirection { get; set; } = "to right"; // to right, to left, to top, to bottom, 45deg, etc.
        public string GradientStartColor { get; set; } = "#667eea";
        public string GradientEndColor { get; set; } = "#764ba2";
        public string GradientMiddleColor { get; set; } = ""; // For 3-color gradients
        public string GradientPosition { get; set; } = "center"; // center, top, bottom, left, right
        
        // Typography
        public string FontFamily { get; set; } = "Arial, sans-serif";
        public int FontSize { get; set; } = 16;
        public string LineHeight { get; set; } = "1.6";
        public string LetterSpacing { get; set; } = "0.5px";
        public string HeadingFontFamily { get; set; } = "Arial, sans-serif";
        public string HeadingFontWeight { get; set; } = "600";
        
        // Visual Effects
        public string BorderRadius { get; set; } = "8px";
        public string BoxShadow { get; set; } = "0 2px 4px rgba(0,0,0,0.1)";
        public string BorderWidth { get; set; } = "1px";
        public string BorderStyle { get; set; } = "solid";
        public string BorderColor { get; set; } = "#dee2e6";
        
        // Animations
        public string TransitionDuration { get; set; } = "0.3s";
        public string TransitionTimingFunction { get; set; } = "ease";
        public string TransitionDelay { get; set; } = "0s";
        public bool EnableHoverEffects { get; set; } = true;
        public bool EnableScrollAnimations { get; set; } = true;
        
        // Layout
        public string ContainerMaxWidth { get; set; } = "1200px";
        public string SectionPadding { get; set; } = "80px 0";
        public string CardPadding { get; set; } = "20px";
        public string SpacingUnit { get; set; } = "1rem";
        
        // Advanced
        public string CustomCSS { get; set; } = "";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
} 