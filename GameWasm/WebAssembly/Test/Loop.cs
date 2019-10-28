using System;

namespace GameWasm.Webassembly.Test
{
    class Loop : Test
    {
        public Loop(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "loop.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            test.CallVoid("empty");;
            assert(test.Call("singular"), (UInt32) 7);
            assert(test.Call("multi"), (UInt32) 8);
            assert(test.Call("nested"), (UInt32) 9);
            assert(test.Call("deep"), (UInt32) 150);

            assert(test.Call("as-select-first"), (UInt32) 1);
            assert(test.Call("as-select-mid"), (UInt32) 2);
            assert(test.Call("as-select-last"), (UInt32) 2);

            test.CallVoid("as-if-condition");;
            assert(test.Call("as-if-then"), (UInt32) 1);
            assert(test.Call("as-if-else"), (UInt32) 2);

            assert(test.Call("as-br_if-first"), (UInt32) 1);
            assert(test.Call("as-br_if-last"), (UInt32) 2);

            assert(test.Call("as-br_table-first"), (UInt32) 1);
            assert(test.Call("as-br_table-last"), (UInt32) 2);

            assert(test.Call("as-call_indirect-first"), (UInt32) 1);
            assert(test.Call("as-call_indirect-mid"), (UInt32) 2);
            assert(test.Call("as-call_indirect-last"), (UInt32) 1);

            test.CallVoid("as-store-first");;
            test.CallVoid("as-store-last");;

            assert(test.Call("as-memory.grow-value"), (UInt32) 1);
            assert(test.Call("as-call-value"), (UInt32) 1);
            assert(test.Call("as-return-value"), (UInt32) 1);
            test.CallVoid("as-drop-operand");;
            assert(test.Call("as-br-value"), (UInt32) 1);
            assert(test.Call("as-local.set-value"), (UInt32) 1);
            assert(test.Call("as-local.tee-value"), (UInt32) 1);
            assert(test.Call("as-global.set-value"), (UInt32) 1);
            assert(test.Call("as-load-operand"), (UInt32) 1);

            assert(test.Call("as-unary-operand"), (UInt32) 0);
            assert(test.Call("as-binary-operand"), (UInt32) 12);
            assert(test.Call("as-test-operand"), (UInt32) 0);
            assert(test.Call("as-compare-operand"), (UInt32) 0);

            assert(test.Call("break-bare"), (UInt32) 19);
            assert(test.Call("break-value"), (UInt32) 18);
            assert(test.Call("break-repeated"), (UInt32) 18);
            assert(test.Call("break-inner"), (UInt32) 0x1f);

            assert(test.Call("effects"), (UInt32) 1);

            assert64(test.Call("while", (UInt64) 0), (UInt64) 1);
            assert64(test.Call("while", (UInt64) 1), (UInt64) 1);
            assert64(test.Call("while", (UInt64) 2), (UInt64) 2);
            assert64(test.Call("while", (UInt64) 3), (UInt64) 6);
            assert64(test.Call("while", (UInt64) 5), (UInt64) 120);
            assert64(test.Call("while", (UInt64) 20), (UInt64) 2432902008176640000);

            assert64(test.Call("for", (UInt64) 0), (UInt64) 1);
            assert64(test.Call("for", (UInt64) 1), (UInt64) 1);
            assert64(test.Call("for", (UInt64) 2), (UInt64) 2);
            assert64(test.Call("for", (UInt64) 3), (UInt64) 6);
            assert64(test.Call("for", (UInt64) 5), (UInt64) 120);
            assert64(test.Call("for", (UInt64) 20), (UInt64) 2432902008176640000);

            assertF32(test.Call("nesting", (float) 0, (float) 7), (float) 0);
            assertF32(test.Call("nesting", (float) 7, (float) 0), (float) 0);
            assertF32(test.Call("nesting", (float) 1, (float) 1), (float) 1);
            assertF32(test.Call("nesting", (float) 1, (float) 2), (float) 2);
            assertF32(test.Call("nesting", (float) 1, (float) 3), (float) 4);
            assertF32(test.Call("nesting", (float) 1, (float) 4), (float) 6);
            assertF32(test.Call("nesting", (float) 1, (float) 100), (float) 2550);
            assertF32(test.Call("nesting", (float) 1, (float) 101), (float) 2601);
            assertF32(test.Call("nesting", (float) 2, (float) 1), (float) 1);
            assertF32(test.Call("nesting", (float) 3, (float) 1), (float) 1);
            assertF32(test.Call("nesting", (float) 10, (float) 1), (float) 1);
            assertF32(test.Call("nesting", (float) 2, (float) 2), (float) 3);
            assertF32(test.Call("nesting", (float) 2, (float) 3), (float) 4);
            assertF32(test.Call("nesting", (float) 7, (float) 4), (float) 10.3095235825);
            assertF32(test.Call("nesting", (float) 7, (float) 100), (float) 4381.54785156);
            assertF32(test.Call("nesting", (float) 7, (float) 101), (float) 2601);
        }
    }
}
