using System;
using System.Collections.Generic;

namespace ConsoleMenu
{
    public class Menu
    {
        protected readonly List<MenuItem> MenuItems = new List<MenuItem>();

        public void AddMenuItem(MenuItem item)
        {
            MenuItems.Add(item);
        }

        public void Render()
        {
            uint i = 0;
            MenuItems.ForEach(item => item.Render(i++, 0));
        }

        public void Run()
        {
            while (true)
            {
                Render();
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Q)
                {
                    break;
                }
            }
        }
    }
}