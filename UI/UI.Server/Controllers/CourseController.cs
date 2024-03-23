using Application.Features.Courses.Commands;
using Application.Features.Courses.Queries;
using Application.ViewModels.Course;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UI.Server.Controllers
{
  [Route("api/course")]
  [ApiController]
  public class CourseController : ControllerBase
  {
    private readonly ISender _mediatrSender;

    public CourseController(ISender mediatrSender)
    {
      _mediatrSender = mediatrSender;
    }

    #region Get All Courses

    [HttpGet]
    public async Task<IActionResult> GetAllCoursesAsync()
    {
      var response = await _mediatrSender.Send(new GetAllCoursesRequest());

      if (response.IsSucceeded)
      {
        return Ok(response.Data);
      }
      else
      {
        return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
      }
    }

    #endregion

    #region Get Course By Id

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCourseAsync(long id)
    {
      var response = await _mediatrSender.Send(new GetCourseByIdRequest(id));

      if (response.IsSucceeded)
      {
        return Ok(response.Data);
      }
      else
      {
        return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
      }
    }

    #endregion

    #region Create Course

    [HttpPost("add")]
    public async Task<IActionResult> CreateCourseAsync([FromBody] CreateCourseDTO createCourseDTO)
    {
      var response = await _mediatrSender.Send(new CreateCourseRequest(createCourseDTO));

      if (response.IsSucceeded)
      {
        return StatusCode(StatusCodes.Status201Created);
      }
      else
      {
        return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
      }
    }

    #endregion

    #region Update Course

    [HttpPut("edit")]
    public async Task<IActionResult> UpdateCourseAsync([FromBody] EditCourseDTO editCourseDTO)
    {
      var response = await _mediatrSender.Send(new UpdateCourseRequest(editCourseDTO));

      if (response.IsSucceeded)
      {
        return StatusCode(StatusCodes.Status200OK);
      }
      else
      {
        return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
      }
    }

    #endregion

    #region Delete Course

    [HttpDelete]
    public async Task<IActionResult> DeleteCourseAsync(long id)
    {
      var response = await _mediatrSender.Send(new DeleteCourseRequest(id));

      if (response.IsSucceeded)
      {
        return StatusCode(StatusCodes.Status204NoContent);
      }
      else
      {
        return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
      }
    }

    #endregion

    #region Restore Course

    [HttpPut("restore")]
    public async Task<IActionResult> RestoreCourseAsync(long id)
    {
      var response = await _mediatrSender.Send(new RestoreCourseRequest(id));

      if (response.IsSucceeded)
      {
        return StatusCode(StatusCodes.Status200OK);
      }
      else
      {
        return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
      }
    }

    #endregion

    #region Get All Course Students

    [HttpGet("courseusers/{id:long}")]
    public async Task<IActionResult> GetAllCourseStudentsAsync(long id)
    {
      var response = await _mediatrSender.Send(new GetAllCourseStudentsRequest(id));

      if (response.IsSucceeded)
      {
        return Ok(response.Data);
      }
      else
      {
        return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
      }
    }

    #endregion

    #region Get Course Teacher

    [HttpGet("courseteacher/{id:long}")]
    public async Task<IActionResult> GetCourseTeacherAsync(long id)
    {
      var response = await _mediatrSender.Send(new GetCourseTeacherRequest(id));

      if (response.IsSucceeded)
      {
        return Ok(response.Data);
      }
      else
      {
        return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
      }
    }

    #endregion

    #region Set Teacher For Course

    [HttpPost("setteacher")]
    public async Task<IActionResult> SetTeacherForCourseAsync([FromBody] UserToCourseDTO userToCourseDTO)
    {
      var response = await _mediatrSender.Send(new SetTeacherForCourseRequest(userToCourseDTO));

      if (response.IsSucceeded)
      {
        return StatusCode(StatusCodes.Status201Created);
      }
      else
      {
        return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
      }
    }

    #endregion

    #region Add Student To Course

    [HttpPost("addstudent")]
    public async Task<IActionResult> AddStudentToCourseAsync([FromBody] UserToCourseDTO userToCourseDTO)
    {
      var response = await _mediatrSender.Send(new AddStudentToCourseRequest(userToCourseDTO));

      if (response.IsSucceeded)
      {
        return StatusCode(StatusCodes.Status201Created);
      }
      else
      {
        return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
      }
    }

    #endregion

    #region Remove Student From Course

    [HttpPost("removestudent")]
    public async Task<IActionResult> RemoveStudentFromCourseAsync([FromBody] UserToCourseDTO userToCourseDTO)
    {
      var response = await _mediatrSender.Send(new RemoveStudentFromCourseRequest(userToCourseDTO));

      if (response.IsSucceeded)
      {
        return StatusCode(StatusCodes.Status201Created);
      }
      else
      {
        return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
      }
    }

    #endregion
  }
}
