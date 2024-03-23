using Application.Repositories;
using Application.ViewModels.Course;
using Domain.BaseModels;
using MediatR;

namespace Application.Features.Courses.Commands
{
  public class AddStudentToCourseRequest : IRequest<ServerResponse<bool>>
  {
    public UserToCourseDTO UserToCourseDTO {  get; set; }

    public AddStudentToCourseRequest(UserToCourseDTO userToCourseDTO)
    {
      UserToCourseDTO = userToCourseDTO;
    }
  }

  public class AddStudentToCourseRequestHandler : IRequestHandler<AddStudentToCourseRequest, ServerResponse<bool>>
  {
    private readonly ICourseRepo _courseRepo;

    public AddStudentToCourseRequestHandler(ICourseRepo courseRepo)
    {
      _courseRepo = courseRepo;
    }

    public async Task<ServerResponse<bool>> Handle(AddStudentToCourseRequest request, CancellationToken cancellationToken)
    {
      var courseResponse = await _courseRepo.AddUserToCourse(request.UserToCourseDTO);

      if (courseResponse == null)
      {
        return new ServerResponse<bool>(false, "خطای سرور", false);
      }

      return courseResponse;
    }
  }
}