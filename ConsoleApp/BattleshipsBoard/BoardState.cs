namespace BattleshipsBoard
{
    public class BoardState
    {
        public int[][,] Board { get; }
        public bool WhiteToMove { get; }

        public BoardState(int[][,] board, bool whiteToMove)
        {
            Board = board;
            WhiteToMove = whiteToMove;
        }
    }
}