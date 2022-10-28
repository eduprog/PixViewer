using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PixViewer.Models.API {
  public class FilterModel {
    public List<SqlParameter> SqlParameters { get; set; }
    public StringBuilder Query { get; set; }
  }
}
