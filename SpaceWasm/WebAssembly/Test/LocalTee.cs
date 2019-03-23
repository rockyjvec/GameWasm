using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class LocalTee : Test
    {
        public LocalTee(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "local_tee.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            assert(test.Call("type-local-i32"), (UInt32) 0);
            assert64(test.Call("type-local-i64"), (UInt64) 0);
            assertF32(test.Call("type-local-f32"), (float) 0);
            assertF64(test.Call("type-local-f64"), (double) 0);

            assert(test.Call("type-param-i32", (UInt32) 2), (UInt32) 10);
            assert64(test.Call("type-param-i64", (UInt64) 3), (UInt64) 11);
            assertF32(test.Call("type-param-f32", (float) 4.4), (float) 11.1);
            assertF64(test.Call("type-param-f64", (double) 5.5), (double) 12.2);

            assert(test.Call("as-block-first", (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-block-mid", (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-block-last", (UInt32) 0), (UInt32) 1);

            assert(test.Call("as-loop-first", (UInt32) 0), (UInt32) 3);
            assert(test.Call("as-loop-mid", (UInt32) 0), (UInt32) 4);
            assert(test.Call("as-loop-last", (UInt32) 0), (UInt32) 5);

            assert(test.Call("as-br-value", (UInt32) 0), (UInt32) 9);

            test.CallVoid("as-br_if-cond", (UInt32) 0);
            assert(test.Call("as-br_if-value", (UInt32) 0), (UInt32) 8);
            assert(test.Call("as-br_if-value-cond", (UInt32) 0), (UInt32) 6);

            test.CallVoid("as-br_table-index", (UInt32) 0);
            assert(test.Call("as-br_table-value", (UInt32) 0), (UInt32) 10);
            assert(test.Call("as-br_table-value-index", (UInt32) 0), (UInt32) 6);

            assert(test.Call("as-return-value", (UInt32) 0), (UInt32) 7);

            assert(test.Call("as-if-cond", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-if-then", (UInt32) 1), (UInt32) 3);
            assert(test.Call("as-if-else", (UInt32) 0), (UInt32) 4);

            assert(test.Call("as-select-first", (UInt32) 0, (UInt32) 1), (UInt32) 5);
            assert(test.Call("as-select-second", (UInt32) 0, (UInt32) 0), (UInt32) 6);
            assert(test.Call("as-select-cond", (UInt32) 0), (UInt32) 0);

            assert(test.Call("as-call-first", (UInt32) 0), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-call-mid", (UInt32) 0), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-call-last", (UInt32) 0), (UInt32) 0xFFFFFFFF);

            assert(test.Call("as-call_indirect-first", (UInt32) 0), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-call_indirect-mid", (UInt32) 0), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-call_indirect-last", (UInt32) 0), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-call_indirect-index", (UInt32) 0), (UInt32) 0xFFFFFFFF);

            test.CallVoid("as-local.set-value");
            assert(test.Call("as-local.tee-value", (UInt32) 0), (UInt32) 1);
            test.CallVoid("as-global.set-value");

            assert(test.Call("as-load-address", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-loadN-address", (UInt32) 0), (UInt32) 0);
            test.CallVoid("as-store-address", (UInt32) 0);
            test.CallVoid("as-store-value", (UInt32) 0);
            test.CallVoid("as-storeN-address", (UInt32) 0);
            test.CallVoid("as-storeN-value", (UInt32) 0);

            assertF32(test.Call("as-unary-operand", (float)0), (float)float.NaN);// -nan:0x0f1e2);
            assert(test.Call("as-binary-left", (UInt32) 0), (UInt32) 13);
            assert(test.Call("as-binary-right", (UInt32) 0), (UInt32) 6);
            assert(test.Call("as-test-operand", (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-compare-left", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-compare-right", (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-convert-operand", (UInt64) 0), (UInt32) 41);
            assert(test.Call("as-memory.grow-size", (UInt32) 0), (UInt32) 1);

            test.CallVoid("type-mixed", (UInt64) 1, (float) 2.2, (double) 3.3, (UInt32) 4, (UInt32) 5);

            assert64(test.Call("write", (UInt64) 1, (float) 2, (double) 3.3, (UInt32) 4, (UInt32) 5), (UInt64) 56);

            assertF64(test.Call("result", (UInt64) 0xFFFFFFFF, (float) -2, (double) -3.3, (UInt32) 0xFFFFFFFC, (UInt32) 0xFFFFFFFB), (double) 34.8);
        }
    }
}
