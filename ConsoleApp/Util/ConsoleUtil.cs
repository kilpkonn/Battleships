using System;

namespace Util
{
    public static class ConsoleUtil
    {
        public static void WriteBlanks()
        {
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.WriteLine();
            }
        }
    }
}