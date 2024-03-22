using Application.Repositories;
using AutoMapper;
using Domain.BaseModels;
using MediatR;

namespace Application.Features.Users.Commands
{
    public class DeleteUserRequest : IRequest<ServerResponse<bool>>
  {
    public long Id { get; set; }

    public DeleteUserRequest(long id)
    {
      Id = id;
    }
  }

  public class DeleteUserRequestHandler : IRequestHandler<DeleteUserRequest, ServerResponse<bool>>
  {
    private readonly IUserRepo _userRepo;

    public DeleteUserRequestHandler(IUserRepo userRepo, IMapper mapper)
    {
      _userRepo = userRepo;
    }

    public async Task<ServerResponse<bool>> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
      var response = await _userRepo.Delete(request.Id);

      if (response == null)
      {
        return new ServerResponse<bool>(false, "خطای سرور", false);
      }

      return response;
    }
  }
}
