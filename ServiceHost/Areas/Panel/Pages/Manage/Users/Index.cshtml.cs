using Application.Features.Courses.Commands;
using Application.Features.Courses.Queries;
using Application.Features.Users.Queries;
using Application.ViewModels.Course;
using Domain.BaseModels;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

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

    public List<User> Users { get; set; }
    public List<CourseTableDTO> Courses { get; set; }

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

      var coursesResponse = await _mediatrSender.Send(new GetAllCoursesTableRequest());

      if (usersResponse.IsSucceeded && coursesResponse.IsSucceeded)
      {
        Users = usersResponse.Data;

        Courses = coursesResponse.Data;

        return Page();
      }
      else
      {
        return RedirectToPage("/Manage/Index", new { area = "Panel" });
      }
    }

    [BindProperty]
    public UserToCourseDTO UserToCourse { get; set; }
    public async Task<IActionResult> OnPostAddUserToCourse()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      if (userId == null)
      {
        return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = "عدم دسترسی", messageSuccess = false });
      }

      var userResponse = await _mediatrSender.Send(new GetUserRequest(long.Parse(userId)));

      if (!userResponse.IsSucceeded && userResponse.Data.Role != Roles.Admin)
      {
        return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = "عدم دسترسی", messageSuccess = false });
      }

      var addUserToCourseResponse = await _mediatrSender.Send(new AddStudentToCourseRequest(UserToCourse));

      if (!addUserToCourseResponse.IsSucceeded)
      {
        return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = addUserToCourseResponse.Message, messageSuccess = false });
      }

      return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = addUserToCourseResponse.Message, messageSuccess = true });
    }
  }
}
