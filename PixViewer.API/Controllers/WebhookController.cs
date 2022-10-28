using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixViewer.BLL;
using PixViewer.Models.API;
using PixViewer.Models.API.Input;
using PixViewer.Models.Project.Output;
using PixViewer.Project.Entities;
using PixViewer.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace PixViewer.API.Controllers {

  [Route("webhook")]
  [ApiController]
  public class WebhookController: ControllerBase {

    [Authorize]
    [HttpPost, Route("create")]
    public IActionResult Create([FromBody] WebhookApiCreateModel webhookModelCreate) {
      try {
        if(!webhookModelCreate.IsFilled())
          return BadRequest();

        if(!webhookModelCreate.Key.IsFilled() || !webhookModelCreate.UrlForNotify.IsFilled())
          return BadRequest();

        var resultCreateAPI = WebhookManagement.Create(webhookModelCreate.Key, webhookModelCreate.UrlForNotify);

        if(resultCreateAPI.Contains("ERRO"))
          return BadRequest();

        var webhook = new WebhookModel {
          Active = true,
          ClientId = Convert.ToInt32(User.Identity.Name),
          LastUpdate = DateTime.UtcNow,
          PixKey = webhookModelCreate.Key,
          RegisterDate = DateTime.UtcNow,
        };

        var response = WebhookBLL.Create(webhook);
        if(response.Contains("ERRO")) {
          WebhookManagement.Delete(webhookModelCreate.Key); // ROLLBACK ACTION
          return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if(response.Contains("OK"))
          return Ok(response);

        var statusCode = Convert.ToInt32(response.GetOnlyNumbers());
        return StatusCode(statusCode, response);
      } catch {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }

    [Authorize]
    [HttpDelete, Route("delete/{key}")]
    public IActionResult Delete([FromRoute] string key) {
      try {
        if(!key.IsFilled())
          return BadRequest();

        var response = WebhookBLL.Delete(WebhookBLL.Get(key));
        if(response.Contains("ERRO"))
          return BadRequest();

        var statusCode = response.GetOnlyNumbers().IsFilled() ? Convert.ToInt32(response.GetOnlyNumbers()) : 0;
        if(statusCode > 0)
          return StatusCode(statusCode, response);

        var resultApi = WebhookManagement.Delete(key);

        if(resultApi.Contains("ERRO"))
          return BadRequest();

        return Ok(response);
      } catch {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }

    [Authorize]
    [HttpGet, Route("get/{key}")]
    public IActionResult Get([FromRoute] string key) {
      try {
        if(!key.IsFilled())
          return BadRequest();

        var result = WebhookManagement.Get(key);

        if(!result.IsFilled())
          return NotFound();

        return Ok(result);
      } catch {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }

  }
}
