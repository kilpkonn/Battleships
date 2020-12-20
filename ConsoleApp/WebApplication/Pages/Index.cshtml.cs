using System.Collections.Generic;
using System.Linq;
using BattleshipsBoard;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AppDbContext _db;
        public List<GameSession> OngoingGames { get; set; } = new();

        public IndexModel(ILogger<IndexModel> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public void OnGet()
        {
            OngoingGames = _db.GameSessions.Select(x => x)
                .Include(x => x.Boats)
                .Include(x => x.BoardStates)
                    .ThenInclude(s => s.BoardTiles)
                .Include(x => x.PlayerWhite)
                .Include(x => x.PlayerBlack)
                .OrderByDescending(x => x.GameSessionId)
                .ToList();
        }

        public string GetSessionState(GameSession session)
        {
            var board = GameBoard.FromGameSession(session);

            if (!board.IsSetupComplete()) return "Setup";
            if (board.GameResult() != null) return "Ended";
            return "Playing";
        }
        
    }
}