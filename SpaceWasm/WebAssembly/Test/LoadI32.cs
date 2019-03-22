using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    public class LoadI32 : Test
    {
        public LoadI32(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "loadi32.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            assert(test.Call("8u_good1", (UInt32)0), (UInt32)97);
            assert(test.Call("8u_good2", (UInt32)0), (UInt32)97);
            assert(test.Call("8u_good3", (UInt32)0), (UInt32)98);
            assert(test.Call("8u_good4", (UInt32)0), (UInt32)99);
            assert(test.Call("8u_good5", (UInt32)0), (UInt32)122);

            assert(test.Call("8s_good1", (UInt32)0), (UInt32)97);
            assert(test.Call("8s_good2", (UInt32)0), (UInt32)97);
            assert(test.Call("8s_good3", (UInt32)0), (UInt32)98);
            assert(test.Call("8s_good4", (UInt32)0), (UInt32)99);
            assert(test.Call("8s_good5", (UInt32)0), (UInt32)122);

            assert(test.Call("16u_good1", (UInt32)0), (UInt32)25185);
            assert(test.Call("16u_good2", (UInt32)0), (UInt32)25185);
            assert(test.Call("16u_good3", (UInt32)0), (UInt32)25442);
            assert(test.Call("16u_good4", (UInt32)0), (UInt32)25699);
            assert(test.Call("16u_good5", (UInt32)0), (UInt32)122);

            assert(test.Call("16s_good1", (UInt32)0), (UInt32)25185);
            assert(test.Call("16s_good2", (UInt32)0), (UInt32)25185);
            assert(test.Call("16s_good3", (UInt32)0), (UInt32)25442);
            assert(test.Call("16s_good4", (UInt32)0), (UInt32)25699);
            assert(test.Call("16s_good5", (UInt32)0), (UInt32)122);

            assert(test.Call("32_good1", (UInt32)0), (UInt32)1684234849);
            assert(test.Call("32_good2", (UInt32)0), (UInt32)1684234849);
            assert(test.Call("32_good3", (UInt32)0), (UInt32)1701077858);
            assert(test.Call("32_good4", (UInt32)0), (UInt32)1717920867);
            assert(test.Call("32_good5", (UInt32)0), (UInt32)122);

            assert(test.Call("8u_good1", (UInt32)65507), (UInt32)0);
            assert(test.Call("8u_good2", (UInt32)65507), (UInt32)0);
            assert(test.Call("8u_good3", (UInt32)65507), (UInt32)0);
            assert(test.Call("8u_good4", (UInt32)65507), (UInt32)0);
            assert(test.Call("8u_good5", (UInt32)65507), (UInt32)0);

            assert(test.Call("8s_good1", (UInt32)65507), (UInt32)0);
            assert(test.Call("8s_good2", (UInt32)65507), (UInt32)0);
            assert(test.Call("8s_good3", (UInt32)65507), (UInt32)0);
            assert(test.Call("8s_good4", (UInt32)65507), (UInt32)0);
            assert(test.Call("8s_good5", (UInt32)65507), (UInt32)0);

            assert(test.Call("16u_good1", (UInt32)65507), (UInt32)0);
            assert(test.Call("16u_good2", (UInt32)65507), (UInt32)0);
            assert(test.Call("16u_good3", (UInt32)65507), (UInt32)0);
            assert(test.Call("16u_good4", (UInt32)65507), (UInt32)0);
            assert(test.Call("16u_good5", (UInt32)65507), (UInt32)0);

            assert(test.Call("16s_good1", (UInt32)65507), (UInt32)0);
            assert(test.Call("16s_good2", (UInt32)65507), (UInt32)0);
            assert(test.Call("16s_good3", (UInt32)65507), (UInt32)0);
            assert(test.Call("16s_good4", (UInt32)65507), (UInt32)0);
            assert(test.Call("16s_good5", (UInt32)65507), (UInt32)0);

            assert(test.Call("32_good1", (UInt32)65507), (UInt32)0);
            assert(test.Call("32_good2", (UInt32)65507), (UInt32)0);
            assert(test.Call("32_good3", (UInt32)65507), (UInt32)0);
            assert(test.Call("32_good4", (UInt32)65507), (UInt32)0);

            //test.Debug = true;
            assert(test.Call("32_good5", (UInt32)65507), (UInt32)0);

            assert(test.Call("8u_good1", (UInt32)65508), (UInt32)0);
            assert(test.Call("8u_good2", (UInt32)65508), (UInt32)0);
            assert(test.Call("8u_good3", (UInt32)65508), (UInt32)0);
            assert(test.Call("8u_good4", (UInt32)65508), (UInt32)0);
            assert(test.Call("8u_good5", (UInt32)65508), (UInt32)0);

            assert(test.Call("8s_good1", (UInt32)65508), (UInt32)0);
            assert(test.Call("8s_good2", (UInt32)65508), (UInt32)0);
            assert(test.Call("8s_good3", (UInt32)65508), (UInt32)0);
            assert(test.Call("8s_good4", (UInt32)65508), (UInt32)0);
            assert(test.Call("8s_good5", (UInt32)65508), (UInt32)0);

            assert(test.Call("16u_good1", (UInt32)65508), (UInt32)0);
            assert(test.Call("16u_good2", (UInt32)65508), (UInt32)0);
            assert(test.Call("16u_good3", (UInt32)65508), (UInt32)0);
            assert(test.Call("16u_good4", (UInt32)65508), (UInt32)0);
            assert(test.Call("16u_good5", (UInt32)65508), (UInt32)0);

            assert(test.Call("16s_good1", (UInt32)65508), (UInt32)0);
            assert(test.Call("16s_good2", (UInt32)65508), (UInt32)0);
            assert(test.Call("16s_good3", (UInt32)65508), (UInt32)0);
            assert(test.Call("16s_good4", (UInt32)65508), (UInt32)0);
            assert(test.Call("16s_good5", (UInt32)65508), (UInt32)0);

            assert(test.Call("32_good1", (UInt32)65508), (UInt32)0);
            assert(test.Call("32_good2", (UInt32)65508), (UInt32)0);
            assert(test.Call("32_good3", (UInt32)65508), (UInt32)0);
            assert(test.Call("32_good4", (UInt32)65508), (UInt32)0);

            assert_trap(delegate () { test.Call("32_good5", (UInt32)65508); }, "out of bounds memory access");

            assert_trap(delegate () { test.CallVoid("8u_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("8s_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("16u_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("16s_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("32_bad", (UInt32)0); }, "out of bounds memory access");

            assert_trap(delegate () { test.CallVoid("8u_bad", (UInt32)1); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("8s_bad", (UInt32)1); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("16u_bad", (UInt32)1); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("16s_bad", (UInt32)1); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("32_bad", (UInt32)1); }, "out of bounds memory access");
        }
    }
}
