using Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Panel.Pages
{
  public class LogoutModel : PageModel
  {
    private readonly ISender _mediatrSender;

    public LogoutModel(ISender mediatrSender)
    {
      _mediatrSender = mediatrSender;
    }

    public async Task<IActionResult> OnGet()
    {
      var logoutResponse = await _mediatrSender.Send(new LogoutRequest());

      if (!logoutResponse)
      {
        return RedirectToPage("/Index", new { message = "خطایی در خروج از حساب کاربری رخ داد", messageSuccess = false });
      }

      return RedirectToPage("/Index", new { message = "شما با موفقیت خارج شدید", messageSuccess = true });
    }
  }
}
