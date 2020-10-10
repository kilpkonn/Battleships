using System;
using System.Linq;
using ConsoleMenu;

namespace ConsoleGame
{
    public class Game
    {
        private const int MinBoardWidth = 10;
        private const int MinBoardHeight = 10;
        private const int MaxBoardWidth = 30;
        private const int MaxBoardHeight = 30;
        
        private Menu Menu { get; set; } = new Menu();
        private MenuItem SetSizeItem { get; set; }

        private int _boardWidth = 10;
        private int BoardWidth
        {
            get => _boardWidth;
            set => _boardWidth = Math.Clamp(value, MinBoardWidth, MaxBoardWidth);
        }

        private int _boardHeight = 10;

        public int BoardHeight
        {
            get => _boardHeight;
            set => _boardHeight = Math.Clamp(value, MinBoardHeight, MaxBoardHeight);
        }

        public Game()
        {
            Menu.AddMenuItem(new MenuItem("Start Game", "Starts game", onSelectedCallback: StartGame));
            Menu.AddMenuItem(new MenuItem("Save game", "Save game to json", onSelectedCallback: SaveGame));
            SetSizeItem = new MenuItem("Set board size", "Set width and height of the board");
            SetSizeItem.AddChildItem(new MenuItem("Set width", "Sets the width of board", onSelectedCallback: SetBoardWidth));
            SetSizeItem.AddChildItem(new MenuItem("Set height", "Sets height of board", onSelectedCallback: SetBoardHeight));
            Menu.AddMenuItem(SetSizeItem);
            Menu.AddMenuItem(new MenuItem("Exit", "Exit the application", onSelectedCallback: Exit));
        }

        public void Run()
        {
            Menu.Run();   
        }

        private void StartGame()
        {
            
        }

        private void SaveGame()
        {
            
        }

        private void SetBoardWidth()
        {
            string input;
            do
            {
                Console.Write($"Enter board width in range [{MinBoardWidth} - {MaxBoardWidth}]: ");
                input = Console.ReadLine() ?? "";
            } while (string.IsNullOrEmpty(input) || !input.All(Char.IsDigit));

            BoardWidth = Convert.ToInt32(input);
            Menu.RevertSelection(1);
        }

        private void SetBoardHeight()
        {
            string input;
            do
            {
                Console.Write($"Enter board height in range [{MinBoardHeight} - {MaxBoardHeight}]: ");
                input = Console.ReadLine() ?? "";
            } while (string.IsNullOrEmpty(input) || !input.All(Char.IsDigit));

            BoardHeight = Convert.ToInt32(input);
            Menu.RevertSelection(1);
        }

        private void Exit()
        {
            Menu.Close();
        }
    }
}