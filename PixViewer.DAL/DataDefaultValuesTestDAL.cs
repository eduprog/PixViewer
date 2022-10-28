using PixViewer.Models.API;
using PixViewer.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixViewer.DAL {
  public static class DataDefaultValuesTestDAL {

    public static void Fill() {
      Console.WriteLine("TB_ACTIONS: " + TB_ACTIONS());
      Console.WriteLine("TB_USER_PROFILE: " + TB_USER_PROFILE());
      Console.WriteLine("TB_USER_ACTIONS: " + TB_USER_ACTIONS());
      Console.WriteLine("ADD MOD & ADM: " + ADDADMIN());
    }


    private static string TB_ACTIONS() {
      using var cli = DB.GetConnection();
      var query = new StringBuilder();
      query.AppendLine("INSERT INTO TB_ACTIONS");
      query.AppendLine("VALUES ");
      query.AppendLine("  ('None',GETDATE(), 1), ");
      query.AppendLine("  ('Read',GETDATE(), 1), ");
      query.AppendLine("  ('Write',GETDATE(), 1), ");
      query.AppendLine("  ('Modify',GETDATE(), 1), ");
      query.AppendLine("  ('Delete',GETDATE(), 1) ");
      return cli.Run(query.ToString());
    }

    private static string TB_USER_PROFILE() {
      using var cli = DB.GetConnection();
      var query = new StringBuilder();
      query.AppendLine("INSERT INTO TB_PROFILE");
      query.AppendLine("VALUES ");
      query.AppendLine("  ('Client',GETDATE(), 1), ");
      query.AppendLine("  ('Basic',GETDATE(), 1), ");
      query.AppendLine("  ('Moderator',GETDATE(), 1), ");
      query.AppendLine("  ('Adminstrator',GETDATE(), 1) ");
      return cli.Run(query.ToString());
    }

    private static string TB_USER_ACTIONS() {
      using var cli = DB.GetConnection();
      var query = new StringBuilder();
      query.AppendLine("INSERT INTO TB_USER_ACTIONS");
      query.AppendLine("VALUES ");
      query.AppendLine("  (1, 1),");
      query.AppendLine("  (2, 2), (2, 3), ");
      query.AppendLine("  (3, 2), (3, 3), (3, 4), ");
      query.AppendLine("  (4, 2), (4, 3), (4, 4), (4, 5) ");
      return cli.Run(query.ToString());
    }


    public static void RESET_TABS() {
      using var cli = DB.GetConnection();
      var query = new StringBuilder();

      query.AppendLine(" DROP TABLE TB_WEBHOOK ");
      query.AppendLine(" DROP TABLE TB_USER");
      query.AppendLine(" DROP TABLE TB_USER_ACTIONS ");
      query.AppendLine(" DROP TABLE TB_ACTIONS ");
      query.AppendLine(" DROP TABLE TB_PROFILE ");
      query.AppendLine(" DROP TABLE TB_HISTORIC_WEBHOOK ");
      query.AppendLine(" DROP TABLE TB_HISTORIC_USER ");

      Console.WriteLine("RESET: " + cli.Run(query.ToString()));

    }

    private static string ADDADMIN() {
      var adm = new UserModel {
        Active = true,
        CreateDate = DateTime.UtcNow,
        FullName = "System Admin",
        Login = "adm",
        LastAccess = DateTime.UtcNow,
        Password = Helper.CryptValue($"adm_123"),
        UserProfile = UserProfile.Adminstrator,
        MaxWebhooksAvailables = 0
      };

      var mod = new UserModel {
        Active = true,
        CreateDate = DateTime.UtcNow,
        FullName = "System mod",
        Login = "mod",
        LastAccess = DateTime.UtcNow,
        Password = Helper.CryptValue($"mod_123"),
        UserProfile = UserProfile.Moderator,
        MaxWebhooksAvailables = 0
      };

      var rec = UserDAL.Create(adm);
      rec += " / " + UserDAL.Create(mod);
      return rec;
    }

  }


}
