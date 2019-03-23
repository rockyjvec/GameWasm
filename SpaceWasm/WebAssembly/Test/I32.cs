using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class I32 : Test
    {
        public I32(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "i32.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            assert(test.Call("add", (UInt32) 1, (UInt32) 1), (UInt32) 2);
            assert(test.Call("add", (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert(test.Call("add", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 0xFFFFFFFE);
            assert(test.Call("add", (UInt32) 0xFFFFFFFF, (UInt32) 1), (UInt32) 0);
            assert(test.Call("add", (UInt32) 0x7fffffff, (UInt32) 1), (UInt32) 0x80000000);
            assert(test.Call("add", (UInt32) 0x80000000, (UInt32) 0xFFFFFFFF), (UInt32) 0x7fffffff);
            assert(test.Call("add", (UInt32) 0x80000000, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("add", (UInt32) 0x3fffffff, (UInt32) 1), (UInt32) 0x40000000);

            assert(test.Call("sub", (UInt32) 1, (UInt32) 1), (UInt32) 0);
            assert(test.Call("sub", (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert(test.Call("sub", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("sub", (UInt32) 0x7fffffff, (UInt32) 0xFFFFFFFF), (UInt32) 0x80000000);
            assert(test.Call("sub", (UInt32) 0x80000000, (UInt32) 1), (UInt32) 0x7fffffff);
            assert(test.Call("sub", (UInt32) 0x80000000, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("sub", (UInt32) 0x3fffffff, (UInt32) 0xFFFFFFFF), (UInt32) 0x40000000);

            assert(test.Call("mul", (UInt32) 1, (UInt32) 1), (UInt32) 1);
            assert(test.Call("mul", (UInt32) 1, (UInt32) 0), (UInt32) 0);
            assert(test.Call("mul", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 1);
            assert(test.Call("mul", (UInt32) 0x10000000, (UInt32) 4096), (UInt32) 0);
            assert(test.Call("mul", (UInt32) 0x80000000, (UInt32) 0), (UInt32) 0);
            assert(test.Call("mul", (UInt32) 0x80000000, (UInt32) 0xFFFFFFFF), (UInt32) 0x80000000);
            assert(test.Call("mul", (UInt32) 0x7fffffff, (UInt32) 0xFFFFFFFF), (UInt32) 0x80000001);
            assert(test.Call("mul", (UInt32) 0x01234567, (UInt32) 0x76543210), (UInt32) 0x358e7470);
            assert(test.Call("mul", (UInt32) 0x7fffffff, (UInt32) 0x7fffffff), (UInt32) 1);

            assert_trap(delegate { test.CallVoid("div_s", (UInt32)1, (UInt32)0); }, "integer divide by zero");
            assert_trap(delegate { test.CallVoid("div_s", (UInt32)0, (UInt32)0); }, "integer divide by zero");
            assert_trap(delegate { test.CallVoid("div_s", (UInt32)0x80000000, (UInt32)0xFFFFFFFF); }, "integer overflow");
            assert(test.Call("div_s", (UInt32) 1, (UInt32) 1), (UInt32) 1);
            assert(test.Call("div_s", (UInt32) 0, (UInt32) 1), (UInt32) 0);
            assert(test.Call("div_s", (UInt32) 0, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("div_s", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 1);
            assert(test.Call("div_s", (UInt32) 0x80000000, (UInt32) 2), (UInt32) 0xc0000000);
            assert(test.Call("div_s", (UInt32) 0x80000001, (UInt32) 1000), (UInt32) 0xffdf3b65);
            assert(test.Call("div_s", (UInt32) 5, (UInt32) 2), (UInt32) 2);
            assert(test.Call("div_s", (UInt32) 0xFFFFFFFB, (UInt32) 2), (UInt32) 0xFFFFFFFE);
            assert(test.Call("div_s", (UInt32) 5, (UInt32) 0xFFFFFFFE), (UInt32) 0xFFFFFFFE);
            assert(test.Call("div_s", (UInt32) 0xFFFFFFFB, (UInt32) 0xFFFFFFFE), (UInt32) 2);
            assert(test.Call("div_s", (UInt32) 7, (UInt32) 3), (UInt32) 2);
            assert(test.Call("div_s", (UInt32) 0xFFFFFFF9, (UInt32) 3), (UInt32) 0xFFFFFFFE);
            assert(test.Call("div_s", (UInt32) 7, (UInt32) 0xFFFFFFFD), (UInt32) 0xFFFFFFFE);
            assert(test.Call("div_s", (UInt32) 0xFFFFFFF9, (UInt32) 0xFFFFFFFD), (UInt32) 2);
            assert(test.Call("div_s", (UInt32) 11, (UInt32) 5), (UInt32) 2);
            assert(test.Call("div_s", (UInt32) 17, (UInt32) 7), (UInt32) 2);

            assert_trap(delegate { test.CallVoid("div_u", (UInt32)1, (UInt32)0); }, "integer divide by zero");
            assert_trap(delegate { test.CallVoid("div_u", (UInt32)0, (UInt32)0); }, "integer divide by zero");
            assert(test.Call("div_u", (UInt32) 1, (UInt32) 1), (UInt32) 1);
            assert(test.Call("div_u", (UInt32) 0, (UInt32) 1), (UInt32) 0);
            assert(test.Call("div_u", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 1);
            assert(test.Call("div_u", (UInt32) 0x80000000, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("div_u", (UInt32) 0x80000000, (UInt32) 2), (UInt32) 0x40000000);
            assert(test.Call("div_u", (UInt32) 0x8ff00ff0, (UInt32) 0x10001), (UInt32) 0x8fef);
            assert(test.Call("div_u", (UInt32) 0x80000001, (UInt32) 1000), (UInt32) 0x20c49b);
            assert(test.Call("div_u", (UInt32) 5, (UInt32) 2), (UInt32) 2);
            assert(test.Call("div_u", (UInt32) 0xFFFFFFFB, (UInt32) 2), (UInt32) 0x7ffffffd);
            assert(test.Call("div_u", (UInt32) 5, (UInt32) 0xFFFFFFFE), (UInt32) 0);
            assert(test.Call("div_u", (UInt32) 0xFFFFFFFB, (UInt32) 0xFFFFFFFE), (UInt32) 0);
            assert(test.Call("div_u", (UInt32) 7, (UInt32) 3), (UInt32) 2);
            assert(test.Call("div_u", (UInt32) 11, (UInt32) 5), (UInt32) 2);
            assert(test.Call("div_u", (UInt32) 17, (UInt32) 7), (UInt32) 2);

            assert_trap(delegate { test.CallVoid("rem_s", (UInt32)1, (UInt32)0); }, "integer divide by zero");
            assert_trap(delegate { test.CallVoid("rem_s", (UInt32)0, (UInt32)0); }, "integer divide by zero");
            assert(test.Call("rem_s", (UInt32) 0x7fffffff, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("rem_s", (UInt32) 1, (UInt32) 1), (UInt32) 0);
            assert(test.Call("rem_s", (UInt32) 0, (UInt32) 1), (UInt32) 0);
            assert(test.Call("rem_s", (UInt32) 0, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("rem_s", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("rem_s", (UInt32) 0x80000000, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("rem_s", (UInt32) 0x80000000, (UInt32) 2), (UInt32) 0);
            assert(test.Call("rem_s", (UInt32) 0x80000001, (UInt32) 1000), (UInt32) 0xFFFFFD79);
            assert(test.Call("rem_s", (UInt32) 5, (UInt32) 2), (UInt32) 1);
            assert(test.Call("rem_s", (UInt32) 0xFFFFFFFB, (UInt32) 2), (UInt32) 0xFFFFFFFF);
            assert(test.Call("rem_s", (UInt32) 5, (UInt32) 0xFFFFFFFE), (UInt32) 1);
            assert(test.Call("rem_s", (UInt32) 0xFFFFFFFB, (UInt32) 0xFFFFFFFE), (UInt32) 0xFFFFFFFF);
            assert(test.Call("rem_s", (UInt32) 7, (UInt32) 3), (UInt32) 1);
            assert(test.Call("rem_s", (UInt32) 0xFFFFFFF9, (UInt32) 3), (UInt32) 0xFFFFFFFF);
            assert(test.Call("rem_s", (UInt32) 7, (UInt32) 0xFFFFFFFD), (UInt32) 1);
            assert(test.Call("rem_s", (UInt32) 0xFFFFFFF9, (UInt32) 0xFFFFFFFD), (UInt32) 0xFFFFFFFF);
            assert(test.Call("rem_s", (UInt32) 11, (UInt32) 5), (UInt32) 1);
            assert(test.Call("rem_s", (UInt32) 17, (UInt32) 7), (UInt32) 3);

            assert_trap(delegate { test.CallVoid("rem_u", (UInt32)1, (UInt32)0); }, "integer divide by zero");
            assert_trap(delegate { test.CallVoid("rem_u", (UInt32)0, (UInt32)0); }, "integer divide by zero");
            assert(test.Call("rem_u", (UInt32) 1, (UInt32) 1), (UInt32) 0);
            assert(test.Call("rem_u", (UInt32) 0, (UInt32) 1), (UInt32) 0);
            assert(test.Call("rem_u", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("rem_u", (UInt32) 0x80000000, (UInt32) 0xFFFFFFFF), (UInt32) 0x80000000);
            assert(test.Call("rem_u", (UInt32) 0x80000000, (UInt32) 2), (UInt32) 0);
            assert(test.Call("rem_u", (UInt32) 0x8ff00ff0, (UInt32) 0x10001), (UInt32) 0x8001);
            assert(test.Call("rem_u", (UInt32) 0x80000001, (UInt32) 1000), (UInt32) 649);
            assert(test.Call("rem_u", (UInt32) 5, (UInt32) 2), (UInt32) 1);
            assert(test.Call("rem_u", (UInt32) 0xFFFFFFFB, (UInt32) 2), (UInt32) 1);
            assert(test.Call("rem_u", (UInt32) 5, (UInt32) 0xFFFFFFFE), (UInt32) 5);
            assert(test.Call("rem_u", (UInt32) 0xFFFFFFFB, (UInt32) 0xFFFFFFFE), (UInt32) 0xFFFFFFFB);
            assert(test.Call("rem_u", (UInt32) 7, (UInt32) 3), (UInt32) 1);
            assert(test.Call("rem_u", (UInt32) 11, (UInt32) 5), (UInt32) 1);
            assert(test.Call("rem_u", (UInt32) 17, (UInt32) 7), (UInt32) 3);

            assert(test.Call("and", (UInt32) 1, (UInt32) 0), (UInt32) 0);
            assert(test.Call("and", (UInt32) 0, (UInt32) 1), (UInt32) 0);
            assert(test.Call("and", (UInt32) 1, (UInt32) 1), (UInt32) 1);
            assert(test.Call("and", (UInt32) 0, (UInt32) 0), (UInt32) 0);
            assert(test.Call("and", (UInt32) 0x7fffffff, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("and", (UInt32) 0x7fffffff, (UInt32) 0xFFFFFFFF), (UInt32) 0x7fffffff);
            assert(test.Call("and", (UInt32) 0xf0f0ffff, (UInt32) 0xfffff0f0), (UInt32) 0xf0f0f0f0);
            assert(test.Call("and", (UInt32) 0xffffffff, (UInt32) 0xffffffff), (UInt32) 0xffffffff);

            assert(test.Call("or", (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert(test.Call("or", (UInt32) 0, (UInt32) 1), (UInt32) 1);
            assert(test.Call("or", (UInt32) 1, (UInt32) 1), (UInt32) 1);
            assert(test.Call("or", (UInt32) 0, (UInt32) 0), (UInt32) 0);
            assert(test.Call("or", (UInt32) 0x7fffffff, (UInt32) 0x80000000), (UInt32) 0xFFFFFFFF);
            assert(test.Call("or", (UInt32) 0x80000000, (UInt32) 0), (UInt32) 0x80000000);
            assert(test.Call("or", (UInt32) 0xf0f0ffff, (UInt32) 0xfffff0f0), (UInt32) 0xffffffff);
            assert(test.Call("or", (UInt32) 0xffffffff, (UInt32) 0xffffffff), (UInt32) 0xffffffff);

            assert(test.Call("xor", (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert(test.Call("xor", (UInt32) 0, (UInt32) 1), (UInt32) 1);
            assert(test.Call("xor", (UInt32) 1, (UInt32) 1), (UInt32) 0);
            assert(test.Call("xor", (UInt32) 0, (UInt32) 0), (UInt32) 0);
            assert(test.Call("xor", (UInt32) 0x7fffffff, (UInt32) 0x80000000), (UInt32) 0xFFFFFFFF);
            assert(test.Call("xor", (UInt32) 0x80000000, (UInt32) 0), (UInt32) 0x80000000);
            assert(test.Call("xor", (UInt32) 0xFFFFFFFF, (UInt32) 0x80000000), (UInt32) 0x7fffffff);
            assert(test.Call("xor", (UInt32) 0xFFFFFFFF, (UInt32) 0x7fffffff), (UInt32) 0x80000000);
            assert(test.Call("xor", (UInt32) 0xf0f0ffff, (UInt32) 0xfffff0f0), (UInt32) 0x0f0f0f0f);
            assert(test.Call("xor", (UInt32) 0xffffffff, (UInt32) 0xffffffff), (UInt32) 0);

            assert(test.Call("shl", (UInt32) 1, (UInt32) 1), (UInt32) 2);
            assert(test.Call("shl", (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert(test.Call("shl", (UInt32) 0x7fffffff, (UInt32) 1), (UInt32) 0xfffffffe);
            assert(test.Call("shl", (UInt32) 0xffffffff, (UInt32) 1), (UInt32) 0xfffffffe);
            assert(test.Call("shl", (UInt32) 0x80000000, (UInt32) 1), (UInt32) 0);
            assert(test.Call("shl", (UInt32) 0x40000000, (UInt32) 1), (UInt32) 0x80000000);
            assert(test.Call("shl", (UInt32) 1, (UInt32) 31), (UInt32) 0x80000000);
            assert(test.Call("shl", (UInt32) 1, (UInt32) 32), (UInt32) 1);
            assert(test.Call("shl", (UInt32) 1, (UInt32) 33), (UInt32) 2);
            assert(test.Call("shl", (UInt32) 1, (UInt32) 0xFFFFFFFF), (UInt32) 0x80000000);
            assert(test.Call("shl", (UInt32) 1, (UInt32) 0x7fffffff), (UInt32) 0x80000000);

            assert(test.Call("shr_s", (UInt32) 1, (UInt32) 1), (UInt32) 0);
            assert(test.Call("shr_s", (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert(test.Call("shr_s", (UInt32) 0xFFFFFFFF, (UInt32) 1), (UInt32) 0xFFFFFFFF);
            assert(test.Call("shr_s", (UInt32) 0x7fffffff, (UInt32) 1), (UInt32) 0x3fffffff);
            assert(test.Call("shr_s", (UInt32) 0x80000000, (UInt32) 1), (UInt32) 0xc0000000);
            assert(test.Call("shr_s", (UInt32) 0x40000000, (UInt32) 1), (UInt32) 0x20000000);
            assert(test.Call("shr_s", (UInt32) 1, (UInt32) 32), (UInt32) 1);
            assert(test.Call("shr_s", (UInt32) 1, (UInt32) 33), (UInt32) 0);
            assert(test.Call("shr_s", (UInt32) 1, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("shr_s", (UInt32) 1, (UInt32) 0x7fffffff), (UInt32) 0);
            assert(test.Call("shr_s", (UInt32) 1, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("shr_s", (UInt32) 0x80000000, (UInt32) 31), (UInt32) 0xFFFFFFFF);
            assert(test.Call("shr_s", (UInt32) 0xFFFFFFFF, (UInt32) 32), (UInt32) 0xFFFFFFFF);
            assert(test.Call("shr_s", (UInt32) 0xFFFFFFFF, (UInt32) 33), (UInt32) 0xFFFFFFFF);
            assert(test.Call("shr_s", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 0xFFFFFFFF);
            assert(test.Call("shr_s", (UInt32) 0xFFFFFFFF, (UInt32) 0x7fffffff), (UInt32) 0xFFFFFFFF);
            assert(test.Call("shr_s", (UInt32) 0xFFFFFFFF, (UInt32) 0x80000000), (UInt32) 0xFFFFFFFF);

            assert(test.Call("shr_u", (UInt32) 1, (UInt32) 1), (UInt32) 0);
            assert(test.Call("shr_u", (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert(test.Call("shr_u", (UInt32) 0xFFFFFFFF, (UInt32) 1), (UInt32) 0x7fffffff);
            assert(test.Call("shr_u", (UInt32) 0x7fffffff, (UInt32) 1), (UInt32) 0x3fffffff);
            assert(test.Call("shr_u", (UInt32) 0x80000000, (UInt32) 1), (UInt32) 0x40000000);
            assert(test.Call("shr_u", (UInt32) 0x40000000, (UInt32) 1), (UInt32) 0x20000000);
            assert(test.Call("shr_u", (UInt32) 1, (UInt32) 32), (UInt32) 1);
            assert(test.Call("shr_u", (UInt32) 1, (UInt32) 33), (UInt32) 0);
            assert(test.Call("shr_u", (UInt32) 1, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("shr_u", (UInt32) 1, (UInt32) 0x7fffffff), (UInt32) 0);
            assert(test.Call("shr_u", (UInt32) 1, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("shr_u", (UInt32) 0x80000000, (UInt32) 31), (UInt32) 1);
            assert(test.Call("shr_u", (UInt32) 0xFFFFFFFF, (UInt32) 32), (UInt32) 0xFFFFFFFF);
            assert(test.Call("shr_u", (UInt32) 0xFFFFFFFF, (UInt32) 33), (UInt32) 0x7fffffff);
            assert(test.Call("shr_u", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 1);
            assert(test.Call("shr_u", (UInt32) 0xFFFFFFFF, (UInt32) 0x7fffffff), (UInt32) 1);
            assert(test.Call("shr_u", (UInt32) 0xFFFFFFFF, (UInt32) 0x80000000), (UInt32) 0xFFFFFFFF);

            assert(test.Call("rotl", (UInt32) 1, (UInt32) 1), (UInt32) 2);
            assert(test.Call("rotl", (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert(test.Call("rotl", (UInt32) 0xFFFFFFFF, (UInt32) 1), (UInt32) 0xFFFFFFFF);
            assert(test.Call("rotl", (UInt32) 1, (UInt32) 32), (UInt32) 1);
            assert(test.Call("rotl", (UInt32) 0xabcd9876, (UInt32) 1), (UInt32) 0x579b30ed);
            assert(test.Call("rotl", (UInt32) 0xfe00dc00, (UInt32) 4), (UInt32) 0xe00dc00f);
            assert(test.Call("rotl", (UInt32) 0xb0c1d2e3, (UInt32) 5), (UInt32) 0x183a5c76);
            assert(test.Call("rotl", (UInt32) 0x00008000, (UInt32) 37), (UInt32) 0x00100000);
            assert(test.Call("rotl", (UInt32) 0xb0c1d2e3, (UInt32) 0xff05), (UInt32) 0x183a5c76);
            assert(test.Call("rotl", (UInt32) 0x769abcdf, (UInt32) 0xffffffed), (UInt32) 0x579beed3);
            assert(test.Call("rotl", (UInt32) 0x769abcdf, (UInt32) 0x8000000d), (UInt32) 0x579beed3);
            assert(test.Call("rotl", (UInt32) 1, (UInt32) 31), (UInt32) 0x80000000);
            assert(test.Call("rotl", (UInt32) 0x80000000, (UInt32) 1), (UInt32) 1);

            assert(test.Call("rotr", (UInt32) 1, (UInt32) 1), (UInt32) 0x80000000);
            assert(test.Call("rotr", (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert(test.Call("rotr", (UInt32) 0xFFFFFFFF, (UInt32) 1), (UInt32) 0xFFFFFFFF);
            assert(test.Call("rotr", (UInt32) 1, (UInt32) 32), (UInt32) 1);
            assert(test.Call("rotr", (UInt32) 0xff00cc00, (UInt32) 1), (UInt32) 0x7f806600);
            assert(test.Call("rotr", (UInt32) 0x00080000, (UInt32) 4), (UInt32) 0x00008000);
            assert(test.Call("rotr", (UInt32) 0xb0c1d2e3, (UInt32) 5), (UInt32) 0x1d860e97);
            assert(test.Call("rotr", (UInt32) 0x00008000, (UInt32) 37), (UInt32) 0x00000400);
            assert(test.Call("rotr", (UInt32) 0xb0c1d2e3, (UInt32) 0xff05), (UInt32) 0x1d860e97);
            assert(test.Call("rotr", (UInt32) 0x769abcdf, (UInt32) 0xffffffed), (UInt32) 0xe6fbb4d5);
            assert(test.Call("rotr", (UInt32) 0x769abcdf, (UInt32) 0x8000000d), (UInt32) 0xe6fbb4d5);
            assert(test.Call("rotr", (UInt32) 1, (UInt32) 31), (UInt32) 2);
            assert(test.Call("rotr", (UInt32) 0x80000000, (UInt32) 31), (UInt32) 1);

            assert(test.Call("clz", (UInt32) 0xffffffff), (UInt32) 0);
            assert(test.Call("clz", (UInt32) 0), (UInt32) 32);
            assert(test.Call("clz", (UInt32) 0x00008000), (UInt32) 16);
            assert(test.Call("clz", (UInt32) 0xff), (UInt32) 24);
            assert(test.Call("clz", (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("clz", (UInt32) 1), (UInt32) 31);
            assert(test.Call("clz", (UInt32) 2), (UInt32) 30);
            assert(test.Call("clz", (UInt32) 0x7fffffff), (UInt32) 1);

            assert(test.Call("ctz", (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("ctz", (UInt32) 0), (UInt32) 32);
            assert(test.Call("ctz", (UInt32) 0x00008000), (UInt32) 15);
            assert(test.Call("ctz", (UInt32) 0x00010000), (UInt32) 16);
            assert(test.Call("ctz", (UInt32) 0x80000000), (UInt32) 31);
            assert(test.Call("ctz", (UInt32) 0x7fffffff), (UInt32) 0);

            assert(test.Call("popcnt", (UInt32) 0xFFFFFFFF), (UInt32) 32);
            assert(test.Call("popcnt", (UInt32) 0), (UInt32) 0);
            assert(test.Call("popcnt", (UInt32) 0x00008000), (UInt32) 1);
            assert(test.Call("popcnt", (UInt32) 0x80008000), (UInt32) 2);
            assert(test.Call("popcnt", (UInt32) 0x7fffffff), (UInt32) 31);
            assert(test.Call("popcnt", (UInt32) 0xAAAAAAAA), (UInt32) 16);
            assert(test.Call("popcnt", (UInt32) 0x55555555), (UInt32) 16);
            assert(test.Call("popcnt", (UInt32) 0xDEADBEEF), (UInt32) 24);

            assert(test.Call("eqz", (UInt32) 0), (UInt32) 1);
            assert(test.Call("eqz", (UInt32) 1), (UInt32) 0);
            assert(test.Call("eqz", (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("eqz", (UInt32) 0x7fffffff), (UInt32) 0);
            assert(test.Call("eqz", (UInt32) 0xffffffff), (UInt32) 0);

            assert(test.Call("eq", (UInt32) 0, (UInt32) 0), (UInt32) 1);
            assert(test.Call("eq", (UInt32) 1, (UInt32) 1), (UInt32) 1);
            assert(test.Call("eq", (UInt32) 0xFFFFFFFF, (UInt32) 1), (UInt32) 0);
            assert(test.Call("eq", (UInt32) 0x80000000, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("eq", (UInt32) 0x7fffffff, (UInt32) 0x7fffffff), (UInt32) 1);
            assert(test.Call("eq", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 1);
            assert(test.Call("eq", (UInt32) 1, (UInt32) 0), (UInt32) 0);
            assert(test.Call("eq", (UInt32) 0, (UInt32) 1), (UInt32) 0);
            assert(test.Call("eq", (UInt32) 0x80000000, (UInt32) 0), (UInt32) 0);
            assert(test.Call("eq", (UInt32) 0, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("eq", (UInt32) 0x80000000, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("eq", (UInt32) 0xFFFFFFFF, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("eq", (UInt32) 0x80000000, (UInt32) 0x7fffffff), (UInt32) 0);
            assert(test.Call("eq", (UInt32) 0x7fffffff, (UInt32) 0x80000000), (UInt32) 0);

            assert(test.Call("ne", (UInt32) 0, (UInt32) 0), (UInt32) 0);
            assert(test.Call("ne", (UInt32) 1, (UInt32) 1), (UInt32) 0);
            assert(test.Call("ne", (UInt32) 0xFFFFFFFF, (UInt32) 1), (UInt32) 1);
            assert(test.Call("ne", (UInt32) 0x80000000, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("ne", (UInt32) 0x7fffffff, (UInt32) 0x7fffffff), (UInt32) 0);
            assert(test.Call("ne", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("ne", (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert(test.Call("ne", (UInt32) 0, (UInt32) 1), (UInt32) 1);
            assert(test.Call("ne", (UInt32) 0x80000000, (UInt32) 0), (UInt32) 1);
            assert(test.Call("ne", (UInt32) 0, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("ne", (UInt32) 0x80000000, (UInt32) 0xFFFFFFFF), (UInt32) 1);
            assert(test.Call("ne", (UInt32) 0xFFFFFFFF, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("ne", (UInt32) 0x80000000, (UInt32) 0x7fffffff), (UInt32) 1);
            assert(test.Call("ne", (UInt32) 0x7fffffff, (UInt32) 0x80000000), (UInt32) 1);

            assert(test.Call("lt_s", (UInt32) 0, (UInt32) 0), (UInt32) 0);
            assert(test.Call("lt_s", (UInt32) 1, (UInt32) 1), (UInt32) 0);
            assert(test.Call("lt_s", (UInt32) 0xFFFFFFFF, (UInt32) 1), (UInt32) 1);
            assert(test.Call("lt_s", (UInt32) 0x80000000, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("lt_s", (UInt32) 0x7fffffff, (UInt32) 0x7fffffff), (UInt32) 0);
            assert(test.Call("lt_s", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("lt_s", (UInt32) 1, (UInt32) 0), (UInt32) 0);
            assert(test.Call("lt_s", (UInt32) 0, (UInt32) 1), (UInt32) 1);
            assert(test.Call("lt_s", (UInt32) 0x80000000, (UInt32) 0), (UInt32) 1);
            assert(test.Call("lt_s", (UInt32) 0, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("lt_s", (UInt32) 0x80000000, (UInt32) 0xFFFFFFFF), (UInt32) 1);
            assert(test.Call("lt_s", (UInt32) 0xFFFFFFFF, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("lt_s", (UInt32) 0x80000000, (UInt32) 0x7fffffff), (UInt32) 1);
            assert(test.Call("lt_s", (UInt32) 0x7fffffff, (UInt32) 0x80000000), (UInt32) 0);

            assert(test.Call("lt_u", (UInt32) 0, (UInt32) 0), (UInt32) 0);
            assert(test.Call("lt_u", (UInt32) 1, (UInt32) 1), (UInt32) 0);
            assert(test.Call("lt_u", (UInt32) 0xFFFFFFFF, (UInt32) 1), (UInt32) 0);
            assert(test.Call("lt_u", (UInt32) 0x80000000, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("lt_u", (UInt32) 0x7fffffff, (UInt32) 0x7fffffff), (UInt32) 0);
            assert(test.Call("lt_u", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("lt_u", (UInt32) 1, (UInt32) 0), (UInt32) 0);
            assert(test.Call("lt_u", (UInt32) 0, (UInt32) 1), (UInt32) 1);
            assert(test.Call("lt_u", (UInt32) 0x80000000, (UInt32) 0), (UInt32) 0);
            assert(test.Call("lt_u", (UInt32) 0, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("lt_u", (UInt32) 0x80000000, (UInt32) 0xFFFFFFFF), (UInt32) 1);
            assert(test.Call("lt_u", (UInt32) 0xFFFFFFFF, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("lt_u", (UInt32) 0x80000000, (UInt32) 0x7fffffff), (UInt32) 0);
            assert(test.Call("lt_u", (UInt32) 0x7fffffff, (UInt32) 0x80000000), (UInt32) 1);

            assert(test.Call("le_s", (UInt32) 0, (UInt32) 0), (UInt32) 1);
            assert(test.Call("le_s", (UInt32) 1, (UInt32) 1), (UInt32) 1);
            assert(test.Call("le_s", (UInt32) 0xFFFFFFFF, (UInt32) 1), (UInt32) 1);
            assert(test.Call("le_s", (UInt32) 0x80000000, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("le_s", (UInt32) 0x7fffffff, (UInt32) 0x7fffffff), (UInt32) 1);
            assert(test.Call("le_s", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 1);
            assert(test.Call("le_s", (UInt32) 1, (UInt32) 0), (UInt32) 0);
            assert(test.Call("le_s", (UInt32) 0, (UInt32) 1), (UInt32) 1);
            assert(test.Call("le_s", (UInt32) 0x80000000, (UInt32) 0), (UInt32) 1);
            assert(test.Call("le_s", (UInt32) 0, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("le_s", (UInt32) 0x80000000, (UInt32) 0xFFFFFFFF), (UInt32) 1);
            assert(test.Call("le_s", (UInt32) 0xFFFFFFFF, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("le_s", (UInt32) 0x80000000, (UInt32) 0x7fffffff), (UInt32) 1);
            assert(test.Call("le_s", (UInt32) 0x7fffffff, (UInt32) 0x80000000), (UInt32) 0);

            assert(test.Call("le_u", (UInt32) 0, (UInt32) 0), (UInt32) 1);
            assert(test.Call("le_u", (UInt32) 1, (UInt32) 1), (UInt32) 1);
            assert(test.Call("le_u", (UInt32) 0xFFFFFFFF, (UInt32) 1), (UInt32) 0);
            assert(test.Call("le_u", (UInt32) 0x80000000, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("le_u", (UInt32) 0x7fffffff, (UInt32) 0x7fffffff), (UInt32) 1);
            assert(test.Call("le_u", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 1);
            assert(test.Call("le_u", (UInt32) 1, (UInt32) 0), (UInt32) 0);
            assert(test.Call("le_u", (UInt32) 0, (UInt32) 1), (UInt32) 1);
            assert(test.Call("le_u", (UInt32) 0x80000000, (UInt32) 0), (UInt32) 0);
            assert(test.Call("le_u", (UInt32) 0, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("le_u", (UInt32) 0x80000000, (UInt32) 0xFFFFFFFF), (UInt32) 1);
            assert(test.Call("le_u", (UInt32) 0xFFFFFFFF, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("le_u", (UInt32) 0x80000000, (UInt32) 0x7fffffff), (UInt32) 0);
            assert(test.Call("le_u", (UInt32) 0x7fffffff, (UInt32) 0x80000000), (UInt32) 1);

            assert(test.Call("gt_s", (UInt32) 0, (UInt32) 0), (UInt32) 0);
            assert(test.Call("gt_s", (UInt32) 1, (UInt32) 1), (UInt32) 0);
            assert(test.Call("gt_s", (UInt32) 0xFFFFFFFF, (UInt32) 1), (UInt32) 0);
            assert(test.Call("gt_s", (UInt32) 0x80000000, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("gt_s", (UInt32) 0x7fffffff, (UInt32) 0x7fffffff), (UInt32) 0);
            assert(test.Call("gt_s", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("gt_s", (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert(test.Call("gt_s", (UInt32) 0, (UInt32) 1), (UInt32) 0);
            assert(test.Call("gt_s", (UInt32) 0x80000000, (UInt32) 0), (UInt32) 0);
            assert(test.Call("gt_s", (UInt32) 0, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("gt_s", (UInt32) 0x80000000, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("gt_s", (UInt32) 0xFFFFFFFF, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("gt_s", (UInt32) 0x80000000, (UInt32) 0x7fffffff), (UInt32) 0);
            assert(test.Call("gt_s", (UInt32) 0x7fffffff, (UInt32) 0x80000000), (UInt32) 1);

            assert(test.Call("gt_u", (UInt32) 0, (UInt32) 0), (UInt32) 0);
            assert(test.Call("gt_u", (UInt32) 1, (UInt32) 1), (UInt32) 0);
            assert(test.Call("gt_u", (UInt32) 0xFFFFFFFF, (UInt32) 1), (UInt32) 1);
            assert(test.Call("gt_u", (UInt32) 0x80000000, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("gt_u", (UInt32) 0x7fffffff, (UInt32) 0x7fffffff), (UInt32) 0);
            assert(test.Call("gt_u", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("gt_u", (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert(test.Call("gt_u", (UInt32) 0, (UInt32) 1), (UInt32) 0);
            assert(test.Call("gt_u", (UInt32) 0x80000000, (UInt32) 0), (UInt32) 1);
            assert(test.Call("gt_u", (UInt32) 0, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("gt_u", (UInt32) 0x80000000, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("gt_u", (UInt32) 0xFFFFFFFF, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("gt_u", (UInt32) 0x80000000, (UInt32) 0x7fffffff), (UInt32) 1);
            assert(test.Call("gt_u", (UInt32) 0x7fffffff, (UInt32) 0x80000000), (UInt32) 0);

            assert(test.Call("ge_s", (UInt32) 0, (UInt32) 0), (UInt32) 1);
            assert(test.Call("ge_s", (UInt32) 1, (UInt32) 1), (UInt32) 1);
            assert(test.Call("ge_s", (UInt32) 0xFFFFFFFF, (UInt32) 1), (UInt32) 0);
            assert(test.Call("ge_s", (UInt32) 0x80000000, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("ge_s", (UInt32) 0x7fffffff, (UInt32) 0x7fffffff), (UInt32) 1);
            assert(test.Call("ge_s", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 1);
            assert(test.Call("ge_s", (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert(test.Call("ge_s", (UInt32) 0, (UInt32) 1), (UInt32) 0);
            assert(test.Call("ge_s", (UInt32) 0x80000000, (UInt32) 0), (UInt32) 0);
            assert(test.Call("ge_s", (UInt32) 0, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("ge_s", (UInt32) 0x80000000, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("ge_s", (UInt32) 0xFFFFFFFF, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("ge_s", (UInt32) 0x80000000, (UInt32) 0x7fffffff), (UInt32) 0);
            assert(test.Call("ge_s", (UInt32) 0x7fffffff, (UInt32) 0x80000000), (UInt32) 1);

            assert(test.Call("ge_u", (UInt32) 0, (UInt32) 0), (UInt32) 1);
            assert(test.Call("ge_u", (UInt32) 1, (UInt32) 1), (UInt32) 1);
            assert(test.Call("ge_u", (UInt32) 0xFFFFFFFF, (UInt32) 1), (UInt32) 1);
            assert(test.Call("ge_u", (UInt32) 0x80000000, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("ge_u", (UInt32) 0x7fffffff, (UInt32) 0x7fffffff), (UInt32) 1);
            assert(test.Call("ge_u", (UInt32) 0xFFFFFFFF, (UInt32) 0xFFFFFFFF), (UInt32) 1);
            assert(test.Call("ge_u", (UInt32) 1, (UInt32) 0), (UInt32) 1);
            assert(test.Call("ge_u", (UInt32) 0, (UInt32) 1), (UInt32) 0);
            assert(test.Call("ge_u", (UInt32) 0x80000000, (UInt32) 0), (UInt32) 1);
            assert(test.Call("ge_u", (UInt32) 0, (UInt32) 0x80000000), (UInt32) 0);
            assert(test.Call("ge_u", (UInt32) 0x80000000, (UInt32) 0xFFFFFFFF), (UInt32) 0);
            assert(test.Call("ge_u", (UInt32) 0xFFFFFFFF, (UInt32) 0x80000000), (UInt32) 1);
            assert(test.Call("ge_u", (UInt32) 0x80000000, (UInt32) 0x7fffffff), (UInt32) 1);
            assert(test.Call("ge_u", (UInt32) 0x7fffffff, (UInt32) 0x80000000), (UInt32) 0);
        }
    }
}
