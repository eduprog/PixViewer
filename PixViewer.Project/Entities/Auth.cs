using PixViewer.Models.Project.Output;
using PixViewer.Models.Project.Settings;
using PixViewer.Utils;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace PixViewer.Project.Entities {
  public static class Auth {
    public static string GetBasicToken() {
      var clientSettings = new ClientModel {
        ClientID = Helper.GetFromAppSettings("client_id"),
        ClientSecret = Helper.GetFromAppSettings("client_secret")
      };
      var concatStr = string.Concat(clientSettings.ClientID, ":", clientSettings.ClientSecret);
      return Convert.ToBase64String(Encoding.Default.GetBytes(concatStr));
    }

    public static string GetBearerToken(AuthorizeModel authorize, string scope, string baseUrl = "", string endpoint = "") {

      if(!endpoint.IsFilled())
        endpoint = Helper.GetFromAppSettings("endpoint_oauth2");

      if(!baseUrl.IsFilled())
        baseUrl = Helper.GetFromAppSettings("base_url");

      var body = JsonSerializer.Serialize(new { grant_type = "client_credentials", scope });
      var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}{endpoint}") {
        Content = new StringContent(body)
      };

      var request = new RequestModel {
        Authorize = authorize.GetValueOrThrowExecption(),
        HttpRequestMessage = requestMessage,
        Host = baseUrl.Replace("https://","").Trim()
      };

      var response = Request.Execute<BearerResponseApiModel>(request).AccessToken;
      return response;
    }


    public static NetworkCredential GetCredentials() {
      var credencial = new NetworkCredential {
        UserName = Helper.GetFromAppSettings("client_id"),
        Password = Helper.GetFromAppSettings("client_secret"),
      };
      return credencial;
    }

    public static X509Certificate2Collection GetX509Certificates() {
      var pem = File.ReadAllText(Helper.GetFromAppSettings("certificate_pem_filePath"));
      var key = File.ReadAllText(Helper.GetFromAppSettings("certificate_key_filePath"));
      var certificates = new X509Certificate2Collection {
        X509Certificate2.CreateFromPem(pem, key)
      };
      return certificates;
    }
  }
}
