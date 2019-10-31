using System;
using System.Collections.Generic;
using System.Linq;
using GameWasm.Webassembly.New;

namespace GameWasm.Webassembly
{
    public class Function
    {
        public Module.Module Module;
        public string Name = "";
        public Type Type;
        public UInt32 Index = 0;
        public int GlobalIndex = 0;
        public Inst[] program = null;
        public Func<Value[], Value[]> native = null;

        public List<byte> LocalTypes = new List<byte>();

        // Standard constructor
        public Function(Module.Module module, String name, Type type = null, UInt32 index = 0xFFFFFFFF, Instruction.Instruction start = null)
        {
            Module = module;
            Name = name;
            Index = index;
            GlobalIndex = Module.Store.runtime.functions.Count;
            Module.Store.runtime.functions.Add(this);

            Type = new Type(new byte[] { }, new byte[] { });
            if (type != null)
            {
                Type = type;
            }
        }
        
        // Native function constructor
        public Function(Module.Module module, String name, Func<Value[], Value[]> action, Type type)
        {
            this.Module = module;
            Name = name;
            Type = type;
            GlobalIndex = Module.Store.runtime.functions.Count;
            Module.Store.runtime.functions.Add(this);
/*            Start = new Instruction.Custom(delegate
            {
                Value[] ret = action(module.Store.CurrentFrame.Locals.ToArray());

                foreach (var v in ret)
                {
                    module.Store.CurrentFrame.Push(v);
                }
            });*/
        }

        protected void NotImplemented()
        {
            throw new Exception("Function not implemented: " + Module.Name + "@" + Name);
        }
        
        public void SetName(string name)
        {
            name =  name + "(" + name + ")";
        }

        public string GetName()
        {
            return Name;
        }
    }
}
