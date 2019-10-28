using System;

namespace GameWasm.Webassembly.Test
{
    class LoadI64 : Test
    {
        public LoadI64(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "loadi64.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

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
            assert_trap(delegate () { test.Call("64_good5", (UInt32)65504); }, "out of bounds memory access");

            assert_trap(delegate () { test.CallVoid("8u_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("8s_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("16u_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("16s_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("32u_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("32s_bad", (UInt32)0); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("64_bad", (UInt32)0); }, "out of bounds memory access");

            assert_trap(delegate () { test.CallVoid("8u_bad", (UInt32)1); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("8s_bad", (UInt32)1); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("16u_bad", (UInt32)1); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("16s_bad", (UInt32)1); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("32u_bad", (UInt32)1); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("32s_bad", (UInt32)1); }, "out of bounds memory access");
            assert_trap(delegate () { test.CallVoid("64_bad", (UInt32)1); }, "out of bounds memory access");
        }
    }
}
