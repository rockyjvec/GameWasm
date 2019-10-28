using System;

namespace GameWasm.Webassembly.Test
{
    class I64 : Test
    {
        public I64(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "i64.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            assert64(test.Call("add", (UInt64) 1, (UInt64) 1), (UInt64) 2);
            assert64(test.Call("add", (UInt64) 1, (UInt64) 0), (UInt64) 1);
            assert64(test.Call("add", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0xFFFFFFFFFFFFFFFE);
            assert64(test.Call("add", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 1), (UInt64) 0);
            assert64(test.Call("add", (UInt64) 0x7fffffffffffffff, (UInt64) 1), (UInt64) 0x8000000000000000);
            assert64(test.Call("add", (UInt64) 0x8000000000000000, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0x7fffffffffffffff);
            assert64(test.Call("add", (UInt64) 0x8000000000000000, (UInt64) 0x8000000000000000), (UInt64) 0);
            assert64(test.Call("add", (UInt64) 0x3fffffff, (UInt64) 1), (UInt64) 0x40000000);

            assert64(test.Call("sub", (UInt64) 1, (UInt64) 1), (UInt64) 0);
            assert64(test.Call("sub", (UInt64) 1, (UInt64) 0), (UInt64) 1);
            assert64(test.Call("sub", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0);
            assert64(test.Call("sub", (UInt64) 0x7fffffffffffffff, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0x8000000000000000);
            assert64(test.Call("sub", (UInt64) 0x8000000000000000, (UInt64) 1), (UInt64) 0x7fffffffffffffff);
            assert64(test.Call("sub", (UInt64) 0x8000000000000000, (UInt64) 0x8000000000000000), (UInt64) 0);
            assert64(test.Call("sub", (UInt64) 0x3fffffff, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0x40000000);

            assert64(test.Call("mul", (UInt64) 1, (UInt64) 1), (UInt64) 1);
            assert64(test.Call("mul", (UInt64) 1, (UInt64) 0), (UInt64) 0);
            assert64(test.Call("mul", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 1);
            assert64(test.Call("mul", (UInt64) 0x1000000000000000, (UInt64) 4096), (UInt64) 0);
            assert64(test.Call("mul", (UInt64) 0x8000000000000000, (UInt64) 0), (UInt64) 0);
            assert64(test.Call("mul", (UInt64) 0x8000000000000000, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0x8000000000000000);
            assert64(test.Call("mul", (UInt64) 0x7fffffffffffffff, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0x8000000000000001);
            assert64(test.Call("mul", (UInt64) 0x0123456789abcdef, (UInt64) 0xfedcba9876543210), (UInt64) 0x2236d88fe5618cf0);
            assert64(test.Call("mul", (UInt64) 0x7fffffffffffffff, (UInt64) 0x7fffffffffffffff), (UInt64) 1);

            assert_trap(delegate { test.CallVoid("div_s", (UInt64)1, (UInt64)0); }, "integer divide by zero");
            assert_trap(delegate { test.CallVoid("div_s", (UInt64)0, (UInt64)0); }, "integer divide by zero");
            assert_trap(delegate { test.CallVoid("div_s", (UInt64)0x8000000000000000, (UInt64)0xFFFFFFFFFFFFFFFF); }, "integer overflow");
            assert64(test.Call("div_s", (UInt64) 1, (UInt64) 1), (UInt64) 1);
            assert64(test.Call("div_s", (UInt64) 0, (UInt64) 1), (UInt64) 0);
            assert64(test.Call("div_s", (UInt64) 0, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0);
            assert64(test.Call("div_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 1);
            assert64(test.Call("div_s", (UInt64) 0x8000000000000000, (UInt64) 2), (UInt64) 0xc000000000000000);
            assert64(test.Call("div_s", (UInt64) 0x8000000000000001, (UInt64) 1000), (UInt64) 0xffdf3b645a1cac09);
            assert64(test.Call("div_s", (UInt64) 5, (UInt64) 2), (UInt64) 2);
            assert64(test.Call("div_s", (UInt64) 0xFFFFFFFFFFFFFFFB, (UInt64) 2), (UInt64) 0xFFFFFFFFFFFFFFFE);
            assert64(test.Call("div_s", (UInt64) 5, (UInt64) 0xFFFFFFFFFFFFFFFE), (UInt64) 0xFFFFFFFFFFFFFFFE);
            assert64(test.Call("div_s", (UInt64) 0xFFFFFFFFFFFFFFFB, (UInt64) 0xFFFFFFFFFFFFFFFE), (UInt64) 2);
            assert64(test.Call("div_s", (UInt64) 7, (UInt64) 3), (UInt64) 2);
            assert64(test.Call("div_s", (UInt64) 0xFFFFFFFFFFFFFFF9, (UInt64) 3), (UInt64) 0xFFFFFFFFFFFFFFFE);
            assert64(test.Call("div_s", (UInt64) 7, (UInt64) 0xFFFFFFFFFFFFFFFD), (UInt64) 0xFFFFFFFFFFFFFFFE);
            assert64(test.Call("div_s", (UInt64) 0xFFFFFFFFFFFFFFF9, (UInt64) 0xFFFFFFFFFFFFFFFD), (UInt64) 2);
            assert64(test.Call("div_s", (UInt64) 11, (UInt64) 5), (UInt64) 2);
            assert64(test.Call("div_s", (UInt64) 17, (UInt64) 7), (UInt64) 2);

            assert_trap(delegate { test.CallVoid("div_u", (UInt64)1, (UInt64)0); }, "integer divide by zero");
            assert_trap(delegate { test.CallVoid("div_u", (UInt64)0, (UInt64)0); }, "integer divide by zero");
            assert64(test.Call("div_u", (UInt64) 1, (UInt64) 1), (UInt64) 1);
            assert64(test.Call("div_u", (UInt64) 0, (UInt64) 1), (UInt64) 0);
            assert64(test.Call("div_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 1);
            assert64(test.Call("div_u", (UInt64) 0x8000000000000000, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0);
            assert64(test.Call("div_u", (UInt64) 0x8000000000000000, (UInt64) 2), (UInt64) 0x4000000000000000);
            assert64(test.Call("div_u", (UInt64) 0x8ff00ff00ff00ff0, (UInt64) 0x100000001), (UInt64) 0x8ff00fef);
            assert64(test.Call("div_u", (UInt64) 0x8000000000000001, (UInt64) 1000), (UInt64) 0x20c49ba5e353f7);
            assert64(test.Call("div_u", (UInt64) 5, (UInt64) 2), (UInt64) 2);
            assert64(test.Call("div_u", (UInt64) 0xFFFFFFFFFFFFFFFB, (UInt64) 2), (UInt64) 0x7ffffffffffffffd);
            assert64(test.Call("div_u", (UInt64) 5, (UInt64) 0xFFFFFFFFFFFFFFFE), (UInt64) 0);
            assert64(test.Call("div_u", (UInt64) 0xFFFFFFFFFFFFFFFB, (UInt64) 0xFFFFFFFFFFFFFFFE), (UInt64) 0);
            assert64(test.Call("div_u", (UInt64) 7, (UInt64) 3), (UInt64) 2);
            assert64(test.Call("div_u", (UInt64) 11, (UInt64) 5), (UInt64) 2);
            assert64(test.Call("div_u", (UInt64) 17, (UInt64) 7), (UInt64) 2);

            assert_trap(delegate { test.CallVoid("rem_s", (UInt64)1, (UInt64)0); }, "integer divide by zero");
            assert_trap(delegate { test.CallVoid("rem_s", (UInt64)0, (UInt64)0); }, "integer divide by zero");
            assert64(test.Call("rem_s", (UInt64) 0x7fffffffffffffff, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0);
            assert64(test.Call("rem_s", (UInt64) 1, (UInt64) 1), (UInt64) 0);
            assert64(test.Call("rem_s", (UInt64) 0, (UInt64) 1), (UInt64) 0);
            assert64(test.Call("rem_s", (UInt64) 0, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0);
            assert64(test.Call("rem_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0);
            assert64(test.Call("rem_s", (UInt64) 0x8000000000000000, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0);
            assert64(test.Call("rem_s", (UInt64) 0x8000000000000000, (UInt64) 2), (UInt64) 0);
            assert64(test.Call("rem_s", (UInt64) 0x8000000000000001, (UInt64) 1000), (UInt64)0xFFFFFFFFFFFFFCD9);
            assert64(test.Call("rem_s", (UInt64) 5, (UInt64) 2), (UInt64) 1);
            assert64(test.Call("rem_s", (UInt64) 0xFFFFFFFFFFFFFFFB, (UInt64) 2), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("rem_s", (UInt64) 5, (UInt64) 0xFFFFFFFFFFFFFFFE), (UInt64) 1);
            assert64(test.Call("rem_s", (UInt64) 0xFFFFFFFFFFFFFFFB, (UInt64) 0xFFFFFFFFFFFFFFFE), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("rem_s", (UInt64) 7, (UInt64) 3), (UInt64) 1);
            assert64(test.Call("rem_s", (UInt64) 0xFFFFFFFFFFFFFFF9, (UInt64) 3), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("rem_s", (UInt64) 7, (UInt64) 0xFFFFFFFFFFFFFFFD), (UInt64) 1);
            assert64(test.Call("rem_s", (UInt64) 0xFFFFFFFFFFFFFFF9, (UInt64) 0xFFFFFFFFFFFFFFFD), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("rem_s", (UInt64) 11, (UInt64) 5), (UInt64) 1);
            assert64(test.Call("rem_s", (UInt64) 17, (UInt64) 7), (UInt64) 3);

            assert_trap(delegate { test.CallVoid("rem_u", (UInt64)1, (UInt64)0); }, "integer divide by zero");
            assert_trap(delegate { test.CallVoid("rem_u", (UInt64)0, (UInt64)0); }, "integer divide by zero");
            assert64(test.Call("rem_u", (UInt64) 1, (UInt64) 1), (UInt64) 0);
            assert64(test.Call("rem_u", (UInt64) 0, (UInt64) 1), (UInt64) 0);
            assert64(test.Call("rem_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0);
            assert64(test.Call("rem_u", (UInt64) 0x8000000000000000, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0x8000000000000000);
            assert64(test.Call("rem_u", (UInt64) 0x8000000000000000, (UInt64) 2), (UInt64) 0);
            assert64(test.Call("rem_u", (UInt64) 0x8ff00ff00ff00ff0, (UInt64) 0x100000001), (UInt64) 0x80000001);
            assert64(test.Call("rem_u", (UInt64) 0x8000000000000001, (UInt64) 1000), (UInt64) 809);
            assert64(test.Call("rem_u", (UInt64) 5, (UInt64) 2), (UInt64) 1);
            assert64(test.Call("rem_u", (UInt64) 0xFFFFFFFFFFFFFFFB, (UInt64) 2), (UInt64) 1);
            assert64(test.Call("rem_u", (UInt64) 5, (UInt64) 0xFFFFFFFFFFFFFFFE), (UInt64) 5);
            assert64(test.Call("rem_u", (UInt64) 0xFFFFFFFFFFFFFFFB, (UInt64) 0xFFFFFFFFFFFFFFFE), (UInt64) 0xFFFFFFFFFFFFFFFB);
            assert64(test.Call("rem_u", (UInt64) 7, (UInt64) 3), (UInt64) 1);
            assert64(test.Call("rem_u", (UInt64) 11, (UInt64) 5), (UInt64) 1);
            assert64(test.Call("rem_u", (UInt64) 17, (UInt64) 7), (UInt64) 3);

            assert64(test.Call("and", (UInt64) 1, (UInt64) 0), (UInt64) 0);
            assert64(test.Call("and", (UInt64) 0, (UInt64) 1), (UInt64) 0);
            assert64(test.Call("and", (UInt64) 1, (UInt64) 1), (UInt64) 1);
            assert64(test.Call("and", (UInt64) 0, (UInt64) 0), (UInt64) 0);
            assert64(test.Call("and", (UInt64) 0x7fffffffffffffff, (UInt64) 0x8000000000000000), (UInt64) 0);
            assert64(test.Call("and", (UInt64) 0x7fffffffffffffff, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0x7fffffffffffffff);
            assert64(test.Call("and", (UInt64) 0xf0f0ffff, (UInt64) 0xfffff0f0), (UInt64) 0xf0f0f0f0);
            assert64(test.Call("and", (UInt64) 0xffffffffffffffff, (UInt64) 0xffffffffffffffff), (UInt64) 0xffffffffffffffff);

            assert64(test.Call("or", (UInt64) 1, (UInt64) 0), (UInt64) 1);
            assert64(test.Call("or", (UInt64) 0, (UInt64) 1), (UInt64) 1);
            assert64(test.Call("or", (UInt64) 1, (UInt64) 1), (UInt64) 1);
            assert64(test.Call("or", (UInt64) 0, (UInt64) 0), (UInt64) 0);
            assert64(test.Call("or", (UInt64) 0x7fffffffffffffff, (UInt64) 0x8000000000000000), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("or", (UInt64) 0x8000000000000000, (UInt64) 0), (UInt64) 0x8000000000000000);
            assert64(test.Call("or", (UInt64) 0xf0f0ffff, (UInt64) 0xfffff0f0), (UInt64) 0xffffffff);
            assert64(test.Call("or", (UInt64) 0xffffffffffffffff, (UInt64) 0xffffffffffffffff), (UInt64) 0xffffffffffffffff);

            assert64(test.Call("xor", (UInt64) 1, (UInt64) 0), (UInt64) 1);
            assert64(test.Call("xor", (UInt64) 0, (UInt64) 1), (UInt64) 1);
            assert64(test.Call("xor", (UInt64) 1, (UInt64) 1), (UInt64) 0);
            assert64(test.Call("xor", (UInt64) 0, (UInt64) 0), (UInt64) 0);
            assert64(test.Call("xor", (UInt64) 0x7fffffffffffffff, (UInt64) 0x8000000000000000), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("xor", (UInt64) 0x8000000000000000, (UInt64) 0), (UInt64) 0x8000000000000000);
            assert64(test.Call("xor", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x8000000000000000), (UInt64) 0x7fffffffffffffff);
            assert64(test.Call("xor", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x7fffffffffffffff), (UInt64) 0x8000000000000000);
            assert64(test.Call("xor", (UInt64) 0xf0f0ffff, (UInt64) 0xfffff0f0), (UInt64) 0x0f0f0f0f);
            assert64(test.Call("xor", (UInt64) 0xffffffffffffffff, (UInt64) 0xffffffffffffffff), (UInt64) 0);

            assert64(test.Call("shl", (UInt64) 1, (UInt64) 1), (UInt64) 2);
            assert64(test.Call("shl", (UInt64) 1, (UInt64) 0), (UInt64) 1);
            assert64(test.Call("shl", (UInt64) 0x7fffffffffffffff, (UInt64) 1), (UInt64) 0xfffffffffffffffe);
            assert64(test.Call("shl", (UInt64) 0xffffffffffffffff, (UInt64) 1), (UInt64) 0xfffffffffffffffe);
            assert64(test.Call("shl", (UInt64) 0x8000000000000000, (UInt64) 1), (UInt64) 0);
            assert64(test.Call("shl", (UInt64) 0x4000000000000000, (UInt64) 1), (UInt64) 0x8000000000000000);
            assert64(test.Call("shl", (UInt64) 1, (UInt64) 63), (UInt64) 0x8000000000000000);
            assert64(test.Call("shl", (UInt64) 1, (UInt64) 64), (UInt64) 1);
            assert64(test.Call("shl", (UInt64) 1, (UInt64) 65), (UInt64) 2);
            assert64(test.Call("shl", (UInt64) 1, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0x8000000000000000);
            assert64(test.Call("shl", (UInt64) 1, (UInt64) 0x7fffffffffffffff), (UInt64) 0x8000000000000000);

            assert64(test.Call("shr_s", (UInt64) 1, (UInt64) 1), (UInt64) 0);
            assert64(test.Call("shr_s", (UInt64) 1, (UInt64) 0), (UInt64) 1);
            assert64(test.Call("shr_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 1), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("shr_s", (UInt64) 0x7fffffffffffffff, (UInt64) 1), (UInt64) 0x3fffffffffffffff);
            assert64(test.Call("shr_s", (UInt64) 0x8000000000000000, (UInt64) 1), (UInt64) 0xc000000000000000);
            assert64(test.Call("shr_s", (UInt64) 0x4000000000000000, (UInt64) 1), (UInt64) 0x2000000000000000);
            assert64(test.Call("shr_s", (UInt64) 1, (UInt64) 64), (UInt64) 1);
            assert64(test.Call("shr_s", (UInt64) 1, (UInt64) 65), (UInt64) 0);
            assert64(test.Call("shr_s", (UInt64) 1, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0);
            assert64(test.Call("shr_s", (UInt64) 1, (UInt64) 0x7fffffffffffffff), (UInt64) 0);
            assert64(test.Call("shr_s", (UInt64) 1, (UInt64) 0x8000000000000000), (UInt64) 1);
            assert64(test.Call("shr_s", (UInt64) 0x8000000000000000, (UInt64) 63), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("shr_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 64), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("shr_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 65), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("shr_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("shr_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x7fffffffffffffff), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("shr_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x8000000000000000), (UInt64) 0xFFFFFFFFFFFFFFFF);

            assert64(test.Call("shr_u", (UInt64) 1, (UInt64) 1), (UInt64) 0);
            assert64(test.Call("shr_u", (UInt64) 1, (UInt64) 0), (UInt64) 1);
            assert64(test.Call("shr_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 1), (UInt64) 0x7fffffffffffffff);
            assert64(test.Call("shr_u", (UInt64) 0x7fffffffffffffff, (UInt64) 1), (UInt64) 0x3fffffffffffffff);
            assert64(test.Call("shr_u", (UInt64) 0x8000000000000000, (UInt64) 1), (UInt64) 0x4000000000000000);
            assert64(test.Call("shr_u", (UInt64) 0x4000000000000000, (UInt64) 1), (UInt64) 0x2000000000000000);
            assert64(test.Call("shr_u", (UInt64) 1, (UInt64) 64), (UInt64) 1);
            assert64(test.Call("shr_u", (UInt64) 1, (UInt64) 65), (UInt64) 0);
            assert64(test.Call("shr_u", (UInt64) 1, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0);
            assert64(test.Call("shr_u", (UInt64) 1, (UInt64) 0x7fffffffffffffff), (UInt64) 0);
            assert64(test.Call("shr_u", (UInt64) 1, (UInt64) 0x8000000000000000), (UInt64) 1);
            assert64(test.Call("shr_u", (UInt64) 0x8000000000000000, (UInt64) 63), (UInt64) 1);
            assert64(test.Call("shr_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 64), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("shr_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 65), (UInt64) 0x7fffffffffffffff);
            assert64(test.Call("shr_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 1);
            assert64(test.Call("shr_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x7fffffffffffffff), (UInt64) 1);
            assert64(test.Call("shr_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x8000000000000000), (UInt64) 0xFFFFFFFFFFFFFFFF);

            assert64(test.Call("rotl", (UInt64) 1, (UInt64) 1), (UInt64) 2);
            assert64(test.Call("rotl", (UInt64) 1, (UInt64) 0), (UInt64) 1);
            assert64(test.Call("rotl", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 1), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("rotl", (UInt64) 1, (UInt64) 64), (UInt64) 1);
            assert64(test.Call("rotl", (UInt64) 0xabcd987602468ace, (UInt64) 1), (UInt64) 0x579b30ec048d159d);
            assert64(test.Call("rotl", (UInt64) 0xfe000000dc000000, (UInt64) 4), (UInt64) 0xe000000dc000000f);
            assert64(test.Call("rotl", (UInt64) 0xabcd1234ef567809, (UInt64) 53), (UInt64) 0x013579a2469deacf);
            assert64(test.Call("rotl", (UInt64) 0xabd1234ef567809c, (UInt64) 63), (UInt64) 0x55e891a77ab3c04e);
            assert64(test.Call("rotl", (UInt64) 0xabcd1234ef567809, (UInt64) 0xf5), (UInt64) 0x013579a2469deacf);
            assert64(test.Call("rotl", (UInt64) 0xabcd7294ef567809, (UInt64) 0xffffffffffffffed), (UInt64) 0xcf013579ae529dea);
            assert64(test.Call("rotl", (UInt64) 0xabd1234ef567809c, (UInt64) 0x800000000000003f), (UInt64) 0x55e891a77ab3c04e);
            assert64(test.Call("rotl", (UInt64) 1, (UInt64) 63), (UInt64) 0x8000000000000000);
            assert64(test.Call("rotl", (UInt64) 0x8000000000000000, (UInt64) 1), (UInt64) 1);

            assert64(test.Call("rotr", (UInt64) 1, (UInt64) 1), (UInt64) 0x8000000000000000);
            assert64(test.Call("rotr", (UInt64) 1, (UInt64) 0), (UInt64) 1);
            assert64(test.Call("rotr", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 1), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("rotr", (UInt64) 1, (UInt64) 64), (UInt64) 1);
            assert64(test.Call("rotr", (UInt64) 0xabcd987602468ace, (UInt64) 1), (UInt64) 0x55e6cc3b01234567);
            assert64(test.Call("rotr", (UInt64) 0xfe000000dc000000, (UInt64) 4), (UInt64) 0x0fe000000dc00000);
            assert64(test.Call("rotr", (UInt64) 0xabcd1234ef567809, (UInt64) 53), (UInt64) 0x6891a77ab3c04d5e);
            assert64(test.Call("rotr", (UInt64) 0xabd1234ef567809c, (UInt64) 63), (UInt64) 0x57a2469deacf0139);
            assert64(test.Call("rotr", (UInt64) 0xabcd1234ef567809, (UInt64) 0xf5), (UInt64) 0x6891a77ab3c04d5e);
            assert64(test.Call("rotr", (UInt64) 0xabcd7294ef567809, (UInt64) 0xffffffffffffffed), (UInt64) 0x94a77ab3c04d5e6b);
            assert64(test.Call("rotr", (UInt64) 0xabd1234ef567809c, (UInt64) 0x800000000000003f), (UInt64) 0x57a2469deacf0139);
            assert64(test.Call("rotr", (UInt64) 1, (UInt64) 63), (UInt64) 2);
            assert64(test.Call("rotr", (UInt64) 0x8000000000000000, (UInt64) 63), (UInt64) 1);

            assert64(test.Call("clz", (UInt64) 0xffffffffffffffff), (UInt64) 0);
            assert64(test.Call("clz", (UInt64) 0), (UInt64) 64);
            assert64(test.Call("clz", (UInt64) 0x00008000), (UInt64) 48);
            assert64(test.Call("clz", (UInt64) 0xff), (UInt64) 56);
            assert64(test.Call("clz", (UInt64) 0x8000000000000000), (UInt64) 0);
            assert64(test.Call("clz", (UInt64) 1), (UInt64) 63);
            assert64(test.Call("clz", (UInt64) 2), (UInt64) 62);
            assert64(test.Call("clz", (UInt64) 0x7fffffffffffffff), (UInt64) 1);

            assert64(test.Call("ctz", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0);
            assert64(test.Call("ctz", (UInt64) 0), (UInt64) 64);
            assert64(test.Call("ctz", (UInt64) 0x00008000), (UInt64) 15);
            assert64(test.Call("ctz", (UInt64) 0x00010000), (UInt64) 16);
            assert64(test.Call("ctz", (UInt64) 0x8000000000000000), (UInt64) 63);
            assert64(test.Call("ctz", (UInt64) 0x7fffffffffffffff), (UInt64) 0);

            assert64(test.Call("popcnt", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 64);
            assert64(test.Call("popcnt", (UInt64) 0), (UInt64) 0);
            assert64(test.Call("popcnt", (UInt64) 0x00008000), (UInt64) 1);
            assert64(test.Call("popcnt", (UInt64) 0x8000800080008000), (UInt64) 4);
            assert64(test.Call("popcnt", (UInt64) 0x7fffffffffffffff), (UInt64) 63);
            assert64(test.Call("popcnt", (UInt64) 0xAAAAAAAA55555555), (UInt64) 32);
            assert64(test.Call("popcnt", (UInt64) 0x99999999AAAAAAAA), (UInt64) 32);
            assert64(test.Call("popcnt", (UInt64) 0xDEADBEEFDEADBEEF), (UInt64) 48);

            assert(test.Call("eqz", (UInt64) 0), (UInt32) 1);
            assert(test.Call("eqz", (UInt64) 1), (UInt32) 0);
            assert(test.Call("eqz", (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("eqz", (UInt64) 0x7fffffffffffffff), (UInt32) 0);
            assert(test.Call("eqz", (UInt64) 0xffffffffffffffff), (UInt32) 0);

            assert(test.Call("eq", (UInt64) 0, (UInt64) 0), (UInt32) 1);
            assert(test.Call("eq", (UInt64) 1, (UInt64) 1), (UInt32) 1);
            assert(test.Call("eq", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 1), (UInt32) 0);
            assert(test.Call("eq", (UInt64) 0x8000000000000000, (UInt64) 0x8000000000000000), (UInt32) 1);
            assert(test.Call("eq", (UInt64) 0x7fffffffffffffff, (UInt64) 0x7fffffffffffffff), (UInt32) 1);
            assert(test.Call("eq", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 1);
            assert(test.Call("eq", (UInt64) 1, (UInt64) 0), (UInt32) 0);
            assert(test.Call("eq", (UInt64) 0, (UInt64) 1), (UInt32) 0);
            assert(test.Call("eq", (UInt64) 0x8000000000000000, (UInt64) 0), (UInt32) 0);
            assert(test.Call("eq", (UInt64) 0, (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("eq", (UInt64) 0x8000000000000000, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 0);
            assert(test.Call("eq", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("eq", (UInt64) 0x8000000000000000, (UInt64) 0x7fffffffffffffff), (UInt32) 0);
            assert(test.Call("eq", (UInt64) 0x7fffffffffffffff, (UInt64) 0x8000000000000000), (UInt32) 0);

            assert(test.Call("ne", (UInt64) 0, (UInt64) 0), (UInt32) 0);
            assert(test.Call("ne", (UInt64) 1, (UInt64) 1), (UInt32) 0);
            assert(test.Call("ne", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 1), (UInt32) 1);
            assert(test.Call("ne", (UInt64) 0x8000000000000000, (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("ne", (UInt64) 0x7fffffffffffffff, (UInt64) 0x7fffffffffffffff), (UInt32) 0);
            assert(test.Call("ne", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 0);
            assert(test.Call("ne", (UInt64) 1, (UInt64) 0), (UInt32) 1);
            assert(test.Call("ne", (UInt64) 0, (UInt64) 1), (UInt32) 1);
            assert(test.Call("ne", (UInt64) 0x8000000000000000, (UInt64) 0), (UInt32) 1);
            assert(test.Call("ne", (UInt64) 0, (UInt64) 0x8000000000000000), (UInt32) 1);
            assert(test.Call("ne", (UInt64) 0x8000000000000000, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 1);
            assert(test.Call("ne", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x8000000000000000), (UInt32) 1);
            assert(test.Call("ne", (UInt64) 0x8000000000000000, (UInt64) 0x7fffffffffffffff), (UInt32) 1);
            assert(test.Call("ne", (UInt64) 0x7fffffffffffffff, (UInt64) 0x8000000000000000), (UInt32) 1);

            assert(test.Call("lt_s", (UInt64) 0, (UInt64) 0), (UInt32) 0);
            assert(test.Call("lt_s", (UInt64) 1, (UInt64) 1), (UInt32) 0);
            assert(test.Call("lt_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 1), (UInt32) 1);
            assert(test.Call("lt_s", (UInt64) 0x8000000000000000, (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("lt_s", (UInt64) 0x7fffffffffffffff, (UInt64) 0x7fffffffffffffff), (UInt32) 0);
            assert(test.Call("lt_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 0);
            assert(test.Call("lt_s", (UInt64) 1, (UInt64) 0), (UInt32) 0);
            assert(test.Call("lt_s", (UInt64) 0, (UInt64) 1), (UInt32) 1);
            assert(test.Call("lt_s", (UInt64) 0x8000000000000000, (UInt64) 0), (UInt32) 1);
            assert(test.Call("lt_s", (UInt64) 0, (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("lt_s", (UInt64) 0x8000000000000000, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 1);
            assert(test.Call("lt_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("lt_s", (UInt64) 0x8000000000000000, (UInt64) 0x7fffffffffffffff), (UInt32) 1);
            assert(test.Call("lt_s", (UInt64) 0x7fffffffffffffff, (UInt64) 0x8000000000000000), (UInt32) 0);

            assert(test.Call("lt_u", (UInt64) 0, (UInt64) 0), (UInt32) 0);
            assert(test.Call("lt_u", (UInt64) 1, (UInt64) 1), (UInt32) 0);
            assert(test.Call("lt_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 1), (UInt32) 0);
            assert(test.Call("lt_u", (UInt64) 0x8000000000000000, (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("lt_u", (UInt64) 0x7fffffffffffffff, (UInt64) 0x7fffffffffffffff), (UInt32) 0);
            assert(test.Call("lt_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 0);
            assert(test.Call("lt_u", (UInt64) 1, (UInt64) 0), (UInt32) 0);
            assert(test.Call("lt_u", (UInt64) 0, (UInt64) 1), (UInt32) 1);
            assert(test.Call("lt_u", (UInt64) 0x8000000000000000, (UInt64) 0), (UInt32) 0);
            assert(test.Call("lt_u", (UInt64) 0, (UInt64) 0x8000000000000000), (UInt32) 1);
            assert(test.Call("lt_u", (UInt64) 0x8000000000000000, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 1);
            assert(test.Call("lt_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("lt_u", (UInt64) 0x8000000000000000, (UInt64) 0x7fffffffffffffff), (UInt32) 0);
            assert(test.Call("lt_u", (UInt64) 0x7fffffffffffffff, (UInt64) 0x8000000000000000), (UInt32) 1);

            assert(test.Call("le_s", (UInt64) 0, (UInt64) 0), (UInt32) 1);
            assert(test.Call("le_s", (UInt64) 1, (UInt64) 1), (UInt32) 1);
            assert(test.Call("le_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 1), (UInt32) 1);
            assert(test.Call("le_s", (UInt64) 0x8000000000000000, (UInt64) 0x8000000000000000), (UInt32) 1);
            assert(test.Call("le_s", (UInt64) 0x7fffffffffffffff, (UInt64) 0x7fffffffffffffff), (UInt32) 1);
            assert(test.Call("le_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 1);
            assert(test.Call("le_s", (UInt64) 1, (UInt64) 0), (UInt32) 0);
            assert(test.Call("le_s", (UInt64) 0, (UInt64) 1), (UInt32) 1);
            assert(test.Call("le_s", (UInt64) 0x8000000000000000, (UInt64) 0), (UInt32) 1);
            assert(test.Call("le_s", (UInt64) 0, (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("le_s", (UInt64) 0x8000000000000000, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 1);
            assert(test.Call("le_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("le_s", (UInt64) 0x8000000000000000, (UInt64) 0x7fffffffffffffff), (UInt32) 1);
            assert(test.Call("le_s", (UInt64) 0x7fffffffffffffff, (UInt64) 0x8000000000000000), (UInt32) 0);

            assert(test.Call("le_u", (UInt64) 0, (UInt64) 0), (UInt32) 1);
            assert(test.Call("le_u", (UInt64) 1, (UInt64) 1), (UInt32) 1);
            assert(test.Call("le_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 1), (UInt32) 0);
            assert(test.Call("le_u", (UInt64) 0x8000000000000000, (UInt64) 0x8000000000000000), (UInt32) 1);
            assert(test.Call("le_u", (UInt64) 0x7fffffffffffffff, (UInt64) 0x7fffffffffffffff), (UInt32) 1);
            assert(test.Call("le_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 1);
            assert(test.Call("le_u", (UInt64) 1, (UInt64) 0), (UInt32) 0);
            assert(test.Call("le_u", (UInt64) 0, (UInt64) 1), (UInt32) 1);
            assert(test.Call("le_u", (UInt64) 0x8000000000000000, (UInt64) 0), (UInt32) 0);
            assert(test.Call("le_u", (UInt64) 0, (UInt64) 0x8000000000000000), (UInt32) 1);
            assert(test.Call("le_u", (UInt64) 0x8000000000000000, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 1);
            assert(test.Call("le_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("le_u", (UInt64) 0x8000000000000000, (UInt64) 0x7fffffffffffffff), (UInt32) 0);
            assert(test.Call("le_u", (UInt64) 0x7fffffffffffffff, (UInt64) 0x8000000000000000), (UInt32) 1);

            assert(test.Call("gt_s", (UInt64) 0, (UInt64) 0), (UInt32) 0);
            assert(test.Call("gt_s", (UInt64) 1, (UInt64) 1), (UInt32) 0);
            assert(test.Call("gt_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 1), (UInt32) 0);
            assert(test.Call("gt_s", (UInt64) 0x8000000000000000, (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("gt_s", (UInt64) 0x7fffffffffffffff, (UInt64) 0x7fffffffffffffff), (UInt32) 0);
            assert(test.Call("gt_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 0);
            assert(test.Call("gt_s", (UInt64) 1, (UInt64) 0), (UInt32) 1);
            assert(test.Call("gt_s", (UInt64) 0, (UInt64) 1), (UInt32) 0);
            assert(test.Call("gt_s", (UInt64) 0x8000000000000000, (UInt64) 0), (UInt32) 0);
            assert(test.Call("gt_s", (UInt64) 0, (UInt64) 0x8000000000000000), (UInt32) 1);
            assert(test.Call("gt_s", (UInt64) 0x8000000000000000, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 0);
            assert(test.Call("gt_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x8000000000000000), (UInt32) 1);
            assert(test.Call("gt_s", (UInt64) 0x8000000000000000, (UInt64) 0x7fffffffffffffff), (UInt32) 0);
            assert(test.Call("gt_s", (UInt64) 0x7fffffffffffffff, (UInt64) 0x8000000000000000), (UInt32) 1);

            assert(test.Call("gt_u", (UInt64) 0, (UInt64) 0), (UInt32) 0);
            assert(test.Call("gt_u", (UInt64) 1, (UInt64) 1), (UInt32) 0);
            assert(test.Call("gt_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 1), (UInt32) 1);
            assert(test.Call("gt_u", (UInt64) 0x8000000000000000, (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("gt_u", (UInt64) 0x7fffffffffffffff, (UInt64) 0x7fffffffffffffff), (UInt32) 0);
            assert(test.Call("gt_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 0);
            assert(test.Call("gt_u", (UInt64) 1, (UInt64) 0), (UInt32) 1);
            assert(test.Call("gt_u", (UInt64) 0, (UInt64) 1), (UInt32) 0);
            assert(test.Call("gt_u", (UInt64) 0x8000000000000000, (UInt64) 0), (UInt32) 1);
            assert(test.Call("gt_u", (UInt64) 0, (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("gt_u", (UInt64) 0x8000000000000000, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 0);
            assert(test.Call("gt_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x8000000000000000), (UInt32) 1);
            assert(test.Call("gt_u", (UInt64) 0x8000000000000000, (UInt64) 0x7fffffffffffffff), (UInt32) 1);
            assert(test.Call("gt_u", (UInt64) 0x7fffffffffffffff, (UInt64) 0x8000000000000000), (UInt32) 0);

            assert(test.Call("ge_s", (UInt64) 0, (UInt64) 0), (UInt32) 1);
            assert(test.Call("ge_s", (UInt64) 1, (UInt64) 1), (UInt32) 1);
            assert(test.Call("ge_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 1), (UInt32) 0);
            assert(test.Call("ge_s", (UInt64) 0x8000000000000000, (UInt64) 0x8000000000000000), (UInt32) 1);
            assert(test.Call("ge_s", (UInt64) 0x7fffffffffffffff, (UInt64) 0x7fffffffffffffff), (UInt32) 1);
            assert(test.Call("ge_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 1);
            assert(test.Call("ge_s", (UInt64) 1, (UInt64) 0), (UInt32) 1);
            assert(test.Call("ge_s", (UInt64) 0, (UInt64) 1), (UInt32) 0);
            assert(test.Call("ge_s", (UInt64) 0x8000000000000000, (UInt64) 0), (UInt32) 0);
            assert(test.Call("ge_s", (UInt64) 0, (UInt64) 0x8000000000000000), (UInt32) 1);
            assert(test.Call("ge_s", (UInt64) 0x8000000000000000, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 0);
            assert(test.Call("ge_s", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x8000000000000000), (UInt32) 1);
            assert(test.Call("ge_s", (UInt64) 0x8000000000000000, (UInt64) 0x7fffffffffffffff), (UInt32) 0);
            assert(test.Call("ge_s", (UInt64) 0x7fffffffffffffff, (UInt64) 0x8000000000000000), (UInt32) 1);

            assert(test.Call("ge_u", (UInt64) 0, (UInt64) 0), (UInt32) 1);
            assert(test.Call("ge_u", (UInt64) 1, (UInt64) 1), (UInt32) 1);
            assert(test.Call("ge_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 1), (UInt32) 1);
            assert(test.Call("ge_u", (UInt64) 0x8000000000000000, (UInt64) 0x8000000000000000), (UInt32) 1);
            assert(test.Call("ge_u", (UInt64) 0x7fffffffffffffff, (UInt64) 0x7fffffffffffffff), (UInt32) 1);
            assert(test.Call("ge_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 1);
            assert(test.Call("ge_u", (UInt64) 1, (UInt64) 0), (UInt32) 1);
            assert(test.Call("ge_u", (UInt64) 0, (UInt64) 1), (UInt32) 0);
            assert(test.Call("ge_u", (UInt64) 0x8000000000000000, (UInt64) 0), (UInt32) 1);
            assert(test.Call("ge_u", (UInt64) 0, (UInt64) 0x8000000000000000), (UInt32) 0);
            assert(test.Call("ge_u", (UInt64) 0x8000000000000000, (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt32) 0);
            assert(test.Call("ge_u", (UInt64) 0xFFFFFFFFFFFFFFFF, (UInt64) 0x8000000000000000), (UInt32) 1);
            assert(test.Call("ge_u", (UInt64) 0x8000000000000000, (UInt64) 0x7fffffffffffffff), (UInt32) 1);
            assert(test.Call("ge_u", (UInt64) 0x7fffffffffffffff, (UInt64) 0x8000000000000000), (UInt32) 0);
        }
    }
}
