using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Stack
{
    public class Stack
    {
        Stack<object> stack = new Stack<object>();
        Stack<Frame> frames = new Stack<Frame>();
        Store store;

        public Stack(Store store)
        {
            this.store = store;
        }

        public void Push(object v)
        {
            this.stack.Push(v);
        }

        public object Pop()
        {
            return this.stack.Pop();
        }

        public object Peek()
        {
            return this.stack.Peek();
        }

        public UInt32 PopI32()
        {
            return (UInt32)this.stack.Pop();
        }

        public UInt32 PeekI32()
        {
            return (UInt32)this.stack.Peek();
        }

        public UInt64 PopI64()
        {
            return (UInt64)this.stack.Pop();
        }

        public UInt64 PeekI64()
        {
            return (UInt64)this.stack.Peek();
        }

        public float PopF32()
        {
            return (float)this.stack.Pop();
        }

        public float PeekF32()
        {
            return (float)this.stack.Peek();
        }

        public double PopF64()
        {
            return (double)this.stack.Pop();
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
                this.frames.Push(this.store.CurrentFrame);
                this.store.CurrentFrame = frame;
            }
        }

        public bool PopFrame()
        {
            if (this.frames.Count() > 0)
            {
                this.store.CurrentFrame = this.frames.Pop();
            }
            else
            {
                this.store.CurrentFrame = null;
            }

            return this.store.CurrentFrame != null;
        }
    }
}
