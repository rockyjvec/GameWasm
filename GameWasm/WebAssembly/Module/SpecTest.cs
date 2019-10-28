namespace GameWasm.Webassembly.Module
{
    class SpecTest : Module
    {
        public SpecTest(Store store) : base("spectest", store)
        {
            //   AddExportFunc("f64-to-int", new byte[] { Type.f64 }, new byte[] { Type.i32 });
            AddExportFunc("print_i32", new byte[] { Type.i32 }, new byte[] { }, PrintI32 );

        }

        public object[] PrintI32(object[] parameters)
        {
            return new object[] { };
        }
    }
}
