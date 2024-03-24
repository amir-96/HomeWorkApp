using Application.Features.Users.Commands;
using Application.ViewModels.User;
using Domain.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ServiceHost.Areas.Panel.Pages.Manage.Users
{
  [Authorize(Roles = Roles.Admin)]
  public class AdminResetPasswordModel : PageModel
  {
    private readonly ISender _mediatrSender;

    public AdminResetPasswordModel(ISender mediatrSender)
    {
      _mediatrSender = mediatrSender;
    }

    public AdminChangePasswordDTO AdminChangePasswordDTOFirst { get; set; }
    public IActionResult OnGet(string userId = null)
    {
      var ownUser = User.FindFirstValue(ClaimTypes.NameIdentifier);

      if (userId == null || ownUser == null)
      {
        return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = "کاربر نامعتبر", messageSuccess = false });
      }

      AdminChangePasswordDTOFirst = new AdminChangePasswordDTO()
      {
        AdminId = long.Parse(ownUser),
        UserId = long.Parse(userId)
      };

      return Page();
    }

    [BindProperty]
    public AdminChangePasswordDTO AdminChangePasswordDTOSecond { get; set; }

    public async Task<IActionResult> OnPostAdminResetPassword()
    {
      var changePasswordResponse = await _mediatrSender.Send(new AdminResetPasswordRequest(AdminChangePasswordDTOSecond));

      if (changePasswordResponse != null && changePasswordResponse.IsSucceeded == true)
      {
        return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = changePasswordResponse.Message, messageSuccess = true });
      }

      return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = changePasswordResponse.Message, messageSuccess = false });
    }
  }
}
