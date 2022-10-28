using System.Text.Json;
using System.Text.Json.Serialization;
using PixViewer.Utils;

namespace PixViewer.Utils {
  public class BooleanConverter: JsonConverter<bool> {
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
      switch(reader.TokenType) {
        case JsonTokenType.True:
          return true;
        case JsonTokenType.False:
          return false;
        case JsonTokenType.Number:
          return reader.GetSByte() == 1;
        case JsonTokenType.String:
          return reader.GetString() switch {
            "true" => true,
            "false" => false,
            "True" => true,
            "False" => false,
            _ => throw new JsonException()
          };
        default:
          throw new JsonException();
      }
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) {
      writer.WriteBooleanValue(value);
    }
  }

}
