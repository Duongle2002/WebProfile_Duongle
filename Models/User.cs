using System.ComponentModel.DataAnnotations;

namespace MyWebProfile.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [MaxLength(50)]
        public string Role { get; set; } = "Admin";

        public bool IsDeleted { get; set; } = false;
    }
} 