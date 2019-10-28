using System;

namespace GameWasm.Webassembly.Test
{
    class MemoryGrow : Test
    {
        public MemoryGrow(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "memory_grow1.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            assert(test.Call("size"), (UInt32) 0);
            assert_trap(delegate { test.CallVoid("store_at_zero"); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("load_at_zero"); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("store_at_page_size"); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("load_at_page_size"); }, "out of bounds memory access");
            assert(test.Call("grow", (UInt32) 1), (UInt32) 0);
            assert(test.Call("size"), (UInt32) 1);
            assert(test.Call("load_at_zero"), (UInt32) 0);
            test.CallVoid("store_at_zero");
            assert(test.Call("load_at_zero"), (UInt32) 2);
            assert_trap(delegate { test.CallVoid("store_at_page_size"); }, "out of bounds memory access");
            assert_trap(delegate { test.CallVoid("load_at_page_size"); }, "out of bounds memory access");
            assert(test.Call("grow", (UInt32) 4), (UInt32) 1);
            assert(test.Call("size"), (UInt32) 5);
            assert(test.Call("load_at_zero"), (UInt32) 2);
            test.CallVoid("store_at_zero");
            assert(test.Call("load_at_zero"), (UInt32) 2);
            assert(test.Call("load_at_page_size"), (UInt32) 0);
            test.CallVoid("store_at_page_size");
            assert(test.Call("load_at_page_size"), (UInt32) 3);
            
            filename = "memory_grow2.wasm";

            store = new Store();
            test = store.LoadModule("test", path + '/' + filename);

            assert(test.Call("grow", (UInt32) 0), (UInt32) 0);
            assert(test.Call("grow", (UInt32) 1), (UInt32) 0);
            assert(test.Call("grow", (UInt32) 0), (UInt32) 1);
            assert(test.Call("grow", (UInt32) 2), (UInt32) 1);
            assert(test.Call("grow", (UInt32) 800), (UInt32) 3);
            assert(test.Call("grow", (UInt32) 0x10000), (UInt32) 0xFFFFFFFF);
            assert(test.Call("grow", (UInt32) 64736), (UInt32) 0xFFFFFFFF);
            assert(test.Call("grow", (UInt32) 1), (UInt32) 803);

            filename = "memory_grow3.wasm";

            store = new Store();
            test = store.LoadModule("test", path + '/' + filename);

            assert(test.Call("grow", (UInt32) 0), (UInt32) 0);
            assert(test.Call("grow", (UInt32) 1), (UInt32) 0);
            assert(test.Call("grow", (UInt32) 1), (UInt32) 1);
            assert(test.Call("grow", (UInt32) 2), (UInt32) 2);
            assert(test.Call("grow", (UInt32) 6), (UInt32) 4);
            assert(test.Call("grow", (UInt32) 0), (UInt32) 10);
            assert(test.Call("grow", (UInt32) 1), (UInt32) 0xFFFFFFFF);
            assert(test.Call("grow", (UInt32) 0x10000), (UInt32) 0xFFFFFFFF);
                

            filename = "memory_grow4.wasm";

            store = new Store();
            test = store.LoadModule("test", path + '/' + filename);
            
            assert(test.Call("check-memory-zero", (UInt32) 0, (UInt32) 0xffff), (UInt32) 0);
            assert(test.Call("grow", (UInt32) 1), (UInt32) 1);
            assert(test.Call("check-memory-zero", (UInt32) 0x10000, (UInt32) 0x1_ffff), (UInt32) 0);
            assert(test.Call("grow", (UInt32) 1), (UInt32) 2);
            assert(test.Call("check-memory-zero", (UInt32) 0x20000, (UInt32) 0x2_ffff), (UInt32) 0);
            assert(test.Call("grow", (UInt32) 1), (UInt32) 3);
            assert(test.Call("check-memory-zero", (UInt32) 0x30000, (UInt32) 0x3_ffff), (UInt32) 0);
            assert(test.Call("grow", (UInt32) 1), (UInt32) 4);
            assert(test.Call("check-memory-zero", (UInt32) 0x40000, (UInt32) 0x4_ffff), (UInt32) 0);
            assert(test.Call("grow", (UInt32) 1), (UInt32) 5);
            assert(test.Call("check-memory-zero", (UInt32) 0x50000, (UInt32) 0x5_ffff), (UInt32) 0);
                
                
            filename = "memory_grow5.wasm";

            store = new Store();
            test = store.LoadModule("test", path + '/' + filename);


            assert(test.Call("as-br-value"), (UInt32) 1);

            test.CallVoid("as-br_if-cond");
            assert(test.Call("as-br_if-value"), (UInt32) 1);
            assert(test.Call("as-br_if-value-cond"), (UInt32) 6);

            test.CallVoid("as-br_table-index");
            assert(test.Call("as-br_table-value"), (UInt32) 1);
            assert(test.Call("as-br_table-value-index"), (UInt32) 6);

            assert(test.Call("as-return-value"), (UInt32) 1);

            assert(test.Call("as-if-cond"), (UInt32) 0);
            assert(test.Call("as-if-then"), (UInt32) 1);
            assert(test.Call("as-if-else"), (UInt32) 1);

            assert(test.Call("as-select-first", (UInt32) 0, (UInt32) 1), (UInt32) 1);
            assert(test.Call("as-select-second", (UInt32) 0, (UInt32) 0), (UInt32) 1);
            assert(test.Call("as-select-cond"), (UInt32) 0);

            assert(test.Call("as-call-first"), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-call-mid"), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-call-last"), (UInt32) 0xFFFFFFFF);

            assert(test.Call("as-call_indirect-first"), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-call_indirect-mid"), (UInt32) 0xFFFFFFFF);
            assert(test.Call("as-call_indirect-last"), (UInt32) 0xFFFFFFFF);
            assert_trap(delegate { test.CallVoid("as-call_indirect-index"); }, "undefined element");

            test.CallVoid("as-local.set-value");
            assert(test.Call("as-local.tee-value"), (UInt32) 1);
            test.CallVoid("as-global.set-value");

            assert(test.Call("as-load-address"), (UInt32) 0);
            assert(test.Call("as-loadN-address"), (UInt32) 0);
            test.CallVoid("as-store-address");
            test.CallVoid("as-store-value");
            test.CallVoid("as-storeN-address"); 
            test.CallVoid("as-storeN-value");

            assert(test.Call("as-unary-operand"), (UInt32) 31);

            assert(test.Call("as-binary-left"), (UInt32) 11);
            assert(test.Call("as-binary-right"), (UInt32) 9);

            assert(test.Call("as-test-operand"), (UInt32) 0);

            assert(test.Call("as-compare-left"), (UInt32) 1);
            assert(test.Call("as-compare-right"), (UInt32) 1);

            assert(test.Call("as-memory.grow-size"), (UInt32) 1);
        }
    }
}
