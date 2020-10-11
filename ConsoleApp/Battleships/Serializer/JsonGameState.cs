using System.Collections.Generic;

namespace Battleships.Serializer
{
    public class JsonGameState
    {
        public bool IsInitialized { get; } = false;
        public bool IsSetup { get; } = true;
        public bool WhiteToMove { get; } = true;
        public Dictionary<string, bool[][]> Boards { get; } = new Dictionary<string, bool[][]>();
        public int Width { get; }
        public int Height { get; }

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