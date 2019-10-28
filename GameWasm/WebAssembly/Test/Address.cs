using System;

namespace GameWasm.Webassembly.Test
{
    class Address : Test
    {
        public Address(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "addressi32.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

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
            assert_trap(delegate { test.CallVoid("32_good5", (UInt32)65508); }, "out of bounds memory access");

            assert_trap(delegate { test.CallVoid("8u_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("8s_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("16u_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("16s_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("32_bad", (UInt32)0); }, "out of bounds memory access");

            assert_trap(delegate { test.CallVoid("8u_bad", (UInt32)1); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("8s_bad", (UInt32)1); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("16u_bad", (UInt32)1); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("16s_bad", (UInt32)1); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("32_bad", (UInt32)1); }, "out of bounds memory access");


            filename = "addressi64.wasm";

            store = new Store();
            test = store.LoadModule("test", path + '/' + filename);


            assert64(test.Call("8u_good1", (UInt32)0), (UInt64)97);
            assert64(test.Call("8u_good2", (UInt32)0), (UInt64)97);
            assert64(test.Call("8u_good3", (UInt32)0), (UInt64)98);
            assert64(test.Call("8u_good4", (UInt32)0), (UInt64)99);
            assert64(test.Call("8u_good5", (UInt32)0), (UInt64)122);

            assert64(test.Call("8s_good1", (UInt32)0), (UInt64)97);
            assert64(test.Call("8s_good2", (UInt32)0), (UInt64)97);
            assert64(test.Call("8s_good3", (UInt32)0), (UInt64)98);
            assert64(test.Call("8s_good4", (UInt32)0), (UInt64)99);
            assert64(test.Call("8s_good5", (UInt32)0), (UInt64)122);

            assert64(test.Call("16u_good1", (UInt32)0), (UInt64)25185);
            assert64(test.Call("16u_good2", (UInt32)0), (UInt64)25185);
            assert64(test.Call("16u_good3", (UInt32)0), (UInt64)25442);
            assert64(test.Call("16u_good4", (UInt32)0), (UInt64)25699);
            assert64(test.Call("16u_good5", (UInt32)0), (UInt64)122);

            assert64(test.Call("16s_good1", (UInt32)0), (UInt64)25185);
            assert64(test.Call("16s_good2", (UInt32)0), (UInt64)25185);
            assert64(test.Call("16s_good3", (UInt32)0), (UInt64)25442);
            assert64(test.Call("16s_good4", (UInt32)0), (UInt64)25699);
            assert64(test.Call("16s_good5", (UInt32)0), (UInt64)122);

            assert64(test.Call("32u_good1", (UInt32)0), (UInt64)1684234849);
            assert64(test.Call("32u_good2", (UInt32)0), (UInt64)1684234849);
            assert64(test.Call("32u_good3", (UInt32)0), (UInt64)1701077858);
            assert64(test.Call("32u_good4", (UInt32)0), (UInt64)1717920867);
            assert64(test.Call("32u_good5", (UInt32)0), (UInt64)122);

            assert64(test.Call("32s_good1", (UInt32)0), (UInt64)1684234849);
            assert64(test.Call("32s_good2", (UInt32)0), (UInt64)1684234849);
            assert64(test.Call("32s_good3", (UInt32)0), (UInt64)1701077858);
            assert64(test.Call("32s_good4", (UInt32)0), (UInt64)1717920867);
            assert64(test.Call("32s_good5", (UInt32)0), (UInt64)122);

            assert64(test.Call("64_good1", (UInt32)0), (UInt64)0x6867666564636261);
            assert64(test.Call("64_good2", (UInt32)0), (UInt64)0x6867666564636261);
            assert64(test.Call("64_good3", (UInt32)0), (UInt64)0x6968676665646362);
            assert64(test.Call("64_good4", (UInt32)0), (UInt64)0x6a69686766656463);
            assert64(test.Call("64_good5", (UInt32)0), (UInt64)122);

            assert64(test.Call("8u_good1", (UInt32)65503), (UInt64)0);
            assert64(test.Call("8u_good2", (UInt32)65503), (UInt64)0);
            assert64(test.Call("8u_good3", (UInt32)65503), (UInt64)0);
            assert64(test.Call("8u_good4", (UInt32)65503), (UInt64)0);
            assert64(test.Call("8u_good5", (UInt32)65503), (UInt64)0);

            assert64(test.Call("8s_good1", (UInt32)65503), (UInt64)0);
            assert64(test.Call("8s_good2", (UInt32)65503), (UInt64)0);
            assert64(test.Call("8s_good3", (UInt32)65503), (UInt64)0);
            assert64(test.Call("8s_good4", (UInt32)65503), (UInt64)0);
            assert64(test.Call("8s_good5", (UInt32)65503), (UInt64)0);

            assert64(test.Call("16u_good1", (UInt32)65503), (UInt64)0);
            assert64(test.Call("16u_good2", (UInt32)65503), (UInt64)0);
            assert64(test.Call("16u_good3", (UInt32)65503), (UInt64)0);
            assert64(test.Call("16u_good4", (UInt32)65503), (UInt64)0);
            assert64(test.Call("16u_good5", (UInt32)65503), (UInt64)0);

            assert64(test.Call("16s_good1", (UInt32)65503), (UInt64)0);
            assert64(test.Call("16s_good2", (UInt32)65503), (UInt64)0);
            assert64(test.Call("16s_good3", (UInt32)65503), (UInt64)0);
            assert64(test.Call("16s_good4", (UInt32)65503), (UInt64)0);
            assert64(test.Call("16s_good5", (UInt32)65503), (UInt64)0);

            assert64(test.Call("32u_good1", (UInt32)65503), (UInt64)0);
            assert64(test.Call("32u_good2", (UInt32)65503), (UInt64)0);
            assert64(test.Call("32u_good3", (UInt32)65503), (UInt64)0);
            assert64(test.Call("32u_good4", (UInt32)65503), (UInt64)0);
            assert64(test.Call("32u_good5", (UInt32)65503), (UInt64)0);

            assert64(test.Call("32s_good1", (UInt32)65503), (UInt64)0);
            assert64(test.Call("32s_good2", (UInt32)65503), (UInt64)0);
            assert64(test.Call("32s_good3", (UInt32)65503), (UInt64)0);
            assert64(test.Call("32s_good4", (UInt32)65503), (UInt64)0);
            assert64(test.Call("32s_good5", (UInt32)65503), (UInt64)0);

            assert64(test.Call("64_good1", (UInt32)65503), (UInt64)0);
            assert64(test.Call("64_good2", (UInt32)65503), (UInt64)0);
            assert64(test.Call("64_good3", (UInt32)65503), (UInt64)0);
            assert64(test.Call("64_good4", (UInt32)65503), (UInt64)0);
            assert64(test.Call("64_good5", (UInt32)65503), (UInt64)0);

            assert64(test.Call("8u_good1", (UInt32)65504), (UInt64)0);
            assert64(test.Call("8u_good2", (UInt32)65504), (UInt64)0);
            assert64(test.Call("8u_good3", (UInt32)65504), (UInt64)0);
            assert64(test.Call("8u_good4", (UInt32)65504), (UInt64)0);
            assert64(test.Call("8u_good5", (UInt32)65504), (UInt64)0);

            assert64(test.Call("8s_good1", (UInt32)65504), (UInt64)0);
            assert64(test.Call("8s_good2", (UInt32)65504), (UInt64)0);
            assert64(test.Call("8s_good3", (UInt32)65504), (UInt64)0);
            assert64(test.Call("8s_good4", (UInt32)65504), (UInt64)0);
            assert64(test.Call("8s_good5", (UInt32)65504), (UInt64)0);

            assert64(test.Call("16u_good1", (UInt32)65504), (UInt64)0);
            assert64(test.Call("16u_good2", (UInt32)65504), (UInt64)0);
            assert64(test.Call("16u_good3", (UInt32)65504), (UInt64)0);
            assert64(test.Call("16u_good4", (UInt32)65504), (UInt64)0);
            assert64(test.Call("16u_good5", (UInt32)65504), (UInt64)0);

            assert64(test.Call("16s_good1", (UInt32)65504), (UInt64)0);
            assert64(test.Call("16s_good2", (UInt32)65504), (UInt64)0);
            assert64(test.Call("16s_good3", (UInt32)65504), (UInt64)0);
            assert64(test.Call("16s_good4", (UInt32)65504), (UInt64)0);
            assert64(test.Call("16s_good5", (UInt32)65504), (UInt64)0);

            assert64(test.Call("32u_good1", (UInt32)65504), (UInt64)0);
            assert64(test.Call("32u_good2", (UInt32)65504), (UInt64)0);
            assert64(test.Call("32u_good3", (UInt32)65504), (UInt64)0);
            assert64(test.Call("32u_good4", (UInt32)65504), (UInt64)0);
            assert64(test.Call("32u_good5", (UInt32)65504), (UInt64)0);

            assert64(test.Call("32s_good1", (UInt32)65504), (UInt64)0);
            assert64(test.Call("32s_good2", (UInt32)65504), (UInt64)0);
            assert64(test.Call("32s_good3", (UInt32)65504), (UInt64)0);
            assert64(test.Call("32s_good4", (UInt32)65504), (UInt64)0);
            assert64(test.Call("32s_good5", (UInt32)65504), (UInt64)0);

            assert64(test.Call("64_good1", (UInt32)65504), (UInt64)0);
            assert64(test.Call("64_good2", (UInt32)65504), (UInt64)0);
            assert64(test.Call("64_good3", (UInt32)65504), (UInt64)0);
            assert64(test.Call("64_good4", (UInt32)65504), (UInt64)0);
            assert_trap(delegate { test.CallVoid("64_good5", (UInt32)65504); }, "out of bounds memory access");

            assert_trap(delegate { test.CallVoid("8u_bad", (UInt32) 0); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("8s_bad", (UInt32) 0); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("16u_bad", (UInt32) 0); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("16s_bad", (UInt32) 0); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("32u_bad", (UInt32) 0); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("32s_bad", (UInt32) 0); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("64_bad", (UInt32) 0); }, "out of bounds memory access");

            assert_trap(delegate { test.CallVoid("8u_bad", (UInt32) 1); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("8s_bad", (UInt32) 1); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("16u_bad", (UInt32) 1); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("16s_bad", (UInt32) 1); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("32u_bad", (UInt32) 0); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("32s_bad", (UInt32) 0); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("64_bad", (UInt32) 1); }, "out of bounds memory access");


            filename = "addressf32.wasm";

            store = new Store();
            test = store.LoadModule("test", path + '/' + filename);

            assertF32(test.Call("32_good1", (UInt32) 0), (float) 0.0);
            assertF32(test.Call("32_good2", (UInt32) 0), (float) 0.0);
            assertF32(test.Call("32_good3", (UInt32) 0), (float) 0.0);
            assertF32(test.Call("32_good4", (UInt32) 0), (float) 0.0);
//            assertF32(test.Call("32_good5", (UInt32) 0), (float) nan:0x500001);

            assertF32(test.Call("32_good1", (UInt32) 65524), (float) 0.0);
            assertF32(test.Call("32_good2", (UInt32) 65524), (float) 0.0);
            assertF32(test.Call("32_good3", (UInt32) 65524), (float) 0.0);
            assertF32(test.Call("32_good4", (UInt32) 65524), (float) 0.0);
            assertF32(test.Call("32_good5", (UInt32) 65524), (float) 0.0);

            assertF32(test.Call("32_good1", (UInt32) 65525), (float) 0.0);
            assertF32(test.Call("32_good2", (UInt32) 65525), (float) 0.0);
            assertF32(test.Call("32_good3", (UInt32) 65525), (float) 0.0);
            assertF32(test.Call("32_good4", (UInt32) 65525), (float) 0.0);
            assert_trap(delegate { test.CallVoid("32_good5", (UInt32)65525); }, "out of bounds memory access");

            assert_trap(delegate { test.CallVoid("32_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("32_bad", (UInt32)1); }, "out of bounds memory access");

            filename = "addressf64.wasm";

            store = new Store();
            test = store.LoadModule("test", path + '/' + filename);

            assertF64(test.Call("64_good1", (UInt32)0), (double)0.0);
            assertF64(test.Call("64_good2", (UInt32)0), (double)0.0);
            assertF64(test.Call("64_good3", (UInt32)0), (double)0.0);
            assertF64(test.Call("64_good4", (UInt32)0), (double)0.0);
            //            assertF64(test.Call("64_good5", (UInt32) 0), (double) nan:0xc000000000000001);

            assertF64(test.Call("64_good1", (UInt32)65510), (double)0.0);
            assertF64(test.Call("64_good2", (UInt32)65510), (double)0.0);
            assertF64(test.Call("64_good3", (UInt32)65510), (double)0.0);
            assertF64(test.Call("64_good4", (UInt32)65510), (double)0.0);
            assertF64(test.Call("64_good5", (UInt32)65510), (double)0.0);

            assertF64(test.Call("64_good1", (UInt32)65511), (double)0.0);
            assertF64(test.Call("64_good2", (UInt32)65511), (double)0.0);
            assertF64(test.Call("64_good3", (UInt32)65511), (double)0.0);
            assertF64(test.Call("64_good4", (UInt32)65511), (double)0.0);
            assert_trap(delegate { test.CallVoid("64_good5", (UInt32)65511); }, "out of bounds memory access");

            assert_trap(delegate { test.CallVoid("64_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("64_bad", (UInt32)1); }, "out of bounds memory access");

        }
    }
}
