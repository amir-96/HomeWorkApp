using Application.Repositories;
using MediatR;

namespace Application.Features.Auth.Commands
{
  public class LogoutRequest : IRequest<bool>
  {
  }

  public class LogoutRequestHandler : IRequestHandler<LogoutRequest, bool>
  {
    private readonly IAuthRepo _authRepo;

    public LogoutRequestHandler(IAuthRepo authRepo)
    {
      _authRepo = authRepo;
    }

    public async Task<bool> Handle(LogoutRequest request, CancellationToken cancellationToken)
    {
      return await _authRepo.LogoutUser();
    }
  }
}
