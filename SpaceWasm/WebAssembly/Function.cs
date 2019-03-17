using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly
{
    public class Function
    {
        bool native = false;
        string name = "";
        public UInt32 Index = 0;
        Func<object[], object[]> action;
        public Type Type;
        private Module.Module module;

        Instruction.Instruction instruction;

        List<byte> localTypes = new List<byte>();

        public Function(Module.Module module, UInt32 index, Type type)
        {
            this.module = module;
            this.Index = index;
            this.name = "f" + (index - 1);
            this.Type = type;
        }

        public Function(Module.Module module, Func<object[], object[]> action, Type type)
        {
            this.module = module;
            this.native = true;
            this.name = "fnative";
            this.action = action;
            this.Type = type;
        }

        public void AddLocal(byte type)
        {
            this.localTypes.Add(type);
        }

        public void SetInstruction(Instruction.Instruction instruction)
        {
            this.instruction = instruction;
        }

        public Instruction.Instruction GetInstruction()
        {
            return this.instruction;
        }

        public Stack.Frame CreateFrame(Store store, Module.Module module, object[] parameters)
        {
            var frame = new Stack.Frame(store, module, this.instruction);
            foreach (var p in parameters)
            {
                frame.Locals.Add(p);
            }

            foreach (var t in this.localTypes)
            {
                object local;
                switch(t)
                {
                    case Type.i32:
                        local = (UInt32)0;
                        break;
                    case Type.i64:
                        local = (UInt64)0;
                        break;
                    case Type.f32:
                        local = (float)0;
                        break;
                    case Type.f64:
                        local = (double)0;
                        break;
                    default:
                        throw new Exception("Invalid local type: 0x" + t.ToString("X"));
                }
                frame.Locals.Add(local);
            }

            Console.WriteLine("locals: " + frame.Locals.Count());
            return frame;
        }

        public void Call(object[] parameters)
        {
            Console.WriteLine("Executing " + this.GetName());

            if (parameters.Length != this.Type.Parameters.Length)
            {
                throw new Exception("Invalid number of parameters in function call.");
            }

            for(int i = 0; i < parameters.Length; i++)
            {
                bool valid = false;
                switch (this.Type.Parameters[0])
                {
                    case Type.i32:
                        if(parameters[i].GetType().ToString() == "System.UInt32")
                            valid = true;
                        break;
                    case Type.i64:
                        if (parameters[i].GetType().ToString() == "System.UInt64")
                            valid = true;
                        break;
                    case Type.f32:
                        if (parameters[i].GetType().ToString() == "System.UInt32")
                            valid = true;
                        break;
                    case Type.f64:
                        if (parameters[i].GetType().ToString() == "System.Double")
                            valid = true;
                        break;
                }

                if (!valid)
                {
                    throw new Exception("Parameter type mismatch");
                }
            }

            if (this.module.Store.CurrentFrame == null)
            {
                this.module.Store.CurrentFrame = this.CreateFrame(this.module.Store, this.module, parameters);
            }
            else
            {
                this.module.Store.Stack.PushFrame(this.CreateFrame(this.module.Store, this.module, parameters));
            }
        }

        public void NativeCall()
        {
            Console.WriteLine("Executing " + this.GetName());

            object[] parameters = new object[this.Type.Parameters.Length];
            for (int i = 0; i < this.Type.Parameters.Length; i++)
            {
                parameters[i] = this.module.Store.Stack.Pop();
                bool valid = false;
                switch (this.Type.Parameters[0])
                {
                    case Type.i32:
                        if (parameters[i].GetType().ToString() == "System.UInt32")
                            valid = true;
                        break;
                    case Type.i64:
                        if (parameters[i].GetType().ToString() == "System.UInt64")
                            valid = true;
                        break;
                    case Type.f32:
                        if (parameters[i].GetType().ToString() == "System.UInt32")
                            valid = true;
                        break;
                    case Type.f64:
                        if (parameters[i].GetType().ToString() == "System.Double")
                            valid = true;
                        break;
                }

                if (!valid)
                {
                    throw new Exception("Parameter type mismatch");
                }
            }

            if (this.module.Store.CurrentFrame == null)
            {
                this.module.Store.CurrentFrame = this.CreateFrame(this.module.Store, this.module, parameters);
            }
            else
            {
                this.module.Store.Stack.PushFrame(this.CreateFrame(this.module.Store, this.module, parameters));
            }
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return this.name;
        }
    }
}
