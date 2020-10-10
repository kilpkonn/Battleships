namespace Battleships
{
    public class GameBoard
    {
        public enum BoardType
        {
            WhiteShips = 0,
            BlackShips = 1,
            WhiteHits = 2,
            BlackHits = 3
        }

        public bool[][,] Board { get; private set; } = new bool[4][,];
        public bool WhiteToMove { get; private set; } = true;

        public GameBoard(int width, int height)
        {
            for (int i = 0; i < 4; i++)
            {
                Board[i] = new bool[height, width];
            }
        }

        public bool PlaceShip(int y, int x)
        {
            if (WhiteToMove && !Board[(int) BoardType.WhiteShips][y, x])
                Board[(int) BoardType.WhiteShips][y, x] = true;
            else if (!WhiteToMove && !Board[(int) BoardType.BlackShips][y, x])
                Board[(int) BoardType.BlackShips][y, x] = true;
            else return false;

            WhiteToMove = !WhiteToMove;
            return true;
        }

        public bool PlaceBomb(int y, int x)
        {
            if (WhiteToMove)
            {
                Board[(int) BoardType.WhiteHits][y, x] = true;
                return Board[(int) BoardType.BlackShips][y, x];
            }
            else
            {
                Board[(int) BoardType.BlackHits][y, x] = true;
                return Board[(int) BoardType.WhiteShips][y, x];
            }
        }
    }
}