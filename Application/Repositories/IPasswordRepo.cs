namespace Application.Repositories
{
    public interface IPasswordRepo
    {
        string HashPassword(string password);
        bool Compare(string password, string hashedPassword);
    }
}
