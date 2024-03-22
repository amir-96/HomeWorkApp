namespace Domain.Models
{
  public class UserCourse
  {
    public long Id { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }

    public long CourseId { get; set; }
    public Course Course { get; set; }
  }
}
