﻿using System;
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

        private Module.Module module;

        Instruction.Instruction instruction;

        public List<byte> LocalTypes = new List<byte>();

        public Function(Module.Module module, byte[] parameters, byte[] results, Instruction.Instruction i)
        {
            this.module = module;
            Type = new Type(parameters, results);
            instruction = i;
        }

        public Function(Module.Module module, UInt32 index, Type type)
        {
            this.module = module;
            Index = index;
            name = "f" + (index);
            Type = type;
        }

        public Function(Module.Module module, Type type)
        {
            this.module = module;
            name = "fnative";
            Type = type;
            instruction = new Instruction.Custom(NotImplemented);
        }

        public Function(Module.Module module, Func<object[], object[]> action, Type type)
        {
            this.module = module;
            name = "fnative";
            Type = type;
            instruction = new Instruction.Custom(delegate
            {
                object[] ret = action(module.Store.CurrentFrame.Locals.ToArray());

                foreach (var v in ret)
                {
                    module.Store.Stack.Push(v);
                }
            });
        }

        protected void NotImplemented()
        {
            throw new Exception("Function not implemented: " + module.Name + "@" + name);
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

        public Stack.Frame CreateFrame(Store store, Module.Module module, object[] parameters)
        {
            var frame = new Stack.Frame(store, module, this, instruction);

            frame.Locals = new object[parameters.Length + LocalTypes.Count()];
            frame.Results = new object[Type.Results.Length];
            int localIndex = 0;

            foreach (var p in parameters.Reverse())
            {
                frame.Locals[localIndex++] = p;
            }

            foreach (var t in LocalTypes)
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
                        local = (Single)0;
                        break;
                    case Type.f64:
                        local = (Double)0;
                        break;
                    default:
                        throw new Exception("Invalid local type: 0x" + t.ToString("X"));
                }

                frame.Locals[localIndex++] = local;
            }

            return frame;
        }

        public void NativeCall()
        {
            object[] parameters = new object[Type.Parameters.Length];
            for (int i = 0; i < Type.Parameters.Length; i++)
            {
                parameters[i] = module.Store.Stack.Pop();
                bool valid = false;
                switch (Type.Parameters[Type.Parameters.Length - i - 1])
                {
                    case Type.i32:
                        if (parameters[i] is UInt32)
                            valid = true;
                        break;
                    case Type.i64:
                        if (parameters[i] is UInt64)
                            valid = true;
                        break;
                    case Type.f32:
                        if (parameters[i] is Single)
                            valid = true;
                        break;
                    case Type.f64:
                        if (parameters[i] is Double)
                            valid = true;
                        break;
                }

                if (!valid)
                {
                    throw new Trap("indirect call type mismatch");
                }
            }

            module.Store.Stack.PushFrame(CreateFrame(module.Store, module, parameters));
            module.Store.Stack.Push(new Stack.Label(new Instruction.End(null), Type.Results));
        }

        public void SetName(string name)
        {
            name =  name + "(" + name + ")";
        }

        public string GetName()
        {
            return name;
        }

        public void HandleReturn(Store store)
        {
            int resultIndex = 0;
            foreach (var r in store.CurrentFrame.Function.Type.Results)
            {
                try
                {
                    switch (r)
                    {
                        case Type.i32:
                            store.CurrentFrame.Results[resultIndex++] = store.Stack.PopI32();
                            break;
                        case Type.i64:
                            store.CurrentFrame.Results[resultIndex++] = store.Stack.PopI64();
                            break;
                        case Type.f32:
                            store.CurrentFrame.Results[resultIndex++] = store.Stack.PopF32();
                            break;
                        case Type.f64:
                            store.CurrentFrame.Results[resultIndex++] = store.Stack.PopF64();
                            break;
                        default:
                            throw new Exception("Invalid return type");
                    }
                }
                catch (System.InvalidCastException e)
                {
                    throw new Trap("indirect call type mismatch");
                }
            }

            while (store.Stack.Size > 0 && (store.Stack.Peek() as Stack.Frame) == null)
            {
                store.Stack.Pop();
            }
        }
    }
}