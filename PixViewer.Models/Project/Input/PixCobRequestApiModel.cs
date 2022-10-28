using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PixViewer.Models.Project.Input {
  [Serializable]
  public class Calendar {
    [JsonPropertyName("expiracao")]
    public int Expires { get; set; }
  }

  [Serializable]
  public class Debtor {

    [Required]
    [JsonPropertyName("cpf")]
    public string Cpf { get; set; }

    [Required]
    [JsonPropertyName("nome")]
    public string Name { get; set; }
  }

  [Serializable]
  public class AdditionalInfo {

    [Required]
    [JsonPropertyName("nome")]
    public string Name { get; set; }

    [Required]
    [JsonPropertyName("valor")]
    public string Value { get; set; }
  }
  [Serializable]
  public class Value {
    [JsonPropertyName("original")]
    public string Original { get; set; }
  }

  [Serializable]
  public class PixCobRequestApiModel {

    [Required]
    [JsonPropertyName("valor")]
    public Value Value { get; set; }

    [Required]
    [JsonPropertyName("calendario")]
    public Calendar Calendar { get; set; }

    [Required]
    [JsonPropertyName("devedor")]
    public Debtor Debtor { get; set; }

    [Required]
    [JsonPropertyName("chave")]
    public string PixKey { get; set; }

    [Required]
    [JsonPropertyName("solicitacaoPagador")]
    public string PayerMessage { get; set; }

    [JsonPropertyName("infoAdicionais")]
    public List<AdditionalInfo> AdditionalInfos { get; set; }

  }

}
