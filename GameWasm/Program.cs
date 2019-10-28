using System;
using System.Runtime;
using GameWasm.Webassembly;

namespace GameWasm
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "test")
            {
                Webassembly.Test.Test.Run("test");

                Console.WriteLine("Test Pass!\nPress any key to continue...");
                
                return;                
            }
            var store = new Store();
            var test = store.LoadModule("test", "/home/rocky/test.wasm");
         //   test.Debug = true;
            store.Modules["wasi_unstable"].Memory.Add((Memory)test.Exports["memory"]);
            test.CallVoid("_start");
        }
    }
}
