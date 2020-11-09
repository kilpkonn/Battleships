using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameSession
    {
        public int GameSessionId { get; set; }
        [MaxLength(64)] 
        public string Name { get; set; } = null!;
        
        public TouchMode TouchMode { get; set; }
        public bool BackToBackMovesOnHit { get; set; }
        
        [Range(10, 32)]
        public int BoardWidth { get; set; }
        
        [Range(10, 32)]
        public int BoardHeight { get; set; }

        public ICollection<Boat> Boats { get; set; } = new List<Boat>();
        public ICollection<BoardState> BoardStates { get; set; } = new List<BoardState>();

        public int PlayerWhiteId { get; set; }
        public Player PlayerWhite { get; set; } = null!;
        
        public int PlayerBlackId { get; set; }
        public Player PlayerBlack { get; set; } = null!;
    }
}