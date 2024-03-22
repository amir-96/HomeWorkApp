using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Mapping
{
  public class UserCourseMapping : IEntityTypeConfiguration<UserCourse>
  {
    public void Configure(EntityTypeBuilder<UserCourse> builder)
    {
      builder.ToTable("UserCourses");

      builder.HasKey(uc => uc.Id);

      // Seed initial data

      builder.HasData(
          new UserCourse
          {
            Id = 1,
            UserId = 1,
            CourseId = 1
          }
      );
    }
  }
}