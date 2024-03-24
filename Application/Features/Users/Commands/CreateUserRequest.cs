using Application.Repositories;
using Application.ViewModels.User;
using AutoMapper;
using Domain.BaseModels;
using Domain.Models;
using MediatR;

namespace Application.Features.Users.Commands
{
    public class CreateUserRequest : IRequest<ServerResponse<bool>>
  {
    public CreateUserDTO createUserDTO {  get; set; }

    public CreateUserRequest(CreateUserDTO createUserDTO)
    {
      this.createUserDTO = createUserDTO;
    }
  }

  public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, ServerResponse<bool>>
  {
    private readonly IUserRepo _userRepo;
    private readonly IMapper _mapper;
    private readonly ILogRepo _logRepo;

    public CreateUserRequestHandler(IUserRepo userRepo, IMapper mapper, ILogRepo logRepo)
    {
      _userRepo = userRepo;
      _mapper = mapper;
      _logRepo = logRepo;
    }

    public async Task<ServerResponse<bool>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
      User user = _mapper.Map<User>(request.createUserDTO);

      user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(request.createUserDTO.Password);

      var isExists = await _userRepo.Exists(u => u.UserName == user.UserName || u.Email == user.Email);

      if (isExists.IsSucceeded && isExists.Data)
      {
        return new ServerResponse<bool>(false, "کاربر با این نام کاربری یا ایمیل وجود دارد", false);
      }

      var response = await _userRepo.Create(user);

      if (response == null)
      {
        return new ServerResponse<bool>(false, "خطای سرور", false);
      }

      await _logRepo.CreateLog($"کاربر {user.UserName} ایجاد شد");

      return response;
    }
  }
}
