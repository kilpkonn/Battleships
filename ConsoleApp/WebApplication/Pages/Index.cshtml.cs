using System.Collections.Generic;
using System.Linq;
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
        public List<GameSession> OngoingGames { get; set; } = new List<GameSession>();

        public IndexModel(ILogger<IndexModel> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public void OnGet()
        {
            OngoingGames = _db.GameSessions.Select(x => x)
                .Include(x => x.PlayerWhite)
                .Include(x => x.PlayerBlack)
                .Reverse()
                .ToList();
        }
        
    }
}