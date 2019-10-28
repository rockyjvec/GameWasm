using System;

namespace GameWasm.Webassembly.Test
{
    class Call : Test
    {
        public Call(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "call.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            assert(test.Call("type-i32"), (UInt32) 0x132);
            assert64(test.Call("type-i64"), (UInt64) 0x164);
            assertF32(test.Call("type-f32"), (float) 0xf32);
            assertF64(test.Call("type-f64"), (double) 0xf64);

            assert(test.Call("type-first-i32"), (UInt32) 32);
            assert64(test.Call("type-first-i64"), (UInt64) 64);
            assertF32(test.Call("type-first-f32"), (float) 1.32);
            assertF64(test.Call("type-first-f64"), (double) 1.64);

            assert(test.Call("type-second-i32"), (UInt32) 32);
            assert64(test.Call("type-second-i64"), (UInt64) 64);
            assertF32(test.Call("type-second-f32"), (float) 32);
            assertF64(test.Call("type-second-f64"), (double) 64.1);

            assert64(test.Call("fac", (UInt64) 0), (UInt64) 1);
            assert64(test.Call("fac", (UInt64) 1), (UInt64) 1);
            assert64(test.Call("fac", (UInt64) 5), (UInt64) 120);
            assert64(test.Call("fac", (UInt64) 25), (UInt64) 7034535277573963776);
            assert64(test.Call("fac-acc", (UInt64) 0, (UInt64) 1), (UInt64) 1);
            assert64(test.Call("fac-acc", (UInt64) 1, (UInt64) 1), (UInt64) 1);
            assert64(test.Call("fac-acc", (UInt64) 5, (UInt64) 1), (UInt64) 120);
            assert64(test.Call("fac-acc", (UInt64) 25, (UInt64) 1), (UInt64) 7034535277573963776);

            assert64(test.Call("fib", (UInt64) 0), (UInt64) 1);
            assert64(test.Call("fib", (UInt64) 1), (UInt64) 1);
            assert64(test.Call("fib", (UInt64) 2), (UInt64) 2);
            assert64(test.Call("fib", (UInt64) 5), (UInt64) 8);
            assert64(test.Call("fib", (UInt64) 20), (UInt64) 10946);

            assert(test.Call("even", (UInt64) 0), (UInt32) 44);
            assert(test.Call("even", (UInt64) 1), (UInt32) 99);
            assert(test.Call("even", (UInt64) 100), (UInt32) 44);
            assert(test.Call("even", (UInt64) 77), (UInt32) 99);
            assert(test.Call("odd", (UInt64) 0), (UInt32) 99);
            assert(test.Call("odd", (UInt64) 1), (UInt32) 44);
            assert(test.Call("odd", (UInt64) 200), (UInt32) 99);
            assert(test.Call("odd", (UInt64) 77), (UInt32) 44);

            assert_trap(delegate { test.CallVoid("runaway"); }, "call stack exhausted");
            assert_trap(delegate { test.CallVoid("mutual-runaway"); }, "call stack exhausted");

            assert(test.Call("as-select-first"), (UInt32) 0x132);
            assert(test.Call("as-select-mid"), (UInt32) 2);
            assert(test.Call("as-select-last"), (UInt32) 2);

            assert(test.Call("as-if-condition"), (UInt32) 1);

            assert(test.Call("as-br_if-first"), (UInt32) 0x132);
            assert(test.Call("as-br_if-last"), (UInt32) 2);

            assert(test.Call("as-br_table-first"), (UInt32) 0x132);
            assert(test.Call("as-br_table-last"), (UInt32) 2);

            assert(test.Call("as-call_indirect-first"), (UInt32) 0x132);
            assert(test.Call("as-call_indirect-mid"), (UInt32) 2);
            assert_trap(delegate { test.CallVoid("as-call_indirect-last"); }, "undefined element");

            test.CallVoid("as-store-first");
            test.CallVoid("as-store-last");

            assert(test.Call("as-memory.grow-value"), (UInt32) 1);
            assert(test.Call("as-return-value"), (UInt32) 0x132);
            test.CallVoid("as-drop-operand");
            assert(test.Call("as-br-value"), (UInt32) 0x132);
            assert(test.Call("as-local.set-value"), (UInt32) 0x132);
            assert(test.Call("as-local.tee-value"), (UInt32) 0x132);
            assert(test.Call("as-global.set-value"), (UInt32) 0x132);
            assert(test.Call("as-load-operand"), (UInt32) 1);

            assertF32(test.Call("as-unary-operand"), (float) 0x0);
            assert(test.Call("as-binary-left"), (UInt32) 11);
            assert(test.Call("as-binary-right"), (UInt32) 9);
            assert(test.Call("as-test-operand"), (UInt32) 0);
            assert(test.Call("as-compare-left"), (UInt32) 1);
            assert(test.Call("as-compare-right"), (UInt32) 1);
            assert64(test.Call("as-convert-operand"), (UInt64) 1);
        }
    }
}
