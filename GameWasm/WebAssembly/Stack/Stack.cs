using System;
using System.Collections.Generic;
using System.Linq;

namespace GameWasm.Webassembly.Stack
{
    public class Stack
    {
        public static UInt32 StackMax = 10000;
        object[] stack;

        public UInt32 Size = 0;
        Store store;
        private UInt32 _frames = 0;
        public object Thrown = null;
        public bool Debug = false;

        public Stack(Store store)
        {
            this.store = store;
            stack = new object[Stack.StackMax];
        }

        public void Push(object v)
        {
            if (Size + 1 >= Stack.StackMax)
            {
                throw new Trap("call stack exhausted");
            }

            stack[Size++] = v;
        }

        public object Pop()
        {            
            return stack[--Size];
        }

        public object Peek()
        {            
            return stack[Size - 1];
        }

        public object PopValue()
        {
            object value = Pop();
            if(value is UInt32 || value is UInt64 || value is float || value is double)
            {
                return value;
            }

            throw new Exception("Top of stack is not a value");
        }

        public Label PopLabel(uint number = 1, bool end = false)
        {
            Queue<object> tmp = new Queue<object>();

            object l;
            do
            {
                l = Pop();
                if (l as Label != null)
                {
                    number--;
                    if (number == 0) break;
                }
                else
                {
                    tmp.Enqueue(l);
                }
            }
            while (true);

            var label = l as Label;

            if(!end && (label.Instruction as Instruction.Loop) != null) return label;

            if (label.Type.Length > tmp.Count())
            {
                throw new Exception("Invalid label arity.");
            }

            for(int i = 0; i < label.Type.Length; i++)
            {
                if (tmp.Count() > 0)
                {
                    switch (label.Type[i])
                    {
                        case Type.i32:
                            Push((UInt32)tmp.Dequeue());
                            continue;
                        case Type.i64:
                            Push((UInt64)tmp.Dequeue());
                            continue;
                        case Type.f32:
                            Push((float)tmp.Dequeue());
                            continue;
                        case Type.f64:
                            Push((double)tmp.Dequeue());
                            continue;
                    }
                }
                throw new Exception("Invalid label arity.");
            }

            return label;
        }

        public UInt32 PopI32()
        {
            return (UInt32)Pop();
        }

        public UInt32 PeekI32()
        {
            return (UInt32)Peek();
        }

        public UInt64 PopI64()
        {
            return (UInt64)Pop();
        }

        public UInt64 PeekI64()
        {
            return (UInt64)Peek();
        }

        public float PopF32()
        {
            return (float)Pop();
        }

        public float PeekF32()
        {
            return (float)Peek();
        }

        public double PopF64()
        {
            return (double)Pop();
        }

        public double PeekF64()
        {
            return (double)Peek();
        }

        public void PushFrame(Frame frame)
        {
            _frames++;
            if(Debug)
                Console.WriteLine(new string(' ', (int)(_frames-1) * 2) + "CALL: " + frame.Function.GetName());
            if(store.CurrentFrame == null)
            {
                store.CurrentFrame = frame;
            }
            else
            {
                Push(store.CurrentFrame);
                store.CurrentFrame = frame;
            }
        }

        public bool PopFrame()
        {
            if (Debug)
                Console.WriteLine(new string(' ', (int)(_frames-1) * 2) + "RETN: " + store.CurrentFrame.Function.GetName());
            _frames--;
            var results = store.CurrentFrame.Results;

            if (Size > 0)
            {
                store.CurrentFrame = (Frame)Pop();
            }
            else
            {
                store.CurrentFrame = null;
            }

            foreach(var r in results)
            {
                Push(r);
            }

            return store.CurrentFrame != null;
        }

        public void Throw(object e)
        {
//            Console.WriteLine("Throw: " + e);
            Thrown = e;
            bool first = true;
            do
            {
                if (!first && store.CurrentFrame.Function.Catcher)
                {
//                    Console.WriteLine("Catch: " + e);
                    return;
                }

                first = false; 
                
                if(Size > 0)
                {
                    if (Peek() as Frame != null)
                    {
                        PopFrame();
                    }
                    else
                    {
                        Pop();
                    }
                }
                else
                {
                    throw new Exception("Unhandled exception: " + Thrown);
                }
            }
            while (store.CurrentFrame != null);
        }
    }
}
