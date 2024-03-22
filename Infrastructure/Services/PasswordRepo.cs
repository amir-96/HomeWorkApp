using Application.Repositories;

namespace Infrastructure.Services
{
  public class PasswordRepo : IPasswordRepo
  {
    public string HashPassword(string password)
    {
      return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Compare(string password, string hashedPassword)
    {
      return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
  }
}
