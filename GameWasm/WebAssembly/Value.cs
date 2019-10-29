using System;
using System.Runtime.InteropServices;
using GameWasm.Webassembly.Stack;

namespace GameWasm.Webassembly
{
    
    public struct Value
    {
    //    [FieldOffset(0)] 
        public byte type;
      //  [FieldOffset(1)]
        public UInt32 i32;
        //[FieldOffset(1)]
        public UInt64 i64;
        //[FieldOffset(1)]
        public float f32;
        //[FieldOffset(1)]
        public double f64;
        //[FieldOffset(1)]
        public Label label;

        public static Value GetI32(UInt32 v)
        {
            Value value = new Value();
            value.type = Type.i32;
            value.i32 = v;
            return value;
        }
    }
}