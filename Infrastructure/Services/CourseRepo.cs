using Application.Repositories;
using Application.ViewModels.Course;
using Domain.BaseModels;
using Domain.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
  public class CourseRepo : BaseService<long, Course>, ICourseRepo
  {
    private readonly ApplicationDbContext _context;
    private readonly IUserRepo _userRepo;
    public CourseRepo(ApplicationDbContext context, IUserRepo userRepo) : base(context)
    {
      _context = context;
      _userRepo = userRepo;
    }

    public async Task<ServerResponse<List<User>>> GetAllCourseStudents(long courseId)
    {
      try
      {
        var courseResponse = await Get(courseId);

        if (courseResponse == null || courseResponse.IsSucceeded == false)
        {
          return new ServerResponse<List<User>> (false, "دوره با این مشخصات پیدا نشد", null);
        }

        var userIds = await _context.UserCourses
            .Where(uc => uc.CourseId == courseId)
            .Select(uc => uc.UserId)
            .ToListAsync();

        var students = await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();

        return new ServerResponse<List<User>>(true, "لیست دانشجویان دوره با موفقیت دریافت شد", students);
      }
      catch (Exception ex)
      {
        string errorMessage = "عملیات با خطا مواجه شد : " + ex.Message;

        if (ex.InnerException != null)
        {
          errorMessage += " | Inner Error: " + ex.InnerException.Message;
        }

        var response = new ServerResponse<List<User>> (false, errorMessage, null);

        return response;
      }
    }

    public async Task<ServerResponse<User>> GetCourseTeacher(long courseId)
    {
      try
      {
        var courseResponse = await Get(courseId);

        if (courseResponse == null || courseResponse.IsSucceeded == false)
        {
          return new ServerResponse<User>(false, "دوره با این مشخصات پیدا نشد", null);
        }

        var teacher = await _userRepo.Get(courseResponse.Data.TeacherId);

        return new ServerResponse<User>(true, "مدرس دوره با موفقیت دریافت شد", teacher.Data);
      }
      catch (Exception ex)
      {
        string errorMessage = "عملیات با خطا مواجه شد : " + ex.Message;

        if (ex.InnerException != null)
        {
          errorMessage += " | Inner Error: " + ex.InnerException.Message;
        }

        var response = new ServerResponse<User>(false, errorMessage, null);

        return response;
      }
    }

    public async Task<ServerResponse<bool>> SetTeacherForCourse(UserToCourseDTO userToCourseDTO)
    {
      try
      {
        var userResponse = await _userRepo.Get(userToCourseDTO.UserId);

        var courseResponse = await Get(userToCourseDTO.CourseId);

        if (userResponse == null || courseResponse == null || userResponse.IsSucceeded == false || courseResponse.IsSucceeded == false)
        {
          return new ServerResponse<bool>(false, "کاربر یا دوره با این مشخصات پیدا نشد", false);
        }

        courseResponse.Data.TeacherId = userToCourseDTO.UserId;

        await Update(courseResponse.Data.Id, courseResponse.Data);

        return new ServerResponse<bool>(true, "مدرس دوره تغییر کرد", true);
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

    public async Task<ServerResponse<bool>> AddUserToCourse(UserToCourseDTO userToCourseDTO)
    {
      try
      {
        var userResponse = await _userRepo.Get(userToCourseDTO.UserId);

        var courseResponse = await Get(userToCourseDTO.CourseId);

        if (userResponse == null || courseResponse == null || userResponse.IsSucceeded == false || courseResponse.IsSucceeded == false)
        {
          return new ServerResponse<bool>(false, "کاربر یا دوره با این مشخصات پیدا نشد", false);
        }

        // Check if the user is already enrolled in the course
        var existingUserCourse = await _context.UserCourses
            .AnyAsync(uc => uc.UserId == userToCourseDTO.UserId && uc.CourseId == userToCourseDTO.CourseId);

        if (existingUserCourse)
        {
          return new ServerResponse<bool>(false, "کاربر قبلاً در دوره ثبت‌نام کرده است", false);
        }

        // Check if the user is the teacher of the course
        if (userToCourseDTO.UserId == courseResponse.Data.TeacherId)
        {
          return new ServerResponse<bool>(false, "استاد دوره نمی‌تواند به لیست دانشجویان دوره اضافه شود", false);
        }

        var userCourse = new UserCourse
        {
          UserId = userToCourseDTO.UserId,
          CourseId = userToCourseDTO.CourseId,
        };

        await _context.UserCourses.AddAsync(userCourse);

        await _context.SaveChangesAsync();

        return new ServerResponse<bool>(true, "کاربر به لیست دانشجویان دوره اضافه شد", true);
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

    public async Task<ServerResponse<bool>> RemoveUserFromCourse(UserToCourseDTO userToCourseDTO)
    {
      try
      {
        var userCourse = await _context.UserCourses
            .SingleOrDefaultAsync(uc => uc.UserId == userToCourseDTO.UserId && uc.CourseId == userToCourseDTO.CourseId);

        if (userCourse == null)
        {
          return new ServerResponse<bool>(false, "کاربر یا دوره با این مشخصات پیدا نشد", false);
        }

        _context.UserCourses.Remove(userCourse);

        await _context.SaveChangesAsync();

        return new ServerResponse<bool>(true, "کاربر از لیست دانشجویان دوره حذف شد", true);
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
  }
}
