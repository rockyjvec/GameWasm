using System;
using System.Collections.Generic;

namespace GameWasm.Webassembly.Stack
{
    public class Frame
    {
        public Function Function;
        public Instruction.Instruction Instruction;
        public Value[] Locals;

        private Stack<Value> _stack;
        
        public Frame(Function function, Instruction.Instruction instruction, Value[] locals)
        {
            Function = function;
            Instruction = instruction;
            Locals = locals;

            _stack = new Stack<Value>();
        }

        public bool Empty()
        {
            return _stack.Count == 0;
        }
        
        public void Push(Value v)
        {
            _stack.Push(v);
        }

        public Value Pop()
        {
            return _stack.Pop();
        }

        public Value Peek()
        {            
            return _stack.Peek();
        }

        public Value PopValue()
        {
            return Pop();
        }

        public void PushI32(UInt32 v)
        {
            Value value = new Value();
            value.type = Type.i32;
            value.i32 = v;
            Push(value);
        }

        public UInt32 PopI32()
        {
            var value = Pop();
            if(value.type != Type.i32) throw new Trap("indirect call type mismatch");
            return value.i32;
        }

        public UInt32 PeekI32()
        {
            var value = Peek();
            if(value.type != Type.i32) throw new Trap("indirect call type mismatch");
            return value.i32;
        }

        public void PushI64(UInt64 v)
        {
            Value value = new Value();
            value.type = Type.i64;
            value.i64 = v;
            Push(value);
        }

        public UInt64 PopI64()
        {
            var value = Pop();
            if(value.type != Type.i64) throw new Trap("indirect call type mismatch");
            return value.i64;
        }

        public UInt64 PeekI64()
        {
            var value = Peek();
            if(value.type != Type.i64) throw new Trap("indirect call type mismatch");
            return value.i64;
        }

        public void PushF32(float v)
        {
            Value value = new Value();
            value.type = Type.f32;
            value.f32 = v;
            Push(value);
        }

        public float PopF32()
        {
            var value = Pop();
            if(value.type != Type.f32) throw new Trap("indirect call type mismatch");
            return value.f32;
        }

        public float PeekF32()
        {
            var value = Peek();
            if(value.type != Type.f32) throw new Trap("indirect call type mismatch");
            return value.f32;
        }

        public void PushF64(double v)
        {
            Value value = new Value();
            value.type = Type.f64;
            value.f64 = v;
            Push(value);
        }

        public double PopF64()
        {
            var value = Pop();
            if(value.type != Type.f64) throw new Trap("indirect call type mismatch");
            return value.f64;
        }

        public double PeekF64()
        {
            var value = Peek();
            if(value.type != Type.f64) throw new Trap("indirect call type mismatch");
            return value.f64;
        }
        
        public void PushLabel(Label v)
        {
            Value value = new Value();
            value.type = Type.label;
            value.label = v;
            Push(value);
        }

        public Label PopLabel(uint number = 1, bool end = false)
        {
            Queue<Value> tmp = new Queue<Value>();

            Value l;
            do
            {
                l = Pop();
                if (l.type == Type.label)
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

            var label = l.label;

            if(!end && (label.Instruction as Instruction.Loop) != null) return label;

            if (label.Type.Length > tmp.Count)
            {
                throw new Exception("Invalid label arity.");
            }

            for(int i = 0; i < label.Type.Length; i++)
            {
                if (tmp.Count > 0)
                {
                    var value = tmp.Dequeue();
                    if (label.Type[i] == value.type)
                    {
                        Push(value);
                        continue;
                    }
                }
                throw new Exception("Invalid label arity.");
            }

            return label;
        }
    }
}
