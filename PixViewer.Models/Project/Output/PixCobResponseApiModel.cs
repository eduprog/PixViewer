using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PixViewer.Models.Project.Output {

  [Serializable]
  public class PixCobResponseApiModel {
    [JsonPropertyName("urlImagemQrCode")]
    public string QrCodeUrl { get; set; }

    [JsonPropertyName("pixCopiaECola")]
    public string PixCopyPaste { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("calendario")]
    public Calendar Calendar { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("txid")]
    public string Txid { get; set; }

    [JsonPropertyName("revisao")]
    public int Revision { get; set; }

    [JsonPropertyName("devedor")]
    public Debtor Debtor { get; set; }

    [JsonPropertyName("valor")]
    public Value Value { get; set; }

    [JsonPropertyName("chave")]
    public string Key { get; set; }

    [JsonPropertyName("solicitacaoPagador")]
    public string PayerMessage { get; set; }

    [JsonPropertyName("infoAdicionais")]
    public List<AdditionalInfo> AdditionalInfos { get; set; }
  }

  [Serializable]
  public class Calendar {
    [JsonPropertyName("expiracao")]
    public int Expires { get; set; }
  }

  [Serializable]
  public class Debtor {
    [JsonPropertyName("cpf")]
    public string Cpf { get; set; }

    [JsonPropertyName("nome")]
    public string Name { get; set; }
  }

  [Serializable]
  public class AdditionalInfo {

    [JsonPropertyName("nome")]
    public string Name { get; set; }

    [JsonPropertyName("valor")]
    public string Value { get; set; }
  }

  [Serializable]
  public class Value {
    [JsonPropertyName("original")]
    public string Original { get; set; }
  }

}
