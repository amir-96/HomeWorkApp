using Application.Repositories;
using Infrastructure.Context;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
  public static class InfrastructureServices
  {
    public static void AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
      services.AddTransient<IUserRepo, UserRepo>();
      services.AddTransient<IPasswordRepo, PasswordRepo>();

      services.AddDbContext<ApplicationDbContext>(opt =>
      {
        opt.UseNpgsql(connectionString);
      });
    }
  }
}
