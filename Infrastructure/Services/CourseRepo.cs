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

    public async Task<ServerResponse<List<CourseTableDTO>>> GetAllCourseTable()
    {
      try
      {
        var courses = await _context.Courses
          .Where(c => c.IsActive == true)
          .Include(c => c.Teacher)
          .Include(c => c.Users)
          .ToListAsync();

        if (courses == null)
        {
          return new ServerResponse<List<CourseTableDTO>>(false, "هیچ دوره ای پیدا نشد", null);
        }

        var courseTableList = new List<CourseTableDTO>();

        foreach (var course in courses)
        {
          var courseTable = new CourseTableDTO()
          {
            Course = course,
            Teacher = course.Teacher,
            Students = course.Users.ToList()
          };

          courseTableList.Add(courseTable);
        }

        return new ServerResponse<List<CourseTableDTO>>(true, "لیست دوره با موفقیت دریافت شد", courseTableList);
      }
      catch (Exception ex)
      {
        string errorMessage = "عملیات با خطا مواجه شد : " + ex.Message;

        if (ex.InnerException != null)
        {
          errorMessage += " | Inner Error: " + ex.InnerException.Message;
        }

        var response = new ServerResponse<List<CourseTableDTO>>(false, errorMessage, null);

        return response;
      }
    }

    public async Task<ServerResponse<List<User>>> GetAllCourseStudents(long courseId)
    {
      try
      {
        var courseResponse = await _context.Courses
          .Where(c => c.IsActive == true)
          .Include(c => c.Users)
          .SingleOrDefaultAsync(c => c.Id == courseId);

        if (courseResponse == null)
        {
          return new ServerResponse<List<User>>(false, "دوره با این مشخصات پیدا نشد", null);
        }

        var courseUsers = courseResponse.Users.ToList();

        return new ServerResponse<List<User>>(true, "لیست دانشجویان دوره با موفقیت دریافت شد", courseUsers);
      }
      catch (Exception ex)
      {
        string errorMessage = "عملیات با خطا مواجه شد : " + ex.Message;

        if (ex.InnerException != null)
        {
          errorMessage += " | Inner Error: " + ex.InnerException.Message;
        }

        var response = new ServerResponse<List<User>>(false, errorMessage, null);

        return response;
      }
    }

    public async Task<ServerResponse<CourseTableDTO>> GetCourseTable(long courseId)
    {
      try
      {
        var course = await _context.Courses
          .Where(c => c.IsActive == true && c.Id == courseId)
          .Include(c => c.Teacher)
          .Include(c => c.Users)
          .SingleOrDefaultAsync();

        if (course == null)
        {
          return new ServerResponse<CourseTableDTO>(false, "دوره ای پیدا نشد", null);
        }

        var courseTable = new CourseTableDTO()
        {
          Course = course,
          Teacher = course.Teacher,
          Students = course.Users.ToList()
        };

        return new ServerResponse<CourseTableDTO>(true, "دوره با موفقیت دریافت شد", courseTable);
      }
      catch (Exception ex)
      {
        string errorMessage = "عملیات با خطا مواجه شد : " + ex.Message;

        if (ex.InnerException != null)
        {
          errorMessage += " | Inner Error: " + ex.InnerException.Message;
        }

        var response = new ServerResponse<CourseTableDTO>(false, errorMessage, null);

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

        var courseResponse = await _context.Courses
          .Where(c => c.IsActive == true)
          .Include(c => c.Users)
          .SingleOrDefaultAsync(c => c.Id == userToCourseDTO.CourseId);

        if (userResponse == null || courseResponse == null || userResponse.IsSucceeded == false)
        {
          return new ServerResponse<bool>(false, "کاربر یا دوره با این مشخصات پیدا نشد", false);
        }

        // Check if the user is already enrolled in the course
        var isEnrolled = await _context.Courses
            .Where(c => c.Id == userToCourseDTO.CourseId)
            .SelectMany(c => c.Users)
            .AnyAsync(u => u.Id == userToCourseDTO.UserId);

        if (isEnrolled)
        {
          return new ServerResponse<bool>(false, "کاربر قبلاً در دوره ثبت‌نام کرده است", false);
        }

        // Check if the user is the teacher of the course
        if (userToCourseDTO.UserId == courseResponse.TeacherId)
        {
          return new ServerResponse<bool>(false, "استاد دوره نمی‌تواند به لیست دانشجویان دوره اضافه شود", false);
        }

        var newStudent = userResponse.Data;

        courseResponse.Users.Add(newStudent);

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
        var userResponse = await _userRepo.Get(userToCourseDTO.UserId);

        var courseResponse = await _context.Courses
          .Where(c => c.IsActive == true)
          .Include(c => c.Users)
          .SingleOrDefaultAsync(c => c.Id == userToCourseDTO.CourseId);

        if (userResponse == null || courseResponse == null || userResponse.IsSucceeded == false)
        {
          return new ServerResponse<bool>(false, "کاربر یا دوره با این مشخصات پیدا نشد", false);
        }

        courseResponse.Users.Remove(userResponse.Data);

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
