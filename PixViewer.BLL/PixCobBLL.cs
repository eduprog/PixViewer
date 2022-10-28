using PixViewer.DAL;
using PixViewer.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixViewer.BLL {
  public static class PixCobBLL {
    public static string Create(CobModel cob) => PixCobDAL.Create(cob);
    public static CobModel Get(int id) => PixCobDAL.Get(id);
    public static CobModel Get(string txid) => PixCobDAL.Get(txid);
    public static List<CobModel> Get() => PixCobDAL.Get();
  }
}
