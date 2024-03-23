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
  public class IndexModel : PageModel
  {
    private readonly ISender _mediatrSender;

    public IndexModel(ISender mediatrSender)
    {
      _mediatrSender = mediatrSender;
    }

    public bool MessageSuccess { get; set; }
    public List<User> Users { get; set; }

    public async Task<IActionResult> OnGet(string message = null, bool messageSuccess = false)
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

      var usersResponse = await _mediatrSender.Send(new GetAllUsersRequest());

      if (usersResponse.IsSucceeded)
      {
        Users = usersResponse.Data;

        return Page();
      }
      else
      {
        ViewData["Notification"] = usersResponse.Message;
        return RedirectToPage("/Panel/Manage/Index");
      }
    }
  }
}
