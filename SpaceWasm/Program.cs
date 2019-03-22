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
            
            WebAssembly.Test.Test.Run("test");

            Console.WriteLine("Test Pass!\nPress any key to continue...");

            Console.ReadKey();
            return;
            
            
            var store = new WebAssembly.Store();
            
            var lua = store.LoadModule("lua", "c:/users/rocky/desktop/main.wasm");
//            lua.DumpExports();
           
  //          Console.WriteLine("Module loaded.");
    //        Console.ReadKey();

            string luaScript = "function hello_lua()\n    print \"Hello Lua!\"\nend\n\nhello_lua()";
            byte[] bytes = Encoding.ASCII.GetBytes(luaScript);
            Console.WriteLine("Attempting malloc of: " + (bytes.Length));
            UInt32 result = 0;
            try
            {
                //lua.Debug = true;
                result = (UInt32)lua.Call("_malloc", (UInt32)1);// bytes.Length);
                Console.WriteLine("Memory allocated: " + result);
                Console.ReadKey();
                (store.Modules["env"].Exports["memory"] as WebAssembly.Memory).SetBytes(result, bytes);
                lua.Debug = true;
                lua.Call("_run_lua", (UInt32)result);
            }
            catch (WebAssembly.Trap e)
            {
                Console.WriteLine(e.Message + "\n" + e.Details);
            }
            /*
            Console.WriteLine("USED INSTRUCTIONS:");
            foreach (var a in WebAssembly.Stack.Frame.UsedInstructions)
            {
                Console.WriteLine(a.Key);
            }*/




  
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
