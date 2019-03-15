using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Module
{
    class Asm2Wasm : Module
    {
        public Asm2Wasm(Store store) : base("asm2wasm", store)
        {
            this.AddExportFunc("f64-to-int", new byte[] { Type.f64 }, new byte[] { Type.i32 });
        }
    }
}
