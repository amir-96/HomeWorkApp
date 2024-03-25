using Application.Repositories;
using Application.ViewModels.User;
using AutoMapper;
using Domain.BaseModels;
using MediatR;

namespace Application.Features.Users.Commands
{
  public class SelfEditResuest : IRequest<ServerResponse<bool>>
  {
    public SelfEditDTO SelfEditDTO { get; set; }

    public SelfEditResuest(SelfEditDTO selfEditDTO)
    {
      SelfEditDTO = selfEditDTO;
    }
  }

  public class SelfEditResuestHandler : IRequestHandler<SelfEditResuest, ServerResponse<bool>>
  {
    private readonly IUserRepo _userRepo;

    public SelfEditResuestHandler(IUserRepo userRepo, IMapper mapper)
    {
      _userRepo = userRepo;
    }

    public async Task<ServerResponse<bool>> Handle(SelfEditResuest request, CancellationToken cancellationToken)
    {
      var oldUserResponse = await _userRepo.Get(request.SelfEditDTO.Id);

      var editedUser = request.SelfEditDTO;

      if (oldUserResponse.IsSucceeded == false)
      {
        return new ServerResponse<bool>(false, "خطای سرور", false);
      }

      oldUserResponse.Data.PhoneNumber = editedUser.PhoneNumber;
      oldUserResponse.Data.FirstName = editedUser.FirstName;
      oldUserResponse.Data.LastName = editedUser.LastName;
      oldUserResponse.Data.UpdatedAt = DateTime.UtcNow;

      var response = await _userRepo.Update(oldUserResponse.Data.Id, oldUserResponse.Data);

      if (response == null)
      {
        return new ServerResponse<bool>(false, "خطای سرور", false);
      }

      return response;
    }
  }
}
