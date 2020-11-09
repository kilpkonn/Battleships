using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class BoardTile
    {
        public int BoardTileId { get; set; }
        
        public int BoardStateId { get; set; }
        public BoardState BoardState { get; set; } = null!;
        
        [Range(2, 64)]
        public int CoordX { get; set; }
        [Range(2, 64)]
        public int CoordY { get; set; }
        
        public int TileWhiteShips { get; set; }
        public int TileBlackShips { get; set; }
        public int TileWhiteHits { get; set; }
        public int TileBlackHits { get; set; }
    }
}