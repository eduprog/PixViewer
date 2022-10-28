using PixViewer.Models.API;
using PixViewer.Utils;
using System.Data.SqlClient;
using System.Text;

namespace PixViewer.DAL {
  public static class PixCobDAL {
    public static string Create(CobModel cob) {
      using var con = DB.GetConnection();
      var query = new StringBuilder();
      var cmd = new SqlCommand();

      query.AppendLine(" INSERT INTO TB_HISTORIC_COB ");
      query.AppendLine(" (REQUESTER_ID, QRCODE, COPY_PASTE, COB_STATUS, EXPIRE_TIME, COB_LOCATION, TX_ID, DEBTOR_NAME, DEBTOR_CPF, PIX_KEY, PAYER_DESCRIPTION, ADDITIONAL_INFOS, MONETARY_VALUE, HISTORIC_DATE) ");
      query.AppendLine(" VALUES ( ");
      query.AppendLine("    @REQUESTER_ID, ");
      query.AppendLine("    @QRCODE, ");
      query.AppendLine("    @COPY_PASTE, ");
      query.AppendLine("    @COB_STATUS, ");
      query.AppendLine("    @EXPIRE_TIME, ");
      query.AppendLine("    @COB_LOCATION, ");
      query.AppendLine("    @TX_ID, ");
      query.AppendLine("    @DEBTOR_NAME, ");
      query.AppendLine("    @DEBTOR_CPF, ");
      query.AppendLine("    @PIX_KEY, ");
      query.AppendLine("    @PAYER_DESCRIPTION, ");
      query.AppendLine("    @ADDITIONAL_INFOS, ");
      query.AppendLine("    @MONETARY_VALUE, ");
      query.AppendLine("    GETDATE() ");
      query.AppendLine(" ) ");

      cmd.Parameters.AddWithValue("REQUESTER_ID", cob.RequesterId);
      cmd.Parameters.AddWithValue("QRCODE", cob.QrCode);
      cmd.Parameters.AddWithValue("COPY_PASTE", cob.CopyPaste);
      cmd.Parameters.AddWithValue("COB_STATUS", cob.Status);
      cmd.Parameters.AddWithValue("EXPIRE_TIME", cob.ExpireTime);
      cmd.Parameters.AddWithValue("COB_LOCATION", cob.CobLocation);
      cmd.Parameters.AddWithValue("TX_ID", cob.TxId);
      cmd.Parameters.AddWithValue("DEBTOR_NAME", cob.DebtorName);
      cmd.Parameters.AddWithValue("DEBTOR_CPF", cob.DebtorCpf);
      cmd.Parameters.AddWithValue("PIX_KEY", cob.PixKey);
      cmd.Parameters.AddWithValue("PAYER_DESCRIPTION", cob.PayerDescription);
      cmd.Parameters.AddWithValue("ADDITIONAL_INFOS", cob.AdditionalInfos);
      cmd.Parameters.AddWithValue("MONETARY_VALUE", cob.Value);
      cmd.CommandText = query.ToString();

      return con.Run(cmd);
    }

    public static List<CobModel> Get() => GetWithFilter().ToList();

    public static CobModel Get(int id) {
      var query = new StringBuilder();
      query.AppendLine(" WHERE ID = @ID ");

      var filter = new FilterModel {
        Query = query,
        SqlParameters = new List<SqlParameter> { new SqlParameter("ID", id) }
      };

      var result = GetWithFilter(filter);
      if(result.IsFilled())
        return result.First();

      return null;
    }

    public static CobModel Get(string txId) {
      var query = new StringBuilder();
      query.AppendLine(" WHERE TX_ID = @TXID ");

      var filter = new FilterModel {
        Query = query,
        SqlParameters = new List<SqlParameter> { new SqlParameter("TXID", txId) }
      };

      var result = GetWithFilter(filter);
      if(result.IsFilled())
        return result.First();

      return null;
    }


    private static IEnumerable<CobModel> GetWithFilter(FilterModel filter = null) {
      using var con = DB.GetConnection();
      var query = new StringBuilder();
      var cmd = new SqlCommand();

      query.AppendLine(" SELECT ");
      query.AppendLine("        ID, ");
      query.AppendLine("        REQUESTER_ID, ");
      query.AppendLine("        QRCODE, ");
      query.AppendLine("        COPY_PASTE, ");
      query.AppendLine("        COB_STATUS, ");
      query.AppendLine("        EXPIRE_TIME, ");
      query.AppendLine("        COB_LOCATION, ");
      query.AppendLine("        TX_ID, ");
      query.AppendLine("        DEBTOR_NAME, ");
      query.AppendLine("        DEBTOR_CPF, ");
      query.AppendLine("        PIX_KEY, ");
      query.AppendLine("        PAYER_DESCRIPTION, ");
      query.AppendLine("        ADDITIONAL_INFOS, ");
      query.AppendLine("        MONETARY_VALUE, ");
      query.AppendLine("        HISTORIC_DATE ");
      query.AppendLine(" FROM TB_HISTORIC_COB ");

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
      return con.ReturnData(cmd).DeserializeTable<CobModel[]>();
    }
  }
}
