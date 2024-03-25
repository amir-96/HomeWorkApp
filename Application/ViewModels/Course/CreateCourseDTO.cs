using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.Course
{
  public class CreateCourseDTO
  {
    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    public string Title { get; set; }
    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    public string Description { get; set; }
    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    public DateOnly StartDate { get; set; }
    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    public int TimeSpan { get; set; }
    public DateOnly EndDate { get; set; }
    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    public decimal Capacity { get; set; }
    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    public string Image { get; set; }

    [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
    public long TeacherId { get; set; }

    public CreateCourseDTO()
    {
      StartDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
      TimeSpan = 60;
      Capacity = 20;
      EndDate = StartDate.AddDays(TimeSpan);
    }
  }
}
