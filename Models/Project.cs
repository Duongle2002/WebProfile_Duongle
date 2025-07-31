using System.ComponentModel.DataAnnotations;

namespace MyWebProfile.Models
{
    public class Project
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Tên dự án là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên dự án không được vượt quá 100 ký tự")]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Mô tả dự án là bắt buộc")]
        public string Description { get; set; } = string.Empty;
        
        public string? ImageUrl { get; set; }
        
        public string? Tags { get; set; } // Lưu dưới dạng "React,.NET,SQL Server"
        
        [Required(ErrorMessage = "Giá dự án là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số dương")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Đánh giá là bắt buộc")]
        [Range(0, 5, ErrorMessage = "Đánh giá phải từ 0 đến 5")]
        public decimal Rating { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public string? GitHubUrl { get; set; }
        public string? LiveDemoUrl { get; set; }
        public string? DeployUrl { get; set; } // Link deploy (Vercel, Netlify, Heroku, etc.)
        public string? DownloadUrl { get; set; }
        public string? VideoUrl { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
} 