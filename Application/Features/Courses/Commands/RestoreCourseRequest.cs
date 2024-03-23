using Application.Repositories;
using Application.ViewModels.Course;
using Domain.BaseModels;
using MediatR;

namespace Application.Features.Courses.Commands
{
  public class RestoreCourseRequest : IRequest<ServerResponse<bool>>
  {
    public long Id { get; set; }

    public RestoreCourseRequest(long id)
    {
      Id = id;
    }
  }

  public class RestoreCourseRequestHandler : IRequestHandler<RestoreCourseRequest, ServerResponse<bool>>
  {
    private readonly ICourseRepo _courseRepo;

    public RestoreCourseRequestHandler(ICourseRepo courseRepo)
    {
      _courseRepo = courseRepo;
    }

    public async Task<ServerResponse<bool>> Handle(RestoreCourseRequest request, CancellationToken cancellationToken)
    {
      var courseResponse = await _courseRepo.Restore(request.Id);

      if (courseResponse == null)
      {
        return new ServerResponse<bool>(false, "خطای سرور", false);
      }

      return courseResponse;
    }
  }
}