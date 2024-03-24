using Application.Repositories;
using Domain.BaseModels;
using Infrastructure.Context;

namespace Infrastructure.Services
{
  public class LogRepo : ILogRepo
  {
    private readonly ApplicationDbContext _context;

    public LogRepo(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task CreateLog(string message)
    {
      var log = new SystemLog(message);

      await _context.SystemLogs.AddAsync(log);

      await _context.SaveChangesAsync();
    }
  }
}
