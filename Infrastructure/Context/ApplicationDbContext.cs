using Domain.BaseModels;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<HomeWork> HomeWorks { get; set; }
    public DbSet<HomeWorkAnswer> HomeWorkAnswers { get; set; }
    public DbSet<SystemLog> SystemLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>()
        .HasMany(x => x.Courses)
        .WithMany(y => y.Users)
        .UsingEntity(j => j.ToTable("UserCourse"));

      modelBuilder.Entity<Course>()
          .HasOne(c => c.Teacher)
          .WithMany()
          .HasForeignKey(c => c.TeacherId)
          .IsRequired();

      modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
      modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
      modelBuilder.Entity<Course>().Property(c => c.Description)
             .HasColumnType("text");

      modelBuilder.Entity<User>()
        .HasData(
          new User
          {
            Id = 1,
            UserName = "amir",
            Email = "amirjob75@gmail.com",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("amir12345"),
            PhoneNumber = "09163097345",
            FirstName = "امیر",
            LastName = "حسینی",
            Role = Roles.Admin,
            Image = "default.png",
            Ballance = 0
          }
      );
    }
  }
}
