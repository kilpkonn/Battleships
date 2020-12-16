using System;
using System.Collections.Generic;
using System.Linq;
using BattleshipsBoard;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BoardState = Domain.BoardState;

namespace WebApplication.Pages
{
    public class Game : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AppDbContext _db;

        public Game(ILogger<IndexModel> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [BindProperty(SupportsGet = true)] public int? SessionId { get; set; }

        [BindProperty(SupportsGet = true)] public int? ClickY { get; set; }
        [BindProperty(SupportsGet = true)] public int? ClickX { get; set; }
        [BindProperty(SupportsGet = true)] public bool IsHorizontal { get; set; } = true;
        [BindProperty(SupportsGet = true)] public int? ShipLength { get; set; }


        public GameSession? GameSession { get; set; }
        public GameBoard? GameBoard { get; set; }

        public void OnPost()
        {
            if (!LoadSession()) return;

            if (ClickX != null && ClickY != null)
            {
                if (GameBoard!.IsSetup)
                {
                    ProcessSetupMove();
                }
                else
                {
                    ProcessPlayMove();
                }
            }
        }

        public void OnGet()
        {
            LoadSession();
        }

        private bool LoadSession()
        {
            GameSession = _db.GameSessions.Select(x => x)
                .Where(x => x.GameSessionId == SessionId)
                .Include(x => x.Boats)
                .Include(x => x.BoardStates)
                .ThenInclude(s => s.BoardTiles)
                .Include(x => x.PlayerWhite)
                .Include(x => x.PlayerBlack)
                .First();
            if (GameSession == null) return false;

            GameBoard = GameBoard.FromGameSession(GameSession);
            return GameBoard != null;
        }

        private void ProcessSetupMove()
        {
            if (ShipLength == null) return;

            GameBoard!.PlaceShip((int) ClickY!, (int) ClickX!, (int) ShipLength, IsHorizontal);

            var state = new BoardState(GameSession!, GameBoard.WhiteToMove);
            GameSession!.BoardStates.Add(state);
            List<BoardTile> boardTiles = new();
            var s = GameBoard.BoardHistory.Last();
            for (int y = 0; y < s.Board[0].GetLength(0); y++)
            {
                for (int x = 0; x < s.Board[0].GetLength(1); x++)
                {
                    boardTiles.Add(
                        new BoardTile(state, x, y,
                            s.Board[(int) GameBoard.BoardType.WhiteShips][y, x],
                            s.Board[(int) GameBoard.BoardType.BlackShips][y, x],
                            s.Board[(int) GameBoard.BoardType.WhiteHits][y, x],
                            s.Board[(int) GameBoard.BoardType.BlackHits][y, x]));
                }
            }

            _db.BoardTiles.AddRange(boardTiles);
            _db.SaveChanges();
        }

        private void ProcessPlayMove()
        {
        }

        public int ShipsToPlaceInSize(int size)
        {
            if (GameBoard == null) return 0;

            var boardType = GameBoard!.WhiteToMove ? GameBoard.BoardType.WhiteShips : GameBoard.BoardType.BlackShips;
            return GameBoard.ShipCounts.GetValueOrDefault(size, 0) -
                   GameBoard.CountShipsWithSize(GameBoard.Board[(int) boardType], size);
        }

        public string CellShipStatus(int y, int x)
        {
            if (GameBoard != null)
            {
                var boardType = GameBoard!.WhiteToMove
                    ? GameBoard.BoardType.WhiteShips
                    : GameBoard.BoardType.BlackShips;
                if (GameBoard.Board[(int) boardType][y, x] != 0) return "ship";
            }

            return "sea";
        }

        public int MaxShipSize()
        {
            if (GameBoard == null) return 0;
            var layer = GameBoard.WhiteToMove ? GameBoard.BoardType.WhiteShips : GameBoard.BoardType.BlackShips;
            return GameBoard.ShipCounts
                .Where(s => GameBoard.CountShipsWithSize(GameBoard.Board[(int) layer], s.Key) < s.Value)
                .Max(s => s.Key);
        }
    }
}