using System;

namespace GameWasm.Webassembly.Test
{
    class LoadF32 : Test
    {
        public LoadF32(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "loadf32.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            assertF32(test.Call("32_good1", (UInt32)0), (float)0.0);
            assertF32(test.Call("32_good2", (UInt32)0), (float)0.0);
            assertF32(test.Call("32_good3", (UInt32)0), (float)0.0);
            assertF32(test.Call("32_good4", (UInt32)0), (float)0.0);
            assertF32(test.Call("32_good5", (UInt32)0), (float)float.NaN);

            assertF32(test.Call("32_good1", (UInt32)65524), (float)0.0);
            assertF32(test.Call("32_good2", (UInt32)65524), (float)0.0);
            assertF32(test.Call("32_good3", (UInt32)65524), (float)0.0);
            assertF32(test.Call("32_good4", (UInt32)65524), (float)0.0);
            assertF32(test.Call("32_good5", (UInt32)65524), (float)0.0);

            assertF32(test.Call("32_good1", (UInt32)65525), (float)0.0);
            assertF32(test.Call("32_good2", (UInt32)65525), (float)0.0);
            assertF32(test.Call("32_good3", (UInt32)65525), (float)0.0);
            assertF32(test.Call("32_good4", (UInt32)65525), (float)0.0);
            assert_trap(delegate () { test.Call("32_good5", (UInt32)65525); }, "out of bounds memory access");

            assert_trap(delegate () { test.CallVoid("32_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("32_bad", (UInt32)1); }, "out of bounds memory access");
        }
    }
}
