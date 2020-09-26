using System;

namespace ConsoleMenu
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            for (int i = 0; i < 5; i++)
            {
                menu.AddMenuItem(GenerateItems(5, $"Item {i}"));
            }
            menu.Run();
        }

        static MenuItem GenerateItems(int levels, string prefix = "Item")
        {
            var item = new MenuItem(
                $"{prefix}",
                $"Help {levels}"
            );

            if (levels <= 0)
            {
                return item;
            }
            
            for (int i = 0; i < 10; i++)
            {
                item.AddChildItem(GenerateItems(levels - 1, $"{prefix} {i}"));
            }

            return item;
        }
    }
}
