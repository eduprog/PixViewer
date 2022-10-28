using PixViewer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixViewer.Project.Service.Token {
  public interface ITokenService {
    public string Generate(int id, List<UserActions> actions);
    public string Refresh();
  }
}
