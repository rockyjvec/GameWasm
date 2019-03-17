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
            var store = new WebAssembly.Store();

            var lua = store.LoadModule("lua", "c:/users/rocky/desktop/main.wasm");

            Console.WriteLine("Module loaded.");

            lua.Execute("_run_lua", (UInt32)0);

            while(store.Step(true))
            {

            }


            Console.WriteLine("Press any key to continue...");

            Console.ReadKey();
        }
    }
}
