using PixViewer.Models.API;
using PixViewer.Utils;
using System.Data.SqlClient;
using System.Text;

namespace PixViewer.DAL {
  public static class WebhookDAL {
    private static List<WebhookModel> GetWithFilter(FilterModel filter = null) {
      using var con = DB.GetConnection();
      var query = new StringBuilder();
      var cmd = new SqlCommand();

      query.AppendLine(" SELECT ");
      query.AppendLine("      ID, ");
      query.AppendLine("      CLIENT_ID, ");
      query.AppendLine("      REGISTER_DATE, ");
      query.AppendLine("      LAST_UPDATE, ");
      query.AppendLine("      PIX_KEY, ");
      query.AppendLine("      ACTIVE ");
      query.AppendLine(" FROM TB_WEBHOOK ");

      #region FILTER
      if(filter.IsFilled()) {
        string queryFilter = filter.Query.ToString();
        queryFilter = queryFilter.Trim().ToUpper()[..5].Contains("WHERE") ? queryFilter : " WHERE " + queryFilter;

        query.Append(queryFilter);

        foreach(var param in filter.SqlParameters)
          cmd.Parameters.AddWithValue(param.ParameterName, param.Value);
      }
      #endregion

      cmd.CommandText = query.ToString();
      var lstModel = con.ReturnData(cmd).DeserializeTable<List<WebhookModel>>();

      return lstModel;
    }

    #region GET

    public static List<WebhookModel> GetAll() => GetWithFilter();

    public static List<WebhookModel> Get(int clientId) {
      var lstParam = new List<SqlParameter>();
      var query = new StringBuilder();

      query.AppendLine(" WHERE CLIENT_ID = @ID ");
      lstParam.Add(new SqlParameter("ID", clientId));

      return GetWithFilter(new FilterModel {
        Query = query,
        SqlParameters = lstParam
      });

    }

    public static WebhookModel Get(string webhookKey) {
      var lstParam = new List<SqlParameter>();
      var query = new StringBuilder();

      query.AppendLine(" WHERE PIX_KEY = @KEY ");
      lstParam.Add(new SqlParameter("KEY", webhookKey));

      var resultFilter = GetWithFilter(new FilterModel {
        Query = query,
        SqlParameters = lstParam
      });

      if(resultFilter.IsFilled())
        return resultFilter.FirstOrDefault();

      return null;
    }

    #endregion

    public static string Create(WebhookModel webhook) {

      using var con = DB.GetConnection();
      var query = new StringBuilder();
      var cmd = new SqlCommand();

      #region CREATE

      query.AppendLine("INSERT INTO TB_WEBHOOK ");
      query.AppendLine("  (CLIENT_ID, REGISTER_DATE, PIX_KEY, LAST_UPDATE, ACTIVE)");
      query.AppendLine("  VALUES (");
      query.AppendLine("    @CLIENT_ID, ");
      query.AppendLine("    @REGISTER_DATE, ");
      query.AppendLine("    @PIX_KEY, ");
      query.AppendLine("    @LAST_UPDATE, ");
      query.AppendLine("    @ACTIVE ");
      query.AppendLine(")");

      #endregion

      #region SAVE HISTORIC

      query.AppendLine(" INSERT INTO TB_HISTORIC_WEBHOOK ");
      query.AppendLine("  (CLIENT_ID, REGISTER_DATE, PIX_KEY, LAST_UPDATE, ACTIVE, HISTORIC_DATE, ACTION_EXECUTED)");
      query.AppendLine("  VALUES (");
      query.AppendLine("    @CLIENT_ID, ");
      query.AppendLine("    @REGISTER_DATE, ");
      query.AppendLine("    @PIX_KEY, ");
      query.AppendLine("    @LAST_UPDATE, ");
      query.AppendLine("    @ACTIVE, ");
      query.AppendLine("    GETDATE(), ");
      query.AppendLine("    @ACTION_EXECUTED ");
      query.AppendLine(")");

      #endregion

      #region PARAMETERS
      cmd.Parameters.AddWithValue("CLIENT_ID", webhook.ClientId);
      cmd.Parameters.AddWithValue("REGISTER_DATE", webhook.RegisterDate);
      cmd.Parameters.AddWithValue("PIX_KEY", webhook.PixKey);
      cmd.Parameters.AddWithValue("LAST_UPDATE", webhook.LastUpdate);
      cmd.Parameters.AddWithValue("ACTION_EXECUTED", UserActions.Write);
      cmd.Parameters.AddWithValue("ACTIVE", webhook.Active);
      #endregion

      cmd.CommandText = query.ToString();
      return con.Run(cmd);
    }

