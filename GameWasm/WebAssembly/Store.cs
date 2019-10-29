using System;
using System.Collections.Generic;
using System.IO;
using GameWasm.Webassembly.Stack;

namespace GameWasm.Webassembly
{
    public class Store
    {
        public Frame CurrentFrame;
        public Dictionary<string, Module.Module> Modules = new Dictionary<string, Module.Module>();
        
        private int _stackMax = 1000;
        private readonly Stack<Frame> _stack;

        public Store()
        {
            _stack = new Stack<Frame>();
            Push(new Frame( null, null, new Value[] { }));
            LoadModule(new Module.Wasi(this));
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
        
        public void Push(Frame f)
        {
            if (_stack.Count > _stackMax)
            {
                throw new Trap("call stack exhausted");
            }

            CurrentFrame = f;
            _stack.Push(f);
        }

        public Frame Pop()
        {
            if (_stack.Count == 0)
            {
                throw new Trap("call stack exhausted");
            }
            
            var last = _stack.Pop();
            if (_stack.Count == 0)
            {
                CurrentFrame = null;
            }
            else
            {
                CurrentFrame = _stack.Peek();
            }
            
            return last;
        }

        public void Clear()
        {
            _stack.Clear();
        }
        
        public void CallFunction(Function f)
        {
            var frame = new Frame(f, f.Start, new Value[f.Type.Parameters.Length + f.LocalTypes.Count]);
            
            frame.PushLabel(new Label(new Instruction.End(null, null) ));

            int localIndex = f.Type.Parameters.Length;

            for (int i = f.Type.Parameters.Length - 1; i >= 0; i--)
            {
                // This might need to be reversed?
                var p = CurrentFrame.Pop();
                frame.Locals[i] = p;
                bool valid = p.type == f.Type.Parameters[i];
                if (!valid)
                {
                    throw new Trap("indirect call type mismatch");
                }
            }

            foreach (var t in f.LocalTypes)
            {
                Value local = new Value();
                local.type = t;
                switch(t)
                {
                    case Type.i32:
                        local.i32 = (UInt32)0;
                        break;
                    case Type.i64:
                        local.i64 = (UInt64)0;
                        break;
                    case Type.f32:
                        local.f32 = (float)0;
                        break;
                    case Type.f64:
                        local.f64 = (double)0;
                        break;
                    default:
                        throw new Exception("Invalid local type: 0x" + t.ToString("X"));
                }

                frame.Locals[localIndex++] = local;
            }
            
            Push(frame);
        }
        
        // Returning false means execution is complete
        public bool Step(int count = 1)
        {
            bool exception = true;

            try
            {
                for (int step = 0; step < count; step++)
                {
                    if (_stack.Count == 1)
                    {
                        exception = false;
                        return false;
                    }
                    else
                    {
                        if (CurrentFrame.Instruction == null)
                        {
                            if(CurrentFrame.Function == null)
                            {
                                exception = false;
                                return false;
                            }
                            else
                            {
                                // Handle return 
                                var lastFrame = Pop();
                                var currentFrame = CurrentFrame;

                                foreach (var r in lastFrame.Function.Type.Results)
                                {
                                    try
                                    {
                                        switch (r)
                                        {
                                            case Type.i32:
                                                currentFrame.PushI32(lastFrame.PopI32());
                                                break;
                                            case Type.i64:
                                                currentFrame.PushI64(lastFrame.PopI64());
                                                break;
                                            case Type.f32:
                                                currentFrame.PushF32(lastFrame.PopF32());
                                                break;
                                            case Type.f64:
                                                currentFrame.PushF64(lastFrame.PopF64());
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

                                if (_stack.Count == 1)
                                {
                                    exception = false;
                                    return false;
                                }
                            }
                        }
                        else
                        {
//                            Console.WriteLine(CurrentFrame.Instruction + " " + CurrentFrame.Instruction.Pointer);
                            CurrentFrame.Instruction = CurrentFrame.Instruction.Execute(CurrentFrame, 10);
                        }
                    }
                }

                exception = false;
            }
            finally
            {
                if (exception)
                {
                    Clear();
                    Push(new Frame(null, null, new Value[] { }));
                }
            }

            return true;
        }
    }
}
