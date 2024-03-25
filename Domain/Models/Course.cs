using Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

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
    public decimal Price { get; set; }
    public int Capacity { get; set; }

    public long TeacherId { get; set; }
    [ForeignKey("TeacherId")]
    public User Teacher { get; set; }

    public ICollection<User> Users { get; set; }
    public ICollection<HomeWork> HomeWorks { get; set; }

    public Course()
    {
      Price = 0;
      Capacity = 20;
      StartDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
      TimeSpan = 60;
      EndDate = StartDate.AddDays(TimeSpan);
      Image = "default.jpg";
    }
  }
}