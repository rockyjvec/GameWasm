using System;

namespace GameWasm.Webassembly.Test
{
    class MemoryRedundancy : Test
    {
        public MemoryRedundancy(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "memory_redundancy.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            assert(test.Call("test_store_to_load"), (UInt32) 0x00000080);
            test.CallVoid("zero_everything");
            assert(test.Call("test_redundant_load"), (UInt32) 0x00000080);
            test.CallVoid("zero_everything");
            assertF32(test.Call("test_dead_store"), (float)4.904545E-44);// 0x1.18p - 144);
            test.CallVoid("zero_everything");
            assert(test.Call("malloc_aliasing"), (UInt32) 43);
        }
    }
}
