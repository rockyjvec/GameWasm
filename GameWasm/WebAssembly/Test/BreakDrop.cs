namespace GameWasm.Webassembly.Test
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
            var test = store.LoadModule("test", path + '/' + filename);

            test.CallVoid("br");
            test.CallVoid("br_if");
            test.CallVoid("br_table");

        }
    }
}
