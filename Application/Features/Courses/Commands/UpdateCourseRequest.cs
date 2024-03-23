using Application.Repositories;
using Application.ViewModels.Course;
using Domain.BaseModels;
using MediatR;

namespace Application.Features.Courses.Commands
{
  public class UpdateCourseRequest : IRequest<ServerResponse<bool>>
  {
    public EditCourseDTO EditCourseDTO { get; set; }

    public UpdateCourseRequest(EditCourseDTO editCourseDTO)
    {
      EditCourseDTO = editCourseDTO;
    }
  }

  public class UpdateCourseRequestHandler : IRequestHandler<UpdateCourseRequest, ServerResponse<bool>>
  {
    private readonly ICourseRepo _courseRepo;

    public UpdateCourseRequestHandler(ICourseRepo courseRepo)
    {
      _courseRepo = courseRepo;
    }

    public async Task<ServerResponse<bool>> Handle(UpdateCourseRequest request, CancellationToken cancellationToken)
    {
      var courseResponse = await _courseRepo.Get(request.EditCourseDTO.Id);

      if (courseResponse == null || courseResponse.IsSucceeded == false)
      {
        return new ServerResponse<bool>(false, "دوره با این مشخصات پیدا نشد", false);
      }

      courseResponse.Data.Title = request.EditCourseDTO.Title;
      courseResponse.Data.Description = request.EditCourseDTO.Description;
      courseResponse.Data.TeacherId = request.EditCourseDTO.TeacherId;
      courseResponse.Data.UpdatedAt = DateTime.UtcNow;

      var updateResponse = await _courseRepo.Update(courseResponse.Data.Id, courseResponse.Data);

      if (courseResponse == null)
      {
        return new ServerResponse<bool>(false, "خطای سرور", false);
      }

      return updateResponse;
    }
  }
}