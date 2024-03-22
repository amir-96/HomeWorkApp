using Application.Repositories;
using AutoMapper;
using Domain.BaseModels;
using Domain.Models;
using MediatR;

namespace Application.Features.Users.Queries
{
    public class GetUserRequest : IRequest<ServerResponse<User>>
  {
    public long Id { get; set; }

    public GetUserRequest(long id)
    {
      Id = id;
    }
  }

  public class GetUserRequestHandler : IRequestHandler<GetUserRequest, ServerResponse<User>>
  {
    private readonly IUserRepo _userRepo;

    public GetUserRequestHandler(IUserRepo userRepo, IMapper mapper)
    {
      _userRepo = userRepo;
    }

    public async Task<ServerResponse<User>> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
      var response = await _userRepo.Get(request.Id);

      if (response == null)
      {
        return new ServerResponse<User>(false, "خطای سرور", null);
      }

      return response;
    }
  }
}
