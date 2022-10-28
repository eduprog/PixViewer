using PixViewer.Models.Project.Input;
using PixViewer.Models.Project.Output;
using PixViewer.Models.Project.Settings;
using PixViewer.Utils;
using System.Text.Json;

namespace PixViewer.Project.Entities {
  public static class PixManagement {

    #region OVERLOAD GET

    public static PixResponseApiModel Get(PixRequestApiModel pixRequest) {

      var authorizeBasic = new AuthorizeModel { TokenValue = Auth.GetBasicToken(), TypeToken = TokenType.Basic };
      var bearerToken = Auth.GetBearerToken(authorizeBasic, string.Format("pix.read"));
      var authorizeBearer = new AuthorizeModel { TokenValue = bearerToken, TypeToken = TokenType.Bearer };
      var endpoint = Helper.GetFromAppSettings("endpoint_pix");
      var baseUrl = Helper.GetFromAppSettings("base_url");

      #region QUERY PARAMS CONFIG

      var query = string.Empty;

      #region RANGE DATE
      var tempEndDate = pixRequest.EndDate.GetValueOrThrowExecption();
      var tempStartDate = pixRequest.StartDate.GetValueOrThrowExecption();

      if(tempStartDate > tempEndDate)
        throw new Exception("the start date cannot be greater than the end date.");

      if(tempEndDate.AddDays(-31) > tempStartDate)
        throw new Exception("The maximum search period exceeds 31 days. Invalid operation.");

      query = string.Concat("fim=", pixRequest.EndDate.ToString("s"));
      query = string.Concat(query, "&inicio=", pixRequest.StartDate.ToString("s"));

      #endregion

      if(pixRequest.TxId.IsFilled())
        query = string.Concat(query, "&txid=", pixRequest.TxId);

      #region KEYS

      if(pixRequest.Cnpj.IsFilled() && pixRequest.Cpf.IsFilled())
        throw new Exception("two keys are being provided (CPF AND CNPJ). Inform only one key.");

      if(pixRequest.Cpf.IsFilled())
        query = string.Concat(query, "&cpf=", pixRequest.Cpf.Replace(".", "").Replace("-", "").Trim());

      else if(pixRequest.Cnpj.IsFilled())
        query = string.Concat(query, "&cnpj=", pixRequest.Cnpj.Replace(".", "").Replace("-", "").Replace(@"\", "").Replace("/", "").Trim());

      else
        throw new Exception("No valid key provided (CPF OR CNPJ)");

      #endregion

      #region PAGE CONTROL
      if(pixRequest.CurrentPage > 0)
        query = string.Concat(query, "&paginacao.paginaAtual=", pixRequest.CurrentPage);

      if(pixRequest.MaxItensPerPage > 0)
        query = string.Concat(query, "&paginacao.itensPorPagina=", pixRequest.MaxItensPerPage);

      #endregion

      #endregion

      var request = new RequestModel {
        Authorize = authorizeBearer,
        Host = baseUrl.Replace("https://", "").Trim(),
        HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}{endpoint}?{query}")
      };

      var response = Request.Execute(request);

      if((int)response.StatusCode > 299)
        return null;

      var body = response.Content.ReadAsStringAsync().Result;
      return JsonSerializer.Deserialize<PixResponseApiModel>(body);

    }

    public static PixResponseApiModel Get(string end2EndId) {
      end2EndId = end2EndId.GetValueOrThrowExecption().Trim();
      var authorizeBasic = new AuthorizeModel { TokenValue = Auth.GetBasicToken(), TypeToken = TokenType.Basic };
      var bearerToken = Auth.GetBearerToken(authorizeBasic, string.Format("pix.read"));
      var authorizeBearer = new AuthorizeModel { TokenValue = bearerToken, TypeToken = TokenType.Bearer };
      var endpoint = Helper.GetFromAppSettings("endpoint_pix");
      var baseUrl = Helper.GetFromAppSettings("base_url");

      var request = new RequestModel {
        Authorize = authorizeBearer,
        Host = baseUrl.Replace("https://", "").Trim(),
        HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}{endpoint}/{end2EndId}")
      };

      var response = Request.Execute(request);

      if((int)response.StatusCode > 299)
        return null;

      var body = response.Content.ReadAsStringAsync().Result;
      return JsonSerializer.Deserialize<PixResponseApiModel>(body);
    }

    #endregion
  }
}

