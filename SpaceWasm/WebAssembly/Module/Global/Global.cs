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
            this.AddExportGlob("NaN", Type.f64, false, double.NaN);
            this.AddExportGlob("Infinity", Type.f64, false, double.PositiveInfinity);
        }
    }
}
