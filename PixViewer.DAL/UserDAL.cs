using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using PixViewer.Utils;
using System.Threading.Tasks;
using PixViewer.Models.API.Input;
using PixViewer.Models.API;

namespace PixViewer.DAL {
  public static class UserDAL {
    private static List<UserModel> GetWithFilter(FilterModel filter = null) {
      using var con = DB.GetConnection();
      var query = new StringBuilder();
      var cmd = new SqlCommand();

      query.AppendLine(" SELECT ");
      query.AppendLine("      ID, ");
      query.AppendLine("      PROFILE_ID, ");
      query.AppendLine("      FULL_NAME, ");
      query.AppendLine("      CRE_LOGIN, ");
      query.AppendLine("      CRE_PASSWORD, ");
      query.AppendLine("      CREATE_DATE, ");
      query.AppendLine("      LAST_ACCESS_DATE, ");
      query.AppendLine("      MAX_WEBHOOKS_AVAILABLES, ");
      query.AppendLine("      ACTIVE ");
      query.AppendLine(" FROM TB_USER ");

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
      var lstModel = con.ReturnData(cmd).DeserializeTable<List<UserModel>>();

      if(!lstModel.IsFilled())
        return null;

      for(int i = 0; i < lstModel.Count; i++) {
        query.Clear();
        cmd.Parameters.Clear();

        query.AppendLine(" SELECT ");
        query.AppendLine("      ACTION_ID AS PERMISSION");
        query.AppendLine(" FROM TB_USER_ACTIONS ");
        query.AppendLine(" WHERE PROFILE_ID = @PROFILE_ID ");

        cmd.CommandText = query.ToString();
        cmd.Parameters.AddWithValue("PROFILE_ID", lstModel[i].UserProfile);

        lstModel[i].Permissions = new List<UserActions>();
        con.ReturnData(cmd).DeserializeTable<List<Dictionary<string, UserActions>>>().ForEach(permission => {
          lstModel[i].Permissions.Add(permission.FirstOrDefault().Value);
        });

      }

      return lstModel;
    }

    public static List<UserModel> GetAll() => GetWithFilter();

    public static UserModel Get(int userId) {
      var lstParam = new List<SqlParameter>();
      var query = new StringBuilder();

      query.AppendLine(" WHERE ID = @ID ");
      lstParam.Add(new SqlParameter("ID", userId));
      var filterResult = GetWithFilter(new FilterModel {
        Query = query,
        SqlParameters = lstParam
      });

      if(filterResult.IsFilled())
        return filterResult.FirstOrDefault();

      return null;
    }

    public static UserModel Get(string uniqueLogin) {
      var lstParam = new List<SqlParameter>();
      var query = new StringBuilder();

      query.AppendLine(" WHERE CRE_LOGIN = @LOGIN ");
      lstParam.Add(new SqlParameter("LOGIN", uniqueLogin));

      var users = GetWithFilter(new FilterModel {
        Query = query,
        SqlParameters = lstParam
      });

      if(users.IsFilled())
        return users.First();

      return null;
    }

    public static UserModel Get(LoginModel login) {
      var lstParam = new List<SqlParameter>();
      var query = new StringBuilder();

      query.AppendLine(" WHERE CRE_LOGIN = @LOGIN ");
      query.AppendLine(" AND CRE_PASSWORD = @PASSWORD ");

      lstParam.Add(new SqlParameter("LOGIN", login.Login));
      lstParam.Add(new SqlParameter("PASSWORD", login.Password));

      var users = GetWithFilter(new FilterModel {
        Query = query,
        SqlParameters = lstParam
      });

      if(users.IsFilled())
        return users.First();

      return null;
    }

