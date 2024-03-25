using Application.Features.Courses.Commands;
using Application.Features.Courses.Queries;
using Domain.BaseModels;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Panel.Pages.Manage.Courses
{
  [Authorize(Roles = Roles.Admin)]
  public class DeleteCourseModel : PageModel
  {
    private readonly ISender _mediatrSender;

    public DeleteCourseModel(ISender mediatrSender)
    {
      _mediatrSender = mediatrSender;
    }

    public Course CourseToDelete { get; set; }
    public async Task<IActionResult> OnGet(string courseId)
    {
      if (courseId == null)
      {
        return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = "دوره نامعتبر", messageSuccess = false });
      }

      var courseResponse = await _mediatrSender.Send(new GetCourseByIdRequest(long.Parse(courseId)));

      if (courseResponse == null || courseResponse.IsSucceeded == false)
      {
        return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = "دوره نامعتبر", messageSuccess = false });
      }

      CourseToDelete = courseResponse.Data;

      return Page();
    }

    public async Task<IActionResult> OnPostDeleteCourse(string id)
    {
      var deleteResponse = await _mediatrSender.Send(new DeleteCourseRequest(long.Parse(id)));

      if (deleteResponse == null && deleteResponse.IsSucceeded == false)
      {
        return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = deleteResponse.Message, messageSuccess = false });
      }

      return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = "دوره با موفقیت حذف شد", messageSuccess = true });
    }
  }
}
