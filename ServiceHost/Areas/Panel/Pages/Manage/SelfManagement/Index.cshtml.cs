using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using Application.ViewModels.User;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ServiceHost.Areas.Panel.Pages.Manage.SelfManagement
{
  [Authorize]
  public class IndexModel : PageModel
  {
    private readonly ISender _mediatrSender;
    private readonly IWebHostEnvironment _environment;

    public IndexModel(ISender mediatrSender, IWebHostEnvironment environment)
    {
      _mediatrSender = mediatrSender;
      _environment = environment;
    }

    public User UserForEdit { get; set; }
    public async Task<IActionResult> OnGet()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      if (userId == null)
      {
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = "کاربر نامعتبر", messageSuccess = false });
      }

      var longId = long.Parse(userId);

      var userResponse = await _mediatrSender.Send(new GetUserRequest(longId));

      if (userResponse.IsSucceeded == false)
      {
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = userResponse.Message, messageSuccess = false });
      }

      UserForEdit = userResponse.Data;

      return Page();
    }

    [BindProperty]
    public IFormFile Image { get; set; }

    [BindProperty]
    public long UserId { get; set; }

    public async Task<IActionResult> OnPostChangeImage()
    {
      if (Image == null || Image.Length == 0)
      {
        ModelState.AddModelError("Image", "Please select a file.");
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = "Please select a file", messageSuccess = false });
      }

      // Limit image size if needed
      if (Image.Length > 5 * 1024 * 1024) // 5 MB
      {
        ModelState.AddModelError("Image", "File size should not exceed 5 MB.");
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = "File size should not exceed 5 MB", messageSuccess = false });
      }

      var allowedTypes = new[] { "image/jpeg", "image/png" };

      if (!allowedTypes.Contains(Image.ContentType))
      {
        ModelState.AddModelError("Image", "Please select a JPG or PNG file.");
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = "Please select a JPG or PNG file", messageSuccess = false });
      }

      var userResponse = await _mediatrSender.Send(new GetUserRequest(UserId));
      
      if (userResponse.IsSucceeded == false)
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = "کاربر نامعتبر", messageSuccess = false });

      var username = userResponse.Data.UserName;

      var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "ProfilePhotos", username);

      if (!Directory.Exists(uploadsFolder))
      {
        Directory.CreateDirectory(uploadsFolder);
      }

      // Check if the current image is not the default one
      if (!string.Equals(userResponse.Data.Image, "default.png", StringComparison.OrdinalIgnoreCase))
      {
        // Construct the full path to the current image file
        var currentImagePath = Path.Combine(uploadsFolder, userResponse.Data.Image);

        // Delete the current image file if it exists
        if (System.IO.File.Exists(currentImagePath))
        {
          System.IO.File.Delete(currentImagePath);
        }
      }

      var uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;

      var filePath = Path.Combine(uploadsFolder, uniqueFileName);

      var changeRequestDTO = new ChangeImageDTO()
      {
        Id = userResponse.Data.Id,
        ImageUrl = uniqueFileName
      };

      var changeImageResponse = await _mediatrSender.Send(new ChangeImageRequest(changeRequestDTO));

      if (!changeImageResponse)
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = "خطا در تغییر تصویر", messageSuccess = false });

      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await Image.CopyToAsync(stream);
      }

      // Save the file path to a database or do further processing as needed

      return RedirectToPage("/Manage/SelfManagement/Index", new { area = "Panel" });
    }

    public async Task<IActionResult> OnPostDeleteImage()
    {
      var userResponse = await _mediatrSender.Send(new GetUserRequest(UserId));

      if (userResponse.IsSucceeded == false)
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = "کاربر نامعتبر", messageSuccess = false });

      var username = userResponse.Data.UserName;

      var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "ProfilePhotos", username);

      if (!Directory.Exists(uploadsFolder))
      {
        Directory.CreateDirectory(uploadsFolder);
      }

      // Check if the current image is not the default one
      if (!string.Equals(userResponse.Data.Image, "default.png", StringComparison.OrdinalIgnoreCase))
      {
        // Construct the full path to the current image file
        var currentImagePath = Path.Combine(uploadsFolder, userResponse.Data.Image);

        // Delete the current image file if it exists
        if (System.IO.File.Exists(currentImagePath))
        {
          System.IO.File.Delete(currentImagePath);
        }
      }

      var changeRequestDTO = new ChangeImageDTO()
      {
        Id = userResponse.Data.Id,
        ImageUrl = "default.png"
      };

      var changeImageResponse = await _mediatrSender.Send(new ChangeImageRequest(changeRequestDTO));

      return RedirectToPage("/Manage/SelfManagement/Index", new { area = "Panel" });
    }

    [BindProperty]
    public SelfEditDTO EditUserDTO { get; set; }

    public async Task<IActionResult> OnPostEditUser()
    {
      var usersResponse = await _mediatrSender.Send(new SelfEditResuest(EditUserDTO));

      if (usersResponse.IsSucceeded)
      {
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = "ویرایش با موفقیت انجام شد", messageSuccess = true });
      }
      else
      {
        return RedirectToPage("/Manage/Index", new { area = "Panel", message = usersResponse.Message, messageSuccess = false });
      }
    }
  }
}
