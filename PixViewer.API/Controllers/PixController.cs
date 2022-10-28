using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixViewer.BLL;
using PixViewer.Models.API;
using PixViewer.Models.Project.Input;
using PixViewer.Project.Entities;
using PixViewer.Utils;

namespace PixViewer.API.Controllers {

  [Route("pix")]
  [ApiController]
  public class PixController: ControllerBase {

    [Authorize]
    [HttpGet, Route("get/{end2EndId}")]
    public IActionResult Get([FromRoute] string end2EndId) {
      try {
        if(!end2EndId.IsFilled())
          return StatusCode(StatusCodes.Status400BadRequest);

        var result = PixManagement.Get(end2EndId);

        if(!result.IsFilled())
          return NotFound();

        return Ok(result);
      } catch {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }

    [Authorize]
    [HttpPost, Route("getByQuery")]
    public IActionResult Get([FromBody] PixRequestApiModel details) {
      try {
        if(!details.IsFilled())
          return BadRequest();

        var result = PixManagement.Get(details);

        if(!result.IsFilled())
          return NotFound();

        return Ok(result);
      } catch {
        return BadRequest();
      }
    }

  }
}
