﻿using Application.Repositories;
using Application.ViewModels.User;
using AutoMapper;
using Domain.BaseModels;
using MediatR;

namespace Application.Features.Users.Commands
{
    public class EditUserRequest : IRequest<ServerResponse<bool>>
  {
    public EditUserDTO SelfEditDTO { get; set; }

    public EditUserRequest(EditUserDTO editUserDTO)
    {
      SelfEditDTO = editUserDTO;
    }
  }

  public class EditUserDTOHandler : IRequestHandler<EditUserRequest, ServerResponse<bool>>
  {
    private readonly IUserRepo _userRepo;

    public EditUserDTOHandler(IUserRepo userRepo, IMapper mapper)
    {
      _userRepo = userRepo;
    }

    public async Task<ServerResponse<bool>> Handle(EditUserRequest request, CancellationToken cancellationToken)
    {
      var oldUserResponse = await _userRepo.Get(request.SelfEditDTO.Id);

      var editedUser = request.SelfEditDTO;

      var isExists = await _userRepo.Exists(u => (u.UserName == editedUser.UserName || u.Email == editedUser.Email) && u.Id != editedUser.Id);

      if (isExists.IsSucceeded && isExists.Data)
      {
        return new ServerResponse<bool>(false, "کاربر با این نام کاربری یا ایمیل وجود دارد", false);
      }

      oldUserResponse.Data.UserName = editedUser.UserName;
      oldUserResponse.Data.Email = editedUser.Email;
      oldUserResponse.Data.PhoneNumber = editedUser.PhoneNumber;
      oldUserResponse.Data.FirstName = editedUser.FirstName;
      oldUserResponse.Data.LastName = editedUser.LastName;
      oldUserResponse.Data.Role = editedUser.Role;
      oldUserResponse.Data.Ballance = editedUser.Ballance;
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
