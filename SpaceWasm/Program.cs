using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new WebAssembly.WebAssembly();

            a.LoadModule("c:/users/rocky/desktop/main.wasm");

            Console.ReadKey();
        }
    }
}
