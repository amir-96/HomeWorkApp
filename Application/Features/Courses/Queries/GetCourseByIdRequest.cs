using Application.Repositories;
using Domain.BaseModels;
using Domain.Models;
using MediatR;

namespace Application.Features.Courses.Queries
{
  public class GetCourseByIdRequest : IRequest<ServerResponse<Course>>
  {
    public long Id { get; set; }

    public GetCourseByIdRequest(long id)
    {
      Id = id;
    }
  }

  public class GetCourseByIdRequestHandler : IRequestHandler<GetCourseByIdRequest, ServerResponse<Course>>
  {
    private readonly ICourseRepo _courseRepo;

    public GetCourseByIdRequestHandler(ICourseRepo courseRepo)
    {
      _courseRepo = courseRepo;
    }

    public async Task<ServerResponse<Course>> Handle(GetCourseByIdRequest request, CancellationToken cancellationToken)
    {
      var courseResponse = await _courseRepo.Get(request.Id);

      if (courseResponse == null)
      {
        return new ServerResponse<Course>(false, "خطای سرور", null);
      }

      return courseResponse;
    }
  }
}