using System;

namespace Util
{
    public static class ConsoleUtil
    {
        public static void WriteBlanks()
        {
            Console.Clear();
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.WriteLine();
            }
        }
    }
}