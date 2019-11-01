namespace GameWasm.Webassembly.Test
{
    class StoreTest : Test
    {
        public StoreTest(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "store.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);
/*
            test.CallVoid("as-block-value");
            test.CallVoid("as-loop-value");

            test.CallVoid("as-br-value");
            test.CallVoid("as-br_if-value");
            test.CallVoid("as-br_if-value-cond");
            test.CallVoid("as-br_table-value");
*/
            test.CallVoid("as-return-value");

            test.CallVoid("as-if-then");
            test.CallVoid("as-if-else");

        }
    }
}
