using System.Collections.Generic;
using System.Linq;
using BattleshipsBoard;
using ConsoleBattleshipsUi;
using ConsoleGame;
using DAL;

namespace Battleships
{
    public class Game
    {
        public enum GameState
        {
            Menu,
            Setup,
            Game
        }

        public const int MinBoardWidth = 10;
        public const int MinBoardHeight = 10;
        public const int MaxBoardWidth = 30;
        public const int MaxBoardHeight = 30;

        private GameMenuUi _menuUi = null!;
        private GameSetupUi _setupUi = null!;
        private GamePlayUi _playUi = null!;
        

        public GameBoard? GameBoard { get; set; }
        public bool IsRunning { get; set; } = true;

        private Stack<BaseState> GameStates { get; } = new Stack<BaseState>();
        public AppDbContext Database { get; }

        public Game(AppDbContext dbCtx)
        {
            Database = dbCtx;
            PushState(GameState.Menu);
        }

        public void Run()
        {
            while (IsRunning)
            {
                GameStates.Peek().Step();
            }
        }

        public void PushState(GameState state)
        {
            BaseState? newState = null;
            switch (state)
            {
                case GameState.Menu:
                    _menuUi =
                        new ConsoleBattleshipsUi.ConsoleMenu(MinBoardWidth, MinBoardHeight, MaxBoardWidth, MaxBoardHeight, LoadDbSessions);
                    newState = new MenuState(this, _menuUi);
                    break;
                case GameState.Setup:
                    _setupUi = new ConsoleSetupView();
                    newState = new SetupState(this, _setupUi);
                    break;
                case GameState.Game:
                    _playUi = new ConsolePlayView();
                    newState = new Battleships.GameState(this, _playUi);
                    break;
            }

            GameStates.Push(newState!);
        }

        public bool PopState()
        {
            if (GameStates.Count == 0) return false;
            GameStates.Pop();
            return true;
        }

        private List<string> LoadDbSessions()
        {
            return Database.GameSessions.Select(x => x.Name).ToList();
        }

        public void Exit()
        {
            IsRunning = false;
        }
    }
}