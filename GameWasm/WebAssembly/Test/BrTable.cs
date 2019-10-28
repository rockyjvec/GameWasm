using System;

namespace GameWasm.Webassembly.Test
{
    class BrTable : Test
    {
        public BrTable(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "brtable.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            test.CallVoid("type-i32");
            test.CallVoid("type-i64");
            test.CallVoid("type-f32");
            test.CallVoid("type-f64");

            test.CallVoid("type-i32-value", (UInt32) 1);
            test.CallVoid("type-i64-value", (UInt64) 2);
            test.CallVoid("type-f32-value", (float) 3);
            test.CallVoid("type-f64-value", (double) 4);

            assert(test.Call("empty", (UInt32) 0), (UInt32) 22);
            assert(test.Call("empty", (UInt32) 1), (UInt32) 22);
            assert(test.Call("empty", (UInt32) 11), (UInt32) 22);
            assert(test.Call("empty", (UInt32) 0xFFFFFFFF), (UInt32) 22);
            assert(test.Call("empty", (UInt32) 0xFFFFFF9C), (UInt32) 22);
            assert(test.Call("empty", (UInt32) 0xffffffff), (UInt32) 22);

            assert(test.Call("empty-value", (UInt32) 0), (UInt32) 33);
            assert(test.Call("empty-value", (UInt32) 1), (UInt32) 33);
            assert(test.Call("empty-value", (UInt32) 11), (UInt32) 33);
            assert(test.Call("empty-value", (UInt32) 0xFFFFFFFF), (UInt32) 33);
            assert(test.Call("empty-value", (UInt32) 0xFFFFFF9C), (UInt32) 33);
            assert(test.Call("empty-value", (UInt32) 0xffffffff), (UInt32) 33);

            assert(test.Call("singleton", (UInt32) 0), (UInt32) 22);
            assert(test.Call("singleton", (UInt32) 1), (UInt32) 20);
            assert(test.Call("singleton", (UInt32) 11), (UInt32) 20);
            assert(test.Call("singleton", (UInt32) 0xFFFFFFFF), (UInt32) 20);
            assert(test.Call("singleton", (UInt32) 0xFFFFFF9C), (UInt32) 20);
            assert(test.Call("singleton", (UInt32) 0xffffffff), (UInt32) 20);

            assert(test.Call("singleton-value", (UInt32) 0), (UInt32) 32);
            assert(test.Call("singleton-value", (UInt32) 1), (UInt32) 33);
            assert(test.Call("singleton-value", (UInt32) 11), (UInt32) 33);
            assert(test.Call("singleton-value", (UInt32) 0xFFFFFFFF), (UInt32) 33);
            assert(test.Call("singleton-value", (UInt32) 0xFFFFFF9C), (UInt32) 33);
            assert(test.Call("singleton-value", (UInt32) 0xffffffff), (UInt32) 33);

            assert(test.Call("multiple", (UInt32) 0), (UInt32) 103);
            assert(test.Call("multiple", (UInt32) 1), (UInt32) 102);
            assert(test.Call("multiple", (UInt32) 2), (UInt32) 101);
            assert(test.Call("multiple", (UInt32) 3), (UInt32) 100);
            assert(test.Call("multiple", (UInt32) 4), (UInt32) 104);
            assert(test.Call("multiple", (UInt32) 5), (UInt32) 104);
            assert(test.Call("multiple", (UInt32) 6), (UInt32) 104);
            assert(test.Call("multiple", (UInt32) 10), (UInt32) 104);
            assert(test.Call("multiple", (UInt32) 0xFFFFFFFF), (UInt32) 104);
            assert(test.Call("multiple", (UInt32) 0xffffffff), (UInt32) 104);

            assert(test.Call("multiple-value", (UInt32) 0), (UInt32) 213);
            assert(test.Call("multiple-value", (UInt32) 1), (UInt32) 212);
            assert(test.Call("multiple-value", (UInt32) 2), (UInt32) 211);
            assert(test.Call("multiple-value", (UInt32) 3), (UInt32) 210);
            assert(test.Call("multiple-value", (UInt32) 4), (UInt32) 214);
            assert(test.Call("multiple-value", (UInt32) 5), (UInt32) 214);
            assert(test.Call("multiple-value", (UInt32) 6), (UInt32) 214);
            assert(test.Call("multiple-value", (UInt32) 10), (UInt32) 214);
            assert(test.Call("multiple-value", (UInt32) 0xFFFFFFFF), (UInt32) 214);
            assert(test.Call("multiple-value", (UInt32) 0xffffffff), (UInt32) 214);

            assert(test.Call("large", (UInt32) 0), (UInt32) 0);
            assert(test.Call("large", (UInt32) 1), (UInt32) 1);
            assert(test.Call("large", (UInt32) 100), (UInt32) 0);
            assert(test.Call("large", (UInt32) 101), (UInt32) 1);
            assert(test.Call("large", (UInt32) 10000), (UInt32) 0);
            assert(test.Call("large", (UInt32) 10001), (UInt32) 1);
            assert(test.Call("large", (UInt32) 1000000), (UInt32) 1);
            assert(test.Call("large", (UInt32) 1000001), (UInt32) 1);

            test.CallVoid("as-block-first");
            test.CallVoid("as-block-mid");
            test.CallVoid("as-block-last");
            test.CallVoid("as-block-value", (UInt32) 2);

            test.CallVoid("as-loop-first", (UInt32) 3);
            test.CallVoid("as-loop-mid", (UInt32) 4);
            test.CallVoid("as-loop-last", (UInt32) 5);

            test.CallVoid("as-br-value", (UInt32) 9);

            test.CallVoid("as-br_if-cond");
            test.CallVoid("as-br_if-value", (UInt32) 8);
            test.CallVoid("as-br_if-value-cond", (UInt32) 9);

            test.CallVoid("as-br_table-index");
            test.CallVoid("as-br_table-value", (UInt32) 10);
            test.CallVoid("as-br_table-value-index", (UInt32) 11);

            test.CallVoid("as-return-value", (UInt64) 7);

            test.CallVoid("as-if-cond", (UInt32) 2);
            assert(test.Call("as-if-then", (UInt32) 1, (UInt32) 6), (UInt32) 3);
            assert(test.Call("as-if-then", (UInt32) 0, (UInt32) 6), (UInt32) 6);
            assert(test.Call("as-if-else", (UInt32) 0, (UInt32) 6), (UInt32) 4);
            assert(test.Call("as-if-else", (UInt32) 1, (UInt32) 6), (UInt32) 6);

            assert(test.Call("as-select-first", (UInt32) 0, (UInt32) 6), (UInt32) 5);
            assert(test.Call("as-select-first", (UInt32) 1, (UInt32) 6), (UInt32) 5);
            assert(test.Call("as-select-second", (UInt32) 0, (UInt32) 6), (UInt32) 6);
            assert(test.Call("as-select-second", (UInt32) 1, (UInt32) 6), (UInt32) 6);
            test.CallVoid("as-select-cond", (UInt32) 7);

            test.CallVoid("as-call-first", (UInt32) 12);
            test.CallVoid("as-call-mid", (UInt32) 13);
            test.CallVoid("as-call-last", (UInt32) 14);

            test.CallVoid("as-call_indirect-first", (UInt32) 20);
            test.CallVoid("as-call_indirect-mid", (UInt32) 21);
            test.CallVoid("as-call_indirect-last", (UInt32) 22);
            test.CallVoid("as-call_indirect-func", (UInt32) 23);

            test.CallVoid("as-local.set-value", (UInt32) 17);
            test.CallVoid("as-local.tee-value", (UInt32) 1);
            test.CallVoid("as-global.set-value", (UInt32) 1);

            test.CallVoid("as-load-address", (float) 1.7);
            test.CallVoid("as-loadN-address", (UInt64) 30);

            test.CallVoid("as-store-address", (UInt32) 30);
            test.CallVoid("as-store-value", (UInt32) 31);
            test.CallVoid("as-storeN-address", (UInt32) 32);
            test.CallVoid("as-storeN-value", (UInt32) 33);

            test.CallVoid("as-unary-operand", (float) 3.4);

            test.CallVoid("as-binary-left", (UInt32) 3);
            test.CallVoid("as-binary-right", (UInt64) 45);

            test.CallVoid("as-test-operand", (UInt32) 44);

            test.CallVoid("as-compare-left", (UInt32) 43);
            test.CallVoid("as-compare-right", (UInt32) 42);

            test.CallVoid("as-convert-operand", (UInt32) 41);

            test.CallVoid("as-memory.grow-size", (UInt32) 40);

            assert(test.Call("nested-block-value", (UInt32) 0), (UInt32) 19);
            assert(test.Call("nested-block-value", (UInt32) 1), (UInt32) 17);
            assert(test.Call("nested-block-value", (UInt32) 2), (UInt32) 16);
            assert(test.Call("nested-block-value", (UInt32) 10), (UInt32) 16);
            assert(test.Call("nested-block-value", (UInt32) 0xFFFFFFFF), (UInt32) 16);
            assert(test.Call("nested-block-value", (UInt32) 100000), (UInt32) 16);

            assert(test.Call("nested-br-value", (UInt32) 0), (UInt32) 8);
            assert(test.Call("nested-br-value", (UInt32) 1), (UInt32) 9);
            assert(test.Call("nested-br-value", (UInt32) 2), (UInt32) 17);
            assert(test.Call("nested-br-value", (UInt32) 11), (UInt32) 17);
            assert(test.Call("nested-br-value", (UInt32) 0xFFFFFFFC), (UInt32) 17);
            assert(test.Call("nested-br-value", (UInt32) 10213210), (UInt32) 17);

            assert(test.Call("nested-br_if-value", (UInt32) 0), (UInt32) 17);
            assert(test.Call("nested-br_if-value", (UInt32) 1), (UInt32) 9);
            assert(test.Call("nested-br_if-value", (UInt32) 2), (UInt32) 8);
            assert(test.Call("nested-br_if-value", (UInt32) 9), (UInt32) 8);
            assert(test.Call("nested-br_if-value", (UInt32) 0xFFFFFFF7), (UInt32) 8);
            assert(test.Call("nested-br_if-value", (UInt32) 999999), (UInt32) 8);

            assert(test.Call("nested-br_if-value-cond", (UInt32) 0), (UInt32) 9);
            assert(test.Call("nested-br_if-value-cond", (UInt32) 1), (UInt32) 8);
            assert(test.Call("nested-br_if-value-cond", (UInt32) 2), (UInt32) 9);
            assert(test.Call("nested-br_if-value-cond", (UInt32) 3), (UInt32) 9);
            assert(test.Call("nested-br_if-value-cond", (UInt32) 0xFFF0BDC0), (UInt32) 9);
            assert(test.Call("nested-br_if-value-cond", (UInt32) 9423975), (UInt32) 9);

            assert(test.Call("nested-br_table-value", (UInt32) 0), (UInt32) 17);
            assert(test.Call("nested-br_table-value", (UInt32) 1), (UInt32) 9);
            assert(test.Call("nested-br_table-value", (UInt32) 2), (UInt32) 8);
            assert(test.Call("nested-br_table-value", (UInt32) 9), (UInt32) 8);
            assert(test.Call("nested-br_table-value", (UInt32) 0xFFFFFFF7), (UInt32) 8);
            assert(test.Call("nested-br_table-value", (UInt32) 999999), (UInt32) 8);

            assert(test.Call("nested-br_table-value-index", (UInt32) 0), (UInt32) 9);
            assert(test.Call("nested-br_table-value-index", (UInt32) 1), (UInt32) 8);
            assert(test.Call("nested-br_table-value-index", (UInt32) 2), (UInt32) 9);
            assert(test.Call("nested-br_table-value-index", (UInt32) 3), (UInt32) 9);
            assert(test.Call("nested-br_table-value-index", (UInt32) 0xFFF0BDC0), (UInt32) 9);
            assert(test.Call("nested-br_table-value-index", (UInt32) 9423975), (UInt32) 9);

            assert(test.Call("nested-br_table-loop-block", (UInt32) 1), (UInt32) 3);
        }
    }
}
