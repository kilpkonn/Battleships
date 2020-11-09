using System.Collections.Generic;

namespace Domain
{
    public class BoardState
    {
        public int BoardStateId { get; set; }
        
        public bool WhiteToMove { get; set; }
        
        public int GameSessionId { get; set; }
        public GameSession GameSession { get; set; } = null!;
        
        public ICollection<BoardTile> BoardTiles { get; set; } = new List<BoardTile>();

        public BoardState()
        {
        }

        public BoardState(GameSession gameSession, bool whiteToMove)
        {
            GameSession = gameSession;
            WhiteToMove = whiteToMove;
        }
    }
}