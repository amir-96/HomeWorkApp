using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Mapping
{
  public class CourseMapping : IEntityTypeConfiguration<Course>
  {
    public void Configure(EntityTypeBuilder<Course> builder)
    {
      builder.ToTable("Courses");

      builder.HasKey(x => x.Id);

      builder.Property(x => x.Title)
          .HasMaxLength(100)
          .IsRequired();

      builder.Property(x => x.Description)
          .HasMaxLength(500);

      // Set default values in the constructor

      builder.HasData(
          new Course
          {
            Id = 1,
            Title = "پایتون مقدماتی",
            Description = "دوره ی پایتون مقدماتی. بهترین دوره برای شروع برنامه نویسی.",
            TeacherId = 1,
          }
      );
    }
  }
}