    public static string Create(UserModel newUser) {

      using var con = DB.GetConnection();
      var query = new StringBuilder();
      var cmd = new SqlCommand();

      #region CREATE
      query.AppendLine("INSERT INTO TB_USER ");
      query.AppendLine("  (PROFILE_ID, FULL_NAME, CRE_LOGIN, CRE_PASSWORD, CREATE_DATE, ACTIVE, MAX_WEBHOOKS_AVAILABLES)");
      query.AppendLine("  VALUES (");
      query.AppendLine("    @PROFILE_ID, ");
      query.AppendLine("    @FULL_NAME, ");
      query.AppendLine("    @CRE_LOGIN, ");
      query.AppendLine("    @CRE_PASSWORD, ");
      query.AppendLine("    @CREATE_DATE, ");
      query.AppendLine("    @ACTIVE, ");
      query.AppendLine("    @MAX_WEBHOOKS_AVAILABLES");
      query.AppendLine(")");
      #endregion

      #region SAVE HISTORIC

      query.AppendLine("INSERT INTO TB_HISTORIC_USER ");
      query.AppendLine("  (PROFILE_ID, FULL_NAME, CRE_LOGIN, CRE_PASSWORD, CREATE_DATE, HISTORIC_DATE, ACTIVE, MAX_WEBHOOKS_AVAILABLES, ACTION_EXECUTED)");
      query.AppendLine("  VALUES (");
      query.AppendLine("    @PROFILE_ID, ");
      query.AppendLine("    @FULL_NAME, ");
      query.AppendLine("    @CRE_LOGIN, ");
      query.AppendLine("    @CRE_PASSWORD, ");
      query.AppendLine("    @CREATE_DATE, ");
      query.AppendLine("    GETDATE(), ");
      query.AppendLine("    @ACTIVE, ");
      query.AppendLine("    @MAX_WEBHOOKS_AVAILABLES, ");
      query.AppendLine("    @ACTION_EXECUTED ");
      query.AppendLine(")");

      #endregion

      #region PARAMETERS
      cmd.Parameters.AddWithValue("ACTION_EXECUTED", UserActions.Write);
      cmd.Parameters.AddWithValue("MAX_WEBHOOKS_AVAILABLES", newUser.MaxWebhooksAvailables);
      cmd.Parameters.AddWithValue("PROFILE_ID", newUser.UserProfile);
      cmd.Parameters.AddWithValue("FULL_NAME", newUser.FullName);
      cmd.Parameters.AddWithValue("CRE_LOGIN", newUser.Login);
      cmd.Parameters.AddWithValue("CRE_PASSWORD", newUser.Password);
      cmd.Parameters.AddWithValue("CREATE_DATE", newUser.CreateDate);
      cmd.Parameters.AddWithValue("ACTIVE", newUser.Active);
      #endregion

      cmd.CommandText = query.ToString();
      return con.Run(cmd);
    }

