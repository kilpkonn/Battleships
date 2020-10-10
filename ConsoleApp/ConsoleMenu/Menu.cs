using System;
using System.Linq;

namespace ConsoleMenu
{
    public class Menu
    {
        private readonly MenuItem _baseMenuItem = new MenuItem("Root", "Use arrows to move!");

        protected MenuItem CurrentMenuItem { get; set; }

        private int _lineIndex = 0;
        private bool _isRunning = true;

        public string HelpText { get; set; } = @"
LEFT_ARROW - return to previous
RIGHT_ARROW - choose current
UP_ARROW - go to upper
DOWN_ARROW - go to below

ESC - Exit";

        public Menu()
        {
            _baseMenuItem.IsSelected = true;
            CurrentMenuItem = _baseMenuItem;
        }

        public void AddMenuItem(MenuItem item)
        {
            _baseMenuItem.AddChildItem(item);
        }

        public void Render()
        {
            Console.Clear();
            CurrentMenuItem.HoverChild(_lineIndex);
            _baseMenuItem.Render(0, -1 * (_baseMenuItem.Width + _baseMenuItem.Padding));
            
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.WriteLine("Use SPACE for help!");
        }

        public void Run()
        {
            WriteBlanks();
            while (_isRunning)
            {
                Render();
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Escape) break;
                if (key.Key == ConsoleKey.Spacebar)
                {
                    ShowHelp();
                    Console.ReadKey();
                    continue;
                }

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        _lineIndex = Math.Max(0, _lineIndex - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        _lineIndex = Math.Min(CurrentMenuItem.GetChildCount() - 1, _lineIndex + 1);
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.RightArrow:
                        CurrentMenuItem = CurrentMenuItem.TrySelectChild(_lineIndex, out _lineIndex);
                        break;
                    case ConsoleKey.LeftArrow:
                        CurrentMenuItem = CurrentMenuItem.TrySelectParent(_lineIndex, out _lineIndex);
                        break;
                    default:
                        break;
                }
            }
            WriteBlanks();
        }

        public void WriteBlanks()
        {
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.WriteLine();
            }
        }

        protected void ShowHelp()
        {
            WriteBlanks();
            int helpWidth = HelpText.Split("\n").Max(l => l.Length);
            int helpHeight = HelpText.Split("\n").Length;
            int left = Math.Max(0, (Console.WindowWidth - helpWidth) / 2);
            int top = Math.Max(0, (Console.WindowHeight - helpHeight) / 2);
            Console.SetCursorPosition(left, top);
            Console.WriteLine(HelpText);
        }
        

        public void Close()
        {
            _isRunning = false;
        }
    }
} 