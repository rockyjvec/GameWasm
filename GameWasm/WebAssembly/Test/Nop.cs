using System;

namespace GameWasm.Webassembly.Test
{
    class Nop : Test
    {
        public Nop(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "nop.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            assert(test.Call("as-func-first"), (UInt32) 1);
            assert(test.Call("as-func-mid"), (UInt32) 2);
            assert(test.Call("as-func-last"), (UInt32) 3);
            assert(test.Call("as-func-everywhere"), (UInt32) 4);

            test.CallVoid("as-drop-first", (UInt32) 0);
            test.CallVoid("as-drop-last", (UInt32) 0);
            test.CallVoid("as-drop-everywhere", (UInt32) 0);

            assert(test.Call("as-select-first", (UInt32) 3), (UInt32) 3);
            assert(test.Call("as-select-mid1", (UInt32) 3), (UInt32) 3);
            assert(test.Call("as-select-mid2", (UInt32) 3), (UInt32) 3);
            assert(test.Call("as-select-last", (UInt32) 3), (UInt32) 3);
            assert(test.Call("as-select-everywhere", (UInt32) 3), (UInt32) 3);

            assert(test.Call("as-block-first"), (UInt32) 2);
            assert(test.Call("as-block-mid"), (UInt32) 2);
            assert(test.Call("as-block-last"), (UInt32) 3);
            assert(test.Call("as-block-everywhere"), (UInt32) 4);

            assert(test.Call("as-loop-first"), (UInt32) 2);
            assert(test.Call("as-loop-mid"), (UInt32) 2);
            assert(test.Call("as-loop-last"), (UInt32) 3);
            assert(test.Call("as-loop-everywhere"), (UInt32) 4);

            test.CallVoid("as-if-condition", (UInt32) 0);
            test.CallVoid("as-if-condition", (UInt32) 0xFFFFFFFF);
            test.CallVoid("as-if-then", (UInt32) 0);
            test.CallVoid("as-if-then", (UInt32) 4);
            test.CallVoid("as-if-else", (UInt32) 0);
            test.CallVoid("as-if-else", (UInt32) 3);

            assert(test.Call("as-br-first", (UInt32) 5), (UInt32) 5);
            assert(test.Call("as-br-last", (UInt32) 6), (UInt32) 6);
            assert(test.Call("as-br-everywhere", (UInt32) 7), (UInt32) 7);

            assert(test.Call("as-br_if-first", (UInt32) 4), (UInt32) 4);
            assert(test.Call("as-br_if-mid", (UInt32) 5), (UInt32) 5);
            assert(test.Call("as-br_if-last", (UInt32) 6), (UInt32) 6);
            assert(test.Call("as-br_if-everywhere", (UInt32) 7), (UInt32) 7);

            assert(test.Call("as-br_table-first", (UInt32) 4), (UInt32) 4);
            assert(test.Call("as-br_table-mid", (UInt32) 5), (UInt32) 5);
            assert(test.Call("as-br_table-last", (UInt32) 6), (UInt32) 6);
            assert(test.Call("as-br_table-everywhere", (UInt32) 7), (UInt32) 7);

            assert(test.Call("as-return-first", (UInt32) 5), (UInt32) 5);
            assert(test.Call("as-return-last", (UInt32) 6), (UInt32) 6);
            assert(test.Call("as-return-everywhere", (UInt32) 7), (UInt32) 7);

            assert(test.Call("as-call-first", (UInt32) 3, (UInt32) 1, (UInt32) 2), (UInt32) 2);
            assert(test.Call("as-call-mid1", (UInt32) 3, (UInt32) 1, (UInt32) 2), (UInt32) 2);
            assert(test.Call("as-call-mid2", (UInt32) 0, (UInt32) 3, (UInt32) 1), (UInt32) 2);
            assert(test.Call("as-call-last", (UInt32) 10, (UInt32) 9, (UInt32) 0xFFFFFFFF), (UInt32) 20);
            assert(test.Call("as-call-everywhere", (UInt32) 2, (UInt32) 1, (UInt32) 5), (UInt32) 0xFFFFFFFE);

            assert(test.Call("as-unary-first", (UInt32) 30), (UInt32) 1);
            assert(test.Call("as-unary-last", (UInt32) 30), (UInt32) 1);
            assert(test.Call("as-unary-everywhere", (UInt32) 12), (UInt32) 2);

            assert(test.Call("as-binary-first", (UInt32) 3), (UInt32) 6);
            assert(test.Call("as-binary-mid", (UInt32) 3), (UInt32) 6);
            assert(test.Call("as-binary-last", (UInt32) 3), (UInt32) 6);
            assert(test.Call("as-binary-everywhere", (UInt32) 3), (UInt32) 6);

            assert(test.Call("as-test-first", (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-test-last", (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-test-everywhere", (UInt32) 0), (UInt32) 1);

            assert(test.Call("as-compare-first", (UInt32) 3), (UInt32) 0);
            assert(test.Call("as-compare-mid", (UInt32) 3), (UInt32) 0);
            assert(test.Call("as-compare-last", (UInt32) 3), (UInt32) 0);
            assert(test.Call("as-compare-everywhere", (UInt32) 3), (UInt32) 1);

            assert(test.Call("as-memory.grow-first", (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-memory.grow-last", (UInt32) 2), (UInt32) 1);
            assert(test.Call("as-memory.grow-everywhere", (UInt32) 12), (UInt32) 3);

            assert(test.Call("as-call_indirect-first"), (UInt32) 1);
            assert(test.Call("as-call_indirect-mid1"), (UInt32) 1);
            assert(test.Call("as-call_indirect-mid2"), (UInt32) 1);
            assert(test.Call("as-call_indirect-last"), (UInt32) 1);
            assert(test.Call("as-call_indirect-everywhere"), (UInt32) 1);

            assert(test.Call("as-local.set-first", (UInt32) 1), (UInt32) 2);
            assert(test.Call("as-local.set-last", (UInt32) 1), (UInt32) 2);
            assert(test.Call("as-local.set-everywhere", (UInt32) 1), (UInt32) 2);

            assert(test.Call("as-local.tee-first", (UInt32) 1), (UInt32) 2);
            assert(test.Call("as-local.tee-last", (UInt32) 1), (UInt32) 2);
            assert(test.Call("as-local.tee-everywhere", (UInt32) 1), (UInt32) 2);

            assert(test.Call("as-global.set-first"), (UInt32) 2);
            assert(test.Call("as-global.set-last"), (UInt32) 2);
            assert(test.Call("as-global.set-everywhere"), (UInt32) 2);

            assert(test.Call("as-load-first", (UInt32) 100), (UInt32) 0);
            assert(test.Call("as-load-last", (UInt32) 100), (UInt32) 0);
            assert(test.Call("as-load-everywhere", (UInt32) 100), (UInt32) 0);

            test.CallVoid("as-store-first", (UInt32) 0, (UInt32) 1);
            test.CallVoid("as-store-mid", (UInt32) 0, (UInt32) 2);
            test.CallVoid("as-store-last", (UInt32) 0, (UInt32) 3);
            test.CallVoid("as-store-everywhere", (UInt32) 0, (UInt32) 4);
        }
    }
}
