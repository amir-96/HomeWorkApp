using Application.Repositories;
using Domain.BaseModels;
using Domain.Models;
using MediatR;

namespace Application.Features.Courses.Queries
{
  public class GetAllCoursesRequest : IRequest<ServerResponse<List<Course>>>
  {
  }

  public class GetAllCoursesRequestHandler : IRequestHandler<GetAllCoursesRequest, ServerResponse<List<Course>>>
  {
    private readonly ICourseRepo _courseRepo;

    public GetAllCoursesRequestHandler(ICourseRepo courseRepo)
    {
      _courseRepo = courseRepo;
    }

    public async Task<ServerResponse<List<Course>>> Handle(GetAllCoursesRequest request, CancellationToken cancellationToken)
    {
      var courseResponse = await _courseRepo.Get();

      if (courseResponse == null)
      {
        return new ServerResponse<List<Course>>(false, "خطای سرور", null);
      }

      return courseResponse;
    }
  }
}
