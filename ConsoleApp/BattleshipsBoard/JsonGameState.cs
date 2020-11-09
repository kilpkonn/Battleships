using System.Collections.Generic;
using System.Linq;
using Configuration;
using Domain;
using Util;

namespace BattleshipsBoard
{
    public class JsonGameState
    {
        public bool IsInitialized { get; set; } = false;
        public bool IsSetup { get; set; } = true;
        public bool WhiteToMove { get; set; } = true;
        public Dictionary<string, int[][]> Boards { get; set; } = new Dictionary<string, int[][]>();
        public Dictionary<string, int> ShipCounts { get; set; } = new Dictionary<string, int>();
        public TouchMode TouchMode { get; set; }
        public bool BackToBackMovesOnHit { get; set; }
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

            ShipCounts = board.ShipCounts.ToDictionary(
                e => e.Key.ToString(),
                e => e.Value);
            TouchMode = board.TouchMode;
            BackToBackMovesOnHit = board.BackToBackHits;
        }

        public static JsonGameState FromGame(GameBoard board)
        {
            return new JsonGameState(board);
        }
    }
}