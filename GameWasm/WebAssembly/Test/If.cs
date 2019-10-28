using System;

namespace GameWasm.Webassembly.Test
{
    class If : Test
    {
        public If(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "if.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            test.CallVoid("empty", (UInt32)0);
            test.CallVoid("empty", (UInt32)1);
            test.CallVoid("empty", (UInt32)100);
            test.CallVoid("empty", (UInt32)0xFFFFFFFE);

            assert(test.Call("singular", (UInt32)0), (UInt32)8);
            assert(test.Call("singular", (UInt32)1), (UInt32)7);
            assert(test.Call("singular", (UInt32)10), (UInt32)7);
            assert(test.Call("singular", (UInt32)0xFFFFFFF5), (UInt32)7);

            assert(test.Call("multi", (UInt32)0), (UInt32)9);
            assert(test.Call("multi", (UInt32)1), (UInt32)8);
            assert(test.Call("multi", (UInt32)13), (UInt32)8);
            assert(test.Call("multi", (UInt32)0xFFFFFFFB), (UInt32)8);

            assert(test.Call("nested", (UInt32)0, (UInt32)0), (UInt32)11);
            assert(test.Call("nested", (UInt32)1, (UInt32)0), (UInt32)10);
            assert(test.Call("nested", (UInt32)0, (UInt32)1), (UInt32)10);
            assert(test.Call("nested", (UInt32)3, (UInt32)2), (UInt32)9);
            assert(test.Call("nested", (UInt32)0, (UInt32)0xFFFFFF9C), (UInt32)10);
            assert(test.Call("nested", (UInt32)10, (UInt32)10), (UInt32)9);
            assert(test.Call("nested", (UInt32)0, (UInt32)0xFFFFFFFF), (UInt32)10);
            assert(test.Call("nested", (UInt32)0xFFFFFF91, (UInt32)0xFFFFFFFE), (UInt32)9);

            assert(test.Call("as-select-first", (UInt32)0), (UInt32)0);
            assert(test.Call("as-select-first", (UInt32)1), (UInt32)1);
            assert(test.Call("as-select-mid", (UInt32)0), (UInt32)2);
            assert(test.Call("as-select-mid", (UInt32)1), (UInt32)2);
            assert(test.Call("as-select-last", (UInt32)0), (UInt32)3);
            assert(test.Call("as-select-last", (UInt32)1), (UInt32)2);

            assert(test.Call("as-loop-first", (UInt32)0), (UInt32)0);
            assert(test.Call("as-loop-first", (UInt32)1), (UInt32)1);
            assert(test.Call("as-loop-mid", (UInt32)0), (UInt32)0);
            assert(test.Call("as-loop-mid", (UInt32)1), (UInt32)1);
            assert(test.Call("as-loop-last", (UInt32)0), (UInt32)0);
            assert(test.Call("as-loop-last", (UInt32)1), (UInt32)1);

            assert(test.Call("as-if-condition", (UInt32)0), (UInt32)3);
            assert(test.Call("as-if-condition", (UInt32)1), (UInt32)2);

            assert(test.Call("as-br_if-first", (UInt32)0), (UInt32)0);
            assert(test.Call("as-br_if-first", (UInt32)1), (UInt32)1);
            assert(test.Call("as-br_if-last", (UInt32)0), (UInt32)3);
            assert(test.Call("as-br_if-last", (UInt32)1), (UInt32)2);

            assert(test.Call("as-br_table-first", (UInt32)0), (UInt32)0);
            assert(test.Call("as-br_table-first", (UInt32)1), (UInt32)1);
            assert(test.Call("as-br_table-last", (UInt32)0), (UInt32)2);
            assert(test.Call("as-br_table-last", (UInt32)1), (UInt32)2);

            assert(test.Call("as-call_indirect-first", (UInt32)0), (UInt32)0);
            assert(test.Call("as-call_indirect-first", (UInt32)1), (UInt32)1);
            assert(test.Call("as-call_indirect-mid", (UInt32)0), (UInt32)2);
            assert(test.Call("as-call_indirect-mid", (UInt32)1), (UInt32)2);
            assert(test.Call("as-call_indirect-last", (UInt32)0), (UInt32)2);
            assert_trap(delegate { test.CallVoid("as-call_indirect-last", (UInt32)1); }, "undefined element");

            test.CallVoid("as-store-first", (UInt32) 0);
            test.CallVoid("as-store-first", (UInt32) 1);
            test.CallVoid("as-store-last", (UInt32) 0);
            test.CallVoid("as-store-last", (UInt32) 1);

            assert(test.Call("as-memory.grow-value", (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-memory.grow-value", (UInt32) 1), (UInt32) 1);

            assert(test.Call("as-call-value", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-call-value", (UInt32) 1), (UInt32) 1);

            assert(test.Call("as-return-value", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-return-value", (UInt32) 1), (UInt32) 1);

            test.CallVoid("as-drop-operand", (UInt32) 0);
            test.CallVoid("as-drop-operand", (UInt32) 1);

            assert(test.Call("as-br-value", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-br-value", (UInt32) 1), (UInt32) 1);

            assert(test.Call("as-local.set-value", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-local.set-value", (UInt32) 1), (UInt32) 1);

            assert(test.Call("as-local.tee-value", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-local.tee-value", (UInt32) 1), (UInt32) 1);

            assert(test.Call("as-global.set-value", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-global.set-value", (UInt32) 1), (UInt32) 1);

            assert(test.Call("as-load-operand", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-load-operand", (UInt32) 1), (UInt32) 0);

            assert(test.Call("as-unary-operand", (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-unary-operand", (UInt32) 1), (UInt32) 0);
            assert(test.Call("as-unary-operand", (UInt32) 0xFFFFFFFF), (UInt32) 0);

            assert(test.Call("as-binary-operand", (UInt32) 0, (UInt32) 0), (UInt32) 15);
            assert(test.Call("as-binary-operand", (UInt32) 0, (UInt32) 1), (UInt32) 0xFFFFFFF4);
            assert(test.Call("as-binary-operand", (UInt32) 1, (UInt32) 0), (UInt32) 0xFFFFFFF1);
            assert(test.Call("as-binary-operand", (UInt32) 1, (UInt32) 1), (UInt32) 12);

            assert(test.Call("as-test-operand", (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-test-operand", (UInt32) 1), (UInt32) 0);

            assert(test.Call("as-compare-operand", (UInt32) 0, (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-compare-operand", (UInt32) 0, (UInt32) 1), (UInt32) 0);
            assert(test.Call("as-compare-operand", (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-compare-operand", (UInt32) 1, (UInt32) 1), (UInt32) 0);

            assert(test.Call("break-bare"), (UInt32) 19);
            assert(test.Call("break-value", (UInt32) 1), (UInt32) 18);
            assert(test.Call("break-value", (UInt32) 0), (UInt32) 21);

            assert(test.Call("effects", (UInt32) 1), (UInt32) 0xFFFFFFF2);
            assert(test.Call("effects", (UInt32) 0), (UInt32) 0xFFFFFFFA);
        }
    }
}
