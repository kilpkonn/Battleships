using System;
using System.Linq;
using ConsoleMenu;

namespace ConsoleBattleships
{
    public class ConsoleMenu : GameMenuUi
    {
        private Menu Menu { get; set; } = new Menu();
        private MenuItem SetSizeItem { get; set; }
        private MenuItem BoardWidthItem { get; set; }
        private MenuItem BoardHeightIem { get; set; }
        private MenuItem LoadGameItem { get; set; }

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

        public override void Step()
        {
            Menu.Run();
        }

        public ConsoleMenu(int minBoardWidth, int minBoardHeight, int maxBoardWidth, int maxBoardHeight) : base(
            minBoardWidth, minBoardHeight, maxBoardWidth, maxBoardHeight)
        {
            Menu.AddMenuItem(new MenuItem("Start Game", "Starts game", onSelectedCallback: StartGame));
            Menu.AddMenuItem(new MenuItem("Save game", "Save game to json", onSelectedCallback: SaveGame));
            LoadGameItem = new MenuItem("Load game", "Load game from file", onSelectedCallback: LoadGameSelected);
            Menu.AddMenuItem(LoadGameItem);
            SetSizeItem = new MenuItem($"Set board size [{BoardWidth} x {BoardHeight}]",
                $"Set width and height of the board. Currently {BoardWidth} x {BoardHeight}");
            BoardWidthItem = new MenuItem($"Set width [{BoardWidth}]",
                $"Sets the width of board. Currently {BoardWidth}", onSelectedCallback: SetBoardWidth);
            BoardHeightIem = new MenuItem($"Set height [{BoardHeight}]",
                $"Sets height of board. Currently {BoardHeight}", onSelectedCallback: SetBoardHeight);
            SetSizeItem.AddChildItem(BoardWidthItem);
            SetSizeItem.AddChildItem(BoardHeightIem);
            Menu.AddMenuItem(SetSizeItem);
            Menu.AddMenuItem(new MenuItem("Exit", "Exit the application", onSelectedCallback: Exit));
        }

        private void StartGame()
        {
            Menu.RevertSelection(2);
            Menu.Close();
            StartGameCallback?.Invoke(_boardWidth, _boardHeight);
        }

        private void SaveGame()
        {
            Util.ConsoleUtil.WriteBlanks();
            Console.SetCursorPosition(0, Console.WindowHeight / 2);
            string defaultName = "game_" + DateTime.Now.ToString("yyyy-MM-dd");
            Console.Write($"Enter file name ({defaultName}): ");
            string name = Console.ReadLine() ?? "";
            if (string.IsNullOrEmpty(name)) name = defaultName;
            if (!name.EndsWith(".json")) name += ".json";
            SaveCallback?.Invoke(name);
            Menu.RevertSelection(1);
        }

        private void LoadGameSelected()
        {
            LoadGameItem.ClearChildItems();
            var files = System.IO.Directory.EnumerateFiles(".", "*.json").ToList();
            files.ForEach(file =>
            {
                LoadGameItem.AddChildItem(new MenuItem(file, $"Load from {file}",
                    onSelectedCallback: () => LoadGame(file)));
            });
        }

        private void LoadGame(string file)
        {
            if (LoadGameCallback?.Invoke(file) ?? false)
            {
                Menu.RevertSelection(2);
            }
            Menu.Close();
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
                Console.Write($"Enter board height in range [{MinBoardHeight} - {MaxBoardHeight}]: ");
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
            ExitCallback?.Invoke();
        }
    }
}