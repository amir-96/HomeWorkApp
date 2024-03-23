using Application.Repositories;
using Application.ViewModels.Course;
using Domain.BaseModels;
using MediatR;

namespace Application.Features.Courses.Commands
{
  public class SetTeacherForCourseRequest : IRequest<ServerResponse<bool>>
  {
    public UserToCourseDTO UserToCourseDTO { get; set; }

    public SetTeacherForCourseRequest(UserToCourseDTO userToCourseDTO)
    {
      UserToCourseDTO = userToCourseDTO;
    }
  }

  public class SetTeacherForCourseRequestHandler : IRequestHandler<SetTeacherForCourseRequest, ServerResponse<bool>>
  {
    private readonly ICourseRepo _courseRepo;

    public SetTeacherForCourseRequestHandler(ICourseRepo courseRepo)
    {
      _courseRepo = courseRepo;
    }

    public async Task<ServerResponse<bool>> Handle(SetTeacherForCourseRequest request, CancellationToken cancellationToken)
    {
      var courseResponse = await _courseRepo.SetTeacherForCourse(request.UserToCourseDTO);

      if (courseResponse == null)
      {
        return new ServerResponse<bool>(false, "خطای سرور", false);
      }

      return courseResponse;
    }
  }
}