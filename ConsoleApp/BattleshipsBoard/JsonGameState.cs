using System.Collections.Generic;
using BattleshipsBoard;
using Util;

namespace Serializer
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

        private JsonGameState(GameBoard board)
        {
            IsInitialized = true;
            IsSetup = board.IsSetup;
            WhiteToMove = board.WhiteToMove;
            Width = board.Width;
            Height = board.Height;
            for (int i = 0; i < 4; i++)
            {
                Boards[i.ToString()] = ArrayUtils.ConvertToJagged(board.Board[i]);
            }
        }

        public static JsonGameState FromGame(GameBoard board)
        {
            return new JsonGameState(board);
        }
    }
}