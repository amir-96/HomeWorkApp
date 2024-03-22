using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.User
{
    public class CreateUserDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }

        [Compare("Password")]
        public string PasswordConfirmation { get; set; }

        [Required]
        [RegularExpression("^(Admin|Teacher|Student)$", ErrorMessage = "Role must be either Admin, Teacher, or Student")]
        public string Role { get; set; }

        public string Image { get; set; }
    }
}
