using Domain.BaseModels;

namespace Domain.Models
{
  public class Course : BaseModel
  {
    public string Title { get; set; }
    public string Description { get; set; }

    public long TeacherId { get; set; }
    public User Teacher { get; set; }

    public ICollection<UserCourse> UserCourses { get; set; }
  }
}
