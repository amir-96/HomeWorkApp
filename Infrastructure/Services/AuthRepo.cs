using Application.Repositories;
using Application.ViewModels.Auth;
using Domain.BaseModels;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AuthenticationProperties = Microsoft.AspNetCore.Authentication.AuthenticationProperties;

namespace Infrastructure.Services
{
    public class AuthRepo : IAuthRepo
  {
    private readonly ApplicationDbContext _context;
    private readonly IPasswordRepo _passwordRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthRepo(ApplicationDbContext context, IPasswordRepo passwordRepo, IHttpContextAccessor httpContextAccessor)
    {
      _context = context;
      _passwordRepo = passwordRepo;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServerResponse<bool>> LoginUser(LoginDTO loginDTO)
    {
      try
      {
        var user = await _context.Users.Where(u => u.IsActive == true).SingleOrDefaultAsync(u => u.UserName == loginDTO.UserNameOrEmail || u.Email == loginDTO.UserNameOrEmail);

        if (user == null)
        {
          return new ServerResponse<bool>(false, "کاربری با این نام کاربری یا ایمیل پیدا نشد", false);
        }

        var isPasswordTrue = _passwordRepo.Compare(loginDTO.Password, user.HashedPassword);

        if (!isPasswordTrue)
        {
          return new ServerResponse<bool>(false, "کلمه ی عبور اشتباه است", false);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("FullName", user.FirstName + " " + user.LastName),
            new Claim(ClaimTypes.Role, user.Role),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
          AllowRefresh = true,
          ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
          IsPersistent = loginDTO.IsPersistent
        };

        await _httpContextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return new ServerResponse<bool>(true, $"{user.FirstName}, شما با موفقیت وارد شدید", true);
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

    public async Task<bool> LogoutUser()
    {
      try
      {
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
