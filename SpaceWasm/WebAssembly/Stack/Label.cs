using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Stack
{
    public class Label
    {
        public UInt32 IP;
        public Label(UInt32 IP)
        {
            this.IP = IP;
        }
    }
}
