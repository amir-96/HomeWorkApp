using System.ComponentModel.DataAnnotations;
using Domain.BaseModels;

namespace Domain.Models
{
    public class User : BaseModel
  {
    public string UserName { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string Role { get; set; }
    public string Image {  get; set; }

    public ICollection<UserCourse> UserCourses { get; set; }

    public User()
    {
      IsActive = true;
      Image = "default.jpg";
    }
  }
}
