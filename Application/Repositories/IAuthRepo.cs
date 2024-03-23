using Application.ViewModels.Auth;
using Domain.BaseModels;

namespace Application.Repositories
{
  public interface IAuthRepo
  {
    Task<ServerResponse<bool>> LoginUser(LoginDTO loginDTO);
    Task<bool> LogoutUser();
  }
}
