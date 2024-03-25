using Application.Features.Courses.Commands;
using Application.Features.Users.Queries;
using Application.ViewModels.Course;
using Domain.BaseModels;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Panel.Pages.Manage.Courses
{
  [Authorize(Roles = Roles.Admin)]
  public class AddCourseModel : PageModel
  {
    private readonly ISender _mediatrSender;
    private readonly IWebHostEnvironment _environment;

    public AddCourseModel(ISender mediatrSender, IWebHostEnvironment environment)
    {
      _mediatrSender = mediatrSender;
      _environment = environment;
    }

    public List<User> Teachers {  get; set; }
    public async Task OnGet()
    {
      var teacherResponse = await _mediatrSender.Send(new GetAllUsersRequest());

      Teachers = teacherResponse.Data.Where(t => t.Role == Roles.Teacher || t.Role == Roles.Admin).ToList();
    }

    [BindProperty]
    public IFormFile Image { get; set; }

    [BindProperty]
    public string StartDate { get; set; }

    [BindProperty]
    public CreateCourseDTO createCourseDTO { get; set; }

    public async Task<IActionResult> OnPostCreateCourse()
    {
      if (Image != null)
      {
        if (Image.Length > 5 * 1024 * 1024) // 5 MB
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

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;

        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          await Image.CopyToAsync(stream);
        }

        createCourseDTO.Image = uniqueFileName;
      }
      else
      {
        createCourseDTO.Image = "default.jpg";
      }

      createCourseDTO.StartDate = ConvertDate(StartDate);

      createCourseDTO.EndDate = ConvertDate(StartDate).AddDays(createCourseDTO.TimeSpan);

      var courseResponse = await _mediatrSender.Send(new CreateCourseRequest(createCourseDTO));

      if (courseResponse.IsSucceeded)
      {
        return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = "دوره با موفقیت ساخته شد", messageSuccess = true });
      }
      else
      {
        return RedirectToPage("/Manage/Courses/Index", new { area = "Panel", message = courseResponse.Message, messageSuccess = false });
      }
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
