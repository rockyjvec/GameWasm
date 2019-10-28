using System;

namespace GameWasm.Webassembly.Test
{
    class Select : Test
    {
        public Select(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "select.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            assert(test.Call("select_i32", (UInt32) 1, (UInt32) 2, (UInt32) 1), (UInt32) 1);
            assert64(test.Call("select_i64", (UInt64) 2, (UInt64) 1, (UInt32) 1), (UInt64) 2);
            assertF32(test.Call("select_f32", (float) 1, (float) 2, (UInt32) 1), (float) 1);
            assertF64(test.Call("select_f64", (double) 1, (double) 2, (UInt32) 1), (double) 1);

            assert(test.Call("select_i32", (UInt32) 1, (UInt32) 2, (UInt32) 0), (UInt32) 2);
            assert(test.Call("select_i32", (UInt32) 2, (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert64(test.Call("select_i64", (UInt64) 2, (UInt64) 1, (UInt32) 0xFFFFFFFF), (UInt64) 2);
            assert64(test.Call("select_i64", (UInt64) 2, (UInt64) 1, (UInt32) 0xf0f0f0f0), (UInt64) 2);

            //assert(test.Call("select_f32", (float) nan, (float) 1, (UInt32) 1), (float) nan);
            //assert(test.Call("select_f32", (float) nan:0x20304, (float) 1, (UInt32) 1), (float) nan:0x20304);
            //assert(test.Call("select_f32", (float) nan, (float) 1, (UInt32) 0), (float) 1);
            //assert(test.Call("select_f32", (float) nan:0x20304, (float) 1, (UInt32) 0), (float) 1);
            //assert(test.Call("select_f32", (float) 2, (float) nan, (UInt32) 1), (float) 2);
            //assert(test.Call("select_f32", (float) 2, (float) nan:0x20304, (UInt32) 1), (float) 2);
            //assert(test.Call("select_f32", (float) 2, (float) nan, (UInt32) 0), (float) nan);
            //assert(test.Call("select_f32", (float) 2, (float) nan:0x20304, (UInt32) 0), (float) nan:0x20304);

            //assert(test.Call("select_f64", (double) nan, (double) 1, (UInt32) 1), (double) nan);
            //assert(test.Call("select_f64", (double) nan:0x20304, (double) 1, (UInt32) 1), (double) nan:0x20304);
            //assert(test.Call("select_f64", (double) nan, (double) 1, (UInt32) 0), (double) 1);
            //assert(test.Call("select_f64", (double) nan:0x20304, (double) 1, (UInt32) 0), (double) 1);
            //assert(test.Call("select_f64", (double) 2, (double) nan, (UInt32) 1), (double) 2);
            //assert(test.Call("select_f64", (double) 2, (double) nan:0x20304, (UInt32) 1), (double) 2);
            //assert(test.Call("select_f64", (double) 2, (double) nan, (UInt32) 0), (double) nan);
            //assert(test.Call("select_f64", (double) 2, (double) nan:0x20304, (UInt32) 0), (double) nan:0x20304);

            assert_trap(delegate { test.CallVoid("select_trap_l", (UInt32)1); }, "unreachable");
            assert_trap(delegate { test.CallVoid("select_trap_l", (UInt32)0); }, "unreachable");
            assert_trap(delegate { test.CallVoid("select_trap_r", (UInt32)1); }, "unreachable");
            assert_trap(delegate { test.CallVoid("select_trap_r", (UInt32)0); }, "unreachable");

            assert(test.Call("as-select-first", (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-select-first", (UInt32) 1), (UInt32) 0);
            assert(test.Call("as-select-mid", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-select-mid", (UInt32) 1), (UInt32) 2);
            assert(test.Call("as-select-last", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-select-last", (UInt32) 1), (UInt32) 3);

            assert(test.Call("as-loop-first", (UInt32) 0), (UInt32) 3);
            assert(test.Call("as-loop-first", (UInt32) 1), (UInt32) 2);
            assert(test.Call("as-loop-mid", (UInt32) 0), (UInt32) 3);
            assert(test.Call("as-loop-mid", (UInt32) 1), (UInt32) 2);
            assert(test.Call("as-loop-last", (UInt32) 0), (UInt32) 3);
            assert(test.Call("as-loop-last", (UInt32) 1), (UInt32) 2);

            test.CallVoid("as-if-condition", (UInt32) 0);
            test.CallVoid("as-if-condition", (UInt32) 1);
            assert(test.Call("as-if-then", (UInt32) 0), (UInt32) 3);
            assert(test.Call("as-if-then", (UInt32) 1), (UInt32) 2);
            assert(test.Call("as-if-else", (UInt32) 0), (UInt32) 3);
            assert(test.Call("as-if-else", (UInt32) 1), (UInt32) 2);

            assert(test.Call("as-br_if-first", (UInt32) 0), (UInt32) 3);
            assert(test.Call("as-br_if-first", (UInt32) 1), (UInt32) 2);
            assert(test.Call("as-br_if-last", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-br_if-last", (UInt32) 1), (UInt32) 2);

            assert(test.Call("as-br_table-first", (UInt32) 0), (UInt32) 3);
            assert(test.Call("as-br_table-first", (UInt32) 1), (UInt32) 2);
            assert(test.Call("as-br_table-last", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-br_table-last", (UInt32) 1), (UInt32) 2);

            assert(test.Call("as-call_indirect-first", (UInt32) 0), (UInt32) 3);
            assert(test.Call("as-call_indirect-first", (UInt32) 1), (UInt32) 2);
            assert(test.Call("as-call_indirect-mid", (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-call_indirect-mid", (UInt32) 1), (UInt32) 1);
            assert_trap(delegate { test.CallVoid("as-call_indirect-last", (UInt32)0); }, "undefined element");
            assert_trap(delegate { test.CallVoid("as-call_indirect-last", (UInt32)1); }, "undefined element");

            test.CallVoid("as-store-first", (UInt32) 0);
            test.CallVoid("as-store-first", (UInt32) 1);
            test.CallVoid("as-store-last", (UInt32) 0);
            test.CallVoid("as-store-last", (UInt32) 1);

            assert(test.Call("as-memory.grow-value", (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-memory.grow-value", (UInt32) 1), (UInt32) 3);

            assert(test.Call("as-call-value", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-call-value", (UInt32) 1), (UInt32) 1);
            assert(test.Call("as-return-value", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-return-value", (UInt32) 1), (UInt32) 1);
            test.CallVoid("as-drop-operand", (UInt32) 0);
            test.CallVoid("as-drop-operand", (UInt32) 1);
            assert(test.Call("as-br-value", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-br-value", (UInt32) 1), (UInt32) 1);
            assert(test.Call("as-local.set-value", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-local.set-value", (UInt32) 1), (UInt32) 1);
            assert(test.Call("as-local.tee-value", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-local.tee-value", (UInt32) 1), (UInt32) 1);
            assert(test.Call("as-global.set-value", (UInt32) 0), (UInt32) 2);
            assert(test.Call("as-global.set-value", (UInt32) 1), (UInt32) 1);
            assert(test.Call("as-load-operand", (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-load-operand", (UInt32) 1), (UInt32) 1);

            assert(test.Call("as-unary-operand", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-unary-operand", (UInt32) 1), (UInt32) 1);
            assert(test.Call("as-binary-operand", (UInt32) 0), (UInt32) 4);
            assert(test.Call("as-binary-operand", (UInt32) 1), (UInt32) 1);
            assert(test.Call("as-test-operand", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-test-operand", (UInt32) 1), (UInt32) 1);
            assert(test.Call("as-compare-left", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-compare-left", (UInt32) 1), (UInt32) 1);
            assert(test.Call("as-compare-right", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-compare-right", (UInt32) 1), (UInt32) 1);
            assert(test.Call("as-convert-operand", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-convert-operand", (UInt32) 1), (UInt32) 1);
        }
    }
}
