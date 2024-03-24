namespace Domain.BaseModels
{
  public class SystemLog
  {
    public long Id { get; set; }
    public string Description { get; set; }
    public DateTime Time { get; set; }

    public SystemLog(string description = "بدون توضیح")
    {
      Description = description;
      Time = DateTime.UtcNow;
    }
  }
}
