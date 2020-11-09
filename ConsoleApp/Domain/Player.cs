using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Player
    {
        public int PlayerId { get; set; }

        [MaxLength(32)] 
        public string Name { get; set; } = null!;
        
        public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();


        public Player()
        {
        }

        public Player(string name)
        {
            Name = name;
        }
    }
}