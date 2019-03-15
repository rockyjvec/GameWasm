using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Module.Global
{
    class Math : Module
    {
        public Math(Store store) : base("global.Math", store)
        {
            this.Exports.Add("pow", new Function(this.Pow, new Type(new byte[] { Type.f64, Type.f64 }, new byte[] { Type.f64 })));
        }

        private object[] Pow(object[] parameters)
        {
            object[] result = new object[1];
            result[0] = (float)System.Math.Pow((double)parameters[0], (double)parameters[1]);
            return result;
        }
    }
}
