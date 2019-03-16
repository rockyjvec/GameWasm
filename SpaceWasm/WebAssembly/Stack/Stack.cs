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

        public void PushValue(Value v)
        {
            this.stack.Push(v);
        }

        public Value PopValue()
        {
            Value v = this.stack.Pop() as Value;

            if (v == null)
            {
                throw new Exception("Invalid expected value on stack");
            }

            return v;
        }

        public void PushLabel(UInt32 IP)
        {
            this.stack.Push(new Label(IP));
        }

        public Label PopLabel(UInt32 IP)
        {
            Label l = this.stack.Pop() as Label;

            if (l == null)
            {
                throw new Exception("Invalid expected label on stack");
            }

            return l;
        }
    }
}
