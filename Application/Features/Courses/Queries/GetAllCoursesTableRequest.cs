using Application.Repositories;
using Application.ViewModels.Course;
using Domain.BaseModels;
using MediatR;

namespace Application.Features.Courses.Queries
{
  public class GetAllCoursesTableRequest : IRequest<ServerResponse<List<CourseTableDTO>>>
  {
  }

  public class GetAllCoursesTableRequestHandler : IRequestHandler<GetAllCoursesTableRequest, ServerResponse<List<CourseTableDTO>>>
  {
    private readonly ICourseRepo _courseRepo;

    public GetAllCoursesTableRequestHandler(ICourseRepo courseRepo)
    {
      _courseRepo = courseRepo;
    }

    public async Task<ServerResponse<List<CourseTableDTO>>> Handle(GetAllCoursesTableRequest request, CancellationToken cancellationToken)
    {
      var courseResponse = await _courseRepo.GetAllCourseTable();

      if (courseResponse == null)
      {
        return new ServerResponse<List<CourseTableDTO>> (false, "خطای سرور", null);
      }

      return courseResponse;
    }
  }
}