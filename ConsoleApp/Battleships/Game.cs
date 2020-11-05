using System.Collections.Generic;
using BattleshipsBoard;
using ConsoleBattleshipsUi;
using ConsoleGame;

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

        private static Game? _instance;

        private readonly GameMenuUi _menuUi =
            new ConsoleBattleshipsUi.ConsoleMenu(MinBoardWidth, MinBoardHeight, MaxBoardWidth, MaxBoardHeight);
        private readonly GameSetupUi _setupUi =
            new ConsoleSetupView();
        private readonly GamePlayUi _playUi = new ConsolePlayView();

        public static Game GetInstance()
        {
            return _instance ??= new Game();
        }

        public GameBoard? GameBoard { get; set; }
        public bool IsRunning { get; set; } = true;

        private Stack<BaseState> GameStates { get; } = new Stack<BaseState>();

        private Game()
        {
            GameStates.Push(new MenuState(this, _menuUi));
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
                    newState = new MenuState(this, _menuUi);
                    break;
                case GameState.Setup:
                    newState = new SetupState(this, _setupUi);
                    break;
                case GameState.Game:
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

        public void Exit()
        {
            IsRunning = false;
        }
    }
}