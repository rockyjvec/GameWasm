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
            /*
            WebAssembly.Test.Test.Run("test");

            Console.WriteLine("Test Pass!\nPress any key to continue...");

            Console.ReadKey();
            return;
            */
            
            var store = new WebAssembly.Store();
            
            var lua = store.LoadModule("lua", "c:/users/rocky/desktop/main.wasm");

            try
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                string luaScript = "function hello_lua()\n    print \"Hello Lua!\"\nend\n\nhello_lua()";
                byte[] bytes = utf8.GetBytes(luaScript);
                UInt32 result = (store.Modules["env"] as WebAssembly.Module.Env).dynamicAlloc((UInt32)bytes.Length + 1);
                store.Modules["env"].Memory[0].SetBytes(result, bytes);
                lua.Call("_run_lua", (UInt32)result);
            }
            catch (WebAssembly.Trap e)
            {
                Console.WriteLine(e.Message + "\n" + e.Details);
            }

            Console.WriteLine("Press any key to continue...");

            Console.ReadKey();
        }
    }
}
