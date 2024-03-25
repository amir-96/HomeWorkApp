using Application.ViewModels.Course;
using Domain;
using Domain.BaseModels;
using Domain.Models;

namespace Application.Repositories
{
  public interface ICourseRepo : IRepository<long, Course>
  {
    Task<ServerResponse<List<CourseTableDTO>>> GetAllCourseTable();
    Task<ServerResponse<List<User>>> GetAllCourseStudents(long courseId);
    Task<ServerResponse<CourseTableDTO>> GetCourseTable(long courseId);
    Task<ServerResponse<User>> GetCourseTeacher(long courseId);
    Task<ServerResponse<bool>> SetTeacherForCourse(UserToCourseDTO userToCourseDTO);
    Task<ServerResponse<bool>> AddUserToCourse(UserToCourseDTO userToCourseDTO);
    Task<ServerResponse<bool>> RemoveUserFromCourse(UserToCourseDTO userToCourseDTO);
  }
}
