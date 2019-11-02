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
        [FieldOffset(0)]
        public byte b0;
        [FieldOffset(1)]
        public byte b1;
        [FieldOffset(2)]
        public byte b2;
        [FieldOffset(3)]
        public byte b3;
        [FieldOffset(4)]
        public byte b4;
        [FieldOffset(5)]
        public byte b5;
        [FieldOffset(6)]
        public byte b6;
        [FieldOffset(7)]
        public byte b7;

        public static Value GetI32(UInt32 v)
        {
            Value value = new Value();
            value.type = Type.i32;
            value.i32 = v;
            return value;
        }
    }
}