using Application.Features.Courses.Commands;
using Application.Features.Courses.Queries;
using Application.Features.Users.Queries;
using Application.ViewModels.Course;
using Domain.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ServiceHost.Areas.Panel.Pages.Manage.Courses
{
  [Authorize(Roles = Roles.Admin + ", " + Roles.Teacher + ", " + Roles.Student)]
  public class CourseInfoModel : PageModel
  {
    private readonly ISender _mediatrSender;

    public CourseInfoModel(ISender mediatrSender)
    {
      _mediatrSender = mediatrSender;
    }

    public CourseTableDTO CourseTable { get; set; }
    public bool IsAdmin { get; set; } = false;
    public bool IsTeacher { get; set; } = false;
    public bool IsStudent { get; set; } = false;
    public async Task<IActionResult> OnGet(string courseId = null, bool teacher = false, bool student = false)
    {
      if (courseId == null)
      {
        return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = "دوره نامعتبر", messageSuccess = false });
      }

      if (teacher)
        IsTeacher = true;
      if (student)
        IsStudent = true;

      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      if (userId == null)
      {
        return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = "کاربر نامعتبر", messageSuccess = false });
      }

      var userResponse = await _mediatrSender.Send(new GetUserRequest(long.Parse(userId)));

      if (userResponse.IsSucceeded && userResponse.Data.Role == Roles.Admin)
      {
        IsAdmin = true;
      }

      var getId = long.Parse(courseId);

      var courseResponse = await _mediatrSender.Send(new GetCourseTableRequest(getId));

      if (courseResponse == null || courseResponse.IsSucceeded == false)
      {
        return RedirectToPage("/Manage/Users/Index", new { area = "Panel", message = courseResponse.Message, messageSuccess = false });
      }

      CourseTable = courseResponse.Data;

      return Page();
    }

    [BindProperty]
    public UserToCourseDTO UsetToCourse {  get; set; }

    public async Task<IActionResult> OnPostDeleteUserFromCourse()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      if (userId == null)
      {
        return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = "عدم دسترسی", messageSuccess = false });
      }

      var userResponse = await _mediatrSender.Send(new GetUserRequest(long.Parse(userId)));

      if (!userResponse.IsSucceeded && userResponse.Data.Role != Roles.Admin)
      {
        return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = "عدم دسترسی", messageSuccess = false });
      }

      var deleteUserFromCourseResponse = await _mediatrSender.Send(new RemoveStudentFromCourseRequest(UsetToCourse));

      if (!deleteUserFromCourseResponse.IsSucceeded)
      {
        return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = deleteUserFromCourseResponse.Message, messageSuccess = false });
      }

      return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = deleteUserFromCourseResponse.Message, messageSuccess = true });
    }
  }
}
