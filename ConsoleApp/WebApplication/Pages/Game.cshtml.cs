using System;
using System.Linq;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

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
        [BindProperty(SupportsGet = true)]
        public int? SessionId { get; set; }

        public void OnGet()
        {
            Console.WriteLine(SessionId);

            var a = _db.GameSessions.Select(x => x)
                .FirstOrDefault(x => x.GameSessionId == SessionId);
            Console.WriteLine(a);
        }
    }
}