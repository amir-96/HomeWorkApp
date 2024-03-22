using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.User
{
    public class EditUserDTO
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(Admin|Teacher|Student)$", ErrorMessage = "Role must be either Admin, Teacher, or Student")]
        public string Role { get; set; }

        public string Image { get; set; }
    }
}
