using Application.Repositories;
using Domain.BaseModels;
using MediatR;

namespace Application.Features.Courses.Commands
{
  public class DeleteCourseRequest : IRequest<ServerResponse<bool>>
  {
    public long Id { get; set; }

    public DeleteCourseRequest(long id)
    {
      Id = id;
    }
  }

  public class DeleteCourseRequestHandler : IRequestHandler<DeleteCourseRequest, ServerResponse<bool>>
  {
    private readonly ICourseRepo _courseRepo;

    public DeleteCourseRequestHandler(ICourseRepo courseRepo)
    {
      _courseRepo = courseRepo;
    }

    public async Task<ServerResponse<bool>> Handle(DeleteCourseRequest request, CancellationToken cancellationToken)
    {
      var courseResponse = await _courseRepo.Delete(request.Id);

      if (courseResponse == null)
      {
        return new ServerResponse<bool>(false, "خطای سرور", false);
      }

      return courseResponse;
    }
  }
}