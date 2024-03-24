using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using Application.Repositories;
using Application.ViewModels.User;
using Domain.BaseModels;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ServiceHost.Areas.Panel.Pages.Manage.Users
{
  [Authorize(Roles = Roles.Admin)]
  public class EditUserModel : PageModel
  {
    private readonly ISender _mediatrSender;

    public EditUserModel(ISender mediatrSender)
    {
      _mediatrSender = mediatrSender;
    }

    public User UserForEdit { get; set; }
    public async Task<IActionResult> OnGet(string userId = null)
    {
      if (userId == null)
      {
        userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
          return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = "کاربر نامعتبر", messageSuccess = false });
        }
      }

      var longId = long.Parse(userId);

      var userResponse = await _mediatrSender.Send(new GetUserRequest(longId));

      if (userResponse.IsSucceeded == false)
      {
        return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = userResponse.Message, messageSuccess = false });
      }

      UserForEdit = userResponse.Data;

      return Page();
    }

    [BindProperty]
    public EditUserDTO EditUserDTO { get; set; }

    public async Task<IActionResult> OnPostEditUser()
    {
      var usersResponse = await _mediatrSender.Send(new EditUserRequest(EditUserDTO));

      if (usersResponse.IsSucceeded)
      {
        return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = "کاربر با موفقیت ویرایش شد", messageSuccess = true });
      }
      else
      {
        return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = usersResponse.Message, messageSuccess = false });
      }
    }
  }
}
