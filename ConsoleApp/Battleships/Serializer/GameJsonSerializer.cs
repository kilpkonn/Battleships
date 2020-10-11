using System.Text.Json;

namespace Battleships.Serializer
{
    public class GameJsonSerializer
    {
        private readonly Game _game;
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };
        
        private GameJsonSerializer(Game game)
        {
            _game = game;
        }

        public static GameJsonSerializer FromGame(Game game)
        {
            return new GameJsonSerializer(game);
        }

        public void SaveToFile(string name)
        {
            string json = JsonSerializer.Serialize(JsonGameState.FromGame(_game), _serializerOptions);
            System.IO.File.WriteAllText(name, json);
        }
    }
}