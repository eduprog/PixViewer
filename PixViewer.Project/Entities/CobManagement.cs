using PixViewer.Models.Project.Input;
using PixViewer.Models.Project.Output;
using PixViewer.Models.Project.Settings;
using PixViewer.Utils;
using System.Text.Json;

namespace PixViewer.Project.Entities {
  public static class CobManagement {
    public static PixCobResponseApiModel Create(PixCobRequestApiModel pixCob) {
      var authorizeBasic = new AuthorizeModel { TokenValue = Auth.GetBasicToken(), TypeToken = TokenType.Basic };
      var bearerToken = Auth.GetBearerToken(authorizeBasic, string.Format("cob.write"));
      var authorizeBearer = new AuthorizeModel { TokenValue = bearerToken, TypeToken = TokenType.Bearer };
      var endpoint = Helper.GetFromAppSettings("endpoint_cob");
      var baseUrl = Helper.GetFromAppSettings("base_url");

      var request = new RequestModel {
        Authorize = authorizeBearer,
        Host = baseUrl.Replace("https://", "").Trim(),
        HttpRequestMessage = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}{endpoint}{Guid.NewGuid().ToString().Replace("-","")}") {
          Content = new StringContent(JsonSerializer.Serialize(pixCob))
        }
      };

      var response = Request.Execute(request);

      if((int)response.StatusCode > 299)
        return null;

      var body = response.Content.ReadAsStringAsync().Result;
      return JsonSerializer.Deserialize<PixCobResponseApiModel>(body);
    }
  }
}
