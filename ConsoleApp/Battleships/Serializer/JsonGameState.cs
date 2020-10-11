using System.Collections.Generic;

namespace Battleships.Serializer
{
    public class JsonGameState
    {
        public bool IsInitialized { get; set; } = false;
        public bool IsSetup { get; set; } = true;
        public bool WhiteToMove { get; set; } = true;
        public Dictionary<string, bool[][]> Boards { get; set; } = new Dictionary<string, bool[][]>();
        public int Width { get; set; }
        public int Height { get; set; }

        public JsonGameState()
        {
        }

        private JsonGameState(Game game)
        {
            if (game.GameBoard == null) return;

            IsInitialized = true;
            IsSetup = game.GameBoard.IsSetup;
            WhiteToMove = game.GameBoard.WhiteToMove;
            Width = game.GameBoard.Width;
            Height = game.GameBoard.Height;
            for (int i = 0; i < 4; i++)
            {
                Boards[i.ToString()] = Util.ArrayUtils.ConvertToJagged(game.GameBoard.Board[i]);
            }
        }

        public static JsonGameState FromGame(Game game)
        {
            return new JsonGameState(game);
        }
    }
}