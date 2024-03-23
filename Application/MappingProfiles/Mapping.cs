using Application.ViewModels.Course;
using Application.ViewModels.User;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfiles
{
    public class Mapping : Profile
  {
    public Mapping()
    {
      CreateMap<CreateUserDTO, User>();
      CreateMap<CreateCourseDTO, Course>();
    }
  }
}
