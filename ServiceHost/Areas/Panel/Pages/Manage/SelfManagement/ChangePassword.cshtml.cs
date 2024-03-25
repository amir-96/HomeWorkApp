using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using Application.ViewModels.User;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ServiceHost.Areas.Panel.Pages.Manage.SelfManagement
{
  [Authorize]
  public class ChangePasswordModel : PageModel
  {
    private readonly ISender _mediatrSender;

    public ChangePasswordModel(ISender mediatrSender)
    {
      _mediatrSender = mediatrSender;
    }

    public User UserForEdit { get; set; }
    public async Task<IActionResult> OnGet()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      if (userId == null)
      {
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = "کاربر نامعتبر", messageSuccess = false });
      }

      var longId = long.Parse(userId);

      var userResponse = await _mediatrSender.Send(new GetUserRequest(longId));

      if (userResponse.IsSucceeded == false)
      {
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = userResponse.Message, messageSuccess = false });
      }

      UserForEdit = userResponse.Data;

      return Page();
    }

    [BindProperty]
    public ChangePasswordDTO ChangePasswordDTO { get; set; }

    public async Task<IActionResult> OnPostChangePassword()
    {
      var usersResponse = await _mediatrSender.Send(new ChangePasswordRequest(ChangePasswordDTO));

      if (usersResponse.IsSucceeded)
      {
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = "تغییر کلمه ی عبور با موفقیت انجام شد", messageSuccess = true });
      }
      else
      {
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = usersResponse.Message, messageSuccess = false });
      }
    }
  }
}
