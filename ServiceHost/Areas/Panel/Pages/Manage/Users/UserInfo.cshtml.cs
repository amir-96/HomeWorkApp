using Application.Repositories;
using Domain.BaseModels;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ServiceHost.Areas.Panel.Pages.Manage.Users
{
  [Authorize(Roles = Roles.Admin)]
  public class UserInfoModel : PageModel
  {
    private readonly IUserRepo _userRepo;

    public UserInfoModel(IUserRepo userRepo)
    {
      _userRepo = userRepo;
    }

    public User UserDetails { get; set; }
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

      var getId = long.Parse(userId);

      var userResponse = await _userRepo.Get(getId);

      if (userResponse == null || userResponse.IsSucceeded == false)
      {
        return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = userResponse.Message, messageSuccess = false });
      }

      UserDetails = userResponse.Data;

      return Page();
    }
  }
}
