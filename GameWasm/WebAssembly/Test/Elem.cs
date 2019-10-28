using System;

namespace GameWasm.Webassembly.Test
{
    class Elem : Test
    {
        public Elem(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "elem.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            assert(test.Call("call-7"), (UInt32) 65);
            assert(test.Call("call-9"), (UInt32) 66);
        }
    }
}
