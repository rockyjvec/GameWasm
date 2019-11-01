using System;

namespace GameWasm.Webassembly.Test
{
    class Block : Test
    {
        public Block(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "block.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            test.CallVoid("empty");
            assert(test.Call("singular"), (UInt32) 7);
            assert(test.Call("multi"), (UInt32) 8);
            assert(test.Call("nested"), (UInt32) 9);
            assert(test.Call("deep"), (UInt32) 150);

            assert(test.Call("as-select-first"), (UInt32) 1);
            assert(test.Call("as-select-mid"), (UInt32) 2);
            assert(test.Call("as-select-last"), (UInt32) 2);

            assert(test.Call("as-loop-first"), (UInt32) 1);
            assert(test.Call("as-loop-mid"), (UInt32) 1);
            assert(test.Call("as-loop-last"), (UInt32) 1);

            test.CallVoid("as-if-condition");
            assert(test.Call("as-if-then"), (UInt32) 1);
            assert(test.Call("as-if-else"), (UInt32) 2);

            assert(test.Call("as-br_if-first"), (UInt32) 1);
            assert(test.Call("as-br_if-last"), (UInt32) 2);

            assert(test.Call("as-br_table-first"), (UInt32) 1);
            assert(test.Call("as-br_table-last"), (UInt32) 2);

            assert(test.Call("as-call_indirect-first"), (UInt32) 1);
            assert(test.Call("as-call_indirect-mid"), (UInt32) 2);
            assert(test.Call("as-call_indirect-last"), (UInt32) 1);

            test.CallVoid("as-store-first");
            test.CallVoid("as-store-last");

            assert(test.Call("as-memory.grow-value"), (UInt32) 1);
            assert(test.Call("as-call-value"), (UInt32) 1);
            assert(test.Call("as-return-value"), (UInt32) 1);
            test.CallVoid("as-drop-operand");
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

            assert(test.Call("break-inner"), (UInt32) 0xf);

            assert(test.Call("effects"), (UInt32) 1);
        }
    }
}
