using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Stack
{
    public class Label
    {
        public Instruction.Instruction Instruction;
        public byte[] Type;
        public Label(Instruction.Instruction i, byte[] type)
        {
            if (type.Length == 1 && type[0] == 64) this.Type = new byte[] { };
            else this.Type = type;
            this.Instruction = i;
        }
    }
}
