using System.Text.Json;

namespace Battleships.Serializer
{
    public class GameJsonDeserializer
    {
        private readonly string _jsonStr;
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
            return JsonSerializer.Deserialize<JsonGameState>(_jsonStr);
        }
    }
}