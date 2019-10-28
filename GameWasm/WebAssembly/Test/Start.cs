using System;

namespace GameWasm.Webassembly.Test
{
    class Start : Test
    {
        public Start(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "start1.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            assert(test.Call("get"), (UInt32) 68);
            test.CallVoid("inc");
            assert(test.Call("get"), (UInt32) 69);
            test.CallVoid("inc");
            assert(test.Call("get"), (UInt32) 70);
                

            filename = "start2.wasm";
            
            store = new Store();
            test = store.LoadModule("test", path + '/' + filename);

            assert(test.Call("get"), (UInt32) 68);
            test.CallVoid("inc");
            assert(test.Call("get"), (UInt32) 69);
            test.CallVoid("inc");
            assert(test.Call("get"), (UInt32) 70);
        }
    }
}
