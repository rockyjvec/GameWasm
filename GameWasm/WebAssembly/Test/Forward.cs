using System;

namespace GameWasm.Webassembly.Test
{
    class Forward : Test
    {
        public Forward(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "forward.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            assert(test.Call("even", (UInt32) 13), (UInt32) 0);
            assert(test.Call("even", (UInt32) 20), (UInt32) 1);
            assert(test.Call("odd", (UInt32) 13), (UInt32) 1);
            assert(test.Call("odd", (UInt32) 20), (UInt32) 0);
        }
    }
}
