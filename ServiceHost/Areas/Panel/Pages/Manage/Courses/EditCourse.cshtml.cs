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
using System.Globalization;

namespace ServiceHost.Areas.Panel.Pages.Manage.Courses
{
  [Authorize(Roles = Roles.Admin)]
  public class EditCourseModel : PageModel
  {
    private readonly ISender _mediatrSender;
    private readonly IWebHostEnvironment _environment;

    public EditCourseModel(ISender mediatrSender, IWebHostEnvironment environment)
    {
      _mediatrSender = mediatrSender;
      _environment = environment;
    }

    public List<User> Teachers { get; set; }
    public Course CurrentCourse { get; set; }
    public async Task<IActionResult> OnGet(string courseId)
    {
      var teacherResponse = await _mediatrSender.Send(new GetAllUsersRequest());

      Teachers = teacherResponse.Data.Where(t => t.Role == Roles.Teacher || t.Role == Roles.Admin).ToList();

      var courseResponse = await _mediatrSender.Send(new GetCourseByIdRequest(long.Parse(courseId)));

      if (courseResponse == null || courseResponse.IsSucceeded == false)
      {
        return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = "خطا در دریافت دوره", messageSuccess = false });
      }

      CurrentCourse = courseResponse.Data;

      return Page();
    }

    [BindProperty]
    public IFormFile Image { get; set; }

    [BindProperty]
    public string StartDate { get; set; }

    [BindProperty]
    public EditCourseDTO editCourseDTO { get; set; }

    public async Task<IActionResult> OnPostEditCourse()
    {
      var courseResponse = await _mediatrSender.Send(new GetCourseByIdRequest(editCourseDTO.Id));

      if (courseResponse == null || courseResponse.IsSucceeded == false)
      {
        return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = "خطا در دریافت دوره", messageSuccess = true });
      }

      if (Image != null)
      {
        if (Image.Length > 5 * 1024 * 1024)
        {
          return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = "File size should not exceed 5 MB", messageSuccess = false });
        }

        var allowedTypes = new[] { "image/jpeg", "image/png" };

        if (!allowedTypes.Contains(Image.ContentType))
        {
          return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = "Please select a JPG or PNG file", messageSuccess = false });
        }

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "CoursePhotos");

        if (!Directory.Exists(uploadsFolder))
        {
          Directory.CreateDirectory(uploadsFolder);
        }

        // Check if the current image is not the default one
        if (!string.Equals(courseResponse.Data.Image, "default.png", StringComparison.OrdinalIgnoreCase))
        {
          // Construct the full path to the current image file
          var currentImagePath = Path.Combine(uploadsFolder, courseResponse.Data.Image);

          // Delete the current image file if it exists
          if (System.IO.File.Exists(currentImagePath))
          {
            System.IO.File.Delete(currentImagePath);
          }
        }

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;

        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          await Image.CopyToAsync(stream);
        }

        editCourseDTO.Image = uniqueFileName;
      }
      else
      {
        editCourseDTO.Image = "default.jpg";
      }

      editCourseDTO.StartDate = ConvertDate(StartDate);

      editCourseDTO.EndDate = ConvertDate(StartDate).AddDays(editCourseDTO.TimeSpan);

      var editCourseResponse = await _mediatrSender.Send(new UpdateCourseRequest(editCourseDTO));

      if (editCourseResponse.IsSucceeded)
      {
        return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = "دوره با موفقیت ویرایش شد", messageSuccess = true });
      }
      else
      {
        return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = editCourseResponse.Message, messageSuccess = false });
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

    private DateOnly ConvertDate(string date)
    {
      // Specify the formats for parsing the date
      string[] formats = { "yyyy/M/d", "yyyy/MM/dd" };

      // Parse the date using multiple formats
      DateTime dateTime;
      if (!DateTime.TryParseExact(date, formats, null, System.Globalization.DateTimeStyles.None, out dateTime))
      {
        // Handle invalid date format
        throw new FormatException("Invalid date format. Date should be in 'yyyy/MM/dd' or 'yyyy/M/d' format.");
      }

      // Convert DateTime to DateOnly
      DateOnly dateOnly = DateOnly.FromDateTime(dateTime);

      return dateOnly;
    }
  }
}
