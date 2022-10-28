using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixViewer.BLL;
using PixViewer.Models.API;
using PixViewer.Models.API.Input;
using PixViewer.Project.Service.Token;
using PixViewer.Utils;

namespace PixViewer.API.Controllers {
  [Route("login")]
  [ApiController]
  public class LoginController: ControllerBase {
    private readonly ITokenService _tokenService;

    public LoginController(ITokenService tokenService) {
      _tokenService = tokenService;
    }

    [HttpPost, Route("authenticate")]
    public IActionResult Authenticate(LoginModel login) {
      try {
        if(!login.IsFilled())
          return BadRequest();

        var user = UserBLL.Get(login);
        if(!user.IsFilled())
          return NotFound();

        var token = _tokenService.Generate(user.Id, user.Permissions);
        var response = new { Token = token, Scheme = Enum.GetName(TokenType.Bearer) };

        return Ok(response);
      } catch(Exception ex) {
        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

      }


    }

  }
}
