using System.ComponentModel.DataAnnotations;

namespace MyWebProfile.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
        [Display(Name = "Họ và tên")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(150, ErrorMessage = "Email không được vượt quá 150 ký tự")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        [Display(Name = "Số điện thoại")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Nội dung tin nhắn là bắt buộc")]
        [StringLength(2000, ErrorMessage = "Nội dung tin nhắn không được vượt quá 2000 ký tự")]
        [Display(Name = "Nội dung tin nhắn")]
        public string Message { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Chủ đề không được vượt quá 100 ký tự")]
        [Display(Name = "Chủ đề")]
        public string? Subject { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
} 