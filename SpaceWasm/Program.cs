using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            WebAssembly.Test.Test.Run("test");

            Console.WriteLine("Test Pass!\nPress any key to continue...");

            Console.ReadKey();
        }
    }
}
