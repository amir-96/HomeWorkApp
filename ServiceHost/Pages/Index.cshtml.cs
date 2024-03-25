using Application.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
  public class IndexModel : PageModel
  {
    private readonly ICourseRepo _courseRepo;

    public IndexModel(ICourseRepo courseRepo)
    {
      _courseRepo = courseRepo;
    }

    public List<User> UsersList { get; set; }
    public async Task OnGet()
    {
      var response = await _courseRepo.GetAllCourseStudents(1);

      UsersList = response.Data;
    }
  }
}
