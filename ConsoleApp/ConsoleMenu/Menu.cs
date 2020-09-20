using System;
using System.Collections.Generic;

namespace ConsoleMenu
{
    public class Menu
    {
        protected readonly List<MenuItem> MenuItems = new List<MenuItem>();
        private int LineIndex { get; set; }
        private int LevelIndex { get; set; }

        public void AddMenuItem(MenuItem item)
        {
            MenuItems.Add(item);
        }

        public void Render()
        {
            int i = 0;
            MenuItems.ForEach(item => item.Render(i++, 0));
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
                        LineIndex = Math.Max(0, LineIndex - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        LineIndex = Math.Min(Console.WindowHeight, LineIndex + 1);
                        break;
                    case ConsoleKey.RightArrow:
                        if (MenuItems[LineIndex].HasChildren())
                        {
                            LevelIndex++;
                            MenuItems[LineIndex].OnSelect();
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        LevelIndex = Math.Max(0, LevelIndex - 1);
                        MenuItems[LevelIndex].OnDeselect();
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