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
        
        [Required(ErrorMessage = "URL hình ảnh là bắt buộc")]
        [Url(ErrorMessage = "URL hình ảnh không hợp lệ")]
        public string ImageUrl { get; set; } = string.Empty;
        
        public string Tags { get; set; } = string.Empty; // Lưu dưới dạng "React,.NET,SQL Server"
        
        [Required(ErrorMessage = "Giá dự án là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số dương")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Đánh giá là bắt buộc")]
        [Range(0, 5, ErrorMessage = "Đánh giá phải từ 0 đến 5")]
        public decimal Rating { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public string GitHubUrl { get; set; } = string.Empty;
        public string LiveDemoUrl { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
    }
} 