using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class LoadF64 : Test
    {
        public LoadF64(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "loadf64.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            assertF64(test.Call("64_good1", (UInt32)0), (double)0.0);
            assertF64(test.Call("64_good2", (UInt32)0), (double)0.0);
            assertF64(test.Call("64_good3", (UInt32)0), (double)0.0);
            assertF64(test.Call("64_good4", (UInt32)0), (double)0.0);
            assertF64(test.Call("64_good5", (UInt32)0), (double)double.NaN);

            assertF64(test.Call("64_good1", (UInt32)65510), (double)0.0);
            assertF64(test.Call("64_good2", (UInt32)65510), (double)0.0);
            assertF64(test.Call("64_good3", (UInt32)65510), (double)0.0);
            assertF64(test.Call("64_good4", (UInt32)65510), (double)0.0);
            assertF64(test.Call("64_good5", (UInt32)65510), (double)0.0);

            assertF64(test.Call("64_good1", (UInt32)65511), (double)0.0);
            assertF64(test.Call("64_good2", (UInt32)65511), (double)0.0);
            assertF64(test.Call("64_good3", (UInt32)65511), (double)0.0);
            assertF64(test.Call("64_good4", (UInt32)65511), (double)0.0);
            assert_trap(delegate () { test.Call("64_good5", (UInt32)65511); }, "out of bounds memory access");

            assert_trap(delegate () { test.CallVoid("64_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("64_bad", (UInt32)1); }, "out of bounds memory access");
        }
    }
}
