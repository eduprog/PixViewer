using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PixViewer.Utils {
  public static class Helper {
    private static string ConvertToDate(this object data) => data.GetSafeValue<DateTime>().ToString("s");

    public static T DeserializeTable<T>(this DataTable table, List<Type> numTypes = null) where T : IEnumerable {
      var jsonOptions = new JsonSerializerOptions() {  
        PropertyNameCaseInsensitive = true, 
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters = { new BooleanConverter() }
      };
      var json = new StringBuilder();
      T typedObject = default;

      if(numTypes == null)
        numTypes = new List<Type> { typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal), typeof(int), typeof(long), typeof(byte) };

      if(table == null)
        return typedObject;

      if(table.Rows.Count <= 0)
        return typedObject;

      json.Append('['); // START JSON LIST

      int countRow = 0;
      foreach(DataRow row in table.Rows) {
        countRow++;

        json.Append('{'); // start json object

        int countCol = 0;
        foreach(DataColumn col in table.Columns) {
          countCol++;

          // TO END THE JSON OBJECT
          if(countCol == table.Columns.Count) {
            // ONLY NUMBERS OR DATES
            if(numTypes.Contains(row[col].GetType()) || col.DataType.Name.ToLower().Contains("bit")) {
              json.Append($"\"{col.ColumnName}\" : {row[col].ToString().Replace(',', '.')}"); // close json line last item (NUMBER ONLY)

            } else if(col.DataType.Name.ToLower().Contains("date")) {
              json.Append($"\"{col.ColumnName}\" : \"{row[col].ConvertToDate()}\""); // close json line last item (DATETIME ONLY)

            } else {
              json.Append($"\"{col.ColumnName}\" : \"{row[col]}\""); // close json object last item

            }

            continue; // prevents json from getting unformatted and skips to next item
          }

          // FOR ITEMS THAT ARE ON THE LIST WAITING TO BE INSERTED
          if(numTypes.Contains(row[col].GetType())) {
            json.Append($"\"{col.ColumnName}\" : {row[col].ToString().Replace(',', '.')},"); // terminate json line with multiple items (NUMBER ONLY)

          } else if(col.DataType.Name.ToLower().Contains("date")) {
            json.Append($"\"{col.ColumnName}\" : \"{row[col].ConvertToDate()}\","); // terminate json line with multiple items (DATETIME ONLY)

          } else {
            json.Append($"\"{col.ColumnName}\" : \"{row[col]}\","); // terminate json line with multiple items

          }

        }

        if(countRow == table.Rows.Count) {
          json.Append('}'); // close json object last item

        } else {
          json.Append("},"); // terminate json object with multiple items

        }
      }

      json.Append(']'); // END JSON LIST

      string jsonConvert = json.ToString(); // redundant - for test only
      typedObject = JsonSerializer.Deserialize<T>(jsonConvert, jsonOptions);
      return typedObject;

    }

    public static T GetSafeValue<T>(this object value) {
      try {
        return (T)Convert.ChangeType(value, typeof(T));
      } catch {
        return default;
      }
    }

    public static T GetSafeValue<T>(this T value) {
      try {
        return (T)Convert.ChangeType(value, typeof(T));
      } catch {
        return default;
      }
    }

    public static T GetValueOrThrowExecption<T>(this T param) {
      if(!param.IsFilled())
        throw new Exception("ERROR # The value entered is null or empty.");

      return param;
    }

    public static bool IsFilled<T>(this T param) {
      if(param == null)
        return false;

      if(param.ToString() == string.Empty)
        return false;

      return true;
    }

    public static string CheckException(Exception exception) {
      string errorMessage = string.Empty;
      while(exception != null) {
        errorMessage = string.Concat(errorMessage, exception.Message, " # ");
        exception = exception.InnerException;
      }

      return errorMessage;
    }

    public static string GetFromAppSettings(string key) => ConfigurationManager.AppSettings[key].GetValueOrThrowExecption();

    public static string GetFromConnectionStrings(string key) => ConfigurationManager.ConnectionStrings[key].ConnectionString.GetValueOrThrowExecption();

    public static string CryptValue(string value) {
      byte[] salt = new byte[128 / 8];
      byte[] codify = KeyDerivation.Pbkdf2(value, salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8);
      return Convert.ToBase64String(codify);
    }

    public static string GetRandomString(int length) {
      string randomString = string.Empty;
      string character = string.Format("0123456789QWERTYUIOPASDFGHJKLZXCVBNMqwertyuipolaksjdhfgzmxncbv");
      for(int i = 0; i < length; i++) {
        Random random = new Random();
        int pos = random.Next(0, character.Length);
        randomString += character[pos];
      }

      return randomString;
    }

    public static string GetOnlyNumbers(this string value) {
      string newStr = string.Empty;
      foreach(char c in value) {
        if(char.IsNumber(c))
          newStr += c;
      }
      return newStr;
    }

    public static string GenerateToken() => string.Concat(Guid.NewGuid(),"-", Guid.NewGuid(), "-", Guid.NewGuid(), "-", Guid.NewGuid());
    
    public static bool IsRoleIn(this ClaimsPrincipal claim, string[] roles) {
      bool exists = false;
      foreach(var role in roles) {
       var result = claim.HasClaim(x => x.Value.Contains(role));
        if(result) {
          exists = true;
          break;
        }
          
      }
      return exists;
    }

    public static bool IsRoleIn(this ClaimsPrincipal claim, string role) => claim.HasClaim(x => x.Value.Contains(role));
      

  }
}
