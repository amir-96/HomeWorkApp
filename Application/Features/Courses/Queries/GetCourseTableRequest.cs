using Application.Repositories;
using Application.ViewModels.Course;
using Domain.BaseModels;
using MediatR;

namespace Application.Features.Courses.Queries
{
  public class GetCourseTableRequest : IRequest<ServerResponse<CourseTableDTO>>
  {
    public long Id { get; set; }

    public GetCourseTableRequest(long id)
    {
      Id = id;
    }
  }

  public class GetCourseTableRequestHandler : IRequestHandler<GetCourseTableRequest, ServerResponse<CourseTableDTO>>
  {
    private readonly ICourseRepo _courseRepo;

    public GetCourseTableRequestHandler(ICourseRepo courseRepo)
    {
      _courseRepo = courseRepo;
    }

    public async Task<ServerResponse<CourseTableDTO>> Handle(GetCourseTableRequest request, CancellationToken cancellationToken)
    {
      var courseResponse = await _courseRepo.GetCourseTable(request.Id);

      if (courseResponse == null)
      {
        return new ServerResponse<CourseTableDTO>(false, "خطای سرور", null);
      }

      return courseResponse;
    }
  }
}