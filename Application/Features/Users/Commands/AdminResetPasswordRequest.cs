using Application.Repositories;
using Application.ViewModels.User;
using Domain.BaseModels;
using MediatR;

namespace Application.Features.Users.Commands
{
  public class AdminResetPasswordRequest : IRequest<ServerResponse<bool>>
  {
    public AdminChangePasswordDTO AdminChangePassword { get; set; }

    public AdminResetPasswordRequest(AdminChangePasswordDTO adminChangePassword)
    {
      AdminChangePassword = adminChangePassword;
    }
  }

  public class AdminResetPasswordRequestHandler : IRequestHandler<AdminResetPasswordRequest, ServerResponse<bool>>
  {
    private readonly IUserRepo _userRepo;
    private readonly ILogRepo _logRepo;

    public AdminResetPasswordRequestHandler(IUserRepo userRepo, ILogRepo logRepo)
    {
      _userRepo = userRepo;
      _logRepo = logRepo;
    }

    public async Task<ServerResponse<bool>> Handle(AdminResetPasswordRequest request, CancellationToken cancellationToken)
    {
      var changePasswordResponse = await _userRepo.AdminChangePassword(request.AdminChangePassword);

      if (changePasswordResponse == null)
      {
        return new ServerResponse<bool>(false, "خطای سرور", false);
      }

      await _logRepo.CreateLog($"Reset password for user id: {request.AdminChangePassword.UserId}");

      return changePasswordResponse;
    }
  }
}
