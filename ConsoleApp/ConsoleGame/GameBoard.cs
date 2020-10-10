namespace ConsoleGame
{
    public class GameBoard
    {
        enum BoardType
        {
            WhiteShips = 0,
            BlackShips = 1,
            WhiteHits = 2,
            BlackHits = 3
        }
        
        public bool [,,] Board { get; private set; }
        public bool WhiteToMove { get; private set; }

        public GameBoard(int width, int height)
        {
            Board = new bool[4, height, width];
            WhiteToMove = true;
        }

        public bool PlaceShip(int y, int x)
        {
            if (WhiteToMove && !Board[(int) BoardType.WhiteShips, y, x]) Board[(int) BoardType.WhiteShips, y, x] = true;
            else if (!WhiteToMove && !Board[(int) BoardType.BlackShips, y, x]) Board[(int) BoardType.BlackShips, y, x] = true;
            else return false;
            
            WhiteToMove = !WhiteToMove;
            return true;
        }

        public bool PlaceBomb(int y, int x)
        {
            if (WhiteToMove)
            {
                Board[(int) BoardType.WhiteHits, y, x] = true;
                return Board[(int) BoardType.BlackShips, y, x];
            }
            else
            {
                Board[(int) BoardType.BlackHits, y, x] = true;
                return Board[(int) BoardType.WhiteShips, y, x];
            }
        }
    }
}