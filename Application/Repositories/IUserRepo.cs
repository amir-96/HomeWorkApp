using Application.ViewModels.User;
using Domain;
using Domain.BaseModels;
using Domain.Models;

namespace Application.Repositories
{
    public interface IUserRepo : IRepository<long, User>
  {
    Task<bool> ChangeImage(ChangeImageDTO changeImageDTO);
    Task<ServerResponse<bool>> ChangePassword(ChangePasswordDTO changePasswordDTO);
    Task<ServerResponse<bool>> AdminChangePassword(AdminChangePasswordDTO adminChangePasswordDTO);
  }
}