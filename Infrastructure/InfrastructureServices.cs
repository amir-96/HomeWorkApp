using Application.Repositories;
using Infrastructure.Context;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
  public static class InfrastructureServices
  {
    public static void AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
      services.AddHttpContextAccessor();

      services.AddTransient<IUserRepo, UserRepo>();
      services.AddTransient<IAuthRepo, AuthRepo>();
      services.AddTransient<IGoogleRecaptcha, GoogleRecaptcha>();
      services.AddTransient<ICourseRepo, CourseRepo>();
      services.AddTransient<IPasswordRepo, PasswordRepo>();
      services.AddTransient<ILogRepo, LogRepo>();

      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
          options.ExpireTimeSpan = TimeSpan.FromHours(2);
          options.SlidingExpiration = true;
          options.LoginPath = "/Panel/Login";
          options.AccessDeniedPath = "/AccessDenied";
        });

      services.AddDbContext<ApplicationDbContext>(opt =>
      {
        opt.UseNpgsql(connectionString);
      });
    }
  }
}
