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
        public Function Function;

        public Instruction.Instruction Instruction;
        public object[] Locals;

        public object[] Results;

        public Frame(Store store, Module.Module module, Function function, Instruction.Instruction instruction)
        {
            this.Store = store;
            this.Module = module;
            this.Function = function;
            this.Instruction = instruction;
        }
    }
}
