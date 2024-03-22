using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using Application.ViewModels.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UI.Server.Controllers
{
    [Route("api/users")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly ISender _mediatrSender;

    public UserController(ISender mediatrSender)
    {
      _mediatrSender = mediatrSender;
    }

    #region Get All Users

    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync()
    {
      var response = await _mediatrSender.Send(new GetAllUsersRequest());

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

    #region Get User By Id

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetUserAsync(long id)
    {
      var response = await _mediatrSender.Send(new GetUserRequest(id));

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

    #region Create User

    [HttpPost("add")]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDTO createUserDTO)
    {
      var response = await _mediatrSender.Send(new CreateUserRequest(createUserDTO));

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

    #region Update User

    [HttpPut("edit")]
    public async Task<IActionResult> UpdateUserAsync([FromBody] EditUserDTO editUserDTO)
    {
      var response = await _mediatrSender.Send(new EditUserRequest(editUserDTO));

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

    #region Update Password

    [HttpPut("password")]
    public async Task<IActionResult> UpdatePasswordUserAsync([FromBody] ChangePasswordDTO changePasswordDTO)
    {
      var response = await _mediatrSender.Send(new ChangePasswordRequest(changePasswordDTO));

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

    #region Update Image

    [HttpPut("image")]
    public async Task<IActionResult> UpdateImageUserAsync([FromBody] ChangeImageDTO changeImageDTO)
    {
      var response = await _mediatrSender.Send(new ChangeImageRequest(changeImageDTO));

      if (response)
      {
        return StatusCode(StatusCodes.Status200OK);
      }
      else
      {
        return StatusCode(StatusCodes.Status405MethodNotAllowed);
      }
    }

    #endregion

    #region Delete User

    [HttpDelete]
    public async Task<IActionResult> DeleteUserAsync(long id)
    {
      var response = await _mediatrSender.Send(new DeleteUserRequest(id));

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

    #region Restore User

    [HttpPut("restore")]
    public async Task<IActionResult> RestoreUserAsync(long id)
    {
      var response = await _mediatrSender.Send(new RestoreUserRequest(id));

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
  }
}
