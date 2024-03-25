using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.User
{
  public class ChangePasswordDTO
  {
    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    public long Id { get; set; }

    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "کلمه ی عبور حداقل 8 کاراکتر است")]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "کلمه ی عبور باید 8 کاراکتر داشته باشد")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "کلمه ی عبور و تکرار آن با هم مطابقت ندارد")]
    public string NewPasswordConfirmation { get; set; }
  }
}
