using System.Text.Json.Serialization;

namespace PixViewer.Models.Project.Output {
  public class WebhookResponseApiModel {

    [JsonPropertyName("webhookUrl")]
    public string WebhookUrl { get; set; }

    [JsonPropertyName("chave")]
    public string Key { get; set; }

    [JsonPropertyName("criacao")]
    public DateTime CreateDate { get; set; }
  }
}