    public static string Update(UserModel newInfos) {
      using var con = DB.GetConnection();
      var query = new StringBuilder();
      var cmd = new SqlCommand();

      #region UPDATE
      query.AppendLine(" UPDATE TB_USER SET");
      query.AppendLine("   CRE_PASSWORD = @CRE_PASSWORD, ");
      query.AppendLine("   PROFILE_ID = @PROFILE_ID, ");
      query.AppendLine("   FULL_NAME = @FULL_NAME, ");
      query.AppendLine("   ACTIVE = @ACTIVE ");

      if(!newInfos.LastAccess.Equals(default)) {
        query.AppendLine("   ,LAST_ACCESS_DATE = @LAST_ACCESS_DATE ");
        cmd.Parameters.AddWithValue("LAST_ACCESS_DATE", newInfos.LastAccess);
      }

      query.AppendLine(" WHERE ID = @ID ");
      #endregion

      #region SAVE HISTORIC

      query.AppendLine(" INSERT INTO TB_HISTORIC_USER ");
      query.AppendLine("  (PROFILE_ID, FULL_NAME, CRE_LOGIN, CRE_PASSWORD, CREATE_DATE, HISTORIC_DATE, ACTIVE, MAX_WEBHOOKS_AVAILABLES, ACTION_EXECUTED)");
      query.AppendLine("  VALUES (");
      query.AppendLine("    @PROFILE_ID, ");
      query.AppendLine("    @FULL_NAME, ");
      query.AppendLine("    @CRE_LOGIN, ");
      query.AppendLine("    @CRE_PASSWORD, ");
      query.AppendLine("    @CREATE_DATE, ");
      query.AppendLine("    GETDATE(), ");
      query.AppendLine("    @ACTIVE, ");
      query.AppendLine("    @MAX_WEBHOOKS_AVAILABLES, ");
      query.AppendLine("    @ACTION_EXECUTED ");
      query.AppendLine(") ");

      #endregion

      #region PARAMETERS
      cmd.Parameters.AddWithValue("PROFILE_ID", newInfos.UserProfile);
      cmd.Parameters.AddWithValue("FULL_NAME", newInfos.FullName);
      cmd.Parameters.AddWithValue("MAX_WEBHOOKS_AVAILABLES", newInfos.MaxWebhooksAvailables);
      cmd.Parameters.AddWithValue("CRE_LOGIN", newInfos.Login);
      cmd.Parameters.AddWithValue("CREATE_DATE", newInfos.CreateDate);
      cmd.Parameters.AddWithValue("CRE_PASSWORD", newInfos.Password);
      cmd.Parameters.AddWithValue("ACTIVE", newInfos.Active);
      cmd.Parameters.AddWithValue("ID", newInfos.Id);
      cmd.Parameters.AddWithValue("ACTION_EXECUTED", UserActions.Modify);
      #endregion

      cmd.CommandText = query.ToString();
      return con.Run(cmd);
    }

    public static string Delete(UserModel user) {
      using var con = DB.GetConnection();
      var query = new StringBuilder();
      var cmd = new SqlCommand();

      #region DELETE
      query.AppendLine(" DELETE ");
      query.AppendLine(" FROM TB_USER ");
      query.AppendLine(" WHERE ID = @USER_ID ");
      #endregion

      #region SAVE HISTORIC

      query.AppendLine(" INSERT INTO TB_HISTORIC_USER ");
      query.AppendLine("  (PROFILE_ID, FULL_NAME, CRE_LOGIN, CRE_PASSWORD, CREATE_DATE, HISTORIC_DATE, ACTIVE, MAX_WEBHOOKS_AVAILABLES, ACTION_EXECUTED)");
      query.AppendLine("  VALUES (");
      query.AppendLine("    @PROFILE_ID, ");
      query.AppendLine("    @FULL_NAME, ");
      query.AppendLine("    @CRE_LOGIN, ");
      query.AppendLine("    @CRE_PASSWORD, ");
      query.AppendLine("    @CREATE_DATE, ");
      query.AppendLine("    GETDATE(), ");
      query.AppendLine("    @ACTIVE, ");
      query.AppendLine("    @MAX_WEBHOOKS_AVAILABLES, ");
      query.AppendLine("    @ACTION_EXECUTED ");
      query.AppendLine(") ");

      #endregion

      #region PARAMETERS
      cmd.Parameters.AddWithValue("PROFILE_ID", user.UserProfile);
      cmd.Parameters.AddWithValue("FULL_NAME", user.FullName);
      cmd.Parameters.AddWithValue("MAX_WEBHOOKS_AVAILABLES", user.MaxWebhooksAvailables);
      cmd.Parameters.AddWithValue("CRE_LOGIN", user.Login);
      cmd.Parameters.AddWithValue("CREATE_DATE", user.CreateDate);
      cmd.Parameters.AddWithValue("CRE_PASSWORD", user.Password);
      cmd.Parameters.AddWithValue("ACTIVE", user.Active);
      cmd.Parameters.AddWithValue("ACTION_EXECUTED", UserActions.Modify);
      #endregion

      cmd.CommandText = query.ToString();
      cmd.Parameters.AddWithValue("USER_ID", user.Id);
      return con.Run(cmd);
    }
  }
}
