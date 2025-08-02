using System.ComponentModel.DataAnnotations;

namespace MyWebProfile.Models
{
    public class EmailSettings
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "SMTP Server là bắt buộc")]
        [Display(Name = "SMTP Server")]
        public string SmtpServer { get; set; } = string.Empty;

        [Required(ErrorMessage = "SMTP Port là bắt buộc")]
        [Display(Name = "SMTP Port")]
        public int SmtpPort { get; set; } = 587;

        [Required(ErrorMessage = "Email gửi là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email gửi")]
        public string FromEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên người gửi là bắt buộc")]
        [Display(Name = "Tên người gửi")]
        public string FromName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email nhận thông báo là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email nhận thông báo")]
        public string ToEmail { get; set; } = string.Empty;

        [Display(Name = "Mật khẩu email")]
        public string EmailPassword { get; set; } = string.Empty;

        [Display(Name = "Bật SSL")]
        public bool EnableSsl { get; set; } = true;

        [Display(Name = "Bật thông báo email")]
        public bool EnableEmailNotification { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
} 