namespace Application.Repositories
{
  public interface ILogRepo
  {
    Task CreateLog(string message);
  }
}
