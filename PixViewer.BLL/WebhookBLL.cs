using PixViewer.DAL;
using PixViewer.Models.API;
using PixViewer.Utils;

namespace PixViewer.BLL {
  public static class WebhookBLL {
    private static readonly string _isNull = "the item is in invalid format. #400";
    private static readonly string _maxRecordsReached = "The user has reached the limit of webhooks to be registered. Contact the administrator to request a limit increase. #403";
    private static readonly string _unexpiredTime = "The minimum time to wait for changing the webhook has not yet been reached. #400";
    private static readonly string _invalidKey = "The informed key has invalid characters or is not a suitable CPF or CNPJ key #400";
    private static readonly string _duplicateKey = "The key provided is already registered. #400";

    private static WebhookModel GetImmutableFields(WebhookModel webhook) {
      var currentWebhook = Get(webhook.PixKey);
      webhook.RegisterDate = currentWebhook.RegisterDate;
      webhook.ClientId = currentWebhook.ClientId;
      return webhook;
    }

    public static List<WebhookModel> GetAll() => WebhookDAL.GetAll();
    public static List<WebhookModel> Get(int userId) => WebhookDAL.Get(userId);
    public static WebhookModel Get(string webhookKey) => WebhookDAL.Get(webhookKey);

    public static string Create(WebhookModel webhook) {

      #region RULE 1: CHECK IF THE CLIENT IS AVAILABLE TO ADD A NEW WEBHOOK.

      if(!webhook.IsFilled())
        return _isNull;

      var client = UserDAL.Get(webhook.ClientId);

      var lstClientWebhook = Get(webhook.ClientId);

      if(lstClientWebhook.IsFilled()) {
        if(lstClientWebhook.Count >= client.MaxWebhooksAvailables)
          return _maxRecordsReached;
      }

      #endregion

      #region RULE 2: NUMBERS ONLY NEEDED, 11 OR 14 DIGITS.

      if(webhook.PixKey.GetOnlyNumbers().Length > 14 || webhook.PixKey.GetOnlyNumbers().Length < 11)
        return _invalidKey;

      #endregion

      #region RULE 3: KEYS CANNOT BE REPEATED
      var allWebhooks = GetAll();
      if(allWebhooks != null) {
        if(allWebhooks.Where(x => x.PixKey == webhook.PixKey).Any())
          return _duplicateKey;
      }

      #endregion

      webhook.LastUpdate = DateTime.UtcNow;
      webhook.RegisterDate = DateTime.UtcNow;

      return WebhookDAL.Create(webhook);
    }

    public static string Update(WebhookModel webhook) {
      #region RULE 1: IT IS ONLY POSSIBLE TO UPDATE A KEY AFTER 7 DAYS OF THE LAST UPDATE.
      if(!webhook.IsFilled())
        return _isNull;

      var currentWebhook = Get(webhook.PixKey);
      webhook = GetImmutableFields(webhook);

      if(currentWebhook.LastUpdate.AddDays(7) > DateTime.UtcNow)
        return _unexpiredTime;
      #endregion

      return WebhookDAL.Update(webhook);
    }

    public static string Delete(WebhookModel webhook) {
      #region RULE 1: IT IS ONLY POSSIBLE TO DELETE A KEY AFTER 7 DAYS OF THE LAST UPDATE.
      if(!webhook.IsFilled())
        return _isNull;

      var currentWebhook = Get(webhook.PixKey);

      if(currentWebhook.LastUpdate.AddDays(7) > DateTime.UtcNow)
        return _unexpiredTime;
      #endregion

      return WebhookDAL.Delete(webhook);
    }


  }
}
