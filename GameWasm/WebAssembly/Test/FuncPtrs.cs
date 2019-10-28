using System;

namespace GameWasm.Webassembly.Test
{
    class FuncPtrs : Test
    {
        public FuncPtrs(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "func_ptrs1.wasm";

            var store = new Store();
            /*
            var test = store.LoadModule("test", path + '/' + filename);

            assert(test.Call("one"), (UInt32) 13);
            assert(test.Call("two", (UInt32) 13), (UInt32) 14);
            assert(test.Call("three", (UInt32) 13), (UInt32) 11);
            test.CallVoid("four", (UInt32) 83);            
            
            */
            filename = "func_ptrs2.wasm";

            store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);
            
            assert(test.Call("callt", (UInt32) 0), (UInt32) 1);
            assert(test.Call("callt", (UInt32) 1), (UInt32) 2);
            assert(test.Call("callt", (UInt32) 2), (UInt32) 3);
            assert(test.Call("callt", (UInt32) 3), (UInt32) 4);
            assert(test.Call("callt", (UInt32) 4), (UInt32) 5);
            assert(test.Call("callt", (UInt32) 5), (UInt32) 1);
            assert(test.Call("callt", (UInt32) 6), (UInt32) 3);
            assert_trap(delegate { test.Call("callt", (UInt32) 7); }, "undefined element");
            assert_trap(delegate { test.Call("callt", (UInt32) 100); }, "undefined element");
            assert_trap(delegate { test.Call("callt", (UInt32) 0xFFFFFFFF); }, "undefined element");

            assert(test.Call("callu", (UInt32) 0), (UInt32) 1);
            assert(test.Call("callu", (UInt32) 1), (UInt32) 2);
            assert(test.Call("callu", (UInt32) 2), (UInt32) 3);
            assert(test.Call("callu", (UInt32) 3), (UInt32) 4);
            assert(test.Call("callu", (UInt32) 4), (UInt32) 5);
            assert(test.Call("callu", (UInt32) 5), (UInt32) 1);
            assert(test.Call("callu", (UInt32) 6), (UInt32) 3);
            assert_trap(delegate { test.Call("callu", (UInt32) 7); }, "undefined element");
            assert_trap(delegate { test.Call("callu", (UInt32) 100); }, "undefined element");
            assert_trap(delegate { test.Call("callu", (UInt32) 0xFFFFFFFF); }, "undefined element");
            
            
            filename = "func_ptrs3.wasm";

            store = new Store();
            test = store.LoadModule("test", path + '/' + filename);


            assert(test.Call("callt", (UInt32) 0), (UInt32) 1);
            assert(test.Call("callt", (UInt32) 1), (UInt32) 2);
            
        }
    }
}
