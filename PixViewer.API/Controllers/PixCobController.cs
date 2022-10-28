using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixViewer.BLL;
using PixViewer.Models.API;
using PixViewer.Models.Project.Input;
using PixViewer.Project.Entities;
using PixViewer.Utils;

namespace PixViewer.API.Controllers {

  [Route("pixcob")]
  [ApiController]
  public class PixCobController: ControllerBase {

    [Authorize]
    [HttpGet, Route("getByTxid/{txid}")]
    public IActionResult Get([FromRoute] string txid) {
      try {
        var result = PixCobBLL.Get(txid);
        if(result.IsFilled())
          return Ok(result);

        return NotFound();
      } catch { return StatusCode(500); }
    }

    [Authorize]
    [HttpGet, Route("getById/{id}")]
    public IActionResult Get([FromRoute] int id) {
      try {
        var result = PixCobBLL.Get(id);
        if(result.IsFilled())
          return Ok(result);

        return NotFound();
      } catch { return StatusCode(500); }
    }

    [Authorize]
    [HttpGet, Route("getAll")]
    public IActionResult Get() {
      try {
        var result = PixCobBLL.Get();
        if(result.IsFilled())
          return Ok(result);

        return NotFound();
      } catch { return StatusCode(500); }
    }

    [Authorize]
    [HttpPost, Route("createCob")]
    public IActionResult Post([FromBody] PixCobRequestApiModel pixCob) {
      try {
        int requesterId = Convert.ToInt32(User.Identity.Name);

        if(!pixCob.IsFilled())
          return BadRequest();

        var dbResult = PixCobBLL.Create(new CobModel {
          AdditionalInfos = string.Join(',', pixCob.AdditionalInfos.Select(x => ($"[ {x.Name} | {x.Value} ]")).ToList()),
          ExpireTime = pixCob.Calendar.Expires,
          PixKey = pixCob.PixKey,
          PayerDescription = pixCob.PayerMessage,
          DebtorCpf = pixCob.Debtor.Cpf,
          DebtorName = pixCob.Debtor.Name,
          Value = pixCob.Value.Original,
          RequesterId = requesterId,
          CobLocation = string.Empty,
          CopyPaste = string.Empty,
          QrCode = string.Empty,
          TxId = string.Empty,
          Status = string.Empty
        });

        var resultApi = CobManagement.Create(pixCob);

        if(resultApi.IsFilled()) {
          dbResult = PixCobBLL.Create(new CobModel {
            AdditionalInfos = string.Join(',', resultApi.AdditionalInfos.Select(x => $"[ {x.Name} | {x.Value} ]").ToList()),
            ExpireTime = resultApi.Calendar.Expires,
            PixKey = resultApi.Key,
            PayerDescription = resultApi.PayerMessage,
            DebtorCpf = resultApi.Debtor.Cpf,
            DebtorName = resultApi.Debtor.Name,
            Value = resultApi.Value.Original,
            RequesterId = requesterId,
            CobLocation = resultApi.Location,
            CopyPaste = resultApi.PixCopyPaste,
            QrCode = resultApi.QrCodeUrl,
            TxId = resultApi.Txid,
            Status = resultApi.Status
          });

          return Ok(resultApi);
        }

        return BadRequest();
      } catch {
        return BadRequest();
      }

    }

  }
}
