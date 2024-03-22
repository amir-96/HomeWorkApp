using Application.Repositories;
using Application.ViewModels.User;
using Domain.BaseModels;
using MediatR;

namespace Application.Features.Users.Commands
{
    public class ChangePasswordRequest : IRequest<ServerResponse<bool>>
  {
    public ChangePasswordDTO ChangePasswordDTO { get; set; }

    public ChangePasswordRequest(ChangePasswordDTO changePasswordDTO)
    {
      ChangePasswordDTO = changePasswordDTO;
    }
  }

  public class ChangePasswordRequestHandler : IRequestHandler<ChangePasswordRequest, ServerResponse<bool>>
  {
    private readonly IUserRepo _userRepo;

    public ChangePasswordRequestHandler(IUserRepo userRepo)
    {
      _userRepo = userRepo;
    }

    public async Task<ServerResponse<bool>> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
      var changePasswordResponse = await _userRepo.ChangePassword(request.ChangePasswordDTO);

      if (changePasswordResponse == null)
      {
        return new ServerResponse<bool>(false, "خطای سرور", false);
      }

      return changePasswordResponse;
    }
  }
}
