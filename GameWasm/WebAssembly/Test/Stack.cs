using System;

namespace GameWasm.Webassembly.Test
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
            var test = store.LoadModule("test", path + '/' + filename);

            assert64(test.Call("fac-expr", (UInt64) 25), (UInt64) 7034535277573963776);
            assert64(test.Call("fac-stack", (UInt64) 25), (UInt64) 7034535277573963776);               
            assert64(test.Call("fac-mixed", (UInt64) 25), (UInt64) 7034535277573963776);               
        }
    }
}
