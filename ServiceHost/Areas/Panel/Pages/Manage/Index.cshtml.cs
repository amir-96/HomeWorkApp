using Domain.BaseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Panel.Pages.Manage
{
  [Authorize]
  public class IndexModel : PageModel
  {
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
  }
}
