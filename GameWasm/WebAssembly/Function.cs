using System;
using System.Collections.Generic;
using System.Linq;

namespace GameWasm.Webassembly
{
    public class Function
    {
        string name = "";
        public UInt32 Index = 0;
        public Type Type;
        public bool Catcher = false;

        public Module.Module Module;

        Instruction.Instruction instruction;

        public List<byte> LocalTypes = new List<byte>();

        public Function(Module.Module module, byte[] parameters, byte[] results, Instruction.Instruction i)
        {
            this.Module = module;
            Type = new Type(parameters, results);
            instruction = i;
        }

        public Function(Module.Module module, UInt32 index, Type type)
        {
            this.Module = module;
            Index = index;
            name = "f" + (index);
            Type = type;
        }

        public Function(Module.Module module, Type type)
        {
            this.Module = module;
            name = "fnative";
            Type = type;
            instruction = new Instruction.Custom(NotImplemented);
        }

        public Function(Module.Module module, Func<object[], object[]> action, Type type)
        {
            this.Module = module;
            name = "fnative";
            Type = type;
            instruction = new Instruction.Custom(delegate
            {
                object[] ret = action(module.Store.Frames.Peek().Locals.ToArray());

                foreach (var v in ret)
                {
                    module.Store.Frames.Peek().Push(v);
                }
            });
        }

        protected void NotImplemented()
        {
            throw new Exception("Function not implemented: " + Module.Name + "@" + name);
        }

        public void AddLocal(byte type)
        {
            LocalTypes.Add(type);
        }

        public void SetInstruction(Instruction.Instruction instruction)
        {
            this.instruction = instruction;
        }

        public Instruction.Instruction GetInstruction()
        {
            return instruction;
        }

        public void SetName(string name)
        {
            name =  name + "(" + name + ")";
        }

        public string GetName()
        {
            return name;
        }
    }
}
