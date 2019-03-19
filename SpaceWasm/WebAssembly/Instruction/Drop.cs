using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class Drop : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.PopValue();

            Console.WriteLine(this.Next);
            return this.Next;
        }

        public Drop(Parser parser) : base(parser, true)
        {
            
        }
    }
}
