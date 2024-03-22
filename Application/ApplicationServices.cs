using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
  public static class ApplicationServices
  {
    public static void AddApplicationServices(this IServiceCollection services)
    {
      services
        .AddAutoMapper(Assembly.GetExecutingAssembly())
        .AddMediatR(Assembly.GetExecutingAssembly());
    }
  }
}
