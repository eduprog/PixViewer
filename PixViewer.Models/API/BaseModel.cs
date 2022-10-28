using PixViewer.Utils;
using System.Text.Json.Serialization;

namespace PixViewer.Models.API {
  public abstract class BaseModel {

    [JsonPropertyName("id")]
    public int Id { get; set; }
  }
}
