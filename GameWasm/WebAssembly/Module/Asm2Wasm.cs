namespace GameWasm.Webassembly.Module
{
    class Asm2Wasm : Module
    {
        public Asm2Wasm(Store store) : base("asm2wasm", store)
        {
            //   AddExportFunc("f64-to-int", new byte[] { Type.f64 }, new byte[] { Type.i32 });
            AddExportFunc("f64-rem", new byte[] { Type.f64, Type.f64 }, new byte[] { Type.f64 });

        }
    }
}
