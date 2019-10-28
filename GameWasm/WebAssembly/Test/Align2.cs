using System;

namespace GameWasm.Webassembly.Test
{
    class Align2 : Test
    {
        public Align2(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "align2.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            assert_trap(delegate () { test.CallVoid("store", (UInt32)65532, (UInt64)0xFFFFFFFFFFFFFFFF); }, "out of bounds memory access");
            assert(test.Call("load", (UInt32)65532), (UInt32)0);
           
        }
    }
}
