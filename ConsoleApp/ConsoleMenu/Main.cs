using System;

namespace ConsoleMenu
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            for (int i = 0; i < 10; i++)
            {
                menu.AddMenuItem(new MenuItem(
                    i.ToString(), 
                    "Help $i", 
                    () => Console.WriteLine("Selected!")));
            }
            
            menu.Run();
        }
    }
}