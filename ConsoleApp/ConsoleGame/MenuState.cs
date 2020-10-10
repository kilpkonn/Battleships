using System;
using System.Linq;
using ConsoleMenu;

namespace ConsoleGame
{
    public class MenuState: BaseState
    {
         private Menu Menu { get; set; } = new Menu();
        private MenuItem SetSizeItem { get; set; }
        private MenuItem BoardWidthItem { get; set; }
        private MenuItem BoardHeightIem { get; set; }

        private int _boardWidth = 10;
        private int BoardWidth
        {
            get => _boardWidth;
            set => _boardWidth = Math.Clamp(value, Game.MinBoardWidth, Game.MaxBoardWidth);
        }

        private int _boardHeight = 10;

        public int BoardHeight
        {
            get => _boardHeight;
            set => _boardHeight = Math.Clamp(value, Game.MinBoardHeight, Game.MaxBoardHeight);
        }

        public MenuState()
        {
            Menu.AddMenuItem(new MenuItem("Start Game", "Starts game", onSelectedCallback: StartGame));
            Menu.AddMenuItem(new MenuItem("Save game", "Save game to json", onSelectedCallback: SaveGame));
            SetSizeItem = new MenuItem($"Set board size [{BoardWidth} x {BoardHeight}]", $"Set width and height of the board. Currently {BoardWidth} x {BoardHeight}");
            BoardWidthItem = new MenuItem($"Set width [{BoardWidth}]", $"Sets the width of board. Currently {BoardWidth}", onSelectedCallback: SetBoardWidth);
            BoardHeightIem = new MenuItem($"Set height [{BoardHeight}]", $"Sets height of board. Currently {BoardHeight}", onSelectedCallback: SetBoardHeight);
            SetSizeItem.AddChildItem(BoardWidthItem);
            SetSizeItem.AddChildItem(BoardHeightIem);
            Menu.AddMenuItem(SetSizeItem);
            Menu.AddMenuItem(new MenuItem("Exit", "Exit the application", onSelectedCallback: Exit));
        }

        public void Step()
        {
            Menu.Run();   
        }

        private void StartGame()
        {
            Game.GetInstance().PushState(Game.GameState.Setup);
            Menu.Close();
        }

        private void SaveGame()
        {
            
        }

        private void SetBoardWidth()
        {
            string input;
            do
            {
                Console.Write($"Enter board width in range [{Game.MinBoardWidth} - {Game.MaxBoardWidth}]: ");
                input = Console.ReadLine() ?? "";
            } while (string.IsNullOrEmpty(input) || !input.All(Char.IsDigit));
            
            BoardWidth = Convert.ToInt32(input);
            SetSizeItem.Preview = $"Set width and height of the board. Currently {BoardWidth} x {BoardHeight}";
            SetSizeItem.Label = $"Set board size [{BoardWidth} x {BoardHeight}]";
            BoardWidthItem.Preview = $"Sets the width of board. Currently {BoardWidth}";
            BoardWidthItem.Label = $"Set width [{BoardWidth}]";
            Menu.RevertSelection(1);
        }

        private void SetBoardHeight()
        {
            string input;
            do
            {
                Console.Write($"Enter board height in range [{Game.MinBoardHeight} - {Game.MaxBoardHeight}]: ");
                input = Console.ReadLine() ?? "";
            } while (string.IsNullOrEmpty(input) || !input.All(Char.IsDigit));
            
            BoardHeight = Convert.ToInt32(input);
            SetSizeItem.Preview = $"Set width and height of the board. Currently {BoardWidth} x {BoardHeight}";
            SetSizeItem.Label = $"Set board size [{BoardWidth} x {BoardHeight}]";
            BoardHeightIem.Preview = $"Sets height of board. Currently {BoardHeight}";
            BoardHeightIem.Label = $"Set height [{BoardHeight}]";
            Menu.RevertSelection(1);
        }

        private void Exit()
        {
            Menu.Close();
        }
    }
}