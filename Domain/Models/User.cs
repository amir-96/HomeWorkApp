using System.ComponentModel.DataAnnotations;
using Domain.BaseModels;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class User : BaseModel
  {
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; }
    [Required]
    [MaxLength(256)]
    public string Email { get; set; }
    [Required]
    [MaxLength(500)]
    public string HashedPassword { get; set; }
    public string PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Required]
    [MaxLength(40)]
    public string Role { get; set; }
    [Required]
    [MaxLength(1000)]
    public string Image {  get; set; }
    public decimal Ballance { get; set; } = new Decimal(0);

    public ICollection<Course> Courses { get; set; }

    public User()
    {
      Ballance = 0;
      IsActive = true;
      Image = "default.png";
    }
  }
}
