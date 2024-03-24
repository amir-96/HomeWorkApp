using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.User
{
  public class SelfEditDTO
  {
    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    public long Id { get; set; }

    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    [RegularExpression(@"^09\d{9}$", ErrorMessage = "{0} نامعتبر است")]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "شماره ی همراه")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "نام باید حداقل 2 کاراکتر باشد")]
    public string FirstName { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "نام خانوادگی باید حداقل 82 کاراکتر باشد")]
    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    public string LastName { get; set; }

    public string Image { get; set; }

    public SelfEditDTO()
    {
      Image = "default.png";
    }
  }
}
