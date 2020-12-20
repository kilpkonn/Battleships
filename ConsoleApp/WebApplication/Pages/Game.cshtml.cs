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
        
        [BindProperty(SupportsGet = true)] public int? SessionId { get; set; }

        [BindProperty(SupportsGet = true)] public int? ClickY { get; set; }
        [BindProperty(SupportsGet = true)] public int? ClickX { get; set; }
        
        public GameSession? GameSession { get; set; }
        public GameBoard? GameBoard { get; set; }

        public Game(ILogger<IndexModel> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult OnPost()
        {
            if (!LoadSession()) return new EmptyResult();
            if (!GameBoard!.IsSetupComplete()) return new RedirectToPageResult("/Setup", new {SessionId});

            if (ClickX != null && ClickY != null)
            {
                ProcessMove();
            }

            return new AcceptedResult();
        }

        public IActionResult OnGet()
        {
            if (!LoadSession())
            {
                return new EmptyResult();
            }

            var result = GameBoard!.GameResult();
            if (result != null)
            {
                return new RedirectToPageResult("/Result", new {SessionId});
            }

            return new PageResult();
        }

        public string CellStatus(int y, int x)
        {
            if (GameBoard != null)
            {
                var boardTypeShips = GameBoard!.WhiteToMove
                    ? GameBoard.BoardType.BlackShips
                    : GameBoard.BoardType.WhiteShips;
                var boardTypeHits = GameBoard!.WhiteToMove
                    ? GameBoard.BoardType.WhiteHits
                    : GameBoard.BoardType.BlackHits;
                var id = GameBoard.Board[(int) boardTypeShips][y, x];
                if (GameBoard.Board[(int) boardTypeHits][y, x] != 0)
                {
                    if (id != 0 && IsSank((int) boardTypeShips, (int) boardTypeHits, y, x, id)) return "sank";
                    if (GameBoard.Board[(int) boardTypeShips][y, x] != 0) return "hit";
                    return "miss";
                }
            }

            return "sea";
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
        
        private void ProcessMove()
        {

            GameBoard!.DropBomb((int) ClickY!, (int) ClickX!);

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

        private bool IsSank(int boardShipsIdx, int boardHitsIdx, int y, int x, int id, HashSet<Tuple<int, int>>? visited = null)
        {
            if (visited == null) visited = new HashSet<Tuple<int, int>>();
            if (visited.Contains(new Tuple<int, int>(y, x))) return true;
            if (GameBoard!.Board[boardShipsIdx][y, x] != id) return true;
            if (GameBoard!.Board[boardShipsIdx][y, x] == id && GameBoard.Board[boardHitsIdx][y, x] == 0) return false;
            visited.Add(new Tuple<int, int>(y, x));

            return (y <= 0 || IsSank(boardShipsIdx, boardHitsIdx, y - 1, x, id, visited)) &&
                   (y >= GameBoard.Height - 1 || IsSank(boardShipsIdx, boardHitsIdx, y + 1, x, id, visited)) &&
                   (x <= 0 || IsSank(boardShipsIdx, boardHitsIdx, y, x - 1, id, visited)) &&
                   (x >= GameBoard.Width - 1 || IsSank(boardShipsIdx, boardHitsIdx, y, x + 1, id, visited));
        }
    }
}