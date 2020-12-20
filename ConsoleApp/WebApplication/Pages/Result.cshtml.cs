using System.Linq;
using BattleshipsBoard;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApplication.Pages
{
    public class Result : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AppDbContext _db;
        
        [BindProperty(SupportsGet = true)] public int? SessionId { get; set; }

        public GameSession? GameSession { get; set; }
        public GameBoard? GameBoard { get; set; }

        public Result(ILogger<IndexModel> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public void OnGet()
        {
            LoadSession();
        }

        public bool IsWhiteWon()
        {
            if (GameBoard?.GameResult() == true)
            {
                return true;
            }
            return false;
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
    }
}