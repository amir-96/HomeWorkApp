using Application.Features.Users.Commands;
using Application.ViewModels.User;
using Domain.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Panel.Pages.Manage.Users
{
  [Authorize(Roles = Roles.Admin)]
  public class AddUserModel : PageModel
  {
    private readonly ISender _mediatrSender;

    public AddUserModel(ISender mediatrSender)
    {
      _mediatrSender = mediatrSender;
    }

    public void OnGet()
    {
    }

    [BindProperty]
    public CreateUserDTO CreateUserDTO { get; set; }

    public async Task<IActionResult> OnPostCreateUser()
    {
      var usersResponse = await _mediatrSender.Send(new CreateUserRequest(CreateUserDTO));

      if (usersResponse.IsSucceeded)
      {
        return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = "کاربر با موفقیت ساخته شد", messageSuccess = true });
      }
      else
      {
        return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = usersResponse.Message, messageSuccess = false });
      }
    }
  }
}
