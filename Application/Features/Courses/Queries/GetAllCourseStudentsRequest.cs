using Application.Repositories;
using Domain.BaseModels;
using Domain.Models;
using MediatR;

namespace Application.Features.Courses.Queries
{
  public class GetAllCourseStudentsRequest : IRequest<ServerResponse<List<User>>>
  {
    public long Id { get; set; }

    public GetAllCourseStudentsRequest(long id)
    {
      Id = id;
    }
  }

  public class GetAllCourseStudentsRequestHandler : IRequestHandler<GetAllCourseStudentsRequest, ServerResponse<List<User>>>
  {
    private readonly ICourseRepo _courseRepo;

    public GetAllCourseStudentsRequestHandler(ICourseRepo courseRepo)
    {
      _courseRepo = courseRepo;
    }

    public async Task<ServerResponse<List<User>>> Handle(GetAllCourseStudentsRequest request, CancellationToken cancellationToken)
    {
      var courseResponse = await _courseRepo.GetAllCourseStudents(request.Id);

      if (courseResponse == null)
      {
        return new ServerResponse<List<User>>(false, "خطای سرور", null);
      }

      return courseResponse;
    }
  }
}