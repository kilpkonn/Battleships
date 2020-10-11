using System.Text.Json;

namespace Serializer
{
    public class GameJsonDeserializer
    {
        private readonly string _jsonStr;

        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        private GameJsonDeserializer(string json)
        {
            _jsonStr = json;
        }

        public static GameJsonDeserializer FromJson(string json)
        {
            return new GameJsonDeserializer(json);
        }

        public JsonGameState Deserialize()
        {
            return JsonSerializer.Deserialize<JsonGameState>(_jsonStr, _serializerOptions);
        }
    }
}