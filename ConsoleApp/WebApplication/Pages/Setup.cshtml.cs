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
    public class Setup : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AppDbContext _db;

        public Setup(ILogger<IndexModel> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [BindProperty(SupportsGet = true)] public int? SessionId { get; set; }
        [BindProperty(SupportsGet = true)] public bool Revert { get; set; } = false;

        [BindProperty] public int? ClickY { get; set; }
        [BindProperty] public int? ClickX { get; set; }
        [BindProperty] public bool IsHorizontal { get; set; } = true;
        [BindProperty] public int? ShipLength { get; set; }

        [BindProperty] public bool GenerateBoard { get; set; } = false;


        public GameSession? GameSession { get; set; }
        public GameBoard? GameBoard { get; set; }

        public IActionResult OnPost()
        {
            if (!LoadSession() || GameBoard!.IsSetupComplete()) return new EmptyResult();

            if (GenerateBoard)
            {
                bool success = false;
                for (int i = 0; i < 10 && !success; i++)
                {
                    if (GameBoard.GenerateBoard())
                    {
                        success = true;
                        SaveState();
                    }
                }
            } else if (ClickX != null && ClickY != null)
            {
                ProcessSetupMove();
            }

            return new AcceptedResult();
        }

        public IActionResult OnGet()
        {
            if (!LoadSession())
            {
                return new PageResult();
            }

            if (GameBoard!.IsSetupComplete())
            {
                return new RedirectToPageResult("/Game", new {SessionId});
            }
            
            if (Revert)
            {
                if (_db.BoardStates.Count() >= 2)
                {
                    var lastState = _db.BoardStates.Select(x => x)
                        .OrderByDescending(x => x.BoardStateId)
                        .Include(x => x.BoardTiles)
                        .First();
                    _db.BoardTiles.RemoveRange(lastState.BoardTiles);
                    _db.BoardStates.Remove(lastState);
                    _db.SaveChanges();
                    LoadSession();
                }
            }

            return new PageResult();
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

            SaveState();
        }

        private void SaveState()
        {
            var state = new BoardState(GameSession!, GameBoard!.WhiteToMove);
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