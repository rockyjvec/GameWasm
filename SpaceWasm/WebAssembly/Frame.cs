using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly
{
    class Frame
    {
        Module.Module module;

        public UInt32 IP;

        public Frame(Module.Module module, UInt32 startIP)
        {
            this.IP = startIP;
        }
    }
}
