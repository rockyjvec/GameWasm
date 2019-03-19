using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly
{
    public class Trap : Exception
    {
        public string Details;

        public Trap(string message, string details = "") : base(message)
        {
            this.Details = details;
        }
    }
}
