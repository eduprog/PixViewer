using PixViewer.Utils;
using System.Text.Json.Serialization;

namespace PixViewer.Models.API {

  [Serializable]
  public class UserModel: BaseModel {

    [JsonPropertyName("last_access_date")]
    public DateTime LastAccess { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("cre_login")]
    public string Login { get; set; }

    [JsonPropertyName("cre_password")]
    public string Password { get; set; }

    [JsonPropertyName("create_date")]
    public DateTime CreateDate { get; set; }

    [JsonPropertyName("profile_id")]
    public UserProfile UserProfile { get; set; }

    [JsonPropertyName("max_webhooks_availables")]
    public int MaxWebhooksAvailables { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("permission")]
    public List<UserActions> Permissions { get; set; }

  }
}
