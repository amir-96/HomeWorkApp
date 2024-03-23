using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.Auth
{
  public class LoginDTO
  {
    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "نام کاربری باید حداقل 8 کاراکتر باشد")]
    public string UserNameOrEmail { get; set; }

    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "کلمه ی عبور باید حداقل 8 کاراکتر باشد")]
    public string Password { get; set; }

    public bool IsPersistent { get; set; }

    public LoginDTO()
    {
      IsPersistent = false;
    }
  }
}
