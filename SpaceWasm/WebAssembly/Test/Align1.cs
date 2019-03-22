using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class Align1 : Test
    {
        public Align1(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "align1.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            assertF32(test.Call("f32_align_switch", (UInt32) 0), (float) 10.0);
            assertF32(test.Call("f32_align_switch", (UInt32) 1), (float) 10.0);
            assertF32(test.Call("f32_align_switch", (UInt32) 2), (float) 10.0);
            assertF32(test.Call("f32_align_switch", (UInt32) 3), (float) 10.0);

            assertF64(test.Call("f64_align_switch", (UInt32) 0), (double) 10.0);
            assertF64(test.Call("f64_align_switch", (UInt32) 1), (double) 10.0);
            assertF64(test.Call("f64_align_switch", (UInt32) 2), (double) 10.0);
            assertF64(test.Call("f64_align_switch", (UInt32) 3), (double) 10.0);
            assertF64(test.Call("f64_align_switch", (UInt32) 4), (double) 10.0);

            assert(test.Call("i32_align_switch", (UInt32) 0, (UInt32) 0), (UInt32) 10);
            assert(test.Call("i32_align_switch", (UInt32) 0, (UInt32) 1), (UInt32) 10);
            assert(test.Call("i32_align_switch", (UInt32) 1, (UInt32) 0), (UInt32) 10);
            assert(test.Call("i32_align_switch", (UInt32) 1, (UInt32) 1), (UInt32) 10);
            assert(test.Call("i32_align_switch", (UInt32) 2, (UInt32) 0), (UInt32) 10);
            assert(test.Call("i32_align_switch", (UInt32) 2, (UInt32) 1), (UInt32) 10);
            assert(test.Call("i32_align_switch", (UInt32) 2, (UInt32) 2), (UInt32) 10);
            assert(test.Call("i32_align_switch", (UInt32) 3, (UInt32) 0), (UInt32) 10);
            assert(test.Call("i32_align_switch", (UInt32) 3, (UInt32) 1), (UInt32) 10);
            assert(test.Call("i32_align_switch", (UInt32) 3, (UInt32) 2), (UInt32) 10);
            assert(test.Call("i32_align_switch", (UInt32) 4, (UInt32) 0), (UInt32) 10);
            assert(test.Call("i32_align_switch", (UInt32) 4, (UInt32) 1), (UInt32) 10);
            assert(test.Call("i32_align_switch", (UInt32) 4, (UInt32) 2), (UInt32) 10);
            assert(test.Call("i32_align_switch", (UInt32) 4, (UInt32) 4), (UInt32) 10);

            assert64(test.Call("i64_align_switch", (UInt32) 0, (UInt32) 0), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 0, (UInt32) 1), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 1, (UInt32) 0), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 1, (UInt32) 1), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 2, (UInt32) 0), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 2, (UInt32) 1), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 2, (UInt32) 2), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 3, (UInt32) 0), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 3, (UInt32) 1), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 3, (UInt32) 2), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 4, (UInt32) 0), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 4, (UInt32) 1), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 4, (UInt32) 2), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 4, (UInt32) 4), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 5, (UInt32) 0), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 5, (UInt32) 1), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 5, (UInt32) 2), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 5, (UInt32) 4), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 6, (UInt32) 0), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 6, (UInt32) 1), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 6, (UInt32) 2), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 6, (UInt32) 4), (UInt64) 10);
            assert64(test.Call("i64_align_switch", (UInt32) 6, (UInt32) 8), (UInt64) 10);
        }
    }
}
