using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApplication.Pages
{
    public class NewGame : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AppDbContext _db;

        [BindProperty] public int BoardWidth { get; set; } = Battleships.Game.MinBoardWidth;
        [BindProperty] public int BoardHeight { get; set; } = Battleships.Game.MinBoardHeight;

        [BindProperty] public string WhiteName { get; set; } = "Player White";
        [BindProperty] public string BlackName { get; set; } = "Player Black";

        [BindProperty] public TouchMode TouchMode { get; set; } = TouchMode.NoTouch;

        [BindProperty] public bool NewMoveOnHit { get; set; } = true;

        [BindProperty] public string GameName { get; set; } = "game_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");


        public NewGame(ILogger<IndexModel> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [BindProperty]
        public Dictionary<int, int> ShipCounts { get; set; } = new Dictionary<int, int>
        {
            {5, 1},
            {4, 2},
            {3, 3},
            {2, 4},
            // {1, 5}
        };

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // TODO: Validate stuff
            Player? playerWhite = _db.Players.FirstOrDefault(x => x.Name == WhiteName);
            Player? playerBlack = _db.Players.FirstOrDefault(x => x.Name == BlackName);

            if (playerWhite == null)
            {
                playerWhite = new Player(WhiteName);
                _db.Players.Add(playerWhite);
            }

            if (playerBlack == null)
            {
                playerBlack = new Player(BlackName);
                _db.Players.Add(playerBlack);
            }

            GameSession gameSession = new GameSession(
                GameName,
                TouchMode,
                NewMoveOnHit,
                BoardWidth,
                BoardHeight,
                playerWhite,
                playerBlack
            );

            List<Boat> boats = ShipCounts
                .Select(x => new Boat(x.Key, x.Value, gameSession))
                .ToList();

            List<BoardState> boardStates = new List<BoardState>();
            var state = new BoardState(gameSession, true);
            boardStates.Add(state);
            
            List<BoardTile> boardTiles = new List<BoardTile>();
            for (int y = 0; y < BoardHeight; y++)
            {
                for (int x = 0; x < BoardWidth; x++)
                {
                    boardTiles.Add(new BoardTile(state, x, y, 0, 0, 0, 0));
                }
            }

            _db.GameSessions.Add(gameSession);
            _db.Boats.AddRange(boats);
            _db.BoardStates.AddRange(boardStates);
            _db.BoardTiles.AddRange(boardTiles);
            _db.SaveChanges();
            
            return new RedirectToPageResult("./Game", new { SessionId = gameSession.GameSessionId});
        }
    }
}