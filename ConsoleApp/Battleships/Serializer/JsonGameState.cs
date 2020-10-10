using System.Collections.Generic;

namespace Battleships.Serializer
{
    public class JsonGameState
    {
        public bool IsSetup { get; } = true;
        public bool WhiteToMove { get; } = true;
        public Dictionary<string, bool[][]> Boards { get; } = new Dictionary<string, bool[][]>();

        private JsonGameState(Game game)
        {
            if (game.GameBoard == null) return;
            
            IsSetup = game.GameBoard.IsSetup;
            WhiteToMove = game.GameBoard.WhiteToMove;
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