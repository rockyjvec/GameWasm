using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class Conversions : Test
    {
        public Conversions(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "conversions.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            assert64(test.Call("i64.extend_i32_s", (UInt32) 0), (UInt64) 0);
            assert64(test.Call("i64.extend_i32_s", (UInt32) 10000), (UInt64) 10000);
//            test.Debug = true;
            assert64(test.Call("i64.extend_i32_s", (UInt32) 0xFFFFD8F0), (UInt64) 0xFFFFFFFFFFFFD8F0);
            assert64(test.Call("i64.extend_i32_s", (UInt32) 0xFFFFFFFF), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("i64.extend_i32_s", (UInt32) 0x7fffffff), (UInt64) 0x000000007fffffff);
            assert64(test.Call("i64.extend_i32_s", (UInt32) 0x80000000), (UInt64) 0xffffffff80000000);

            assert64(test.Call("i64.extend_i32_u", (UInt32) 0), (UInt64) 0);
            assert64(test.Call("i64.extend_i32_u", (UInt32) 10000), (UInt64) 10000);
            assert64(test.Call("i64.extend_i32_u", (UInt32) 0xFFFFD8F0), (UInt64) 0x00000000ffffd8f0);
            assert64(test.Call("i64.extend_i32_u", (UInt32) 0xFFFFFFFF), (UInt64) 0xffffffff);
            assert64(test.Call("i64.extend_i32_u", (UInt32) 0x7fffffff), (UInt64) 0x000000007fffffff);
            assert64(test.Call("i64.extend_i32_u", (UInt32) 0x80000000), (UInt64) 0x0000000080000000);

            assert(test.Call("i32.wrap_i64", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 0xFFFFFFFF);
            assert(test.Call("i32.wrap_i64", (UInt64) 0xFFFF7960), (UInt32) 0xFFFF7960);
            assert(test.Call("i32.wrap_i64", (UInt64) 0x80000000), (UInt32) 0x80000000);
            assert(test.Call("i32.wrap_i64", (UInt64) 0xffffffff7fffffff), (UInt32) 0x7fffffff);
            assert(test.Call("i32.wrap_i64", (UInt64) 0xffffffff00000000), (UInt32) 0x00000000);
            assert(test.Call("i32.wrap_i64", (UInt64) 0xfffffffeffffffff), (UInt32) 0xffffffff);
            assert(test.Call("i32.wrap_i64", (UInt64) 0xffffffff00000001), (UInt32) 0x00000001);
            assert(test.Call("i32.wrap_i64", (UInt64) 0), (UInt32) 0);
            assert(test.Call("i32.wrap_i64", (UInt64) 1311768467463790320), (UInt32) 0x9abcdef0);
            assert(test.Call("i32.wrap_i64", (UInt64) 0x00000000ffffffff), (UInt32) 0xffffffff);
            assert(test.Call("i32.wrap_i64", (UInt64) 0x0000000100000000), (UInt32) 0x00000000);
            assert(test.Call("i32.wrap_i64", (UInt64) 0x0000000100000001), (UInt32) 0x00000001);

            assert(test.Call("i32.trunc_f32_s", (float) 0.0), (UInt32) 0);
            assert(test.Call("i32.trunc_f32_s", (float) -0.0), (UInt32) 0);
//            assert(test.Call("i32.trunc_f32_s", (float) 0x1p - 149), (UInt32) 0);
//            assert(test.Call("i32.trunc_f32_s", (float) -0x1p - 149), (UInt32) 0);
            assert(test.Call("i32.trunc_f32_s", (float) 1.0), (UInt32) 1);
//            assert(test.Call("i32.trunc_f32_s", (float) 0x1.19999ap + 0), (UInt32) 1);
            assert(test.Call("i32.trunc_f32_s", (float) 1.5), (UInt32) 1);
            assert(test.Call("i32.trunc_f32_s", (float) -1.0), (UInt32) 0xFFFFFFFF);
            //assert(test.Call("i32.trunc_f32_s", (float) -0x1.19999ap + 0), (UInt32) 0xFFFFFFFF);
            assert(test.Call("i32.trunc_f32_s", (float) -1.5), (UInt32) 0xFFFFFFFF);
            assert(test.Call("i32.trunc_f32_s", (float) -1.9), (UInt32) 0xFFFFFFFF);
            assert(test.Call("i32.trunc_f32_s", (float) -2.0), (UInt32) 0xFFFFFFFE);
            assert(test.Call("i32.trunc_f32_s", (float) 2147483520.0), (UInt32) 2147483520);
            assert(test.Call("i32.trunc_f32_s", (float) 0x80000000), (UInt32) 0x80000000);
//            assert_trap(delegate { test.CallVoid("i32.trunc_f32_s", (float)2147483648.0); }, "integer overflow");
 //           assert_trap(delegate { test.CallVoid("i32.trunc_f32_s", (float)-2147483904.0); }, "integer overflow");
            //assert_trap(delegate { test.CallVoid("i32.trunc_f32_s", (float) inf); "integer overflow")
            //assert_trap(delegate { test.CallVoid("i32.trunc_f32_s", (float) -inf); "integer overflow")
            //assert_trap(delegate { test.CallVoid("i32.trunc_f32_s", (float) nan); "invalid conversion to integer")
            //assert_trap(delegate { test.CallVoid("i32.trunc_f32_s", (float) nan:0x200000); "invalid conversion to integer")
            //assert_trap(delegate { test.CallVoid("i32.trunc_f32_s", (float) -nan); "invalid conversion to integer")
//            assert_trap(delegate { test.CallVoid("i32.trunc_f32_s", (float) -nan:0x200000); "invalid conversion to integer")

            assert(test.Call("i32.trunc_f32_u", (float) 0.0), (UInt32) 0);
            assert(test.Call("i32.trunc_f32_u", (float) -0.0), (UInt32) 0);
//            assert(test.Call("i32.trunc_f32_u", (float) 0x1p - 149), (UInt32) 0);
            //assert(test.Call("i32.trunc_f32_u", (float) -0x1p - 149), (UInt32) 0);
            assert(test.Call("i32.trunc_f32_u", (float) 1.0), (UInt32) 1);
//            assert(test.Call("i32.trunc_f32_u", (float) 0x1.19999ap + 0), (UInt32) 1);
            assert(test.Call("i32.trunc_f32_u", (float) 1.5), (UInt32) 1);
            assert(test.Call("i32.trunc_f32_u", (float) 1.9), (UInt32) 1);
            assert(test.Call("i32.trunc_f32_u", (float) 2.0), (UInt32) 2);
            assert(test.Call("i32.trunc_f32_u", (float) 2147483648), (UInt32)0x80000000); //; ; 0x1.00000p + 31-> 8000 0000
            assert(test.Call("i32.trunc_f32_u", (float) 4294967040.0), (UInt32) 0xFFFFFF00);
            //            assert(test.Call("i32.trunc_f32_u", (float) -0x1.ccccccp - 1), (UInt32) 0);
            //assert(test.Call("i32.trunc_f32_u", (float) -0x1.fffffep - 1), (UInt32) 0);
//            assert_trap(delegate { test.CallVoid("i32.trunc_f32_u", (float)4294967296.0); }, "integer overflow");
//            assert_trap(delegate { test.CallVoid("i32.trunc_f32_u", (float)-1.0); }, "integer overflow");
//            assert_trap(delegate { test.CallVoid("i32.trunc_f32_u", (float) inf); "integer overflow")
            //assert_trap(delegate { test.CallVoid("i32.trunc_f32_u", (float) -inf); "integer overflow")
            //assert_trap(delegate { test.CallVoid("i32.trunc_f32_u", (float) nan); "invalid conversion to integer")
            //assert_trap(delegate { test.CallVoid("i32.trunc_f32_u", (float) nan:0x200000); "invalid conversion to integer")
            //assert_trap(delegate { test.CallVoid("i32.trunc_f32_u", (float) -nan); "invalid conversion to integer")
            //assert_trap(delegate { test.CallVoid("i32.trunc_f32_u", (float) -nan:0x200000); "invalid conversion to integer")

            assert(test.Call("i32.trunc_f64_s", (double) 0.0), (UInt32) 0);
            assert(test.Call("i32.trunc_f64_s", (double) -0.0), (UInt32) 0);
            //assert(test.Call("i32.trunc_f64_s", (double) 0x0.0000000000001p - 1022), (UInt32) 0);
            //assert(test.Call("i32.trunc_f64_s", (double) -0x0.0000000000001p - 1022), (UInt32) 0);
            assert(test.Call("i32.trunc_f64_s", (double) 1.0), (UInt32) 1);
            //assert(test.Call("i32.trunc_f64_s", (double) 0x1.199999999999ap + 0), (UInt32) 1);
            assert(test.Call("i32.trunc_f64_s", (double) 1.5), (UInt32) 1);
            assert(test.Call("i32.trunc_f64_s", (double) -1.0), (UInt32) 0xFFFFFFFF);
            //assert(test.Call("i32.trunc_f64_s", (double) -0x1.199999999999ap + 0), (UInt32) 0xFFFFFFFF);
            assert(test.Call("i32.trunc_f64_s", (double) -1.5), (UInt32) 0xFFFFFFFF);
            assert(test.Call("i32.trunc_f64_s", (double) -1.9), (UInt32) 0xFFFFFFFF);
            assert(test.Call("i32.trunc_f64_s", (double) -2.0), (UInt32) 0xFFFFFFFE);
            assert(test.Call("i32.trunc_f64_s", (double) 2147483647.0), (UInt32) 2147483647);
            assert(test.Call("i32.trunc_f64_s", (double) -2147483648.0), (UInt32) 0x80000000);
//            assert_trap(delegate { test.CallVoid("i32.trunc_f64_s", (double)2147483648.0); }, "integer overflow");
  //          assert_trap(delegate { test.CallVoid("i32.trunc_f64_s", (double)-2147483649.0); }, "integer overflow");
//            assert_trap(delegate { test.CallVoid("i32.trunc_f64_s", (double) inf); "integer overflow")
            //assert_trap(delegate { test.CallVoid("i32.trunc_f64_s", (double) -inf); "integer overflow")
            //assert_trap(delegate { test.CallVoid("i32.trunc_f64_s", (double) nan); "invalid conversion to integer")
            //assert_trap(delegate { test.CallVoid("i32.trunc_f64_s", (double) nan:0x4000000000000); "invalid conversion to integer")
            //assert_trap(delegate { test.CallVoid("i32.trunc_f64_s", (double) -nan); "invalid conversion to integer")
            //assert_trap(delegate { test.CallVoid("i32.trunc_f64_s", (double) -nan:0x4000000000000); "invalid conversion to integer")

            assert(test.Call("i32.trunc_f64_u", (double) 0.0), (UInt32) 0);
            assert(test.Call("i32.trunc_f64_u", (double) -0.0), (UInt32) 0);
//            assert(test.Call("i32.trunc_f64_u", (double) 0x0.0000000000001p - 1022), (UInt32) 0);
            //assert(test.Call("i32.trunc_f64_u", (double) -0x0.0000000000001p - 1022), (UInt32) 0);
            assert(test.Call("i32.trunc_f64_u", (double) 1.0), (UInt32) 1);
            //assert(test.Call("i32.trunc_f64_u", (double) 0x1.199999999999ap + 0), (UInt32) 1);
            assert(test.Call("i32.trunc_f64_u", (double) 1.5), (UInt32) 1);
            assert(test.Call("i32.trunc_f64_u", (double) 1.9), (UInt32) 1);
            assert(test.Call("i32.trunc_f64_u", (double) 2.0), (UInt32) 2);
            assert(test.Call("i32.trunc_f64_u", (double) 2147483648), (UInt32) 0x80000000); //; ; 0x1.00000p + 31-> 8000 0000
            assert(test.Call("i32.trunc_f64_u", (double) 4294967295.0), (UInt32) 0xFFFFFFFF);
//            assert(test.Call("i32.trunc_f64_u", (double) -0x1.ccccccccccccdp - 1), (UInt32) 0);
//            assert(test.Call("i32.trunc_f64_u", (double) -0x1.fffffffffffffp - 1), (UInt32) 0);
            assert(test.Call("i32.trunc_f64_u", (double) 1e8), (UInt32) 100000000);
//            assert_trap(delegate { test.CallVoid("i32.trunc_f64_u", (double)4294967296.0); }, "integer overflow");
//            assert_trap(delegate { test.CallVoid("i32.trunc_f64_u", (double)-1.0); }, "integer overflow");
  //          assert_trap(delegate { test.CallVoid("i32.trunc_f64_u", (double)1e16); }, "integer overflow");
    //        assert_trap(delegate { test.CallVoid("i32.trunc_f64_u", (double)1e30); }, "integer overflow");
      //      assert_trap(delegate { test.CallVoid("i32.trunc_f64_u", (double)9223372036854775808); }, "integer overflow");
//            assert_trap(delegate { test.CallVoid("i32.trunc_f64_u", (double) inf); "integer overflow")
//            assert_trap(delegate { test.CallVoid("i32.trunc_f64_u", (double) -inf); "integer overflow")
            //assert_trap(delegate { test.CallVoid("i32.trunc_f64_u", (double) nan); "invalid conversion to integer")
//            assert_trap(delegate { test.CallVoid("i32.trunc_f64_u", (double) nan:0x4000000000000); "invalid conversion to integer")
//            assert_trap(delegate { test.CallVoid("i32.trunc_f64_u", (double) -nan); "invalid conversion to integer")
//            assert_trap(delegate { test.CallVoid("i32.trunc_f64_u", (double) -nan:0x4000000000000); "invalid conversion to integer")

            assert64(test.Call("i64.trunc_f32_s", (float) 0.0), (UInt64) 0);
            assert64(test.Call("i64.trunc_f32_s", (float) -0.0), (UInt64) 0);
//            assert(test.Call("i64.trunc_f32_s", (float) 0x1p - 149), (UInt64) 0);
            //assert(test.Call("i64.trunc_f32_s", (float) -0x1p - 149), (UInt64) 0);
            assert64(test.Call("i64.trunc_f32_s", (float) 1.0), (UInt64) 1);
//            assert(test.Call("i64.trunc_f32_s", (float) 0x1.19999ap + 0), (UInt64) 1);
            assert64(test.Call("i64.trunc_f32_s", (float) 1.5), (UInt64) 1);
            assert64(test.Call("i64.trunc_f32_s", (float) -1.0), (UInt64) 0xFFFFFFFFFFFFFFFF);
//            assert(test.Call("i64.trunc_f32_s", (float) -0x1.19999ap + 0), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("i64.trunc_f32_s", (float) -1.5), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("i64.trunc_f32_s", (float) -1.9), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("i64.trunc_f32_s", (float) -2.0), (UInt64) 0xFFFFFFFFFFFFFFFE);
            assert64(test.Call("i64.trunc_f32_s", (float) 4294967296), (UInt64) 4294967296);// ; ; 0x1.00000p + 32-> 1 0000 0000
            assert64(test.Call("i64.trunc_f32_s", (float) -4294967296), (UInt64) 0xFFFFFFFF00000000); //; ; -0x1.00000p + 32->ffff ffff 0000 0000
            assert64(test.Call("i64.trunc_f32_s", (float) 9223371487098961920.0), (UInt64) 9223371487098961920);
            
            assert64(test.Call("i64.trunc_f32_s", (float) -9223372036854775808.0), (UInt64)0x8000000000000000);
//            assert_trap(delegate { test.CallVoid("i64.trunc_f32_s", (float)9223372036854775808.0); }, "integer overflow");
            //assert_trap(delegate { test.CallVoid("i64.trunc_f32_s", (float)-9223373136366403584.0); }, "integer overflow");
//            assert_trap(delegate { test.CallVoid("i64.trunc_f32_s", (float) inf); "integer overflow")
            //assert_trap(delegate { test.CallVoid("i64.trunc_f32_s", (float) -inf); "integer overflow")
            //assert_trap(delegate { test.CallVoid("i64.trunc_f32_s", (float) nan); "invalid conversion to integer")
            //assert_trap(delegate { test.CallVoid("i64.trunc_f32_s", (float) nan:0x200000); "invalid conversion to integer")
            //assert_trap(delegate { test.CallVoid("i64.trunc_f32_s", (float) -nan); "invalid conversion to integer")
            //assert_trap(delegate { test.CallVoid("i64.trunc_f32_s", (float) -nan:0x200000); "invalid conversion to integer")

            assert64(test.Call("i64.trunc_f32_u", (float) 0.0), (UInt64) 0);
            assert64(test.Call("i64.trunc_f32_u", (float) -0.0), (UInt64) 0);
            //assert(test.Call("i64.trunc_f32_u", (float) 0x1p - 149), (UInt64) 0);
            //assert(test.Call("i64.trunc_f32_u", (float) -0x1p - 149), (UInt64) 0);
            assert64(test.Call("i64.trunc_f32_u", (float) 1.0), (UInt64) 1);
            //assert(test.Call("i64.trunc_f32_u", (float) 0x1.19999ap + 0), (UInt64) 1);
            assert64(test.Call("i64.trunc_f32_u", (float) 1.5), (UInt64) 1);
            assert64(test.Call("i64.trunc_f32_u", (float) 4294967296), (UInt64) 4294967296);
            assert64(test.Call("i64.trunc_f32_u", (float) 18446742974197923840.0), (UInt64)0xFFFFFF0000000000);
            //assert(test.Call("i64.trunc_f32_u", (float) -0x1.ccccccp - 1), (UInt64) 0);
            //assert(test.Call("i64.trunc_f32_u", (float) -0x1.fffffep - 1), (UInt64) 0);
            //assert_trap(delegate { test.CallVoid("i64.trunc_f32_u", (float)18446744073709551616.0); }, "integer overflow");
            //assert_trap(delegate { test.CallVoid("i64.trunc_f32_u", (float)-1.0); }, "integer overflow");
            //assert_trap(delegate { test.CallVoid("i64.trunc_f32_u", (float) inf); "integer overflow")
            //assert_trap(delegate { test.CallVoid("i64.trunc_f32_u", (float) -inf); "integer overflow")
            //assert_trap(delegate { test.CallVoid("i64.trunc_f32_u", (float) nan); "invalid conversion to integer")
            //assert_trap(delegate { test.CallVoid("i64.trunc_f32_u", (float) nan:0x200000); "invalid conversion to integer")
            //assert_trap(delegate { test.CallVoid("i64.trunc_f32_u", (float) -nan); "invalid conversion to integer")
            //assert_trap(delegate { test.CallVoid("i64.trunc_f32_u", (float) -nan:0x200000); "invalid conversion to integer")

            assert64(test.Call("i64.trunc_f64_s", (double) 0.0), (UInt64) 0);
            assert64(test.Call("i64.trunc_f64_s", (double) -0.0), (UInt64) 0);
//            assert(test.Call("i64.trunc_f64_s", (double) 0x0.0000000000001p - 1022), (UInt64) 0);
            //assert(test.Call("i64.trunc_f64_s", (double) -0x0.0000000000001p - 1022), (UInt64) 0);
            assert64(test.Call("i64.trunc_f64_s", (double) 1.0), (UInt64) 1);
            //assert(test.Call("i64.trunc_f64_s", (double) 0x1.199999999999ap + 0), (UInt64) 1);
            assert64(test.Call("i64.trunc_f64_s", (double) 1.5), (UInt64) 1);
            assert64(test.Call("i64.trunc_f64_s", (double) -1.0), (UInt64) 0xFFFFFFFFFFFFFFFF);
            //assert(test.Call("i64.trunc_f64_s", (double) -0x1.199999999999ap + 0), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("i64.trunc_f64_s", (double) -1.5), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("i64.trunc_f64_s", (double) -1.9), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("i64.trunc_f64_s", (double) -2.0), (UInt64) 0xFFFFFFFFFFFFFFFE);
            assert64(test.Call("i64.trunc_f64_s", (double) 4294967296), (UInt64) 4294967296); //; ; 0x1.00000p + 32-> 1 0000 0000
            assert64(test.Call("i64.trunc_f64_s", (double) -4294967296), (UInt64) 0xFFFFFFFF00000000); //; ; -0x1.00000p + 32->ffff ffff 0000 0000
            assert64(test.Call("i64.trunc_f64_s", (double) 9223372036854774784.0), (UInt64) 9223372036854774784);
            assert64(test.Call("i64.trunc_f64_s", (double) -9223372036854775808.0), (UInt64) 0x8000000000000000);
            //assert_trap(delegate { test.CallVoid("i64.trunc_f64_s", (double)9223372036854775808.0); }, "integer overflow");
            //assert_trap(delegate { test.CallVoid("i64.trunc_f64_s", (double)-9223372036854777856.0); }, "integer overflow");
//            assert_trap(delegate { test.CallVoid("i64.trunc_f64_s", (double) inf); "integer overflow")
//            assert_trap(delegate { test.CallVoid("i64.trunc_f64_s", (double) -inf); "integer overflow")
//            assert_trap(delegate { test.CallVoid("i64.trunc_f64_s", (double) nan); "invalid conversion to integer")
//            assert_trap(delegate { test.CallVoid("i64.trunc_f64_s", (double) nan:0x4000000000000); "invalid conversion to integer")
//            assert_trap(delegate { test.CallVoid("i64.trunc_f64_s", (double) -nan); "invalid conversion to integer")
//            assert_trap(delegate { test.CallVoid("i64.trunc_f64_s", (double) -nan:0x4000000000000); "invalid conversion to integer")

            assert64(test.Call("i64.trunc_f64_u", (double) 0.0), (UInt64) 0);
            assert64(test.Call("i64.trunc_f64_u", (double) -0.0), (UInt64) 0);
//            assert(test.Call("i64.trunc_f64_u", (double) 0x0.0000000000001p - 1022), (UInt64) 0);
//            assert(test.Call("i64.trunc_f64_u", (double) -0x0.0000000000001p - 1022), (UInt64) 0);
            assert64(test.Call("i64.trunc_f64_u", (double) 1.0), (UInt64) 1);
//            assert(test.Call("i64.trunc_f64_u", (double) 0x1.199999999999ap + 0), (UInt64) 1);
            assert64(test.Call("i64.trunc_f64_u", (double) 1.5), (UInt64) 1);
            assert64(test.Call("i64.trunc_f64_u", (double) 4294967295), (UInt64) 0xffffffff);
            assert64(test.Call("i64.trunc_f64_u", (double) 4294967296), (UInt64) 0x100000000);
            assert64(test.Call("i64.trunc_f64_u", (double) 18446744073709549568.0), (UInt64)0xFFFFFFFFFFFFF800);
//            assert(test.Call("i64.trunc_f64_u", (double) -0x1.ccccccccccccdp - 1), (UInt64) 0);
//            assert(test.Call("i64.trunc_f64_u", (double) -0x1.fffffffffffffp - 1), (UInt64) 0);
            assert64(test.Call("i64.trunc_f64_u", (double) 1e8), (UInt64) 100000000);
            assert64(test.Call("i64.trunc_f64_u", (double) 1e16), (UInt64) 10000000000000000);
            assert64(test.Call("i64.trunc_f64_u", (double) 9223372036854775808), (UInt64) 0x8000000000000000);
            //assert_trap(delegate { test.CallVoid("i64.trunc_f64_u", (double)18446744073709551616.0); }, "integer overflow");
            //assert_trap(delegate { test.CallVoid("i64.trunc_f64_u", (double)-1.0); }, "integer overflow");
//            assert_trap(delegate { test.CallVoid("i64.trunc_f64_u", (double) inf); "integer overflow")
//            assert_trap(delegate { test.CallVoid("i64.trunc_f64_u", (double) -inf); "integer overflow")
//            assert_trap(delegate { test.CallVoid("i64.trunc_f64_u", (double) nan); "invalid conversion to integer")
//            assert_trap(delegate { test.CallVoid("i64.trunc_f64_u", (double) nan:0x4000000000000); "invalid conversion to integer")
//            assert_trap(delegate { test.CallVoid("i64.trunc_f64_u", (double) -nan); "invalid conversion to integer")
//            assert_trap(delegate { test.CallVoid("i64.trunc_f64_u", (double) -nan:0x4000000000000); "invalid conversion to integer")

            assertF32(test.Call("f32.convert_i32_s", (UInt32) 1), (float) 1.0);
            assertF32(test.Call("f32.convert_i32_s", (UInt32) 0xFFFFFFFF), (float) -1.0);
            assertF32(test.Call("f32.convert_i32_s", (UInt32) 0), (float) 0.0);
            assertF32(test.Call("f32.convert_i32_s", (UInt32) 2147483647), (float) 2147483648);
            assertF32(test.Call("f32.convert_i32_s", (UInt32) 0x80000000), (float) -2147483648);
//            assertF32(test.Call("f32.convert_i32_s", (UInt32) 1234567890), (float) 0x1.26580cp + 30);
//            ; ; Test rounding directions.
             assertF32(test.Call("f32.convert_i32_s", (UInt32) 16777217), (float) 16777216.0);
            assertF32(test.Call("f32.convert_i32_s", (UInt32) 0xFEFFFFFF), (float) -16777216.0);
            assertF32(test.Call("f32.convert_i32_s", (UInt32) 16777219), (float) 16777220.0);
            assertF32(test.Call("f32.convert_i32_s", (UInt32) 0xFEFFFFFD), (float) -16777220.0);

            assertF32(test.Call("f32.convert_i64_s", (UInt64) 1), (float) 1.0);
            assertF32(test.Call("f32.convert_i64_s", (UInt64) 0xFFFFFFFFFFFFFFFF), (float) -1.0);
            assertF32(test.Call("f32.convert_i64_s", (UInt64) 0), (float) 0.0);
            assertF32(test.Call("f32.convert_i64_s", (UInt64) 9223372036854775807), (float) 9223372036854775807);
            assertF32(test.Call("f32.convert_i64_s", (UInt64) 0x8000000000000000), (float) -9223372036854775808);
//            assert(test.Call("f32.convert_i64_s", (UInt64) 314159265358979), (float) 0x1.1db9e8p + 48); ; ; PI
//             ; ; Test rounding directions.
              assertF32(test.Call("f32.convert_i64_s", (UInt64) 16777217), (float) 16777216.0);
            assertF32(test.Call("f32.convert_i64_s", (UInt64) 0xFFFFFFFFFEFFFFFF), (float) -16777216.0);
            assertF32(test.Call("f32.convert_i64_s", (UInt64) 16777219), (float) 16777220.0);
            assertF32(test.Call("f32.convert_i64_s", (UInt64) 0xFFFFFFFFFEFFFFFD), (float) -16777220.0);

            assertF64(test.Call("f64.convert_i32_s", (UInt32) 1), (double) 1.0);
            assertF64(test.Call("f64.convert_i32_s", (UInt32) 0xFFFFFFFF), (double) -1.0);
            assertF64(test.Call("f64.convert_i32_s", (UInt32) 0), (double) 0.0);
            assertF64(test.Call("f64.convert_i32_s", (UInt32) 2147483647), (double) 2147483647);
            assertF64(test.Call("f64.convert_i32_s", (UInt32) 0x80000000), (double) -2147483648);
            assertF64(test.Call("f64.convert_i32_s", (UInt32) 987654321), (double) 987654321);

            assertF64(test.Call("f64.convert_i64_s", (UInt64) 1), (double) 1.0);
            assertF64(test.Call("f64.convert_i64_s", (UInt64) 0xFFFFFFFFFFFFFFFF), (double) -1.0);
            assertF64(test.Call("f64.convert_i64_s", (UInt64) 0), (double) 0.0);
            assertF64(test.Call("f64.convert_i64_s", (UInt64) 9223372036854775807), (double) 9223372036854775807);
            assertF64(test.Call("f64.convert_i64_s", (UInt64) 0x8000000000000000), (double) -9223372036854775808);
            assertF64(test.Call("f64.convert_i64_s", (UInt64) 4669201609102990), (double) 4669201609102990);// ; ; Feigenbaum
  //          ; ; Test rounding directions.
            assertF64(test.Call("f64.convert_i64_s", (UInt64) 9007199254740993), (double) 9007199254740992);
            assertF64(test.Call("f64.convert_i64_s", (UInt64) 0xFFDFFFFFFFFFFFFF), (double) -9007199254740992);
            assertF64(test.Call("f64.convert_i64_s", (UInt64) 9007199254740995), (double) 9007199254740996);
            assertF64(test.Call("f64.convert_i64_s", (UInt64) 0xFFDFFFFFFFFFFFFD), (double) -9007199254740996);

            assertF32(test.Call("f32.convert_i32_u", (UInt32) 1), (float) 1.0);
            assertF32(test.Call("f32.convert_i32_u", (UInt32) 0), (float) 0.0);
            assertF32(test.Call("f32.convert_i32_u", (UInt32) 2147483647), (float) 2147483648);
            assertF32(test.Call("f32.convert_i32_u", (UInt32) 0x80000000), (float) 2147483648);
//            assertF32(test.Call("f32.convert_i32_u", (UInt32) 0x12345678), (float) 0x1.234568p + 28);
            assertF32(test.Call("f32.convert_i32_u", (UInt32) 0xffffffff), (float) 4294967296.0);
//            assertF32(test.Call("f32.convert_i32_u", (UInt32) 0x80000080), (float) 0x1.000000p + 31);
//            assertF32(test.Call("f32.convert_i32_u", (UInt32) 0x80000081), (float) 0x1.000002p + 31);
//            assertF32(test.Call("f32.convert_i32_u", (UInt32) 0x80000082), (float) 0x1.000002p + 31);
//            assertF32(test.Call("f32.convert_i32_u", (UInt32) 0xfffffe80), (float) 0x1.fffffcp + 31);
//            assertF32(test.Call("f32.convert_i32_u", (UInt32) 0xfffffe81), (float) 0x1.fffffep + 31);
//            assertF32(test.Call("f32.convert_i32_u", (UInt32) 0xfffffe82), (float) 0x1.fffffep + 31);
//            ; ; Test rounding directions.
             assertF32(test.Call("f32.convert_i32_u", (UInt32) 16777217), (float) 16777216.0);
            assertF32(test.Call("f32.convert_i32_u", (UInt32) 16777219), (float) 16777220.0);

            assertF32(test.Call("f32.convert_i64_u", (UInt64) 1), (float) 1.0);
            assertF32(test.Call("f32.convert_i64_u", (UInt64) 0), (float) 0.0);
            assertF32(test.Call("f32.convert_i64_u", (UInt64) 9223372036854775807), (float) 9223372036854775807);
            assertF32(test.Call("f32.convert_i64_u", (UInt64) 0x8000000000000000), (float) 9223372036854775808);
            assertF32(test.Call("f32.convert_i64_u", (UInt64) 0xffffffffffffffff), (float) 18446744073709551616.0);
//            ; ; Test rounding directions.
             assertF32(test.Call("f32.convert_i64_u", (UInt64) 16777217), (float) 16777216.0);
            assertF32(test.Call("f32.convert_i64_u", (UInt64) 16777219), (float) 16777220.0);

            assertF64(test.Call("f64.convert_i32_u", (UInt32) 1), (double) 1.0);
            assertF64(test.Call("f64.convert_i32_u", (UInt32) 0), (double) 0.0);
            assertF64(test.Call("f64.convert_i32_u", (UInt32) 2147483647), (double) 2147483647);
            assertF64(test.Call("f64.convert_i32_u", (UInt32) 0x80000000), (double) 2147483648);
            assertF64(test.Call("f64.convert_i32_u", (UInt32) 0xffffffff), (double) 4294967295.0);

            assertF64(test.Call("f64.convert_i64_u", (UInt64) 1), (double) 1.0);
            assertF64(test.Call("f64.convert_i64_u", (UInt64) 0), (double) 0.0);
            assertF64(test.Call("f64.convert_i64_u", (UInt64) 9223372036854775807), (double) 9223372036854775807);
            assertF64(test.Call("f64.convert_i64_u", (UInt64) 0x8000000000000000), (double) 9223372036854775808);
            assertF64(test.Call("f64.convert_i64_u", (UInt64) 0xffffffffffffffff), (double) 18446744073709551616.0);
            //assertF64(test.Call("f64.convert_i64_u", (UInt64) 0x8000000000000400), (double) 0x1.0000000000000p + 63);
//            assertF64(test.Call("f64.convert_i64_u", (UInt64) 0x8000000000000401), (double) 0x1.0000000000001p + 63);
//            assertF64(test.Call("f64.convert_i64_u", (UInt64) 0x8000000000000402), (double) 0x1.0000000000001p + 63);
//            assertF64(test.Call("f64.convert_i64_u", (UInt64) 0xfffffffffffff400), (double) 0x1.ffffffffffffep + 63);
//            assertF64(test.Call("f64.convert_i64_u", (UInt64) 0xfffffffffffff401), (double) 0x1.fffffffffffffp + 63);
//            assertF64(test.Call("f64.convert_i64_u", (UInt64) 0xfffffffffffff402), (double) 0x1.fffffffffffffp + 63);
            //; ; Test rounding directions.
             assertF64(test.Call("f64.convert_i64_u", (UInt64) 9007199254740993), (double) 9007199254740992);
            assertF64(test.Call("f64.convert_i64_u", (UInt64) 9007199254740995), (double) 9007199254740996);

            assertF64(test.Call("f64.promote_f32", (float) 0.0), (double) 0.0);
            assertF64(test.Call("f64.promote_f32", (float) -0.0), (double) -0.0);
//            assertF64(test.Call("f64.promote_f32", (float) 0x1p - 149), (double) 0x1p - 149);
            //assertF64(test.Call("f64.promote_f32", (float) -0x1p - 149), (double) -0x1p - 149);
            assertF64(test.Call("f64.promote_f32", (float) 1.0), (double) 1.0);
            assertF64(test.Call("f64.promote_f32", (float) -1.0), (double) -1.0);
            //assertF64(test.Call("f64.promote_f32", (float) -0x1.fffffep + 127), (double) -0x1.fffffep + 127);
            //assertF64(test.Call("f64.promote_f32", (float) 0x1.fffffep + 127), (double) 0x1.fffffep + 127);
            //; ; Generated randomly by picking a random int and reinterpret it to float.
             //assertF64(test.Call("f64.promote_f32", (float) 0x1p - 119), (double) 0x1p - 119);
            //; ; Generated randomly by picking a random float.
             //assertF64(test.Call("f64.promote_f32", (float) 0x1.8f867ep + 125), (double) 6.6382536710104395e+37);
            //assertF64(test.Call("f64.promote_f32", (float) inf), (double) inf);
            //assertF64(test.Call("f64.promote_f32", (float) -inf), (double) -inf);
            //(assert_return_canonical_nan(invoke "f64.promote_f32", (float) nan);)
            //(assert_return_arithmetic_nan(invoke "f64.promote_f32", (float) nan:0x200000);)
            //(assert_return_canonical_nan(invoke "f64.promote_f32", (float) -nan);)
            //(assert_return_arithmetic_nan(invoke "f64.promote_f32", (float) -nan:0x200000);)

            assertF32(test.Call("f32.demote_f64", (double) 0.0), (float) 0.0);
            assertF32(test.Call("f32.demote_f64", (double) -0.0), (float) -0.0);
//            assertF32(test.Call("f32.demote_f64", (double) 0x0.0000000000001p - 1022), (float) 0.0);
            //assertF32(test.Call("f32.demote_f64", (double) -0x0.0000000000001p - 1022), (float) -0.0);
            assertF32(test.Call("f32.demote_f64", (double) 1.0), (float) 1.0);
            assertF32(test.Call("f32.demote_f64", (double) -1.0), (float) -1.0);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.fffffe0000000p - 127), (float) 0x1p - 126);
            //assertF32(test.Call("f32.demote_f64", (double) -0x1.fffffe0000000p - 127), (float) -0x1p - 126);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.fffffdfffffffp - 127), (float) 0x1.fffffcp - 127);
            //assertF32(test.Call("f32.demote_f64", (double) -0x1.fffffdfffffffp - 127), (float) -0x1.fffffcp - 127);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1p - 149), (float) 0x1p - 149);
            //assertF32(test.Call("f32.demote_f64", (double) -0x1p - 149), (float) -0x1p - 149);
            ////assertF32(test.Call("f32.demote_f64", (double) 0x1.fffffd0000000p + 127), (float) 0x1.fffffcp + 127);
            //assertF32(test.Call("f32.demote_f64", (double) -0x1.fffffd0000000p + 127), (float) -0x1.fffffcp + 127);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.fffffd0000001p + 127), (float) 0x1.fffffep + 127);
            //assertF32(test.Call("f32.demote_f64", (double) -0x1.fffffd0000001p + 127), (float) -0x1.fffffep + 127);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.fffffep + 127), (float) 0x1.fffffep + 127);
            //assertF32(test.Call("f32.demote_f64", (double) -0x1.fffffep + 127), (float) -0x1.fffffep + 127);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.fffffefffffffp + 127), (float) 0x1.fffffep + 127);
            //assertF32(test.Call("f32.demote_f64", (double) -0x1.fffffefffffffp + 127), (float) -0x1.fffffep + 127);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.ffffffp + 127), (float) inf);
            //assertF32(test.Call("f32.demote_f64", (double) -0x1.ffffffp + 127), (float) -inf);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1p - 119), (float) 0x1p - 119);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.8f867ep + 125), (float) 0x1.8f867ep + 125);
            //assertF32(test.Call("f32.demote_f64", (double) inf), (float) inf);
            //assertF32(test.Call("f32.demote_f64", (double) -inf), (float) -inf);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.0000000000001p + 0), (float) 1.0);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.fffffffffffffp - 1), (float) 1.0);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.0000010000000p + 0), (float) 0x1.000000p + 0);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.0000010000001p + 0), (float) 0x1.000002p + 0);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.000002fffffffp + 0), (float) 0x1.000002p + 0);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.0000030000000p + 0), (float) 0x1.000004p + 0);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.0000050000000p + 0), (float) 0x1.000004p + 0);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.0000010000000p + 24), (float) 0x1.0p + 24);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.0000010000001p + 24), (float) 0x1.000002p + 24);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.000002fffffffp + 24), (float) 0x1.000002p + 24);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.0000030000000p + 24), (float) 0x1.000004p + 24);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.4eae4f7024c7p + 108), (float) 0x1.4eae5p + 108);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.a12e71e358685p - 113), (float) 0x1.a12e72p - 113);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.cb98354d521ffp - 127), (float) 0x1.cb9834p - 127);
            //assertF32(test.Call("f32.demote_f64", (double) -0x1.6972b30cfb562p + 1), (float) -0x1.6972b4p + 1);
            //assertF32(test.Call("f32.demote_f64", (double) -0x1.bedbe4819d4c4p + 112), (float) -0x1.bedbe4p + 112);
            //(assertF32_return_canonical_nan(invoke "f32.demote_f64", (double) nan);)
            //(assertF32_return_arithmetic_nan(invoke "f32.demote_f64", (double) nan:0x4000000000000);)
            //(assertF32_return_canonical_nan(invoke "f32.demote_f64", (double) -nan);)
            //(assertF32_return_arithmetic_nan(invoke "f32.demote_f64", (double) -nan:0x4000000000000);)
            //assertF32(test.Call("f32.demote_f64", (double) 0x1p - 1022), (float) 0.0);
            //assertF32(test.Call("f32.demote_f64", (double) -0x1p - 1022), (float) -0.0);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.0p - 150), (float) 0.0);
            //assertF32(test.Call("f32.demote_f64", (double) -0x1.0p - 150), (float) -0.0);
            //assertF32(test.Call("f32.demote_f64", (double) 0x1.0000000000001p - 150), (float) 0x1p - 149);
            //assertF32(test.Call("f32.demote_f64", (double) -0x1.0000000000001p - 150), (float) -0x1p - 149);

            assertF32(test.Call("f32.reinterpret_i32", (UInt32) 0), (float) 0.0);
            assertF32(test.Call("f32.reinterpret_i32", (UInt32) 0x80000000), (float) -0.0);
//            assertF32(test.Call("f32.reinterpret_i32", (UInt32) 1), (float) 0x1p - 149);
            //assertF32(test.Call("f32.reinterpret_i32", (UInt32) 0xFFFFFFFF), (float) -nan:0x7fffff);
            //assertF32(test.Call("f32.reinterpret_i32", (UInt32) 123456789), (float) 0x1.b79a2ap - 113);
            //assertF32(test.Call("f32.reinterpret_i32", (UInt32) -2147483647), (float) -0x1p - 149);
            //assertF32(test.Call("f32.reinterpret_i32", (UInt32) 0x7f800000), (float) inf);
            //assertF32(test.Call("f32.reinterpret_i32", (UInt32) 0xff800000), (float) -inf);
            //assertF32(test.Call("f32.reinterpret_i32", (UInt32) 0x7fc00000), (float) nan);
            //assertF32(test.Call("f32.reinterpret_i32", (UInt32) 0xffc00000), (float) -nan);
            //assertF32(test.Call("f32.reinterpret_i32", (UInt32) 0x7fa00000), (float) nan:0x200000);
            //assertF32(test.Call("f32.reinterpret_i32", (UInt32) 0xffa00000), (float) -nan:0x200000);

            assertF64(test.Call("f64.reinterpret_i64", (UInt64) 0), (double) 0.0);
            //assertF64(test.Call("f64.reinterpret_i64", (UInt64) 1), (double) 0x0.0000000000001p - 1022);
            //assertF64(test.Call("f64.reinterpret_i64", (UInt64) 0xFFFFFFFFFFFFFFFF), (double) -nan:0xfffffffffffff);
            assertF64(test.Call("f64.reinterpret_i64", (UInt64) 0x8000000000000000), (double) -0.0);
//            assertF64(test.Call("f64.reinterpret_i64", (UInt64) 1234567890), (double) 0x0.00000499602d2p - 1022);
            //assertF64(test.Call("f64.reinterpret_i64", (UInt64) -9223372036854775807), (double) -0x0.0000000000001p - 1022);
            //assertF64(test.Call("f64.reinterpret_i64", (UInt64) 0x7ff0000000000000), (double) inf);
            //assertF64(test.Call("f64.reinterpret_i64", (UInt64) 0xfff0000000000000), (double) -inf);
            //assertF64(test.Call("f64.reinterpret_i64", (UInt64) 0x7ff8000000000000), (double) nan);
            //assertF64(test.Call("f64.reinterpret_i64", (UInt64) 0xfff8000000000000), (double) -nan);
            //assertF64(test.Call("f64.reinterpret_i64", (UInt64) 0x7ff4000000000000), (double) nan:0x4000000000000);
            //assertF64(test.Call("f64.reinterpret_i64", (UInt64) 0xfff4000000000000), (double) -nan:0x4000000000000);

            assert(test.Call("i32.reinterpret_f32", (float) 0.0), (UInt32) 0);
            assert(test.Call("i32.reinterpret_f32", (float) -0.0), (UInt32) 0x80000000);
            //assert(test.Call("i32.reinterpret_f32", (float) 0x1p - 149), (UInt32) 1);
            //assert(test.Call("i32.reinterpret_f32", (float) -nan:0x7fffff), (UInt32) 0xFFFFFFFF);
            //assert(test.Call("i32.reinterpret_f32", (float) -0x1p - 149), (UInt32) 0x80000001);
            assert(test.Call("i32.reinterpret_f32", (float) 1.0), (UInt32) 1065353216);
            assert(test.Call("i32.reinterpret_f32", (float) 3.1415926), (UInt32) 1078530010);
            //assert(test.Call("i32.reinterpret_f32", (float) 0x1.fffffep + 127), (UInt32) 2139095039);
            //assert(test.Call("i32.reinterpret_f32", (float) -0x1.fffffep + 127), (UInt32) -8388609);
            //assert(test.Call("i32.reinterpret_f32", (float) inf), (UInt32) 0x7f800000);
            //assert(test.Call("i32.reinterpret_f32", (float) -inf), (UInt32) 0xff800000);
            //assert(test.Call("i32.reinterpret_f32", (float) nan), (UInt32) 0x7fc00000);
            //assert(test.Call("i32.reinterpret_f32", (float) -nan), (UInt32) 0xffc00000);
            //assert(test.Call("i32.reinterpret_f32", (float) nan:0x200000), (UInt32) 0x7fa00000);
            //assert(test.Call("i32.reinterpret_f32", (float) -nan:0x200000), (UInt32) 0xffa00000);

            assert64(test.Call("i64.reinterpret_f64", (double) 0.0), (UInt64) 0);
            assert64(test.Call("i64.reinterpret_f64", (double) -0.0), (UInt64) 0x8000000000000000);
            //assert64(test.Call("i64.reinterpret_f64", (double) 0x0.0000000000001p - 1022), (UInt64) 1);
            //assert64(test.Call("i64.reinterpret_f64", (double) -nan:0xfffffffffffff), (UInt64) 0xFFFFFFFFFFFFFFFF);
            //assert64(test.Call("i64.reinterpret_f64", (double) -0x0.0000000000001p - 1022), (UInt64) 0x8000000000000001);
            assert64(test.Call("i64.reinterpret_f64", (double) 1.0), (UInt64) 4607182418800017408);
            assert64(test.Call("i64.reinterpret_f64", (double) 3.14159265358979), (UInt64) 4614256656552045841);
//            assert64(test.Call("i64.reinterpret_f64", (double) 0x1.fffffffffffffp + 1023), (UInt64) 9218868437227405311);
            //assert64(test.Call("i64.reinterpret_f64", (double) -0x1.fffffffffffffp + 1023), (UInt64) -4503599627370497);
            //assert64(test.Call("i64.reinterpret_f64", (double) inf), (UInt64) 0x7ff0000000000000);
            //assert64(test.Call("i64.reinterpret_f64", (double) -inf), (UInt64) 0xfff0000000000000);
            //assert64(test.Call("i64.reinterpret_f64", (double) nan), (UInt64) 0x7ff8000000000000);
            //assert64(test.Call("i64.reinterpret_f64", (double) -nan), (UInt64) 0xfff8000000000000);
            //assert64(test.Call("i64.reinterpret_f64", (double) nan:0x4000000000000), (UInt64) 0x7ff4000000000000);
            //assert64(test.Call("i64.reinterpret_f64", (double) -nan:0x4000000000000), (UInt64) 0xfff4000000000000);*/
        }
    }
}
