using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class Endianness : Test
    {
        public Endianness(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "endianness.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            assert(test.Call("i32_load16_s", (UInt32) 0xFFFFFFFF), (UInt32) 0xFFFFFFFF);
            assert(test.Call("i32_load16_s", (UInt32) 0xFFFFEF6E), (UInt32) 0xFFFFEF6E);
            assert(test.Call("i32_load16_s", (UInt32) 42), (UInt32) 42);
            assert(test.Call("i32_load16_s", (UInt32) 0x3210), (UInt32) 0x3210);

            assert(test.Call("i32_load16_u", (UInt32) 0xFFFFFFFF), (UInt32) 0xFFFF);
            assert(test.Call("i32_load16_u", (UInt32) 0xFFFFEF6E), (UInt32) 61294);
            assert(test.Call("i32_load16_u", (UInt32) 42), (UInt32) 42);
            assert(test.Call("i32_load16_u", (UInt32) 0xCAFE), (UInt32) 0xCAFE);

            assert(test.Call("i32_load", (UInt32) 0xFFFFFFFF), (UInt32) 0xFFFFFFFF);
            assert(test.Call("i32_load", (UInt32) 0xFD78A84E), (UInt32) 0xFD78A84E);
            assert(test.Call("i32_load", (UInt32) 42424242), (UInt32) 42424242);
            assert(test.Call("i32_load", (UInt32) 0xABAD1DEA), (UInt32) 0xABAD1DEA);

            assert64(test.Call("i64_load16_s", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("i64_load16_s", (UInt64) 0xFFFFFFFFFFFFEF6E), (UInt64) 0xFFFFFFFFFFFFEF6E);
            assert64(test.Call("i64_load16_s", (UInt64) 42), (UInt64) 42);
            assert64(test.Call("i64_load16_s", (UInt64) 0x3210), (UInt64) 0x3210);

            assert64(test.Call("i64_load16_u", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0xFFFF);
            assert64(test.Call("i64_load16_u", (UInt64) 0xFFFFFFFFFFFFEF6E), (UInt64) 61294);
            assert64(test.Call("i64_load16_u", (UInt64) 42), (UInt64) 42);
            assert64(test.Call("i64_load16_u", (UInt64) 0xCAFE), (UInt64) 0xCAFE);

            assert64(test.Call("i64_load32_s", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("i64_load32_s", (UInt64) 0xFFFFFFFFFD78A84E), (UInt64) 0xFFFFFFFFFD78A84E);
            assert64(test.Call("i64_load32_s", (UInt64) 42424242), (UInt64) 42424242);
            assert64(test.Call("i64_load32_s", (UInt64) 0x12345678), (UInt64) 0x12345678);

            assert64(test.Call("i64_load32_u", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0xFFFFFFFF);
            assert64(test.Call("i64_load32_u", (UInt64) 0xFFFFFFFFFD78A84E), (UInt64) 4252543054);
            assert64(test.Call("i64_load32_u", (UInt64) 42424242), (UInt64) 42424242);
            assert64(test.Call("i64_load32_u", (UInt64) 0xABAD1DEA), (UInt64) 0xABAD1DEA);

            assert64(test.Call("i64_load", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("i64_load", (UInt64) 0xFFFFFFFFFD78A84E), (UInt64) 0xFFFFFFFFFD78A84E);
            assert64(test.Call("i64_load", (UInt64) 0xABAD1DEA), (UInt64) 0xABAD1DEA);
            assert64(test.Call("i64_load", (UInt64) 0xABADCAFEDEAD1DEA), (UInt64) 0xABADCAFEDEAD1DEA);

            assertF32(test.Call("f32_load", (float) -1), (float) -1);
            assertF32(test.Call("f32_load", (float) 1234e-5), (float) 1234e-5);
            assertF32(test.Call("f32_load", (float) 4242.4242), (float) 4242.4242);
//            assert(test.Call("f32_load", (float) 0x1.fffffep + 127), (float) 0x1.fffffep + 127);

            assertF64(test.Call("f64_load", (double) -1), (double) -1);
            assertF64(test.Call("f64_load", (double) 123456789e-5), (double) 123456789e-5);
            assertF64(test.Call("f64_load", (double) 424242.424242), (double) 424242.424242);
  //          assert(test.Call("f64_load", (double) 0x1.fffffffffffffp + 1023), (double) 0x1.fffffffffffffp + 1023);


            assert(test.Call("i32_store16", (UInt32) 0xFFFFFFFF), (UInt32) 0xFFFF);
            assert(test.Call("i32_store16", (UInt32) 0xFFFFEF6E), (UInt32) 61294);
            assert(test.Call("i32_store16", (UInt32) 42), (UInt32) 42);
            assert(test.Call("i32_store16", (UInt32) 0xCAFE), (UInt32) 0xCAFE);

            assert(test.Call("i32_store", (UInt32) 0xFFFFFFFF), (UInt32) 0xFFFFFFFF);
            assert(test.Call("i32_store", (UInt32) 0xFFFFEF6E), (UInt32) 0xFFFFEF6E);
            assert(test.Call("i32_store", (UInt32) 42424242), (UInt32) 42424242);
            assert(test.Call("i32_store", (UInt32) 0xDEADCAFE), (UInt32) 0xDEADCAFE);

            assert64(test.Call("i64_store16", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0xFFFF);
            assert64(test.Call("i64_store16", (UInt64) 0xFFFFFFFFFFFFEF6E), (UInt64) 61294);
            assert64(test.Call("i64_store16", (UInt64) 42), (UInt64) 42);
            assert64(test.Call("i64_store16", (UInt64) 0xCAFE), (UInt64) 0xCAFE);

            assert64(test.Call("i64_store32", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0xFFFFFFFF);
            assert64(test.Call("i64_store32", (UInt64) 0xFFFFFFFFFFFFEF6E), (UInt64) 4294963054);
            assert64(test.Call("i64_store32", (UInt64) 42424242), (UInt64) 42424242);
            assert64(test.Call("i64_store32", (UInt64) 0xDEADCAFE), (UInt64) 0xDEADCAFE);

            assert64(test.Call("i64_store", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("i64_store", (UInt64) 0xFFFFFFFFFD78A84E), (UInt64) 0xFFFFFFFFFD78A84E);
            assert64(test.Call("i64_store", (UInt64) 0xABAD1DEA), (UInt64) 0xABAD1DEA);
            assert64(test.Call("i64_store", (UInt64) 0xABADCAFEDEAD1DEA), (UInt64) 0xABADCAFEDEAD1DEA);

            assertF32(test.Call("f32_store", (float) -1), (float) -1);
            assertF32(test.Call("f32_store", (float) 1234e-5), (float) 1234e-5);
            assertF32(test.Call("f32_store", (float) 4242.4242), (float) 4242.4242);
//            assert(test.Call("f32_store", (float) 0x1.fffffep + 127), (float) 0x1.fffffep + 127);

            assertF64(test.Call("f64_store", (double) -1), (double) -1);
            assertF64(test.Call("f64_store", (double) 123456789e-5), (double) 123456789e-5);
            assertF64(test.Call("f64_store", (double) 424242.424242), (double) 424242.424242);
  //          assert(test.Call("f64_store", (double) 0x1.fffffffffffffp + 1023), (double) 0x1.fffffffffffffp + 1023);
        }
    }
}
