using System;

namespace GameWasm.Webassembly.Test
{
    class LeftToRight : Test
    {
        public LeftToRight(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "left-to-right.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            test.CallVoid("i32_add", (UInt32) 0x0102);     test.CallVoid("i64_add", (UInt32) 0x0102);
            test.CallVoid("i32_sub", (UInt32) 0x0102);     test.CallVoid("i64_sub", (UInt32) 0x0102);
            test.CallVoid("i32_mul", (UInt32) 0x0102);     test.CallVoid("i64_mul", (UInt32) 0x0102);
            test.CallVoid("i32_div_s", (UInt32) 0x0102);   test.CallVoid("i64_div_s", (UInt32) 0x0102);
            test.CallVoid("i32_div_u", (UInt32) 0x0102);   test.CallVoid("i64_div_u", (UInt32) 0x0102);
            test.CallVoid("i32_rem_s", (UInt32) 0x0102);   test.CallVoid("i64_rem_s", (UInt32) 0x0102);
            test.CallVoid("i32_rem_u", (UInt32) 0x0102);   test.CallVoid("i64_rem_u", (UInt32) 0x0102);
            test.CallVoid("i32_and", (UInt32) 0x0102);     test.CallVoid("i64_and", (UInt32) 0x0102);
            test.CallVoid("i32_or", (UInt32) 0x0102);      test.CallVoid("i64_or", (UInt32) 0x0102);
            test.CallVoid("i32_xor", (UInt32) 0x0102);     test.CallVoid("i64_xor", (UInt32) 0x0102);
            test.CallVoid("i32_shl", (UInt32) 0x0102);     test.CallVoid("i64_shl", (UInt32) 0x0102);
            test.CallVoid("i32_shr_u", (UInt32) 0x0102);   test.CallVoid("i64_shr_u", (UInt32) 0x0102);
            test.CallVoid("i32_shr_s", (UInt32) 0x0102);   test.CallVoid("i64_shr_s", (UInt32) 0x0102);
            test.CallVoid("i32_eq", (UInt32) 0x0102);      test.CallVoid("i64_eq", (UInt32) 0x0102);
            test.CallVoid("i32_ne", (UInt32) 0x0102);      test.CallVoid("i64_ne", (UInt32) 0x0102);
            test.CallVoid("i32_lt_s", (UInt32) 0x0102);    test.CallVoid("i64_lt_s", (UInt32) 0x0102);
            test.CallVoid("i32_le_s", (UInt32) 0x0102);    test.CallVoid("i64_le_s", (UInt32) 0x0102);
            test.CallVoid("i32_lt_u", (UInt32) 0x0102);    test.CallVoid("i64_lt_u", (UInt32) 0x0102);
            test.CallVoid("i32_le_u", (UInt32) 0x0102);    test.CallVoid("i64_le_u", (UInt32) 0x0102);
            test.CallVoid("i32_gt_s", (UInt32) 0x0102);    test.CallVoid("i64_gt_s", (UInt32) 0x0102);
            test.CallVoid("i32_ge_s", (UInt32) 0x0102);    test.CallVoid("i64_ge_s", (UInt32) 0x0102);
            test.CallVoid("i32_gt_u", (UInt32) 0x0102);    test.CallVoid("i64_gt_u", (UInt32) 0x0102);
            test.CallVoid("i32_ge_u", (UInt32) 0x0102);    test.CallVoid("i64_ge_u", (UInt32) 0x0102);
            test.CallVoid("i32_store", (UInt32) 0x0102);   test.CallVoid("i64_store", (UInt32) 0x0102);
            test.CallVoid("i32_store8", (UInt32) 0x0102);  test.CallVoid("i64_store8", (UInt32) 0x0102);
            test.CallVoid("i32_store16", (UInt32) 0x0102); test.CallVoid("i64_store16", (UInt32) 0x0102);
            test.CallVoid("i64_store32", (UInt32) 0x0102);
            test.CallVoid("i32_call", (UInt32) 0x0102);    test.CallVoid("i64_call", (UInt32) 0x0102);
            test.CallVoid("i32_call_indirect", (UInt32) 0x010204);
            test.CallVoid("i64_call_indirect", (UInt32) 0x010204);
            test.CallVoid("i32_select", (UInt32) 0x010205);
            test.CallVoid("i64_select", (UInt32) 0x010205);

            test.CallVoid("f32_add", (UInt32) 0x0102);     test.CallVoid("f64_add", (UInt32) 0x0102);
            test.CallVoid("f32_sub", (UInt32) 0x0102);     test.CallVoid("f64_sub", (UInt32) 0x0102);
            test.CallVoid("f32_mul", (UInt32) 0x0102);     test.CallVoid("f64_mul", (UInt32) 0x0102);
            test.CallVoid("f32_div", (UInt32) 0x0102);     test.CallVoid("f64_div", (UInt32) 0x0102);
            test.CallVoid("f32_copysign", (UInt32) 0x0102);test.CallVoid("f64_copysign", (UInt32) 0x0102);
            test.CallVoid("f32_eq", (UInt32) 0x0102);      test.CallVoid("f64_eq", (UInt32) 0x0102);
            test.CallVoid("f32_ne", (UInt32) 0x0102);      test.CallVoid("f64_ne", (UInt32) 0x0102);
            test.CallVoid("f32_lt", (UInt32) 0x0102);      test.CallVoid("f64_lt", (UInt32) 0x0102);
            test.CallVoid("f32_le", (UInt32) 0x0102);      test.CallVoid("f64_le", (UInt32) 0x0102);
            test.CallVoid("f32_gt", (UInt32) 0x0102);      test.CallVoid("f64_gt", (UInt32) 0x0102);
            test.CallVoid("f32_ge", (UInt32) 0x0102);      test.CallVoid("f64_ge", (UInt32) 0x0102);
            test.CallVoid("f32_min", (UInt32) 0x0102);     test.CallVoid("f64_min", (UInt32) 0x0102);
            test.CallVoid("f32_max", (UInt32) 0x0102);     test.CallVoid("f64_max", (UInt32) 0x0102);
            test.CallVoid("f32_store", (UInt32) 0x0102);   test.CallVoid("f64_store", (UInt32) 0x0102);
            test.CallVoid("f32_call", (UInt32) 0x0102);    test.CallVoid("f64_call", (UInt32) 0x0102);
            test.CallVoid("f32_call_indirect", (UInt32) 0x010204);
            test.CallVoid("f64_call_indirect", (UInt32) 0x010204);
            test.CallVoid("f32_select", (UInt32) 0x010205);
            test.CallVoid("f64_select", (UInt32) 0x010205);

            test.CallVoid("br_if", (UInt32) 0x0102);
            test.CallVoid("br_table", (UInt32) 0x0102);
        }
    }
}
