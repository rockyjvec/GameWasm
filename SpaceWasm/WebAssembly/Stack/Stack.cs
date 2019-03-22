using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Stack
{
    public class Stack
    {
        public static UInt32 STACK_MAX = 1000;
        Stack<object> stack = new Stack<object>();
        public UInt32 Size = 0;
        Store store;

        public Stack(Store store)
        {
            this.store = store;
        }

        public void Push(object v)
        {
            this.stack.Push(v);
            this.Size++;

            if (Size > Stack.STACK_MAX)
            {
                throw new Trap("call stack exhausted");
            }
        }

        public object Pop()
        {
            var v = this.stack.Pop();
            this.Size--;
            return v;
        }

        public object Peek()
        {
            var v = this.stack.Peek();
            return v;
        }

        public object PopValue()
        {
            var value = this.Peek();
            switch(value.GetType().ToString())
            {

                case "System.UInt32":
                case "System.UInt64":
                case "System.Single":
                case "System.Double":
                    return this.Pop();
            }
            throw new Exception("Could not pop value from stack.");
        }

        public Label PopLabel(uint number = 1)
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
            } while (true);

            var label = l as Label; 
            if (label.Type.Length > tmp.Count())
            {
                throw new Exception("Invalid label arity.");
            }

            for(int i = 0; i < label.Type.Length; i++)
            {
                switch(label.Type[i])
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
            return (UInt32)this.stack.Peek();
        }

        public UInt64 PopI64()
        {
            return (UInt64)this.Pop();
        }

        public UInt64 PeekI64()
        {
            return (UInt64)this.stack.Peek();
        }

        public float PopF32()
        {
            return (float)this.Pop();
        }

        public float PeekF32()
        {
            return (float)this.stack.Peek();
        }

        public double PopF64()
        {
            return (double)this.Pop();
        }

        public double PeekF64()
        {
            return (double)this.stack.Peek();
        }

        public void PushFrame(Frame frame)
        {
            if(this.store.CurrentFrame == null)
            {
                this.store.CurrentFrame = frame;
            }
            else
            {
                this.stack.Push(this.store.CurrentFrame);
                this.store.CurrentFrame = frame;
            }
        }

        public bool PopFrame()
        {
            var results = this.store.CurrentFrame.Results;

            if (this.stack.Count() > 0)
            {
                this.store.CurrentFrame = (Frame)this.stack.Pop();
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

        public object[] ToArray()
        {
            return this.stack.ToArray();
        }
    }
}
