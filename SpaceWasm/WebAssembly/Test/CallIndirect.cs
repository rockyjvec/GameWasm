using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class CallIndirect : Test
    {
        public CallIndirect(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "call_indirect.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            assert(test.Call("type-i32"), (UInt32) 0x132);
            assert64(test.Call("type-i64"), (UInt64) 0x164);
            assertF32(test.Call("type-f32"), (float) 0xf32);
            assertF64(test.Call("type-f64"), (double) 0xf64);

            assert64(test.Call("type-index"), (UInt64) 100);

            assert(test.Call("type-first-i32"), (UInt32) 32);
            assert64(test.Call("type-first-i64"), (UInt64) 64);
            assertF32(test.Call("type-first-f32"), (float) 1.32);
            assertF64(test.Call("type-first-f64"), (double) 1.64);

            assert(test.Call("type-second-i32"), (UInt32) 32);
            assert64(test.Call("type-second-i64"), (UInt64) 64);
            assertF32(test.Call("type-second-f32"), (float) 32);
            assertF64(test.Call("type-second-f64"), (double) 64.1);

            assert64(test.Call("dispatch", (UInt32) 5, (UInt64) 2), (UInt64) 2);
            assert64(test.Call("dispatch", (UInt32) 5, (UInt64) 5), (UInt64) 5);
            assert64(test.Call("dispatch", (UInt32) 12, (UInt64) 5), (UInt64) 120);
            assert64(test.Call("dispatch", (UInt32) 13, (UInt64) 5), (UInt64) 8);
            assert64(test.Call("dispatch", (UInt32) 20, (UInt64) 2), (UInt64) 2);
            assert_trap(delegate { test.CallVoid("dispatch", (UInt32)0, (UInt64)2); }, "indirect call type mismatch");
            assert_trap(delegate { test.CallVoid("dispatch", (UInt32)15, (UInt64)2); }, "indirect call type mismatch");
            assert_trap(delegate { test.CallVoid("dispatch", (UInt32)29, (UInt64)2); }, "undefined element");
            assert_trap(delegate { test.CallVoid("dispatch", (UInt32)0xFFFFFFFF, (UInt64)2); }, "undefined element");
            assert_trap(delegate { test.CallVoid("dispatch", (UInt32)1213432423, (UInt64)2); }, "undefined element");

            assert64(test.Call("dispatch-structural-i64", (UInt32) 5), (UInt64) 9);
            assert64(test.Call("dispatch-structural-i64", (UInt32) 12), (UInt64) 362880);
            assert64(test.Call("dispatch-structural-i64", (UInt32) 13), (UInt64) 55);
            assert64(test.Call("dispatch-structural-i64", (UInt32) 20), (UInt64) 9);
            assert_trap(delegate { test.CallVoid("dispatch-structural-i64", (UInt32)11); }, "indirect call type mismatch");
            assert_trap(delegate { test.CallVoid("dispatch-structural-i64", (UInt32)22); }, "indirect call type mismatch");

            assert(test.Call("dispatch-structural-i32", (UInt32) 4), (UInt32) 9);
            assert(test.Call("dispatch-structural-i32", (UInt32) 23), (UInt32) 362880);
            assert(test.Call("dispatch-structural-i32", (UInt32) 26), (UInt32) 55);
            assert(test.Call("dispatch-structural-i32", (UInt32) 19), (UInt32) 9);
            assert_trap(delegate { test.CallVoid("dispatch-structural-i32", (UInt32)9); }, "indirect call type mismatch");
            assert_trap(delegate { test.CallVoid("dispatch-structural-i32", (UInt32)21); }, "indirect call type mismatch");

            assertF32(test.Call("dispatch-structural-f32", (UInt32) 6), (float) 9.0);
            assertF32(test.Call("dispatch-structural-f32", (UInt32) 24), (float) 362880.0);
            assertF32(test.Call("dispatch-structural-f32", (UInt32) 27), (float) 55.0);
            assertF32(test.Call("dispatch-structural-f32", (UInt32) 21), (float) 9.0);
            assert_trap(delegate { test.CallVoid("dispatch-structural-f32", (UInt32)8); }, "indirect call type mismatch");
            assert_trap(delegate { test.CallVoid("dispatch-structural-f32", (UInt32)19); }, "indirect call type mismatch");

            assertF64(test.Call("dispatch-structural-f64", (UInt32) 7), (double) 9.0);
            assertF64(test.Call("dispatch-structural-f64", (UInt32) 25), (double) 362880.0);
            assertF64(test.Call("dispatch-structural-f64", (UInt32) 28), (double) 55.0);
            assertF64(test.Call("dispatch-structural-f64", (UInt32) 22), (double) 9.0);
            assert_trap(delegate { test.CallVoid("dispatch-structural-f64", (UInt32)10); }, "indirect call type mismatch");
//            assert_trap(delegate { test.CallVoid("dispatch-structural-f64", (UInt32)18); }, "indirect call type mismatch");

            assert64(test.Call("fac-i64", (UInt64) 0), (UInt64) 1);
            assert64(test.Call("fac-i64", (UInt64) 1), (UInt64) 1);
            assert64(test.Call("fac-i64", (UInt64) 5), (UInt64) 120);
            assert64(test.Call("fac-i64", (UInt64) 25), (UInt64) 7034535277573963776);

            assert(test.Call("fac-i32", (UInt32) 0), (UInt32) 1);
            assert(test.Call("fac-i32", (UInt32) 1), (UInt32) 1);
            assert(test.Call("fac-i32", (UInt32) 5), (UInt32) 120);
            assert(test.Call("fac-i32", (UInt32) 10), (UInt32) 3628800);

            assertF32(test.Call("fac-f32", (float) 0.0), (float) 1.0);
            assertF32(test.Call("fac-f32", (float) 1.0), (float) 1.0);
            assertF32(test.Call("fac-f32", (float) 5.0), (float) 120.0);
            assertF32(test.Call("fac-f32", (float) 10.0), (float) 3628800.0);

            assertF64(test.Call("fac-f64", (double) 0.0), (double) 1.0);
            assertF64(test.Call("fac-f64", (double) 1.0), (double) 1.0);
            assertF64(test.Call("fac-f64", (double) 5.0), (double) 120.0);
            assertF64(test.Call("fac-f64", (double) 10.0), (double) 3628800.0);

            assert64(test.Call("fib-i64", (UInt64) 0), (UInt64) 1);
            assert64(test.Call("fib-i64", (UInt64) 1), (UInt64) 1);
            assert64(test.Call("fib-i64", (UInt64) 2), (UInt64) 2);
            assert64(test.Call("fib-i64", (UInt64) 5), (UInt64) 8);
            assert64(test.Call("fib-i64", (UInt64) 20), (UInt64) 10946);

            assert(test.Call("fib-i32", (UInt32) 0), (UInt32) 1);
            assert(test.Call("fib-i32", (UInt32) 1), (UInt32) 1);
            assert(test.Call("fib-i32", (UInt32) 2), (UInt32) 2);
            assert(test.Call("fib-i32", (UInt32) 5), (UInt32) 8);
            assert(test.Call("fib-i32", (UInt32) 20), (UInt32) 10946);

            assertF32(test.Call("fib-f32", (float) 0.0), (float) 1.0);
            assertF32(test.Call("fib-f32", (float) 1.0), (float) 1.0);
            assertF32(test.Call("fib-f32", (float) 2.0), (float) 2.0);
            assertF32(test.Call("fib-f32", (float) 5.0), (float) 8.0);
            assertF32(test.Call("fib-f32", (float) 20.0), (float) 10946.0);

            assertF64(test.Call("fib-f64", (double) 0.0), (double) 1.0);
            assertF64(test.Call("fib-f64", (double) 1.0), (double) 1.0);
            assertF64(test.Call("fib-f64", (double) 2.0), (double) 2.0);
            assertF64(test.Call("fib-f64", (double) 5.0), (double) 8.0);
            assertF64(test.Call("fib-f64", (double) 20.0), (double) 10946.0);

            assert(test.Call("even", (UInt32) 0), (UInt32) 44);
            assert(test.Call("even", (UInt32) 1), (UInt32) 99);
            assert(test.Call("even", (UInt32) 100), (UInt32) 44);
            assert(test.Call("even", (UInt32) 77), (UInt32) 99);
            assert(test.Call("odd", (UInt32) 0), (UInt32) 99);
            assert(test.Call("odd", (UInt32) 1), (UInt32) 44);
            assert(test.Call("odd", (UInt32) 200), (UInt32) 99);
            assert(test.Call("odd", (UInt32) 77), (UInt32) 44);

            assert_trap(delegate { test.CallVoid("runaway"); }, "call stack exhausted");
            assert_trap(delegate { test.CallVoid("mutual-runaway"); }, "call stack exhausted");

            assert(test.Call("as-select-first"), (UInt32) 0x132);
            assert(test.Call("as-select-mid"), (UInt32) 2);
            assert(test.Call("as-select-last"), (UInt32) 2);

            assert(test.Call("as-if-condition"), (UInt32) 1);

            assert64(test.Call("as-br_if-first"), (UInt64) 0x164);
            assert(test.Call("as-br_if-last"), (UInt32) 2);

            assertF32(test.Call("as-br_table-first"), (float) 0xf32);
            assert(test.Call("as-br_table-last"), (UInt32) 2);

            test.CallVoid("as-store-first");
            test.CallVoid("as-store-last");

            assert(test.Call("as-memory.grow-value"), (UInt32) 1);
            assert(test.Call("as-return-value"), (UInt32) 1);
            test.CallVoid("as-drop-operand");
            assertF32(test.Call("as-br-value"), (float) 1);
            assertF64(test.Call("as-local.set-value"), (double) 1);
            assertF64(test.Call("as-local.tee-value"), (double) 1);
            assertF64(test.Call("as-global.set-value"), (double) 1.0);
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
