using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class Func1 : Test
    {
        public Func1(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "func1.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            test.CallVoid("type-use-1");
            assert(test.Call("type-use-2"), (UInt32)0);
            test.CallVoid("type-use-3", (UInt32)1);

            assert(test.Call("type-use-4", (UInt32)1, (double)1, (UInt32)1), (UInt32)0);
            assert(test.Call("type-use-5"), (UInt32)0);
            test.CallVoid("type-use-6", (UInt32)1);
            assert(test.Call("type-use-7", (UInt32)1, (double)1, (UInt32)1), (UInt32)0);

            assert(test.Call("local-first-i32"), (UInt32)0);
            assert64(test.Call("local-first-i64"), (UInt64)0);
            assertF32(test.Call("local-first-f32"), (float)0);
            assertF64(test.Call("local-first-f64"), (double)0);
            assert(test.Call("local-second-i32"), (UInt32)0);
            assert64(test.Call("local-second-i64"), (UInt64)0);
            assertF32(test.Call("local-second-f32"), (float)0);
            assertF64(test.Call("local-second-f64"), (double)0);
            assertF64(test.Call("local-mixed"), (double)0);

            assert(test.Call("param-first-i32", (UInt32)2, (UInt32)3), (UInt32)2);
            assert64(test.Call("param-first-i64", (UInt64)2, (UInt64)3), (UInt64)2);
            assertF32(test.Call("param-first-f32", (float)2, (float)3), (float)2);
            assertF64(test.Call("param-first-f64", (double)2, (double)3), (double)2);
            assert(test.Call("param-second-i32", (UInt32)2, (UInt32)3), (UInt32)3);
            assert64(test.Call("param-second-i64", (UInt64)2, (UInt64)3), (UInt64)3);
            assertF32(test.Call("param-second-f32", (float)2, (float)3), (float)3);
            assertF64(test.Call("param-second-f64", (double)2, (double)3), (double)3);

            assertF64(test.Call("param-mixed", (float)1, (UInt32)2, (UInt64)3, (UInt32)4, (double)5.5, (UInt32)6), (double)5.5);

            test.CallVoid("empty");
            test.CallVoid("value-void");
            assert(test.Call("value-i32"), (UInt32)77);
            assert64(test.Call("value-i64"), (UInt64)7777);
            assertF32(test.Call("value-f32"), (float)77.7);
            assertF64(test.Call("value-f64"), (double)77.77);

            test.CallVoid("value-block-void");
            assert(test.Call("value-block-i32"), (UInt32)77);

            test.CallVoid("return-empty");
            assert(test.Call("return-i32"), (UInt32)78);
            assert64(test.Call("return-i64"), (UInt64)7878);
            assertF32(test.Call("return-f32"), (float)78.7);
            assertF64(test.Call("return-f64"), (double)78.78);
            assert(test.Call("return-block-i32"), (UInt32)77);

            test.CallVoid("break-empty");
            assert(test.Call("break-i32"), (UInt32)79);
            assert64(test.Call("break-i64"), (UInt64)7979);
            assertF32(test.Call("break-f32"), (float)79.9);
            assertF64(test.Call("break-f64"), (double)79.79);
            assert(test.Call("break-block-i32"), (UInt32)77);

            test.CallVoid("break-br_if-empty", (UInt32)0);
            test.CallVoid("break-br_if-empty", (UInt32)2);
            assert(test.Call("break-br_if-num", (UInt32)0), (UInt32)51);
            assert(test.Call("break-br_if-num", (UInt32)1), (UInt32)50);

            test.CallVoid("break-br_table-empty", (UInt32)0);
            test.CallVoid("break-br_table-empty", (UInt32)1);
            test.CallVoid("break-br_table-empty", (UInt32)5);
            test.CallVoid("break-br_table-empty", (UInt32)0xFFFFFFFF);
            assert(test.Call("break-br_table-num", (UInt32)0), (UInt32)50);
            assert(test.Call("break-br_table-num", (UInt32)1), (UInt32)50);
            assert(test.Call("break-br_table-num", (UInt32)10), (UInt32)50);
            assert(test.Call("break-br_table-num", (UInt32)0xFFFFFF9C), (UInt32)50);

            test.CallVoid("break-br_table-nested-empty", (UInt32)0);
            test.CallVoid("break-br_table-nested-empty", (UInt32)1);
            test.CallVoid("break-br_table-nested-empty", (UInt32)3);
            test.CallVoid("break-br_table-nested-empty", (UInt32)0xFFFFFFFE);

            assert(test.Call("break-br_table-nested-num", (UInt32)0), (UInt32)52);
            assert(test.Call("break-br_table-nested-num", (UInt32)1), (UInt32)50);
            assert(test.Call("break-br_table-nested-num", (UInt32)2), (UInt32)52);
            assert(test.Call("break-br_table-nested-num", (UInt32)0xFFFFFFFD), (UInt32)52);

            assert(test.Call("init-local-i32"), (UInt32)0);
            assert64(test.Call("init-local-i64"), (UInt64)0);
            assertF32(test.Call("init-local-f32"), (float)0);
            assertF64(test.Call("init-local-f64"), (double)0);
        }
    }
}
