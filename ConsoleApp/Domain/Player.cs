using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Player
    {
        public int PlayerId { get; set; }

        [MaxLength(32)] 
        public string Name { get; set; } = null!;
        
        public int GameSessionId { get; set; }
        public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();
    }
}