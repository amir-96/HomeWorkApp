using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using Domain.BaseModels;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Panel.Pages.Manage.Users
{
  [Authorize(Roles = Roles.Admin)]
  public class DeleteUserModel : PageModel
  {
    private readonly ISender _mediatrSender;

    public DeleteUserModel(ISender mediatrSender)
    {
      _mediatrSender = mediatrSender;
    }

    public User UserToDelete { get; set; }
    public async Task<IActionResult> OnGet(string userId)
    {
      if (userId == null)
      {
        return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = "کاربر نامعتبر", messageSuccess = false });
      }

      var userResponse = await _mediatrSender.Send(new GetUserRequest(long.Parse(userId)));

      if (userResponse == null || userResponse.IsSucceeded == false)
      {
        return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = "کاربر نامعتبر", messageSuccess = false });
      }

      UserToDelete = userResponse.Data;

      return Page();
    }

    public async Task<IActionResult> OnPostDeleteUser(string id)
    {
      var deleteResponse = await _mediatrSender.Send(new DeleteUserRequest(long.Parse(id)));

      if (deleteResponse == null && deleteResponse.IsSucceeded == false)
      {
        return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = deleteResponse.Message, messageSuccess = false });
      }

      return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = "کاربر با موفقیت حذف شد", messageSuccess = true });
    }
  }
}
