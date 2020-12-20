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

        public void OnGet()
        {
            if (!LoadSession())
            {
                return;
            }
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
                if (GameBoard.Board[(int) boardTypeHits][y, x] != 0)
                {
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
    }
}