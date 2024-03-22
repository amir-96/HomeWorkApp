using Application.Repositories;
using Application.ViewModels.User;
using AutoMapper;
using MediatR;

namespace Application.Features.Users.Commands
{
  public class ChangeImageRequest : IRequest<bool>
  {
    public ChangeImageDTO ChangeImageDTO { get; set; }

    public ChangeImageRequest(ChangeImageDTO changeImageDTO)
    {
      ChangeImageDTO = changeImageDTO;
    }
  }

  public class ChangeImageRequestHandler : IRequestHandler<ChangeImageRequest, bool>
  {
    private readonly IUserRepo _userRepo;

    public ChangeImageRequestHandler(IUserRepo userRepo, IMapper mapper)
    {
      _userRepo = userRepo;
    }

    public async Task<bool> Handle(ChangeImageRequest request, CancellationToken cancellationToken)
    {
      var response = await _userRepo.ChangeImage(request.ChangeImageDTO);

      return response;
    }
  }
}
