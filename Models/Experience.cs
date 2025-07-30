using System.ComponentModel.DataAnnotations;

namespace MyWebProfile.Models
{
    public class Experience
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Chức danh là bắt buộc")]
        [StringLength(100, ErrorMessage = "Chức danh không được vượt quá 100 ký tự")]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Tên công ty là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên công ty không được vượt quá 100 ký tự")]
        public string Company { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Thời gian làm việc là bắt buộc")]
        public string Period { get; set; } = string.Empty; // "2022 - Hiện tại"
        
        [Required(ErrorMessage = "Mô tả công việc là bắt buộc")]
        public string Description { get; set; } = string.Empty;
        
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public int DisplayOrder { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
} 