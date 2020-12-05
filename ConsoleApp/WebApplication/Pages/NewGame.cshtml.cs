using System;
using System.Collections.Generic;
using System.Diagnostics;
using Battleships;
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

        [BindProperty]
        public int BoardWidth { get; set; } = Game.MinBoardWidth;
        [BindProperty]
        public int BoardHeight { get; set; } = Game.MinBoardHeight;

        [BindProperty]
        public TouchMode TouchMode { get; set; } = TouchMode.NoTouch;

        [BindProperty]
        public bool NewMoveOnHit { get; set; } = true;
        
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

        public void OnPost()
        {
            Console.WriteLine(BoardWidth);
        }
    }
}