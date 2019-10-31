using System;
using System.Runtime.InteropServices;

namespace GameWasm.Webassembly
{
    
    [StructLayout(LayoutKind.Explicit)]
    public struct Value
    {
        [FieldOffset(8)] 
        public byte type;
        [FieldOffset(0)]
        public UInt32 i32;
        [FieldOffset(0)]
        public UInt64 i64;
        [FieldOffset(0)]
        public float f32;
        [FieldOffset(0)]
        public double f64;

        public static Value GetI32(UInt32 v)
        {
            Value value = new Value();
            value.type = Type.i32;
            value.i32 = v;
            return value;
        }
    }
}