using Domain.BaseModels;

namespace Domain.Models
{
  public class HomeWork : BaseModel
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly StartDate { get; set; }
    public int TimeSpan { get; set; }
    public DateOnly EndDate { get; set; }
    public string Image { get; set; }

    public long CourseId { get; set; }
    public Course Course { get; set; }

    public ICollection<HomeWorkAnswer> HomeWorkAnswers { get; set; }

    public HomeWork()
    {
      StartDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
      TimeSpan = 7;
      EndDate = StartDate.AddDays(TimeSpan);
      Image = "default.jpg";
    }
  }
}
