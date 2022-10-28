using Microsoft.IdentityModel.Tokens;
using PixViewer.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PixViewer.Project.Service.Token {
  public class TokenService: ITokenService {
    
    private static readonly string _secret = "59049f1b-3419-432b-bf4e-e49aef24cdbe-8414727c-aaf2-4b73-ba18-359e27c4f277-600f0356-d1c4-405a-9159-8c1da0459ad6-a895b6f4-2e5f-4967-8177-f956d85014e1";

    public static byte[] GetSecret() => Encoding.ASCII.GetBytes(_secret);

    public string Generate(int id, List<UserActions> actions) {

      var key = Encoding.ASCII.GetBytes(_secret);
      var descriptor = new SecurityTokenDescriptor() {
        Expires = DateTime.UtcNow.AddHours(8),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        Subject = new ClaimsIdentity(new[] {
          new Claim(ClaimTypes.Name, id.ToString()),
          new Claim(ClaimTypes.Role, string.Join(',', actions).ToLower().Trim())
        })
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(descriptor);

      return tokenHandler.WriteToken(token);
    }

    public string Refresh() {
      throw new NotImplementedException();
    }
  }
}
