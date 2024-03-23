namespace Application.ViewModels.Course
{
  public class CreateCourseDTO
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly StartDate { get; set; }
    public int TimeSpan { get; set; }
    public DateOnly EndDate { get; set; }

    public long TeacherId { get; set; }

    public CreateCourseDTO()
    {
      StartDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
      TimeSpan = 60;
      EndDate = StartDate.AddDays(TimeSpan);
    }
  }
}
