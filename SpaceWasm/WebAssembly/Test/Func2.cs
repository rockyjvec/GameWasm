using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class Func2 : Test
    {
        public Func2(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "func2.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            test.CallVoid("signature-explicit-reused");
            test.CallVoid("signature-implicit-reused");
            test.CallVoid("signature-explicit-duplicate");
            test.CallVoid("signature-implicit-duplicate");
        }
    }
}
