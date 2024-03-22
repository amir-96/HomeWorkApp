using Application.Repositories;
using Application.ViewModels.User;
using Domain.BaseModels;
using Domain.Models;
using Infrastructure.Context;
using System.Text.RegularExpressions;

namespace Infrastructure.Services
{
    public class UserRepo : BaseService<long, User>, IUserRepo
  {
    private readonly ApplicationDbContext _context;
    private readonly IPasswordRepo _passwordRepo;
    public UserRepo(ApplicationDbContext context, IPasswordRepo passwordRepo) : base(context)
    {
      _context = context;
      _passwordRepo = passwordRepo;
    }

    public async Task<bool> ChangeImage(ChangeImageDTO changeImageDTO)
    {
      try
      {
        var userResponse = await Get(changeImageDTO.Id);

        if (userResponse == null || userResponse.Data == null)
        {
          return false;
        }

        if (!IsValidImageUrl(changeImageDTO.ImageUrl))
        {
          return false;
        }

        userResponse.Data.Image = changeImageDTO.ImageUrl;

        var response = await Update(userResponse.Data.Id, userResponse.Data);

        if (response == null)
        {
          return false;
        }

        return true;
      }
      catch
      {
        return false;
      }
    }

    public async Task<ServerResponse<bool>> ChangePassword(ChangePasswordDTO changePasswordDTO)
    {
      try
      {
        var userResponse = await Get(changePasswordDTO.Id);

        if (userResponse == null || userResponse.Data == null)
        {
          return new ServerResponse<bool>(false, "کاربر با این مشخصات پیدا نشد", false);
        }

        if (changePasswordDTO.NewPassword != changePasswordDTO.NewPasswordConfirmation)
        {
          return new ServerResponse<bool>(false, "کلمه ی عبور با تکرار آن مطابقت ندارد", false);
        }

        if (_passwordRepo.Compare(changePasswordDTO.OldPassword, userResponse.Data.HashedPassword) == false)
        {
          return new ServerResponse<bool>(false, "کلمه ی عبور فعلی صحیح نیست", false);
        }

        userResponse.Data.HashedPassword = _passwordRepo.HashPassword(changePasswordDTO.NewPassword);

        await Update(userResponse.Data.Id, userResponse.Data);

        var response = new ServerResponse<bool>(true, "کلمه ی عبور با موفقیت تغییر کرد", true);

        return response;
      }
      catch (Exception ex)
      {
        string errorMessage = "عملیات با خطا مواجه شد : " + ex.Message;

        if (ex.InnerException != null)
        {
          errorMessage += " | Inner Error: " + ex.InnerException.Message;
        }

        var response = new ServerResponse<bool>(false, errorMessage, false);

        return response;
      }
    }

    private bool IsValidImageUrl(string imageUrl)
    {
      // Define a regular expression pattern to match common image extensions
      string pattern = @"(.*)(\.jpg|\.jpeg|\.png|\.bmp)$";

      // Use Regex.IsMatch to check if the image URL matches the pattern
      return Regex.IsMatch(imageUrl, pattern, RegexOptions.IgnoreCase);
    }
  }
}
