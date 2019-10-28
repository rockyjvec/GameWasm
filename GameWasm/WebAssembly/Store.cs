using System;
using System.Collections.Generic;
using System.IO;
using GameWasm.Webassembly.Stack;

namespace GameWasm.Webassembly
{
    public class Store
    {
        public Dictionary<string, Module.Module> Modules = new Dictionary<string, Module.Module>();
        public Stack<Stack.Frame> Frames = new Stack<Stack.Frame>();
        
        private int _stackMax = 1000;
            
        public Store()
        {
            Frames.Push(new Frame(this, null, null));
            init();
        }

        public Module.Module LoadModule(string name, string fileName)
        {         
            return LoadModule(name, File.ReadAllBytes(fileName));
        }

        public Module.Module LoadModule(string name, byte[] bytes)
        {
            return LoadModule(new Module.Module(name, this, bytes));
        }

        public Module.Module LoadModule(Module.Module module)
        {
            Modules.Add(module.Name, module);
            return module;
        }

        private void init()
        {
            LoadModule(new Module.Wasi(this));
        }

        public void CallFunction(Function f)
        {
            var frame = new Frame(this, f, f.GetInstruction());
            
            frame.Push(new Label(new Instruction.End(null), f.Type.Results));
            frame.Locals = new object[f.Type.Parameters.Length + f.LocalTypes.Count];
            int localIndex = f.Type.Parameters.Length;

            for (int i = f.Type.Parameters.Length - 1; i >= 0; i--)
            {
                // This might need to be reversed?
                var p = Frames.Peek().Pop();
                frame.Locals[i] = p;
                bool valid = false;
                switch (f.Type.Parameters[i])
                {
                    case Type.i32:
                        if (p is UInt32)
                            valid = true;
                        break;
                    case Type.i64:
                        if (p is UInt64)
                            valid = true;
                        break;
                    case Type.f32:
                        if (p is Single)
                            valid = true;
                        break;
                    case Type.f64:
                        if (p is Double)
                            valid = true;
                        break;
                }

                if (!valid)
                {
                    throw new Trap("indirect call type mismatch");
                }
            }

            foreach (var t in f.LocalTypes)
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
            
            Frames.Push(frame);
        }
        
        // Returning false means execution is complete
        public bool Step(int count = 1)
        {
            bool exception = true;

            try
            {
                if (Frames.Count > _stackMax)
                {
                    throw new Trap("call stack exhausted");
                }

                for (int step = 0; step < count; step++)
                {
                    if (Frames.Count == 1)
                    {
                        exception = false;
                        return false;
                    }
                    else
                    {
                        if (Frames.Peek().Instruction == null)
                        {
                            if(Frames.Peek().Function == null)
                            {
                                exception = false;
                                return false;
                            }
                            else
                            {
                                // Handle return 
                                var lastFrame = Frames.Pop();
                                var currentFrame = Frames.Peek();

                                foreach (var r in lastFrame.Function.Type.Results)
                                {
                                    try
                                    {
                                        switch (r)
                                        {
                                            case Type.i32:
                                                currentFrame.Push(lastFrame.PopI32());
                                                break;
                                            case Type.i64:
                                                currentFrame.Push(lastFrame.PopI64());
                                                break;
                                            case Type.f32:
                                                currentFrame.Push(lastFrame.PopF32());
                                                break;
                                            case Type.f64:
                                                currentFrame.Push(lastFrame.PopF64());
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

                                if (Frames.Count == 1)
                                {
                                    exception = false;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            Frames.Peek().Instruction = Frames.Peek().Instruction.Run(Frames.Peek());
                        }
                    }
                }

                exception = false;
            }
            finally
            {
                if (exception)
                {
                    Frames.Clear();
                    Frames.Push(new Frame(this, null, null));
                }
            }

            return true;
        }
    }
}
