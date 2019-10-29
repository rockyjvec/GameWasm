using System;
using System.Collections.Generic;

namespace GameWasm.Webassembly.Stack
{
    public class Frame
    {
        public Function Function;
        public Instruction.Instruction Instruction;
        public object[] Locals;

        private Stack<object> _stack;
        
        public Frame(Function function, Instruction.Instruction instruction, object[] locals)
        {
            Function = function;
            Instruction = instruction;
            Locals = locals;

            _stack = new Stack<object>();
        }

        public bool Empty()
        {
            return _stack.Count == 0;
        }
        
        public void Push(object v)
        {
            _stack.Push(v);
        }

        public object Pop()
        {
            return _stack.Pop();
        }

        public object Peek()
        {            
            return _stack.Peek();
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

            if (label.Type.Length > tmp.Count)
            {
                throw new Exception("Invalid label arity.");
            }

            for(int i = 0; i < label.Type.Length; i++)
            {
                if (tmp.Count > 0)
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
            return (UInt32) Pop();
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
        
    }
}
