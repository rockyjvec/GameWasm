using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class Labels : Test
    {
        public Labels(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "labels.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            assert(test.Call("block"), (UInt32) 1);
            assert(test.Call("loop1"), (UInt32) 5);
            assert(test.Call("loop2"), (UInt32) 8);
            assert(test.Call("loop3"), (UInt32) 1);
            assert(test.Call("loop4", (UInt32) 8), (UInt32) 16);
            assert(test.Call("loop5"), (UInt32) 2);
            assert(test.Call("loop6"), (UInt32) 3);
            assert(test.Call("if"), (UInt32) 5);
            assert(test.Call("if2"), (UInt32) 5);
            assert(test.Call("switch", (UInt32) 0), (UInt32) 50);
            assert(test.Call("switch", (UInt32) 1), (UInt32) 20);
            assert(test.Call("switch", (UInt32) 2), (UInt32) 20);
            assert(test.Call("switch", (UInt32) 3), (UInt32) 3);
            assert(test.Call("switch", (UInt32) 4), (UInt32) 50);
            assert(test.Call("switch", (UInt32) 5), (UInt32) 50);
            assert(test.Call("return", (UInt32) 0), (UInt32) 0);
            assert(test.Call("return", (UInt32) 1), (UInt32) 2);
            assert(test.Call("return", (UInt32) 2), (UInt32) 2);
            assert(test.Call("br_if0"), (UInt32) 0x1d);
            assert(test.Call("br_if1"), (UInt32) 1);
            assert(test.Call("br_if2"), (UInt32) 1);
            assert(test.Call("br_if3"), (UInt32) 2);
            assert(test.Call("br"), (UInt32) 1);
            assert(test.Call("shadowing"), (UInt32) 1);
            assert(test.Call("redefinition"), (UInt32) 5);
        }
    }
}
