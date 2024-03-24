using Application.Features.Auth.Commands;
using Application.Repositories;
using Application.ViewModels.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Panel.Pages
{
  public class LoginModel : PageModel
  {
    private readonly ISender _mediatrSender;
    private readonly IGoogleRecaptcha _googleRecapcha;

    public LoginModel(ISender mediatrSender, IGoogleRecaptcha googleRecapcha)
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
    public LoginDTO LoginDTO { get; set; }

    public async Task<IActionResult> OnPostLoginUser()
    {
      if (! await _googleRecapcha.IsSatisfy())
      {
        return RedirectToPage("/Login", new { area = "Panel", message = "خطا در احراز هویت کپچا", messageSuccess = false });
      }

      var loginResponse = await _mediatrSender.Send(new LoginRequest(LoginDTO));

      if (loginResponse != null && loginResponse.IsSucceeded == true)
      {
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = loginResponse.Message, messageSuccess = true });
      }
      else
      {
        return RedirectToPage("/Login", new { area = "Panel", message = loginResponse.Message, messageSuccess = false });
      }
    }
  }
}
