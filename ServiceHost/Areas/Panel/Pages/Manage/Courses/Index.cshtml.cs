using Application.Features.Courses.Queries;
using Application.ViewModels.Course;
using Domain.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace ServiceHost.Areas.Panel.Pages.Manage.Courses
{
  [Authorize(Roles = Roles.Admin)]
  public class IndexModel : PageModel
  {
    private readonly ISender _mediatrSender;

    public IndexModel(ISender mediatrSender)
    {
      _mediatrSender = mediatrSender;
    }

    public List<CourseTableDTO> CourseTable { get; set; }

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

      var courseResponse = await _mediatrSender.Send(new GetAllCoursesTableRequest());

      if (courseResponse.IsSucceeded)
      {
        CourseTable = courseResponse.Data;

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
