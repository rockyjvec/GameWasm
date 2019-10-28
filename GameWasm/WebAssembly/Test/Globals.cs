using System;

namespace GameWasm.Webassembly.Test
{
    class Globals : Test
    {
        public Globals(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "globals.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            assert(test.Call("get-a"), (UInt32) 0xFFFFFFFE);
            assert64(test.Call("get-b"), (UInt64) 0xFFFFFFFFFFFFFFFB);
            assert(test.Call("get-x"), (UInt32) 0xFFFFFFF4);
            assert64(test.Call("get-y"), (UInt64) 0xFFFFFFFFFFFFFFF1);

            assertF32(test.Call("get-1"), (float) -3);
            assertF64(test.Call("get-2"), (double) -4);
            assertF32(test.Call("get-5"), (float) -13);
            assertF64(test.Call("get-6"), (double) -14);

            test.CallVoid("set-x", (UInt32) 6);
            test.CallVoid("set-y", (UInt64) 7);
            test.CallVoid("set-5", (float) 8);
            test.CallVoid("set-6", (double) 9);

            assert(test.Call("get-x"), (UInt32) 6);
            assert64(test.Call("get-y"), (UInt64) 7);
            assertF32(test.Call("get-5"), (float) 8);
            assertF64(test.Call("get-6"), (double) 9);

            assert(test.Call("as-select-first"), (UInt32) 6);
            assert(test.Call("as-select-mid"), (UInt32) 2);
            assert(test.Call("as-select-last"), (UInt32) 2);

            assert(test.Call("as-loop-first"), (UInt32) 6);
            assert(test.Call("as-loop-mid"), (UInt32) 6);
            assert(test.Call("as-loop-last"), (UInt32) 6);

            assert(test.Call("as-if-condition"), (UInt32) 2);
            assert(test.Call("as-if-then"), (UInt32) 6);
            assert(test.Call("as-if-else"), (UInt32) 6);

            assert(test.Call("as-br_if-first"), (UInt32) 6);
            assert(test.Call("as-br_if-last"), (UInt32) 2);

            assert(test.Call("as-br_table-first"), (UInt32) 6);
            assert(test.Call("as-br_table-last"), (UInt32) 2);

            assert(test.Call("as-call_indirect-first"), (UInt32) 6);
            assert(test.Call("as-call_indirect-mid"), (UInt32) 2);
            assert_trap(delegate { test.CallVoid("as-call_indirect-last"); }, "undefined element");

            test.CallVoid("as-store-first");
            test.CallVoid("as-store-last");
            assert(test.Call("as-load-operand"), (UInt32) 1);
            assert(test.Call("as-memory.grow-value"), (UInt32) 1);

            assert(test.Call("as-call-value"), (UInt32) 6);

            assert(test.Call("as-return-value"), (UInt32) 6);
            test.CallVoid("as-drop-operand");
            assert(test.Call("as-br-value"), (UInt32) 6);

            assert(test.Call("as-local.set-value", (UInt32) 1), (UInt32) 6);
            assert(test.Call("as-local.tee-value", (UInt32) 1), (UInt32) 6);
            assert(test.Call("as-global.set-value"), (UInt32) 6);

            assert(test.Call("as-unary-operand"), (UInt32) 0);
            assert(test.Call("as-binary-operand"), (UInt32) 36);
            assert(test.Call("as-compare-operand"), (UInt32) 1);

        }
    }
}
