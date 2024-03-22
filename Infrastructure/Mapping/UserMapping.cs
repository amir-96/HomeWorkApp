using Domain.BaseModels;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mapping
{
  public class UserMapping : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {
      builder.ToTable("Users");

      builder.HasKey(x => x.Id);

      builder.Property(x => x.UserName)
        .HasMaxLength(100)
        .IsRequired();
      builder.HasIndex(x => x.UserName)
        .IsUnique();
      builder.Property(x => x.Email)
          .HasMaxLength(256)
          .IsRequired();
      builder.HasIndex(x => x.Email)
        .IsUnique();
      builder.Property(x => x.HashedPassword)
        .HasMaxLength(500)
        .IsRequired();
      builder.Property(x => x.Role)
        .HasMaxLength(40)
        .IsRequired();
      builder.Property(x => x.Image)
        .HasMaxLength(1000)
        .IsRequired();

      // Set default values in the constructor

      builder.HasData(
          new User
          {
            Id = 1,
            UserName = "amir",
            Email = "amirjob75@gmail.com",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("amir12345"),
            Role = Roles.Admin,
            Image = "default.jpg"
          }
      );
    }
  }
}
