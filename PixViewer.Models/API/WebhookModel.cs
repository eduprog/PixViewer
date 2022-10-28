using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PixViewer.Models.API {
  public class WebhookModel: BaseModel {

    [JsonPropertyName("client_id")]
    public int ClientId { get; set; }

    [JsonPropertyName("register_date")]
    public DateTime RegisterDate { get; set; }

    [JsonPropertyName("pix_key")]
    public string PixKey { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("last_update")]
    public DateTime LastUpdate { get; set; }

  }
}
