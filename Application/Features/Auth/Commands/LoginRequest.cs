using Application.Repositories;
using Application.ViewModels.Auth;
using Domain.BaseModels;
using MediatR;

namespace Application.Features.Auth.Commands
{
  public class LoginRequest : IRequest<ServerResponse<bool>>
  {
    public LoginDTO LoginDTO { get; set; }

    public LoginRequest(LoginDTO loginDTO)
    {
      LoginDTO = loginDTO;
    }
  }

  public class LoginRequestHandler : IRequestHandler<LoginRequest, ServerResponse<bool>>
  {
    private readonly IAuthRepo _authRepo;

    public LoginRequestHandler(IAuthRepo authRepo)
    {
      _authRepo = authRepo;
    }

    public async Task<ServerResponse<bool>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
      var loginResponse = await _authRepo.LoginUser(request.LoginDTO);

      return loginResponse;
    }
  }
}
