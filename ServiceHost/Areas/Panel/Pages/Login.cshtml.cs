using Application.Features.Auth.Commands;
using Application.ViewModels.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Panel.Pages
{
  public class LoginModel : PageModel
  {
    private readonly ISender _mediatrSender;

    public LoginModel(ISender mediatrSender)
    {
      _mediatrSender = mediatrSender;
    }

    public void OnGet(string message = null, bool messageSuccess = false)
    {
      if (message != null)
      {
        if (messageSuccess)
        {
          ViewData["SuccessNotif"] = message;
        }
        else
        {
          ViewData["FailureNotif"] = message;
        }
      }
    }

    [BindProperty]
    public LoginDTO LoginDTO { get; set; }

    public async Task<IActionResult> OnPostLoginUser()
    {
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
