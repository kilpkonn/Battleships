using System.Collections.Generic;

namespace ConsoleGame
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

        private static Game? _instance = null;

        public static Game GetInstance()
        {
            return _instance ??= new Game();
        }

        public GameBoard? GameBoard { get; set; }
        public bool IsRunning { get; set; } = true;

        private Stack<BaseState> GameStates { get; set; } = new Stack<BaseState>();

        private Game()
        {
            GameStates.Push(new MenuState());
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
                    newState = new MenuState();
                    break;
                case GameState.Setup:
                    newState = new SetupState();
                    break;
                case GameState.Game:
                    newState = new ConsoleGame.GameState();
                    break;
            }

            GameStates.Push(newState!);
        }
    }
}