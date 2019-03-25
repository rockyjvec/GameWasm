using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Stack
{
    public class Stack
    {
        public static UInt32 STACK_MAX = 10000;
        object[] stack;

        public UInt32 Size = 0;
        Store store;
        UInt32 frames = 0;
        public object Thrown = null;
        public bool Debug = false;

        public Stack(Store store)
        {
            this.store = store;
            stack = new object[Stack.STACK_MAX];
        }

        public void Push(object v)
        {
            if (Size + 1 >= Stack.STACK_MAX)
            {
                throw new Trap("call stack exhausted");
            }

            this.stack[this.Size++] = v;
        }

        public object Pop()
        {            
            return this.stack[--this.Size];
        }

        public object Peek()
        {            
            return this.stack[this.Size - 1];
        }

        public object PopValue()
        {
            object value = this.Pop();
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
                l = this.Pop();
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
                            this.Push((UInt32)tmp.Dequeue());
                            continue;
                        case Type.i64:
                            this.Push((UInt64)tmp.Dequeue());
                            continue;
                        case Type.f32:
                            this.Push((float)tmp.Dequeue());
                            continue;
                        case Type.f64:
                            this.Push((double)tmp.Dequeue());
                            continue;
                    }
                }
                throw new Exception("Invalid label arity.");
            }

            return label;
        }

        public UInt32 PopI32()
        {
            return (UInt32)this.Pop();
        }

        public UInt32 PeekI32()
        {
            return (UInt32)this.Peek();
        }

        public UInt64 PopI64()
        {
            return (UInt64)this.Pop();
        }

        public UInt64 PeekI64()
        {
            return (UInt64)this.Peek();
        }

        public float PopF32()
        {
            return (float)this.Pop();
        }

        public float PeekF32()
        {
            return (float)this.Peek();
        }

        public double PopF64()
        {
            return (double)this.Pop();
        }

        public double PeekF64()
        {
            return (double)this.Peek();
        }

        public void PushFrame(Frame frame)
        {
            this.frames++;
            if(this.Debug)
                Console.WriteLine(new string(' ', (int)(this.frames-1) * 2) + "CALL: " + frame.Function.GetName());
            if(this.store.CurrentFrame == null)
            {
                this.store.CurrentFrame = frame;
            }
            else
            {
                this.Push(this.store.CurrentFrame);
                this.store.CurrentFrame = frame;
            }
        }

        public bool PopFrame()
        {
            if (this.Debug)
                Console.WriteLine(new string(' ', (int)(this.frames-1) * 2) + "RETN: " + this.store.CurrentFrame.Function.GetName());
            this.frames--;
            var results = this.store.CurrentFrame.Results;

            if (this.Size > 0)
            {
                this.store.CurrentFrame = (Frame)this.Pop();
            }
            else
            {
                this.store.CurrentFrame = null;
            }

            foreach(var r in results)
            {
                this.Push(r);
            }

            return this.store.CurrentFrame != null;
        }

        public void Throw(object e)
        {
//            Console.WriteLine("Throw: " + e);
            this.Thrown = e;
            bool first = true;
            do
            {
                if (!first && this.store.CurrentFrame.Function.Catcher)
                {
//                    Console.WriteLine("Catch: " + e);
                    return;
                }

                first = false; 
                
                if(this.Size > 0)
                {
                    if (this.Peek() as Frame != null)
                    {
                        this.PopFrame();
                    }
                    else
                    {
                        this.Pop();
                    }
                }
                else
                {
                    throw new Exception("Unhandled exception: " + this.Thrown);
                }
            }
            while (this.store.CurrentFrame != null);
        }
    }
}
