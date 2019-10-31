using System;
using System.Runtime.InteropServices;

namespace GameWasm.Webassembly.New
{
    // The runtime version of an instruction
    //[StructLayout(LayoutKind.Explicit)]
    public struct Inst
    {
       // [FieldOffset(22)] 
        public Instruction.Instruction i;
       // [FieldOffset(17)] 
        public UInt32 pointer;
        //[FieldOffset(16)] 
        public byte opCode;
        //[FieldOffset(0)]
        public UInt32 i32;
      //  [FieldOffset(0)]
        public UInt64 i64;
    //    [FieldOffset(0)]
        public float f32;
  //      [FieldOffset(0)]
        public double f64;
//        [FieldOffset(8)]
        public UInt32[] table;
    }
}