using Application.Repositories;
using AutoMapper;
using Domain.BaseModels;
using MediatR;

namespace Application.Features.Users.Commands
{
    public class RestoreUserRequest : IRequest<ServerResponse<bool>>
  {
    public long Id { get; set; }

    public RestoreUserRequest(long id)
    {
      Id = id;
    }
  }

  public class RestoreUserRequestHandler : IRequestHandler<RestoreUserRequest, ServerResponse<bool>>
  {
    private readonly IUserRepo _userRepo;

    public RestoreUserRequestHandler(IUserRepo userRepo, IMapper mapper)
    {
      _userRepo = userRepo;
    }

    public async Task<ServerResponse<bool>> Handle(RestoreUserRequest request, CancellationToken cancellationToken)
    {
      var response = await _userRepo.Restore(request.Id);

      if (response == null)
      {
        return new ServerResponse<bool>(false, "خطای سرور", false);
      }

      return response;
    }
  }
}