    public static string Update(WebhookModel newInfos) {
      using var con = DB.GetConnection();
      var query = new StringBuilder();
      var cmd = new SqlCommand();

      #region UPDATE
      query.AppendLine("UPDATE TB_WEBHOOK SET");
      query.AppendLine("   LAST_UPDATE = @LAST_UPDATE, ");
      query.AppendLine("   PIX_KEY = @PIX_KEY, ");
      query.AppendLine("   ACTIVE = @ACTIVE ");
      query.AppendLine(" WHERE ID = @ID");
      #endregion

      #region SAVE HISTORIC

      query.AppendLine(" INSERT INTO TB_HISTORIC_WEBHOOK ");
      query.AppendLine("  (CLIENT_ID, REGISTER_DATE, PIX_KEY, LAST_UPDATE, ACTIVE, HISTORIC_DATE, ACTION_EXECUTED)");
      query.AppendLine("  VALUES (");
      query.AppendLine("    @CLIENT_ID, ");
      query.AppendLine("    @REGISTER_DATE, ");
      query.AppendLine("    @PIX_KEY, ");
      query.AppendLine("    @LAST_UPDATE, ");
      query.AppendLine("    @ACTIVE, ");
      query.AppendLine("    GETDATE(), ");
      query.AppendLine("    @ACTION_EXECUTED ");
      query.AppendLine(")");

      #endregion

      #region PARAMETERS
      cmd.Parameters.AddWithValue("CLIENT_ID", newInfos.ClientId);
      cmd.Parameters.AddWithValue("REGISTER_DATE", newInfos.RegisterDate);
      cmd.Parameters.AddWithValue("PIX_KEY", newInfos.PixKey);
      cmd.Parameters.AddWithValue("LAST_UPDATE", newInfos.LastUpdate);
      cmd.Parameters.AddWithValue("ACTION_EXECUTED", UserActions.Modify);
      cmd.Parameters.AddWithValue("ACTIVE", newInfos.Active);
      cmd.Parameters.AddWithValue("ID", newInfos.Id);
      #endregion

      cmd.CommandText = query.ToString();
      return con.Run(cmd);
    }

    public static string Delete(WebhookModel webhook) {
      using var con = DB.GetConnection();
      var query = new StringBuilder();
      var cmd = new SqlCommand();

      #region DELETE

      query.AppendLine(" DELETE ");
      query.AppendLine(" FROM TB_WEBHOOK ");
      query.AppendLine(" WHERE ID = @WEBHOOK_ID");

      #endregion

      #region SAVE HISTORIC

      query.AppendLine(" INSERT INTO TB_HISTORIC_WEBHOOK ");
      query.AppendLine("  (CLIENT_ID, REGISTER_DATE, PIX_KEY, LAST_UPDATE, ACTIVE, HISTORIC_DATE, ACTION_EXECUTED)");
      query.AppendLine("  VALUES (");
      query.AppendLine("    @CLIENT_ID, ");
      query.AppendLine("    @REGISTER_DATE, ");
      query.AppendLine("    @PIX_KEY, ");
      query.AppendLine("    @LAST_UPDATE, ");
      query.AppendLine("    @ACTIVE, ");
      query.AppendLine("    GETDATE(), ");
      query.AppendLine("    @ACTION_EXECUTED ");
      query.AppendLine(")");

      #endregion

      #region PARAMETERS
      cmd.Parameters.AddWithValue("WEBHOOK_ID", webhook.Id);
      cmd.Parameters.AddWithValue("CLIENT_ID", webhook.ClientId);
      cmd.Parameters.AddWithValue("REGISTER_DATE", webhook.RegisterDate);
      cmd.Parameters.AddWithValue("PIX_KEY", webhook.PixKey);
      cmd.Parameters.AddWithValue("LAST_UPDATE", webhook.LastUpdate);
      cmd.Parameters.AddWithValue("ACTION_EXECUTED", UserActions.Delete);
      cmd.Parameters.AddWithValue("ACTIVE", webhook.Active);
      #endregion

      cmd.CommandText = query.ToString();
      return con.Run(cmd);
    }
  }
}
