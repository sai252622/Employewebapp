using System.ComponentModel.DataAnnotations;

namespace Employewebapp.Models
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; }

        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
