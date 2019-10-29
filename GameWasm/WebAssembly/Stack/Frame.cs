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
        private Stack<Label> _labels;
        
        public Frame(Function function, Instruction.Instruction instruction, Value[] locals)
        {
            Function = function;
            Instruction = instruction;
            Locals = locals;

            _stack = new Stack<Value>();
            _labels = new Stack<Label>();
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
            v.Stack = _stack.Count;
            _labels.Push(v);
        }

        public Label PopLabel(uint number = 1, bool end = false)
        {
            Label l = _labels.Pop();
            for (; number > 1; number--)
            {
                l = _labels.Pop();
            }

            if (end)
            {
                for(; _stack.Count > l.Stack; _stack.Pop()) {}
            }

            return l;
        }
    }
}
