using PixViewer.Models.Project.Settings;
using PixViewer.Utils;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace PixViewer.Project.Entities {
  public static class Request {
    public static HttpResponseMessage Execute(RequestModel request) {
      var requestMessage = request.HttpRequestMessage.GetValueOrThrowExecption();

      #region HANDLER CONFIGURATION

      var handler = new HttpClientHandler();

      foreach(var certificate in Auth.GetX509Certificates())
        handler.ClientCertificates.Add(certificate);

      handler.Credentials = Auth.GetCredentials();
      handler.AllowAutoRedirect = false;
      handler.SslProtocols = SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;

      #endregion

      #region HTTP REQUEST MESSAGE CONFIGURATION

      if(request.Authorize.IsFilled())
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(Enum.GetName(request.Authorize.TypeToken), request.Authorize.TokenValue);

      if(requestMessage.Content.IsFilled())
        requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

      if(!request.Host.IsFilled())
        request.Host = Helper.GetFromAppSettings("base_url").Replace("https://", "").Trim();

      requestMessage.Headers.Add("Host", request.Host);
      requestMessage.Headers.Add("Connection", "keep-alive");
      requestMessage.Headers.Add("Accept-Encoding", "gzip, deflate, br");
      requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
      requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

      #endregion

      using var http = new HttpClient(handler) {
        BaseAddress = new Uri(Helper.GetFromAppSettings("base_url")),
        Timeout = TimeSpan.FromSeconds(10)
      };

      return http.Send(requestMessage);
    }

    public static T Execute<T>(RequestModel request) {
      var requestMessage = request.HttpRequestMessage.GetValueOrThrowExecption();
      var authorize = request.Authorize;

      #region HANDLER CONFIGURATION

      var handler = new HttpClientHandler();

      foreach(var certificate in Auth.GetX509Certificates())
        handler.ClientCertificates.Add(certificate);

      handler.Credentials = Auth.GetCredentials();
      handler.AllowAutoRedirect = false;
      handler.SslProtocols = SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;

      #endregion

      #region HTTP REQUEST MESSAGE CONFIGURATION

      if(authorize.IsFilled())
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(Enum.GetName(authorize.TypeToken), authorize.TokenValue);

      if(requestMessage.Content.IsFilled())
        requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

      if(!request.Host.IsFilled())
        request.Host = Helper.GetFromAppSettings("base_url").Replace("https://", "").Trim();

      requestMessage.Headers.Add("Host", request.Host);
      requestMessage.Headers.Add("Connection", "keep-alive");
      requestMessage.Headers.Add("Accept-Encoding", "gzip, deflate, br");
      requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
      requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

      #endregion

      using var http = new HttpClient(handler) {
        BaseAddress = new Uri(Helper.GetFromAppSettings("base_url")),
        Timeout = TimeSpan.FromSeconds(10)
      };

      var response = http.Send(requestMessage);

      if((int)response.StatusCode > 299)
        return default;

      try {
        var body = response.Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<T>(body);
      } catch(Exception) {

        return default;
      }

    }

  }
}
