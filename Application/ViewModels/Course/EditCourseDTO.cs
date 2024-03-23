namespace Application.ViewModels.Course
{
  public class EditCourseDTO : CreateCourseDTO
  {
    public long Id { get; set; }

    public EditCourseDTO()
    {
      StartDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
      TimeSpan = 60;
      EndDate = StartDate.AddDays(TimeSpan);
    }
  }
}
