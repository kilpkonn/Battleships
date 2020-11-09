using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Boat
    {
        public int BoatId { get; set; }
        
        [Range(1, 32)]
        public int Lenght { get; set; }
        
        [Range(1, 32)]
        public int Amount { get; set; }
        
        public int GameSessionId { get; set; }
        public GameSession GameSession { get; set; } = null!;


        public Boat()
        {
        }

        public Boat(int lenght, int amount, GameSession gameSession)
        {
            Lenght = lenght;
            Amount = amount;
            GameSession = gameSession;
        }
    }
}