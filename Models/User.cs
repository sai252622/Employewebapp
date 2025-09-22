using System.ComponentModel.DataAnnotations;

namespace Employewebapp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public string? ResetToken { get; set; }

        public DateTime? ResetTokenExpiry { get; set; }

        public int RoleId { get; set; }
    }
}
