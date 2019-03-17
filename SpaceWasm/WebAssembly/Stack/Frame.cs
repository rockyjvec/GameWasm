using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Stack
{
    public class Frame
    {
        public Store Store;
        public Module.Module Module;

        public Instruction.Instruction Instruction;
        public List<object> Locals = new List<object>();
        public Stack<Instruction.Instruction> Labels = new Stack<Instruction.Instruction>();

        public Frame(Store store, Module.Module module, Instruction.Instruction instruction)
        {
            this.Store = store;
            this.Module = module;
            this.Instruction = instruction;
        }

        public bool Step(bool debug = false)
        {
            if(debug)
                Console.WriteLine(this.Instruction);
            this.Instruction = this.Instruction.Run(this.Store);

            return !(this.Instruction == null);
        }
    }
}
