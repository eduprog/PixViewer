using PixViewer.Models.Project.Output;
using PixViewer.Models.Project.Settings;
using PixViewer.Utils;
using System.Text.Json;

namespace PixViewer.Project.Entities {
  public static class WebhookManagement {

    #region CREATE
    public static string Create(string key, string nofityUrl) {
      var authorizeBasic = new AuthorizeModel { TokenValue = Auth.GetBasicToken(), TypeToken = TokenType.Basic };
      var bearerToken = Auth.GetBearerToken(authorizeBasic, string.Format("webhook.write"));
      var authorizeBearer = new AuthorizeModel { TokenValue = bearerToken, TypeToken = TokenType.Bearer };
      var endpoint = Helper.GetFromAppSettings("endpoint_webhook");
      var baseUrl = Helper.GetFromAppSettings("base_url");

      var countDigits = key.GetValueOrThrowExecption().GetOnlyNumbers().Length;
      if(countDigits > 14 && countDigits < 11)
        throw new Exception("the key is not valid.");

      var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}{endpoint}{key}") {
        Content = new StringContent(JsonSerializer.Serialize(new { webhookUrl = nofityUrl }))
      };

      var request = new RequestModel {
        Authorize = authorizeBearer,
        Host = baseUrl.Replace("https://", "").Trim(),
        HttpRequestMessage = requestMessage
      };

      var response = Request.Execute(request);

      if((int)response.StatusCode > 299)
        return string.Format($"[ STATE: ERROR | ACTION: CREATE WEBHOOK | KEY: {key}]");

      return string.Format($"[ STATE: SUCCESS | ACTION: CREATE WEBHOOK | KEY: {key} ]");
    }
    #endregion

    #region GET
    public static WebhookResponseApiModel Get(string key) {
      try {
        var authorizeBasic = new AuthorizeModel { TokenValue = Auth.GetBasicToken(), TypeToken = TokenType.Basic };
        var bearerToken = Auth.GetBearerToken(authorizeBasic, string.Format("webhook.read"));
        var authorizeBearer = new AuthorizeModel { TokenValue = bearerToken, TypeToken = TokenType.Bearer };
        var endpoint = Helper.GetFromAppSettings("endpoint_webhook");
        var baseUrl = Helper.GetFromAppSettings("base_url");

        var request = new RequestModel {
          Authorize = authorizeBearer,
          Host = baseUrl.Replace("https://", "").Trim(),
          HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}{endpoint}{key}")
        };

        var response = Request.Execute(request);

        if((int)response.StatusCode > 299)
          return null;

        var body = response.Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<WebhookResponseApiModel>(body);
      } catch(Exception) {

        return null;
      }
      
    }
    #endregion

    #region DELETE

    public static string Delete(string key) {
      var authorizeBasic = new AuthorizeModel { TokenValue = Auth.GetBasicToken(), TypeToken = TokenType.Basic };
      var bearerToken = Auth.GetBearerToken(authorizeBasic, string.Format("webhook.write webhook.read"));
      var authorizeBearer = new AuthorizeModel { TokenValue = bearerToken, TypeToken = TokenType.Bearer };
      var endpoint = Helper.GetFromAppSettings("endpoint_webhook");
      var baseUrl = Helper.GetFromAppSettings("base_url");

      var request = new RequestModel {
        Authorize = authorizeBearer,
        Host = baseUrl.Replace("https://", "").Trim(),
        HttpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{baseUrl}{endpoint}{key}")
      };

      var response = Request.Execute(request);

      if((int)response.StatusCode > 299)
        return string.Format($"[ STATE: ERROR | ACTION: DELETE WEBHOOK | KEY: {key}]");

      return string.Format($"[ STATE: SUCCESS | ACTION: DELETE WEBHOOK | KEY: {key} ]");
    }
    #endregion

  }

}
