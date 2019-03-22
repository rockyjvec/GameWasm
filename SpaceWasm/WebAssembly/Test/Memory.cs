using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class Memory : Test
    {
        public Memory(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "memory.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            assert(test.Call("data"), (UInt32) 1);
            assertF64(test.Call("cast"), (double) 42.0);

            assert(test.Call("i32_load8_s", (UInt32) 0xFFFFFFFF), (UInt32)0xFFFFFFFF);
            assert(test.Call("i32_load8_u", (UInt32) 0xFFFFFFFF), (UInt32) 255);
            assert(test.Call("i32_load16_s", (UInt32) 0xFFFFFFFF), (UInt32) 0xFFFFFFFF);
            assert(test.Call("i32_load16_u", (UInt32) 0xFFFFFFFF), (UInt32) 65535);

            assert(test.Call("i32_load8_s", (UInt32) 100), (UInt32) 100);
            assert(test.Call("i32_load8_u", (UInt32) 200), (UInt32) 200);
            assert(test.Call("i32_load16_s", (UInt32) 20000), (UInt32) 20000);
            assert(test.Call("i32_load16_u", (UInt32) 40000), (UInt32) 40000);

            assert(test.Call("i32_load8_s", (UInt32) 0xfedc6543), (UInt32) 0x43);
            assert(test.Call("i32_load8_s", (UInt32) 0x3456cdef), (UInt32) 0xffffffef);
            assert(test.Call("i32_load8_u", (UInt32) 0xfedc6543), (UInt32) 0x43);
            assert(test.Call("i32_load8_u", (UInt32) 0x3456cdef), (UInt32) 0xef);
            assert(test.Call("i32_load16_s", (UInt32) 0xfedc6543), (UInt32) 0x6543);
            assert(test.Call("i32_load16_s", (UInt32) 0x3456cdef), (UInt32) 0xffffcdef);
            assert(test.Call("i32_load16_u", (UInt32) 0xfedc6543), (UInt32) 0x6543);
            assert(test.Call("i32_load16_u", (UInt32) 0x3456cdef), (UInt32) 0xcdef);

            assert64(test.Call("i64_load8_s", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("i64_load8_u", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 255);
            assert64(test.Call("i64_load16_s", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("i64_load16_u", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 65535);
            assert64(test.Call("i64_load32_s", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 0xFFFFFFFFFFFFFFFF);
            assert64(test.Call("i64_load32_u", (UInt64) 0xFFFFFFFFFFFFFFFF), (UInt64) 4294967295);

            assert64(test.Call("i64_load8_s", (UInt64) 100), (UInt64) 100);
            assert64(test.Call("i64_load8_u", (UInt64) 200), (UInt64) 200);
            assert64(test.Call("i64_load16_s", (UInt64) 20000), (UInt64) 20000);
            assert64(test.Call("i64_load16_u", (UInt64) 40000), (UInt64) 40000);
            assert64(test.Call("i64_load32_s", (UInt64) 20000), (UInt64) 20000);
            assert64(test.Call("i64_load32_u", (UInt64) 40000), (UInt64) 40000);

            assert64(test.Call("i64_load8_s", (UInt64) 0xfedcba9856346543), (UInt64) 0x43);
            assert64(test.Call("i64_load8_s", (UInt64) 0x3456436598bacdef), (UInt64) 0xffffffffffffffef);
            assert64(test.Call("i64_load8_u", (UInt64) 0xfedcba9856346543), (UInt64) 0x43);
            assert64(test.Call("i64_load8_u", (UInt64) 0x3456436598bacdef), (UInt64) 0xef);
            assert64(test.Call("i64_load16_s", (UInt64) 0xfedcba9856346543), (UInt64) 0x6543);
            assert64(test.Call("i64_load16_s", (UInt64) 0x3456436598bacdef), (UInt64) 0xffffffffffffcdef);
            assert64(test.Call("i64_load16_u", (UInt64) 0xfedcba9856346543), (UInt64) 0x6543);
            assert64(test.Call("i64_load16_u", (UInt64) 0x3456436598bacdef), (UInt64) 0xcdef);
            assert64(test.Call("i64_load32_s", (UInt64) 0xfedcba9856346543), (UInt64) 0x56346543);
            assert64(test.Call("i64_load32_s", (UInt64) 0x3456436598bacdef), (UInt64) 0xffffffff98bacdef);
            assert64(test.Call("i64_load32_u", (UInt64) 0xfedcba9856346543), (UInt64) 0x56346543);
            assert64(test.Call("i64_load32_u", (UInt64) 0x3456436598bacdef), (UInt64) 0x98bacdef);
        }
    }
}
