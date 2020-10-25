namespace Battleships
{
    public class Configuration
    {
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }

        public TouchMode TouchMode { get; set; } = TouchMode.NoTouch;

        public Configuration(int boardWidth, int boardHeight)
        {
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
        }
    }
}