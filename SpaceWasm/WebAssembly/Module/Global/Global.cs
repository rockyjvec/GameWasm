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
            this.AddExportGlob("NaN", double.NaN);
            this.AddExportGlob("Infinity", double.PositiveInfinity);
        }
    }
}
