using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class Switch : Test
    {
        public Switch(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "switch.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            assert(test.Call("stmt", (UInt32) 0), (UInt32) 0);
            assert(test.Call("stmt", (UInt32) 1), (UInt32) 0xFFFFFFFF);
            assert(test.Call("stmt", (UInt32) 2), (UInt32) 0xFFFFFFFE);
            assert(test.Call("stmt", (UInt32) 3), (UInt32) 0xFFFFFFFD);
            assert(test.Call("stmt", (UInt32) 4), (UInt32) 100);
            assert(test.Call("stmt", (UInt32) 5), (UInt32) 101);
            assert(test.Call("stmt", (UInt32) 6), (UInt32) 102);
            assert(test.Call("stmt", (UInt32) 7), (UInt32) 100);
            assert(test.Call("stmt", (UInt32) 0xFFFFFFF6), (UInt32) 102);

            assert64(test.Call("expr", (UInt64) 0), (UInt64) 0);
            assert64(test.Call("expr", (UInt64) 1), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("expr", (UInt64) 2), (UInt64) 0xFFFFFFFFFFFFFFFE);
            assert64(test.Call("expr", (UInt64) 3), (UInt64) 0xFFFFFFFFFFFFFFFD);
            assert64(test.Call("expr", (UInt64) 6), (UInt64) 101);
            assert64(test.Call("expr", (UInt64) 7), (UInt64) 0xFFFFFFFFFFFFFFFB);
            assert64(test.Call("expr", (UInt64) 0xFFFFFFFFFFFFFFF6), (UInt64) 100);

            assert(test.Call("arg", (UInt32) 0), (UInt32) 110);
            assert(test.Call("arg", (UInt32) 1), (UInt32) 12);
            assert(test.Call("arg", (UInt32) 2), (UInt32) 4);
            assert(test.Call("arg", (UInt32) 3), (UInt32) 1116);
            assert(test.Call("arg", (UInt32) 4), (UInt32) 118);
            assert(test.Call("arg", (UInt32) 5), (UInt32) 20);
            assert(test.Call("arg", (UInt32) 6), (UInt32) 12);
            assert(test.Call("arg", (UInt32) 7), (UInt32) 1124);
            assert(test.Call("arg", (UInt32) 8), (UInt32) 126);

            assert(test.Call("corner"), (UInt32) 1);        
        }
    }
}
