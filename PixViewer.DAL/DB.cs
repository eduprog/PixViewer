using PixViewer.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixViewer.DAL {
  public class DB: IDisposable {
    private readonly SqlConnection con = null;
    private SqlTransaction tran = null;
    private SqlCommand cmd = null;

    #region PRIVATE METHODS

    private void RollBack() {
      if(this.cmd.Transaction != null)
        this.cmd.Transaction.Rollback();
    }

    private void Commit() {
      if(this.cmd.Transaction != null)
        this.cmd.Transaction.Commit();
    }

    private SqlConnection Open() {
      if(con.State == ConnectionState.Closed) {
        con.Open();
        tran = con.BeginTransaction();
      }
      return con;
    }

    private void Close() {
      if(con.State == ConnectionState.Open)
        con.Close();
    }

    #endregion

    private DB() {
      this.con = new SqlConnection(Helper.GetFromConnectionStrings(CheckDev()));
    }

    private static string CheckDev() {
      if(Environment.UserName.ToLower().Equals("lucas"))
        return string.Format("pwrdtb_des");

      return string.Format("pwrdtb_prod");
    }

    public static DB GetConnection() => new();

    public string Run(SqlCommand command) {
      this.cmd = command;
      try {
        this.cmd.Connection = this.Open();
        this.cmd.Transaction = tran;
        this.cmd.ExecuteNonQuery();
        this.Commit();
        return string.Format($"## SUCCESS ## {DateTime.UtcNow} ## OK ##");
      } catch(Exception ex) {
        this.RollBack();

        return string.Format($"## ERRO ## {DateTime.UtcNow} ## {ex.Message} ##");
      }
    }
    public string Run(string query) => Run(new SqlCommand(query));
    public DataTable ReturnData(SqlCommand command) {
      var table = new DataTable();
      this.cmd = command;
      try {
        var adapter = new SqlDataAdapter();
        this.cmd.Connection = this.Open();
        this.cmd.Transaction = tran;

        adapter.SelectCommand = this.cmd;
        adapter.Fill(table);
        this.Commit();
      } catch {
        table = null;
        this.RollBack();
      }
      return table;
    }
    public DataTable ReturnData(string query) => ReturnData(new SqlCommand(query));
    
    public void Dispose() {
      this.Close();
      GC.SuppressFinalize(this);
      GC.Collect();
    }
  }

}

