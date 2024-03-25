using Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
  public class HomeWorkAnswer : BaseModel
  {
    public string Description { get; set; }
    public string File { get; set; }
    public int Score { get; set; }

    public long HomeWorkId { get; set; }
    [ForeignKey("HomeWorkId")]
    public HomeWork HomeWork { get; set; }

    public HomeWorkAnswer()
    {
      File = "default.zip";
    }
  }
}
