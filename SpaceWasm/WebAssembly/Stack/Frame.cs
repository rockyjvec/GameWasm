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
        public List<Value> Locals = new List<Value>();
        public Stack<Instruction.Instruction> Labels = new Stack<Instruction.Instruction>();
        public WebAssembly.Stack.Stack Stack = new WebAssembly.Stack.Stack();

        public Frame(Store store, Module.Module module, Instruction.Instruction instruction)
        {
            this.Store = store;
            this.Module = module;
            this.Instruction = instruction;
        }

        public bool Step()
        {
            this.Instruction = this.Instruction.Run(this.Store);

            return !(this.Instruction == null);
        }
    }
}
