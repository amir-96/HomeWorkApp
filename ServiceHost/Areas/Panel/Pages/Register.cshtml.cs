using Application.Features.Auth.Commands;
using Application.Features.Users.Commands;
using Application.Repositories;
using Application.ViewModels.Auth;
using Application.ViewModels.User;
using Domain.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Panel.Pages
{
  public class RegisterModel : PageModel
  {
    private readonly ISender _mediatrSender;
    private readonly IGoogleRecaptcha _googleRecapcha;

    public RegisterModel(ISender mediatrSender, IGoogleRecaptcha googleRecapcha)
    {
      _mediatrSender = mediatrSender;
      _googleRecapcha = googleRecapcha;
    }

    public IActionResult OnGet(string message = null, bool messageSuccess = false)
    {
      if (User.Identity.IsAuthenticated)
      {
        return RedirectToPage("/Manage/Index", new { area = "Panel" });
      }

      if (message != null)
      {
        if (messageSuccess)
        {
          ViewData["SuccessNotif"] = message;

          return Page();
        }
        else
        {
          ViewData["FailureNotif"] = message;

          return Page();
        }
      }

      return Page();
    }

    [BindProperty]
    public CreateUserDTO CreateUser { get; set; }
    public LoginDTO NewUserLogin { get; set; }

    public async Task<IActionResult> OnPostRegisterUser()
    {
      if (!await _googleRecapcha.IsSatisfy())
      {
        return RedirectToPage("/Register", new { area = "Panel", message = "خطا در احراز هویت کپچا", messageSuccess = false });
      }

      CreateUser.Role = Roles.Student;

      var registerResponse = await _mediatrSender.Send(new CreateUserRequest(CreateUser));

      if (registerResponse != null && registerResponse.IsSucceeded == true)
      {
        await _mediatrSender.Send(new LoginRequest(new LoginDTO() { UserNameOrEmail = CreateUser.UserName, Password = CreateUser.Password }));

        return RedirectToPage("/Index", new { message = registerResponse.Message, messageSuccess = true });
      }
      else
      {
        return RedirectToPage("/Register", new { area = "Panel", message = registerResponse.Message, messageSuccess = false });
      }
    }
  }
}
