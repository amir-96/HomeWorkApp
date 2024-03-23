using Application.Repositories;
using Application.ViewModels.Course;
using AutoMapper;
using Domain.BaseModels;
using Domain.Models;
using MediatR;

namespace Application.Features.Courses.Commands
{
  public class CreateCourseRequest : IRequest<ServerResponse<bool>>
  {
    public CreateCourseDTO CreateCourseDTO { get; set; }

    public CreateCourseRequest(CreateCourseDTO createCourseDTO)
    {
      CreateCourseDTO = createCourseDTO;
    }
  }

  public class CreateCourseRequestHandler : IRequestHandler<CreateCourseRequest, ServerResponse<bool>>
  {
    private readonly ICourseRepo _courseRepo;
    private readonly IMapper _mapper;

    public CreateCourseRequestHandler(ICourseRepo courseRepo, IMapper mapper)
    {
      _courseRepo = courseRepo;
      _mapper = mapper;
    }

    public async Task<ServerResponse<bool>> Handle(CreateCourseRequest request, CancellationToken cancellationToken)
    {
      var course = _mapper.Map<Course>(request.CreateCourseDTO);

      var courseResponse = await _courseRepo.Create(course);

      if (courseResponse == null)
      {
        return new ServerResponse<bool>(false, "خطای سرور", false);
      }

      return courseResponse;
    }
  }
}