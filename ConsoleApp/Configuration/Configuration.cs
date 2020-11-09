using System.Collections.Generic;
using Domain;

namespace Configuration
{
    public class Configuration
    {
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public TouchMode TouchMode { get; set; } = TouchMode.NoTouch;
        public bool BackToBackMovesOnHit { get; set; } = true;

        public Dictionary<int, int> ShipCounts { get; } = new Dictionary<int, int>
        {
            {5, 1},
            // {4, 2},
            // {3, 3},
            // {2, 4},
            // {1, 5}
        };

        public Configuration(int boardWidth, int boardHeight)
        {
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
        }
    }
}