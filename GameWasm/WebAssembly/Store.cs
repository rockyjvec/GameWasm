using System;
using System.Collections.Generic;
using System.IO;
using GameWasm.Webassembly.Stack;

namespace GameWasm.Webassembly
{
    public class Store
    {
        public Dictionary<string, Module.Module> Modules = new Dictionary<string, Module.Module>();
        
        private int _stackMax = 1000;
        private int _stackPtr = 0;
        private Frame[] _stack;
        public Frame CurrentFrame;

        public void Push(Frame f)
        {
            if (_stackPtr + 1 > _stackMax)
            {
                throw new Trap("call stack exhausted");
            }
//            if(f.Function != null)
  //            Console.WriteLine(new String(' ', _stackPtr * 2) + f.Function.Module.Name + "@" + f.Function.GetName() );
            CurrentFrame = f;
            _stack[_stackPtr++] = f;
        }

        public Frame Pop()
        {
            if (_stackPtr == 0)
            {
                throw new Trap("call stack exhausted");
            }
            
            if (_stackPtr == 1)
            {
                CurrentFrame = null;
            }
            else
            {
                CurrentFrame = _stack[_stackPtr - 2];
            }
            
            var last = _stack[--_stackPtr];
            _stack[_stackPtr] = null;
            return last;
        }

        public void Clear()
        {
            for (; _stackPtr > 0; Pop())
            {
                
            }
        }
        
        public Store()
        {
            _stack = new Frame[_stackMax];
            Push(new Frame(this, null, null, new Object[] { }));
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
            var frame = new Frame(this, f, f.Start, new object[f.Type.Parameters.Length + f.LocalTypes.Count]);
            
            frame.Push(new Label(new Instruction.End(null), f.Type.Results));

            int localIndex = f.Type.Parameters.Length;

            for (int i = f.Type.Parameters.Length - 1; i >= 0; i--)
            {
                // This might need to be reversed?
                var p = CurrentFrame.Pop();
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
                    if (_stackPtr == 1)
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

                                if (_stackPtr == 1)
                                {
                                    exception = false;
                                    return false;
                                }
                            }
                        }
                        else
                        {
//                            Console.WriteLine(CurrentFrame.Instruction + " " + CurrentFrame.Instruction.Pointer);
                            CurrentFrame.Instruction = CurrentFrame.Instruction.Execute(CurrentFrame);
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
                    Push(new Frame(this, null, null, new object[] { }));
                }
            }

            return true;
        }
    }
}
