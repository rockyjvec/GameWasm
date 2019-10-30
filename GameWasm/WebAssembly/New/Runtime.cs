using System;
using System.CodeDom;
using System.Collections.Generic;

namespace GameWasm.Webassembly.New
{
    public class Runtime
    {
        private Stack<Label> labels;
        private Inst[] program;
        private Value[] vStack;
        private Value[] locals;
        private Value[] globals;
        private int localBasePtr = 0; // this represents the current function calls offset within the locals array
        private int vStackPtr = 0;
        private int ip = 0;
        
        
        public Runtime()
        {
            // TODO: these could be set differently
            vStack = new Value[1000];
            globals = new Value[1000];
            locals = new Value[1000000];
        }

        private Label PopLabel(int number, bool end = false)
        {
            Label l = labels.Pop();
            for (; number > 1; number--)
            {
                l = labels.Pop();
            }

            if (end)
            {
                vStackPtr = l.vStackPtr;
            }

            return l;
        }
        
        // Returns true while there is still work to be done.
        public bool Run(int steps = 1000)
        {
            for (; ip < program.Length && steps >= 0; ip++)
            {
                var inst = program[ip];
                switch (inst.opCode)
                {
                    case 0x00: // unreachable
                        throw new Trap("unreachable");
                        break;
                    case 0x01: // nop
                        break;
                    case 0x02: // block
                        labels.Push(new Label((int)inst.i32, vStackPtr));
                        break;
                    case 0x03: // loop
                        labels.Push(new Label(ip, vStackPtr));
                        break;
                    case 0x04: // if
                        if (vStack[--vStackPtr].i32 > 0)
                        {
                            if((int)inst.i32 > 0)
                                labels.Push(new Label((int)program[(int)inst.i32].i32, vStackPtr));
                            else
                                labels.Push(new Label((int)inst.i32, vStackPtr));
                        }
                        else
                        {
                            if ((int)inst.i32 > 0)
                                labels.Push(new Label((int)program[(int)inst.i32].i32, vStackPtr));
                            ip = (int) inst.i32;
                        }                        
                        break;
                    case 0x05: // else
                        ip = (int) inst.i32 - 1;
                        break;
                    case 0x0B: // end
                        PopLabel(1, true);
                        break;
                    case 0x0C: // br
                    {
                        Label l = PopLabel((int) inst.i32 + 1);

                        if (program[l.ip].opCode == 0x03 /*loop*/)
                            ip = l.ip - 1;
                        else
                            ip = l.ip;

                        break;
                    }
                    case 0x0D: // br_if
                    {
                        if (vStack[--vStackPtr].i32 > 0)
                        {
                            Label l = PopLabel((int) inst.i32 + 1);

                            if (program[l.ip].opCode == 0x03 /*loop*/)
                                ip = l.ip - 1;
                            else
                                ip = l.ip;
                        }

                        break;
                    }
                    case 0x0E: // br_table
                    {
                        UInt32 index = vStack[--vStackPtr].i32;

                        if (index >= inst.table.Length)
                        {
                            index = inst.i32;
                        }
                        else
                        {
                            index = inst.table[(int) index];
                        }

                        Label l = PopLabel((int) index + 1);

                        if (program[l.ip].opCode == 0x03 /*loop*/)
                            ip = l.ip - 1;
                        else
                            ip = l.ip;

                        break;
                    }
                    case 0x0F: // return
                        throw new Exception("return not implemented");
                        break;
                    case 0x10: // call
                        throw new Exception("call not implemented");
                        break;
                    case 0x11: // call_indirect
                        throw new Exception("call_indirect not implemented");
                        break;

                    /* Parametric Instructions */

                    case 0x1A: // drop
                        vStackPtr--;
                        break;
                    case 0x1B: // select
                        if(vStack[vStackPtr - 1].i32 == 0)
                        {
                            vStack[vStackPtr - 3] = vStack[vStackPtr - 2];
                        }
                        vStackPtr -= 2;
                        break;

                    /* Variable Instructions */

                    case 0x20: // local.get
                        vStack[vStackPtr++] = locals[localBasePtr + inst.i32];
                        break;
                    case 0x21: // local.set
                        locals[localBasePtr + inst.i32] = vStack[--vStackPtr];
                        break;
                    case 0x22: // local.tee
                        locals[localBasePtr + inst.i32] = vStack[vStackPtr - 1];
                        break;
                    case 0x23: // global.get
                        vStack[vStackPtr++] = globals[inst.i32];
                        break;
                    case 0x24: // global.set
                        globals[inst.i32] = vStack[--vStackPtr];
                        break;

                    /* Memory Instructions */

                    case 0x28: // i32.load
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x29: // i64.load
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x2A: // f32.load
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x2B: // f64.load
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x2C: // i32.load8_s
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x2D: // i32.load8_u
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x2E: // i32.load16_s
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x2F: // i32.load16_u
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x30: // i64.load8_s
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x31: // i64.load8_u
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x32: // i64.load16_s
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x33: // i64.load16_u
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x34: // i64.load32_s
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x35: // i64.load32_u
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x36: // i32.store
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x37: // i64.store
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x38: // f32.store
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x39: // f64.store
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x3A: // i32.store8
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x3B: // i32.store16
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x3C: // i64.store8
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x3D: // i64.store16
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x3E: // i64.store32
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x3F: // memory.size
                        throw new Exception("memory functions not implemented");
                        break;
                    case 0x40: // memory.grow
                        throw new Exception("memory functions not implemented");
                        break;

                    /* Numeric Instructions */

                    // These could be optimized by passing the const values as already created Value types
                    case 0x41: // i32.const
                    {
                        Value v = new Value();
                        v.type = Type.i32;
                        v.i32 = inst.i32;
                        break;
                    }
                    case 0x42: // i64.const
                    {
                        Value v = new Value();
                        v.type = Type.i64;
                        v.i64 = inst.i64;
                        break;
                    }
                    case 0x43: // f32.const
                    {
                        Value v = new Value();
                        v.type = Type.f32;
                        v.f32 = inst.f32;
                        break;
                    }
                    case 0x44: // f64.const
                    {
                        Value v = new Value();
                        v.type = Type.f64;
                        v.f64 = inst.f64;
                        break;
                    }

                    case 0x45: // i32.eqz
                        break;
                    case 0x46: // i32.eq
                        break;
                    case 0x47: // i32.ne
                        break;
                    case 0x48: // i32.lt_s
                        break;
                    case 0x49: // i32.lt_u
                        break;
                    case 0x4A: // i32.gt_s
                        break;
                    case 0x4B: // i32.gt_u
                        break;
                    case 0x4C: // i32.le_s
                        break;
                    case 0x4D: // i32.le_u
                        break;
                    case 0x4E: // i32.ge_s
                        break;
                    case 0x4F: // i32.ge_u
                        break;

                    case 0x50: // i64.eqz
                        break;
                    case 0x51: // i64.eq
                        break;
                    case 0x52: // i64.ne
                        break;
                    case 0x53: // i64.lt_s
                        break;
                    case 0x54: // i64.lt_u
                        break;
                    case 0x55: // i64.gt_s
                        break;
                    case 0x56: // i64.gt_u
                        break;
                    case 0x57: // i64.le_s
                        break;
                    case 0x58: // i64.le_u
                        break;
                    case 0x59: // i64.ge_s
                        break;
                    case 0x5A: // i64.ge_u
                        break;

                    case 0x5B: // f32.eq
                        break;
                    case 0x5C: // f32.ne
                        break;
                    case 0x5D: // f32.lt
                        break;
                    case 0x5E: // f32.gt
                        break;
                    case 0x5F: // f32.le
                        break;
                    case 0x60: // f32.ge
                        break;

                    case 0x61: // f64.eq
                        break;
                    case 0x62: // f64.ne
                        break;
                    case 0x63: // f64.lt
                        break;
                    case 0x64: // f64.gt
                        break;
                    case 0x65: // f64.le
                        break;
                    case 0x66: // f64.ge
                        break;

                    case 0x67: // i32.clz
                        break;
                    case 0x68: // i32.ctz
                        break;
                    case 0x69: // i32.popcnt
                        break;
                    case 0x6A: // i32.add
                        break;
                    case 0x6B: // i32.sub
                        break;
                    case 0x6C: // i32.mul
                        break;
                    case 0x6D: // i32.div_s
                        break;
                    case 0x6E: // i32.div_u
                        break;
                    case 0x6F: // i32.rem_s
                        break;
                    case 0x70: // i32.rem_u
                        break;
                    case 0x71: // i32.and
                        break;
                    case 0x72: // i32.or
                        break;
                    case 0x73: // i32.xor
                        break;
                    case 0x74: // i32.shl
                        break;
                    case 0x75: // i32.shr_s
                        break;
                    case 0x76: // i32.shr_u
                        break;
                    case 0x77: // i32.rotl
                        break;
                    case 0x78: // i32.rotr
                        break;

                    case 0x79: // i64.clz
                        break;
                    case 0x7A: // i64.ctz
                        break;
                    case 0x7B: // i64.popcnt
                        break;
                    case 0x7C: // i64.add
                        break;
                    case 0x7D: // i64.sub
                        break;
                    case 0x7E: // i64.mul
                        break;
                    case 0x7F: // i64.div_s
                        break;
                    case 0x80: // i64.div_u
                        break;
                    case 0x81: // i64.rem_s
                        break;
                    case 0x82: // i64.rem_u
                        break;
                    case 0x83: // i64.and
                        break;
                    case 0x84: // i64.or
                        break;
                    case 0x85: // i64.xor
                        break;
                    case 0x86: // i64.shl
                        break;
                    case 0x87: // i64.shr_s
                        break;
                    case 0x88: // i64.shr_u
                        break;
                    case 0x89: // i64.rotl
                        break;
                    case 0x8A: // i64.rotr
                        break;

                    case 0x8B: // f32.abs
                        break;
                    case 0x8C: // f32.neg
                        break;
                    case 0x8D: // f32.ceil
                        break;
                    case 0x8E: // f32.floor
                        break;
                    case 0x8F: // f32.trunc
                        break;
                    case 0x90: // f32.nearest
                        break;
                    case 0x91: // f32.sqrt
                        break;
                    case 0x92: // f32.add
                        break;
                    case 0x93: // f32.sub
                        break;
                    case 0x94: // f32.mul
                        break;
                    case 0x95: // f32.div
                        break;
                    case 0x96: // f32.min
                        break;
                    case 0x97: // f32.max
                        break;
                    case 0x98: // f32.copysign
                        break;

                    case 0x99: // f64.abs
                        break;
                    case 0x9A: // f64.neg
                        break;
                    case 0x9B: // f64.ceil
                        break;
                    case 0x9C: // f64.floor
                        break;
                    case 0x9D: // f64.trunc
                        break;
                    case 0x9E: // f64.nearest
                        break;
                    case 0x9F: // f64.sqrt
                        break;
                    case 0xA0: // f64.add
                        break;
                    case 0xA1: // f64.sub
                        break;
                    case 0xA2: // f64.mul
                        break;
                    case 0xA3: // f64.div
                        break;
                    case 0xA4: // f64.min
                        break;
                    case 0xA5: // f64.max
                        break;
                    case 0xA6: // f64.copysign
                        break;

                    case 0xA7: // i32.wrap_i64
                        break;
                    case 0xA8: // i32.trunc_f32_s
                        break;
                    case 0xA9: // i32.trunc_f32_u
                        break;
                    case 0xAA: // i32.trunc_f64_s
                        break;
                    case 0xAB: // i32.trunc_f64_u
                        break;
                    case 0xAC: // i64.extend_i32_s
                        break;
                    case 0xAD: // i64.extend_i32_u
                        break;
                    case 0xAE: // i64.trunc_f32_s
                        break;
                    case 0xAF: // i64.trunc_f32_u
                        break;
                    case 0xB0: // i64.trunc_f64_s
                        break;
                    case 0xB1: // i64.trunc_f64_u
                        break;
                    case 0xB2: // f32.convert_i32_s
                        break;
                    case 0xB3: // f32.convert_i32_u
                        break;
                    case 0xB4: // f32.convert_i64_s
                        break;
                    case 0xB5: // f32.convert_i64_u
                        break;
                    case 0xB6: // f32.demote_f64
                        break;
                    case 0xB7: // f64.convert_i32_s
                        break;
                    case 0xB8: // f64.convert_i32_u
                        break;
                    case 0xB9: // f64.convert_i64_s
                        break;
                    case 0xBA: // f64.convert_i64.u
                        break;
                    case 0xBB: // f64.promote_f32
                        break;
                    case 0xBC: // i32.reinterpret_f32
                        break;
                    case 0xBD: // i64.reinterpret_i32
                        break;
                    case 0xBE: // f32.reinterpret_i32
                        break;
                    case 0xBF: // f64.reinterpret_i64
                        break;
                }

                steps--;
            }

            return true;
        }
    }
}