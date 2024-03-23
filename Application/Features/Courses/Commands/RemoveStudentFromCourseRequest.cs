using Application.Repositories;
using Application.ViewModels.Course;
using Domain.BaseModels;
using MediatR;

namespace Application.Features.Courses.Commands
{
  public class RemoveStudentFromCourseRequest : IRequest<ServerResponse<bool>>
  {
    public UserToCourseDTO UserToCourseDTO { get; set; }

    public RemoveStudentFromCourseRequest(UserToCourseDTO userToCourseDTO)
    {
      UserToCourseDTO = userToCourseDTO;
    }
  }

  public class RemoveStudentFromCourseRequestHandler : IRequestHandler<RemoveStudentFromCourseRequest, ServerResponse<bool>>
  {
    private readonly ICourseRepo _courseRepo;

    public RemoveStudentFromCourseRequestHandler(ICourseRepo courseRepo)
    {
      _courseRepo = courseRepo;
    }

    public async Task<ServerResponse<bool>> Handle(RemoveStudentFromCourseRequest request, CancellationToken cancellationToken)
    {
      var courseResponse = await _courseRepo.RemoveUserFromCourse(request.UserToCourseDTO);

      if (courseResponse == null)
      {
        return new ServerResponse<bool>(false, "خطای سرور", false);
      }

      return courseResponse;
    }
  }
}