using System;

namespace GameWasm.Webassembly.Test
{
    class LocalGet : Test
    {
        public LocalGet(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "local_get.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            assert(test.Call("type-local-i32"), (UInt32) 0);
            assert64(test.Call("type-local-i64"), (UInt64) 0);
            assertF32(test.Call("type-local-f32"), (float) 0);
            assertF64(test.Call("type-local-f64"), (double) 0);

            assert(test.Call("type-param-i32", (UInt32) 2), (UInt32) 2);
            assert64(test.Call("type-param-i64", (UInt64) 3), (UInt64) 3);
            assertF32(test.Call("type-param-f32", (float) 4.4), (float) 4.4);
            assertF64(test.Call("type-param-f64", (double) 5.5), (double) 5.5);

            assert(test.Call("as-block-value", (UInt32) 6), (UInt32) 6);
            assert(test.Call("as-loop-value", (UInt32) 7), (UInt32) 7);

            assert(test.Call("as-br-value", (UInt32) 8), (UInt32) 8);
            assert(test.Call("as-br_if-value", (UInt32) 9), (UInt32) 9);
            assert(test.Call("as-br_if-value-cond", (UInt32) 10), (UInt32) 10);
            assert(test.Call("as-br_table-value", (UInt32) 1), (UInt32) 2);

            assert(test.Call("as-return-value", (UInt32) 0), (UInt32) 0);

            assert(test.Call("as-if-then", (UInt32) 1), (UInt32) 1);
            assert(test.Call("as-if-else", (UInt32) 0), (UInt32) 0);

            test.CallVoid("type-mixed", (UInt64) 1, (float) 2.2, (double) 3.3, (UInt32) 4, (UInt32) 5);
            assertF64(test.Call("read", (UInt64) 1, (float) 2, (double) 3.3, (UInt32) 4, (UInt32) 5), (double)34.8);
        }
    }
}
