namespace Application.Repositories
{
  public interface IGoogleRecaptcha
  {
    Task<bool> IsSatisfy();
  }
}
