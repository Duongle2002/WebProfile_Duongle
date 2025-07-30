using System.ComponentModel.DataAnnotations;

namespace MyWebProfile.Models
{
    public class Skill
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Tên kỹ năng là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên kỹ năng không được vượt quá 50 ký tự")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Mô tả kỹ năng là bắt buộc")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Mức độ thành thạo là bắt buộc")]
        [Range(0, 100, ErrorMessage = "Mức độ thành thạo phải từ 0 đến 100")]
        public int Proficiency { get; set; }
        
        [Required(ErrorMessage = "Loại kỹ năng là bắt buộc")]
        public string Category { get; set; } = string.Empty; // Frontend, Backend, Database, etc.
        
        public string Icon { get; set; } = string.Empty; // CSS class hoặc icon name
        
        public int DisplayOrder { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
} 