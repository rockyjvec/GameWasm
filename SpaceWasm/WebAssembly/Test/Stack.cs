using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class Stack : Test
    {
        public Stack(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "stack.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            assert64(test.Call("fac-expr", (UInt64) 25), (UInt64) 7034535277573963776);
            assert64(test.Call("fac-stack", (UInt64) 25), (UInt64) 7034535277573963776);               
            assert64(test.Call("fac-mixed", (UInt64) 25), (UInt64) 7034535277573963776);               
        }
    }
}
