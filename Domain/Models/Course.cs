using Domain.BaseModels;

namespace Domain.Models
{
  public class Course : BaseModel
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly StartDate { get; set; }
    public int TimeSpan { get; set; }
    public DateOnly EndDate { get; set; }
    public string Image {  get; set; }

    public long TeacherId { get; set; }
    public User Teacher { get; set; }

    public ICollection<UserCourse> UserCourses { get; set; }
    public ICollection<HomeWork> HomeWorks { get; set; }

    public Course()
    {
      StartDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
      TimeSpan = 60;
      EndDate = StartDate.AddDays(TimeSpan);
      Image = "default.jpg";
    }
  }
}