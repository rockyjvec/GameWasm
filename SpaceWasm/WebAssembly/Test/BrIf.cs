using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class BrIf : Test
    {
        public BrIf(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "brif.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            test.CallVoid("type-i32");
            test.CallVoid("type-i64");
            test.CallVoid("type-f32");
            test.CallVoid("type-f64");

            assert(test.Call("type-i32-value"), (UInt32) 1);
            assert64(test.Call("type-i64-value"), (UInt64) 2);
            assertF32(test.Call("type-f32-value"), (float) 3);
            assertF64(test.Call("type-f64-value"), (double) 4);

            assert(test.Call("as-block-first", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-block-first", (UInt32) 1), (UInt32) 3);
            assert(test.Call("as-block-mid", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-block-mid", (UInt32) 1), (UInt32) 3);
            
            test.CallVoid("as-block-last", (UInt32)0);
            test.CallVoid("as-block-last", (UInt32)1);

            assert(test.Call("as-block-first-value", (UInt32) 0), (UInt32) 11);
            assert(test.Call("as-block-first-value", (UInt32) 1), (UInt32) 10);
            assert(test.Call("as-block-mid-value", (UInt32) 0), (UInt32) 21);
            assert(test.Call("as-block-mid-value", (UInt32) 1), (UInt32) 20);
            assert(test.Call("as-block-last-value", (UInt32) 0), (UInt32) 11);
            assert(test.Call("as-block-last-value", (UInt32) 1), (UInt32) 11);

            assert(test.Call("as-loop-first", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-loop-first", (UInt32) 1), (UInt32) 3);
            assert(test.Call("as-loop-mid", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-loop-mid", (UInt32) 1), (UInt32) 4);
            test.CallVoid("as-loop-last", (UInt32) 0);
            test.CallVoid("as-loop-last", (UInt32) 1);

            assert(test.Call("as-br-value"), (UInt32) 1);

            test.CallVoid("as-br_if-cond");
            assert(test.Call("as-br_if-value"), (UInt32) 1);
            assert(test.Call("as-br_if-value-cond", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-br_if-value-cond", (UInt32) 1), (UInt32) 1);

            test.CallVoid("as-br_table-index");
            assert(test.Call("as-br_table-value"), (UInt32) 1);
            assert(test.Call("as-br_table-value-index"), (UInt32) 1);

            assert64(test.Call("as-return-value"), (UInt64) 1);

            assert(test.Call("as-if-cond", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-if-cond", (UInt32) 1), (UInt32) 1);
            test.CallVoid("as-if-then", (UInt32) 0, (UInt32) 0);
            test.CallVoid("as-if-then", (UInt32) 4, (UInt32) 0);
            test.CallVoid("as-if-then", (UInt32) 0, (UInt32) 1);
            test.CallVoid("as-if-then", (UInt32) 4, (UInt32) 1);
            test.CallVoid("as-if-else", (UInt32) 0, (UInt32) 0);
            test.CallVoid("as-if-else", (UInt32) 3, (UInt32) 0);
            test.CallVoid("as-if-else", (UInt32) 0, (UInt32) 1);
            test.CallVoid("as-if-else", (UInt32) 3, (UInt32) 1);

            assert(test.Call("as-select-first", (UInt32) 0), (UInt32) 3);
            assert(test.Call("as-select-first", (UInt32) 1), (UInt32) 3);
            assert(test.Call("as-select-second", (UInt32) 0), (UInt32) 3);
            assert(test.Call("as-select-second", (UInt32) 1), (UInt32) 3);
            assert(test.Call("as-select-cond"), (UInt32) 3);

            assert(test.Call("as-call-first"), (UInt32) 12);
            assert(test.Call("as-call-mid"), (UInt32) 13);
            assert(test.Call("as-call-last"), (UInt32) 14);

            assert(test.Call("as-call_indirect-func"), (UInt32) 4);
            assert(test.Call("as-call_indirect-first"), (UInt32) 4);
            assert(test.Call("as-call_indirect-mid"), (UInt32) 4);
            assert(test.Call("as-call_indirect-last"), (UInt32) 4);

            assert(test.Call("as-local.set-value", (UInt32) 0), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-local.set-value", (UInt32) 1), (UInt32) 17);

            assert(test.Call("as-local.tee-value", (UInt32) 0), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-local.tee-value", (UInt32) 1), (UInt32) 1);

            assert(test.Call("as-global.set-value", (UInt32) 0), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-global.set-value", (UInt32) 1), (UInt32) 1);

            assert(test.Call("as-load-address"), (UInt32) 1);
            assert(test.Call("as-loadN-address"), (UInt32) 30);

            assert(test.Call("as-store-address"), (UInt32) 30);
            assert(test.Call("as-store-value"), (UInt32) 31);
            assert(test.Call("as-storeN-address"), (UInt32) 32);
            assert(test.Call("as-storeN-value"), (UInt32) 33);

            assertF64(test.Call("as-unary-operand"), (double) 1.0);
            assert(test.Call("as-binary-left"), (UInt32) 1);
            assert(test.Call("as-binary-right"), (UInt32) 1);
            assert(test.Call("as-test-operand"), (UInt32) 0);
            assert(test.Call("as-compare-left"), (UInt32) 1);
            assert(test.Call("as-compare-right"), (UInt32) 1);
            assert(test.Call("as-memory.grow-size"), (UInt32) 1);

            assert(test.Call("nested-block-value", (UInt32) 0), (UInt32) 21);
            assert(test.Call("nested-block-value", (UInt32) 1), (UInt32) 9);
            assert(test.Call("nested-br-value", (UInt32) 0), (UInt32) 5);
            assert(test.Call("nested-br-value", (UInt32) 1), (UInt32) 9);
            assert(test.Call("nested-br_if-value", (UInt32) 0), (UInt32) 5);
            assert(test.Call("nested-br_if-value", (UInt32) 1), (UInt32) 9);
            assert(test.Call("nested-br_if-value-cond", (UInt32) 0), (UInt32) 5);
            assert(test.Call("nested-br_if-value-cond", (UInt32) 1), (UInt32) 9);
            assert(test.Call("nested-br_table-value", (UInt32) 0), (UInt32) 5);
            assert(test.Call("nested-br_table-value", (UInt32) 1), (UInt32) 9);
            assert(test.Call("nested-br_table-value-index", (UInt32) 0), (UInt32) 5);
            assert(test.Call("nested-br_table-value-index", (UInt32) 1), (UInt32) 9);
        }
    }
}
