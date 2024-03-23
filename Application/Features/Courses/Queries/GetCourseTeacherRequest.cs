using Application.Repositories;
using Domain.BaseModels;
using Domain.Models;
using MediatR;

namespace Application.Features.Courses.Queries
{
  public class GetCourseTeacherRequest : IRequest<ServerResponse<User>>
  {
    public long Id { get; set; }

    public GetCourseTeacherRequest(long id)
    {
      Id = id;
    }
  }

  public class GetCourseTeacherRequestHandler : IRequestHandler<GetCourseTeacherRequest, ServerResponse<User>>
  {
    private readonly ICourseRepo _courseRepo;

    public GetCourseTeacherRequestHandler(ICourseRepo courseRepo)
    {
      _courseRepo = courseRepo;
    }

    public async Task<ServerResponse<User>> Handle(GetCourseTeacherRequest request, CancellationToken cancellationToken)
    {
      var courseResponse = await _courseRepo.GetCourseTeacher(request.Id);

      if (courseResponse == null)
      {
        return new ServerResponse<User>(false, "خطای سرور", null);
      }

      return courseResponse;
    }
  }
}