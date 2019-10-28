using System;

namespace GameWasm.Webassembly.Test
{
    class LocalSet : Test
    {
        public LocalSet(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "local_set.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            test.CallVoid("type-local-i32");
            test.CallVoid("type-local-i64");
            test.CallVoid("type-local-f32");
            test.CallVoid("type-local-f64");

            test.CallVoid("type-param-i32", (UInt32) 2);
            test.CallVoid("type-param-i64", (UInt64) 3);
            test.CallVoid("type-param-f32", (float) 4.4);
            test.CallVoid("type-param-f64", (double) 5.5);

            test.CallVoid("as-block-value", (UInt32) 0);
            test.CallVoid("as-loop-value", (UInt32) 0);

            test.CallVoid("as-br-value", (UInt32) 0);
            test.CallVoid("as-br_if-value", (UInt32) 0);
            test.CallVoid("as-br_if-value-cond", (UInt32) 0);
            test.CallVoid("as-br_table-value", (UInt32) 0);

            test.CallVoid("as-return-value", (UInt32) 0);

            test.CallVoid("as-if-then", (UInt32) 1);
            test.CallVoid("as-if-else", (UInt32) 0);

            test.CallVoid("type-mixed", (UInt64) 1, (float) 2.2, (double) 3.3, (UInt32) 4, (UInt32) 5);

            assert64(test.Call("write", (UInt64) 1, (float) 2, (double) 3.3, (UInt32) 4, (UInt32) 5), (UInt64) 56);
        }
    }
}
