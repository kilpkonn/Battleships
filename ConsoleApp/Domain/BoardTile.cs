using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class BoardTile
    {
        public int BoardTileId { get; set; }

        public int BoardStateId { get; set; }
        public BoardState BoardState { get; set; } = null!;

        [Range(2, 64)] public int CoordX { get; set; }
        [Range(2, 64)] public int CoordY { get; set; }

        public int TileWhiteShips { get; set; }
        public int TileBlackShips { get; set; }
        public int TileWhiteHits { get; set; }
        public int TileBlackHits { get; set; }

        public BoardTile()
        {
        }

        public BoardTile(
            BoardState boardState,
            int coordX,
            int coordY,
            int tileWhiteShips,
            int tileBlackShips,
            int tileWhiteHits,
            int tileBlackHits
        )
        {
            BoardState = boardState;
            CoordX = coordX;
            CoordY = coordY;
            TileWhiteShips = tileWhiteShips;
            TileBlackShips = tileBlackShips;
            TileWhiteHits = tileWhiteHits;
            TileBlackHits = tileBlackHits;
        }
    }
}