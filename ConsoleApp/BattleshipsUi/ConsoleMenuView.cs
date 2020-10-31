using System;
using System.Linq;
using Battleships;
using ConsoleMenu;

namespace ConsoleBattleshipsUi
{
    public class ConsoleMenu : GameMenuUi
    {
        private Menu Menu { get; set; } = new Menu();
        private MenuItem SetSizeItem { get; set; }
        private MenuItem BoardWidthItem { get; set; }
        private MenuItem BoardHeightIem { get; set; }
        private MenuItem LoadGameItem { get; set; }
        private MenuItem TouchModeItem { get; set; }
        private MenuItem ShipCountItem { get; set; }


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
            TouchModeItem = new MenuItem($"Set touch mode [{Configuration.TouchMode.ToString()}]",
                "Set whether ships can touch or not");
            TouchModeItem.AddChildItem(new MenuItem(TouchMode.NoTouch.ToString(), "Boats cannot touch each other",
                onSelectedCallback: () => SetTouchMode(TouchMode.NoTouch)));
            TouchModeItem.AddChildItem(new MenuItem(TouchMode.CornersTouch.ToString(), "Boats can touch each other with corners",
                onSelectedCallback: () => SetTouchMode(TouchMode.CornersTouch)));
            TouchModeItem.AddChildItem(new MenuItem(TouchMode.AllTouch.ToString(), "Boats can touch each other",
                onSelectedCallback: () => SetTouchMode(TouchMode.AllTouch)));
            Menu.AddMenuItem(TouchModeItem);
            ShipCountItem = new MenuItem("Set ship counts", "Set amount of each ship for game");
            ShipCountItem.OnHoverCallback = GenerateShipCounts;
            Menu.AddMenuItem(ShipCountItem);
            Menu.AddMenuItem(new MenuItem("Exit", "Exit the application", onSelectedCallback: Exit));
        }

        private void StartGame()
        {
            Menu.RevertSelection(2);
            Menu.Close();
            StartGameCallback?.Invoke(Configuration);
        }

        private void GenerateShipCounts()
        {
            ShipCountItem.ClearChildItems();
            foreach (var (shipSize, count) in Configuration.ShipCounts)
            {
                ShipCountItem.AddChildItem(
                    new MenuItem($"Ships with {shipSize} lenght [{count}]",
                        $"Set amount of ships with length of {shipSize}. Currently {count}",
                        onSelectedCallback: () => SetShipCount(shipSize)));
            }
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

        private void SetShipCount(int shipSize)
        {
            string input;
            do
            {
                Console.Write($"Enter amount of ships you want to be with length {shipSize}: ");
                input = Console.ReadLine() ?? "";
            } while (string.IsNullOrEmpty(input) || !input.All(Char.IsDigit));

            Configuration.ShipCounts[shipSize] = Convert.ToInt32(input);
            GenerateShipCounts();
            Menu.RevertSelection(1);
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

        private void SetTouchMode(TouchMode mode)
        {
            Configuration.TouchMode = mode;
            TouchModeItem.Label = $"Set touch mode [{mode.ToString()}]";
        }

        private void Exit()
        {
            Menu.Close();
            ExitCallback?.Invoke();
        }
    }
}