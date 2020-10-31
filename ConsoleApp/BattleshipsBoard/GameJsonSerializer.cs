using System.IO;
using System.Text.Json;

namespace BattleshipsBoard
{
    public class GameJsonSerializer
    {
        private readonly GameBoard _gameBoard;
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };
        
        private GameJsonSerializer(GameBoard gameBoard)
        {
            _gameBoard = gameBoard;
        }

        public static GameJsonSerializer FromGameBoard(GameBoard board)
        {
            return new GameJsonSerializer(board);
        }

        public void SaveToFile(string name)
        {
            string json = JsonSerializer.Serialize(JsonGameState.FromGame(_gameBoard), _serializerOptions);
            File.WriteAllText(name, json);
        }
    }
}