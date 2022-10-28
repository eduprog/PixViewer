using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixViewer.BLL;
using PixViewer.Models.API;
using PixViewer.Models.API.Input;
using PixViewer.Utils;

namespace PixViewer.API.Controllers {
  [Route("user")]
  [ApiController]
  public class UserController: ControllerBase {

    [Authorize]
    [HttpPost, Route("create")]
    public IActionResult Create([FromBody] UserCreateModel userInput) {
      try {
        if(!User.IsRoleIn("write")) { return Forbid(); }

        var requester = UserBLL.Get(Convert.ToInt32(User.Identity.Name));
        var newUser = new UserModel {
          Active = true,
          CreateDate = DateTime.UtcNow,
          FullName = userInput.FullName,
          Login = userInput.Login,
          Password = userInput.Password,
          MaxWebhooksAvailables = userInput.MaxWebhooksAvailables,
          UserProfile = userInput.UserProfile
        };

        var responseAction = UserBLL.Create(newUser, requester);

        if(responseAction.Contains("ERRO"))
          return BadRequest(responseAction);

        else if(responseAction.Contains("OK"))
          return Ok(responseAction);

        var statusCode = responseAction.GetOnlyNumbers().IsFilled()? Convert.ToInt32(responseAction.GetOnlyNumbers()) : 400;
        return StatusCode(statusCode, responseAction);
      } catch(Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }

    }

    [Authorize]
    [HttpDelete, Route("delete/{userId}")]
    public IActionResult Delete([FromRoute] int userId) {
      try {
        if(!User.IsRoleIn("delete")) { return Forbid(); }

        var requester = UserBLL.Get(Convert.ToInt32(User.Identity.Name));
        var userForDel = UserBLL.Get(userId);

        if(!userForDel.IsFilled())
          return NotFound();

        var responseAction = UserBLL.Delete(userForDel, requester);
        if(responseAction.Contains("ERRO"))
          return BadRequest(responseAction);

        else if(responseAction.Contains("OK"))
          return Ok(responseAction);

        var statusCode = responseAction.GetOnlyNumbers().IsFilled() ? Convert.ToInt32(responseAction.GetOnlyNumbers()) : 400;
        return StatusCode(statusCode, responseAction);

      } catch(Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }


    }

    [Authorize]
    [HttpGet, Route("getById/{userId}")]
    public IActionResult Get([FromRoute] int userId) {
      try {
        if(!User.IsRoleIn("read")) { return Forbid(); }
        var requester = UserBLL.Get(Convert.ToInt32(User.Identity.Name));

        var userInfo = UserBLL.Get(userId, requester);

        if(!userInfo.IsFilled())
          return NotFound();

        return Ok(userInfo);
      } catch(Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }

    }

    [Authorize]
    [HttpGet, Route("getByIdentifier/{uniqueIdentifier}")]
    public IActionResult Get([FromRoute] string uniqueIdentifier) {
      try {
        if(!User.IsRoleIn("read")) { return Forbid(); }
        var requester = UserBLL.Get(Convert.ToInt32(User.Identity.Name));

        var userInfo = UserBLL.Get(uniqueIdentifier, requester);

        if(!userInfo.IsFilled())
          return NotFound();

        return Ok(userInfo);
      } catch(Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }

    }

    [Authorize]
    [HttpPut, Route("update")]
    public IActionResult Update([FromBody] UserUpdateModel userInput) {
      try {
        if(!User.IsRoleIn("modify")) { return Forbid(); }

        var requester = UserBLL.Get(Convert.ToInt32(User.Identity.Name));
        var userForUpdate = new UserModel {
          Active = userInput.Active,
          FullName = userInput.FullName,
          LastAccess = userInput.LastAccess,
          Id = userInput.Id,
          Password = userInput.Password,
          UserProfile = userInput.UserProfile,
          MaxWebhooksAvailables = userInput.MaxWebhooksAvailables
        };

        var responseAction = UserBLL.Update(userForUpdate, requester);

        if(responseAction.Contains("ERRO"))
          return BadRequest(responseAction);

        else if(responseAction.Contains("OK"))
          return Ok(responseAction);

        var statusCode = responseAction.GetOnlyNumbers().IsFilled() ? Convert.ToInt32(responseAction.GetOnlyNumbers()) : 400;
        return StatusCode(statusCode, responseAction);

      } catch(Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); }
    }

  }
}
