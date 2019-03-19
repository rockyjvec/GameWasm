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
            lua.DumpExports();
           
            Console.WriteLine("Module loaded.");
            Console.ReadKey();

            string luaScript = "function hello_lua()\n    print \"Hello Lua!\"\nend\n\nhello_lua()";
            byte[] bytes = Encoding.ASCII.GetBytes(luaScript);
//            Buffer.BlockCopy(bytes, 0, lua.Memory[0].Buffer, 21216, bytes.Length);
//            Buffer.BlockCopy(bytes, 0, (store.Modules["env"].Exports["memory"] as WebAssembly.Memory).Buffer, 21216, bytes.Length);
            Console.WriteLine("Attempting malloc of: " + bytes.Length);
            lua.Execute("_malloc", (UInt32) bytes.Length);
            while (store.Step(true))
            {
            }
            Console.WriteLine("Stack Size: " + store.Stack.Size);
            UInt32 pointer = store.Stack.PopI32();

            Console.WriteLine("Memory allocated: " + pointer);

            Console.ReadKey();
  
            /*
                      lua.Execute("_run_lua", (UInt32)2156);
                      while (store.Step(true))
                      {
                      }
              */

//            var t = new WebAssembly.Test("c:/users/rocky/Downloads");
            Console.WriteLine("Press any key to continue...");

            Console.ReadKey();
        }
    }
}
