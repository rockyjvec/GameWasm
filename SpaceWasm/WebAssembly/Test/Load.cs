using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class Load : Test
    {
        public Load(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "load.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            assert(test.Call("as-br-value"), (UInt32) 0);

            test.CallVoid("as-br_if-cond");
            assert(test.Call("as-br_if-value"), (UInt32) 0);
            assert(test.Call("as-br_if-value-cond"), (UInt32) 7);

            test.CallVoid("as-br_table-index");
            assert(test.Call("as-br_table-value"), (UInt32) 0);
            assert(test.Call("as-br_table-value-index"), (UInt32) 6);

            assert(test.Call("as-return-value"), (UInt32) 0);

            assert(test.Call("as-if-cond"), (UInt32) 1);
            assert(test.Call("as-if-then"), (UInt32) 0);
            assert(test.Call("as-if-else"), (UInt32) 0);

            assert(test.Call("as-select-first", (UInt32) 0, (UInt32) 1), (UInt32) 0);
            assert(test.Call("as-select-second", (UInt32) 0, (UInt32) 0), (UInt32) 0);
            assert(test.Call("as-select-cond"), (UInt32) 1);

            assert(test.Call("as-call-first"), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-call-mid"), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-call-last"), (UInt32) 0xFFFFFFFF);

            assert(test.Call("as-call_indirect-first"), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-call_indirect-mid"), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-call_indirect-last"), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-call_indirect-index"), (UInt32) 0xFFFFFFFF);

            test.CallVoid("as-local.set-value");
            assert(test.Call("as-local.tee-value"), (UInt32) 0);
            test.CallVoid("as-global.set-value");

            assert(test.Call("as-load-address"), (UInt32) 0);
            assert(test.Call("as-loadN-address"), (UInt32) 0);
            test.CallVoid("as-store-address");
            test.CallVoid("as-store-value");
            test.CallVoid("as-storeN-address");
            test.CallVoid("as-storeN-value");

            assert(test.Call("as-unary-operand"), (UInt32) 32);

            assert(test.Call("as-binary-left"), (UInt32) 10);
            assert(test.Call("as-binary-right"), (UInt32) 10);

            assert(test.Call("as-test-operand"), (UInt32) 1);

            assert(test.Call("as-compare-left"), (UInt32) 1);
            assert(test.Call("as-compare-right"), (UInt32) 1);

            assert(test.Call("as-memory.grow-size"), (UInt32) 1);
        }
    }
}
