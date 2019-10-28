using System;

namespace GameWasm.Webassembly.Test
{
    class Fac : Test
    {
        public Fac(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "fac.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);


            assert64(test.Call("fac-rec", (UInt64) 25), (UInt64) 7034535277573963776);
            assert64(test.Call("fac-iter", (UInt64) 25), (UInt64) 7034535277573963776);
            assert64(test.Call("fac-rec-named", (UInt64) 25), (UInt64) 7034535277573963776);
            assert64(test.Call("fac-iter-named", (UInt64) 25), (UInt64) 7034535277573963776);
            assert64(test.Call("fac-opt", (UInt64) 25), (UInt64) 7034535277573963776);            
        }
    }
}
