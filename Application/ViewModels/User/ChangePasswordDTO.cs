using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.User
{
    public class ChangePasswordDTO
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        public string NewPasswordConfirmation { get; set; }
    }
}
