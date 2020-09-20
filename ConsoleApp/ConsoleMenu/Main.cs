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
                var item = new MenuItem(
                    i.ToString(),
                    "Help $i",
                    () => Console.WriteLine("Selected!")
                    );

                for (int j = 0; j < 5; j++)
                {
                    item.AddChildItem(new MenuItem(
                        i.ToString(),
                        "Help $i",
                        () => Console.WriteLine("Selected child!")
                    ));
                }
                
                menu.AddMenuItem(item);
            }
            
            menu.Run();
        }
    }
}