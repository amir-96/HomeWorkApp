using Application.Repositories;
using Domain.BaseModels;
using Domain.Models;
using MediatR;

namespace Application.Features.Users.Queries
{
    public class GetAllUsersRequest : IRequest<ServerResponse<List<User>>>
  {
  }

  public class GetAllUsersRequestHandler : IRequestHandler<GetAllUsersRequest, ServerResponse<List<User>>>
  {
    private readonly IUserRepo _userRepo;

    public GetAllUsersRequestHandler(IUserRepo userRepo)
    {
      _userRepo = userRepo;
    }

    public async Task<ServerResponse<List<User>>> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
    {
      var response = await _userRepo.Get();

      if (response == null)
      {
        return new ServerResponse<List<User>> (false, "خطای سرور", null);
      }

      return response;
    }
  }
}
