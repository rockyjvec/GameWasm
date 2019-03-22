using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Test
{
    class BreakDrop : Test
    {
        public BreakDrop(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "break-drop.wasm";

            var store = new Store();
            var test = store.LoadModule("test", this.path + '/' + filename);

            test.CallVoid("br");
            test.CallVoid("br_if");
            test.CallVoid("br_table");

        }
    }
}
