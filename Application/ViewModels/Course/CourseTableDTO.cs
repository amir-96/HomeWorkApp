namespace Application.ViewModels.Course
{
  public class CourseTableDTO
  {
    public Domain.Models.Course Course { get; set; }
    public Domain.Models.User Teacher { get; set; }
    public List<Domain.Models.User> Students { get; set; }
  }
}
