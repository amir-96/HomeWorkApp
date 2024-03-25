using Application.Features.Courses.Queries;
using Application.Features.Users.Queries;
using Application.ViewModels.Course;
using Domain.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Security.Claims;

namespace ServiceHost.Areas.Panel.Pages.Manage.Courses
{
  [Authorize(Roles = Roles.Admin + ", " + Roles.Teacher + ", " + Roles.Student)]
  public class IndexModel : PageModel
  {
    private readonly ISender _mediatrSender;

    public IndexModel(ISender mediatrSender)
    {
      _mediatrSender = mediatrSender;
    }

    public List<CourseTableDTO> CourseTable { get; set; }
    public bool IsTeacher { get; set; } = false;
    public bool IsStudent { get; set; } = false;

    public async Task<IActionResult> OnGet(string message = null, bool messageSuccess = false, bool isTeacher = false, bool isStudent = false)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      if (userId == null)
      {
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = "کاربر نامعتبر", messageSuccess = false });
      }

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

      var userResponse = await _mediatrSender.Send(new GetUserRequest(long.Parse(userId)));

      if (userResponse.IsSucceeded == false)
      {
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = "کاربر نامعتبر", messageSuccess = false });
      }

      var courseResponse = await _mediatrSender.Send(new GetAllCoursesTableRequest());

      if (courseResponse.IsSucceeded)
      {
        if (userResponse.Data.Role == Roles.Admin)
        {
          CourseTable = courseResponse.Data;
        }
        else if (userResponse.Data.Role == Roles.Teacher)
        {
          CourseTable = courseResponse.Data.Where(c => c.Teacher.Id == userResponse.Data.Id).ToList();
          IsTeacher = true;
        }
        else if (userResponse.Data.Role == Roles.Student)
        {
          CourseTable = courseResponse.Data.Where(c => c.Students.Any(s => s.Id == userResponse.Data.Id)).ToList();
          IsStudent = true;
        }
        else
        {
          return RedirectToPage("/Manage/Index", new { area = "Panel", message = "کاربر نامعتبر", messageSuccess = false });
        }

        if (isTeacher)
        {
          CourseTable = CourseTable.Where(c => c.Teacher.Id == userResponse.Data.Id).ToList();
          IsTeacher = true;
        }

        if (isStudent)
        {
          CourseTable = CourseTable.Where(c => c.Students.Any(s => s.Id == userResponse.Data.Id)).ToList();
          IsStudent = true;
        }

        return Page();
      }
      else
      {
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = courseResponse.Message });
      }
    }

    public string ToShamsi(DateTime dateTime)
    {
      var persianCalendar = new PersianCalendar();
      int year = persianCalendar.GetYear(dateTime);
      int month = persianCalendar.GetMonth(dateTime);
      int day = persianCalendar.GetDayOfMonth(dateTime);
      return $"{year}/{month:D2}/{day:D2}";
    }

    public string ToShamsi(DateOnly dateOnly)
    {
      // Convert DateOnly to DateTime by creating a DateTime with the same date
      DateTime dateTime = new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);

      // Call the ToShamsi(DateTime) method
      return ToShamsi(dateTime);
    }
  }
}
