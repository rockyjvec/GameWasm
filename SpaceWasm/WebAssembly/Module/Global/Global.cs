using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Module.Global
{
    class Global : Module
    {
        public Global(Store store) : base("global", store)
        {
            this.AddExportGlob("NaN", new Value(Type.f64, false, 0));
            this.AddExportGlob("Infinity", new Value(Type.f64, false, 1));
        }
    }
}
