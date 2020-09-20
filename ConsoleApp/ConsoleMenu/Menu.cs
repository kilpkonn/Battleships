using System;
using System.Collections.Generic;

namespace ConsoleMenu
{
    public class Menu
    {
        private readonly MenuItem _baseMenuItem = new MenuItem("Root", "Use arrows to move!", () => {});

        protected MenuItem CurrentMenuItem { get; set; }
        private int _lineIndex = 0;

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
            _baseMenuItem.Render(0, 0);
        }

        public void Run()
        {
            WriteBlanks();
            while (true)
            {
                Render();
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Q)
                {
                    break;
                }

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        _lineIndex = Math.Max(0, _lineIndex - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        _lineIndex = Math.Min(Console.WindowHeight - 1, _lineIndex + 1);
                        break;
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
        }

        protected void WriteBlanks()
        {
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.WriteLine();
            }
        }
    }
}