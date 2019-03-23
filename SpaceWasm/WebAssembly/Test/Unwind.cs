using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class Unwind : Test
    {
        public Unwind(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "unwind.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            assert_trap(delegate { test.CallVoid("func-unwind-by-unreachable"); }, "unreachable");
            test.CallVoid("func-unwind-by-br");
            assert(test.Call("func-unwind-by-br-value"), (UInt32) 9);
            test.CallVoid("func-unwind-by-br_if");
            assert(test.Call("func-unwind-by-br_if-value"), (UInt32) 9);
            test.CallVoid("func-unwind-by-br_table");
            assert(test.Call("func-unwind-by-br_table-value"), (UInt32) 9);
            assert(test.Call("func-unwind-by-return"), (UInt32) 9);

            assert_trap(delegate { test.CallVoid("block-unwind-by-unreachable"); }, "unreachable");
            assert(test.Call("block-unwind-by-br"), (UInt32) 9);
            assert(test.Call("block-unwind-by-br-value"), (UInt32) 9);
            assert(test.Call("block-unwind-by-br_if"), (UInt32) 9);
            assert(test.Call("block-unwind-by-br_if-value"), (UInt32) 9);
            assert(test.Call("block-unwind-by-br_table"), (UInt32) 9);
            assert(test.Call("block-unwind-by-br_table-value"), (UInt32) 9);
            assert(test.Call("block-unwind-by-return"), (UInt32) 9);

            assert_trap(delegate { test.CallVoid("block-nested-unwind-by-unreachable"); }, "unreachable");
            assert(test.Call("block-nested-unwind-by-br"), (UInt32) 9);
            assert(test.Call("block-nested-unwind-by-br-value"), (UInt32) 9);
            assert(test.Call("block-nested-unwind-by-br_if"), (UInt32) 9);
            assert(test.Call("block-nested-unwind-by-br_if-value"), (UInt32) 9);
            assert(test.Call("block-nested-unwind-by-br_table"), (UInt32) 9);
            assert(test.Call("block-nested-unwind-by-br_table-value"), (UInt32) 9);
            assert(test.Call("block-nested-unwind-by-return"), (UInt32) 9);

            assert_trap(delegate { test.CallVoid("unary-after-unreachable"); }, "unreachable");
            assert(test.Call("unary-after-br"), (UInt32) 9);
            assert(test.Call("unary-after-br_if"), (UInt32) 9);
            assert(test.Call("unary-after-br_table"), (UInt32) 9);
            assert(test.Call("unary-after-return"), (UInt32) 9);

            assert_trap(delegate { test.CallVoid("binary-after-unreachable"); }, "unreachable");
            assert(test.Call("binary-after-br"), (UInt32) 9);
            assert(test.Call("binary-after-br_if"), (UInt32) 9);
            assert(test.Call("binary-after-br_table"), (UInt32) 9);
            assert(test.Call("binary-after-return"), (UInt32) 9);

            assert_trap(delegate { test.CallVoid("select-after-unreachable"); }, "unreachable");
            assert(test.Call("select-after-br"), (UInt32) 9);
            assert(test.Call("select-after-br_if"), (UInt32) 9);
            assert(test.Call("select-after-br_table"), (UInt32) 9);
            assert(test.Call("select-after-return"), (UInt32) 9);

            assert_trap(delegate { test.CallVoid("block-value-after-unreachable"); }, "unreachable");
            assert(test.Call("block-value-after-br"), (UInt32) 9);
            assert(test.Call("block-value-after-br_if"), (UInt32) 9);
            assert(test.Call("block-value-after-br_table"), (UInt32) 9);
            assert(test.Call("block-value-after-return"), (UInt32) 9);

            assert_trap(delegate { test.CallVoid("loop-value-after-unreachable"); }, "unreachable");
            assert(test.Call("loop-value-after-br"), (UInt32) 9);
            assert(test.Call("loop-value-after-br_if"), (UInt32) 9);
            assert(test.Call("loop-value-after-br_table"), (UInt32) 9);
            assert(test.Call("loop-value-after-return"), (UInt32) 9);
        }
    }
}
