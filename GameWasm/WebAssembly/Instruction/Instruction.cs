using System;
using System.Collections.Generic;
using System.Linq;
using GameWasm.Webassembly.Stack;

namespace GameWasm.Webassembly.Instruction
{
    public class Instruction
    {
        public Function Function;
        public UInt32 Pointer;
        public Instruction Next = null;
        
        private object[] _constants = null;
        Parser parser;

        public const bool optimizer = true;
        public bool isCallInstruction = false;
        public bool isControlInstruction = false;
        public bool isConstantInstruction = false;

        public Instruction Execute(Frame f, int steps = 0)
        {
            if (_constants != null)
            {
                for(int i = 0; i < _constants.Length; i++)
                {
                    var v = _constants[i]; 
                    f.Push(v);
                }
            }

            var next = Run(f);
            if (optimizer && next != null && steps > 0 && !isCallInstruction )
            {
                return next.Execute(f, steps - 1);
            }

            return next;
        }
        
        public Instruction(Parser parser, Function f, bool implemented = false)
        {
            this.parser = parser;
            this.Function = f;

            this.isCallInstruction =
                (this as Call) != null ||
                (this as CallIndirect) != null ||
                false;

            this.isControlInstruction = //(this as Block) != null ||
                       //  (this as Br) != null || 
                       //  (this as BrIf) != null || 
                       //  (this as BrTable) != null ||
                       (this as Call) != null ||
                       (this as CallIndirect) != null ||
                       //  (this as Custom) != null ||
                       //    (this as Drop) != null ||
                       //      (this as Else) != null ||
                       //(this as End) != null ||
                       //       (this as If) != null ||
                       //         (this as Loop) != null ||
                       //(this as Return) != null ||
                       //           (this as Select) != null  ||
                       false;
            
            this.isConstantInstruction = (this as I32const) != null || (this as I64const) != null || (this as F32const) != null ||
                                         (this as F64const) != null;

            if(!implemented)
            {
                throw new Exception("Instruction not implemmented: " + this);
            }
        }

        public virtual void End(Instruction end)
        {
            throw new Exception("End not implementedin " + this);
        }

        protected virtual Instruction Run(Frame frame)
        {
            throw new Exception("Run not implemented in " + this);
        }
        
        public static Instruction Consume(Parser parser, Function f, bool debug)
        {
            Stack<Instruction> controlFlowStack = new Stack<Instruction>();
            Instruction start = null;
            Instruction last = null;

            bool done = false;

            while (!done)
            {
                Instruction current = null;
                UInt32 pointer = parser.GetPointer();
                byte code = parser.GetByte();

                switch (code)
                {
                    /* Control Instructions */

                    case 0x00: // unreachable
                        current = new Unreachable(parser, f); break;
                    case 0x01: // nop
                        current = new Nop(parser, f); break;
                    case 0x02: // block
                        current = new Block(parser, f);
                        controlFlowStack.Push(current);
                        break;
                    case 0x03: // loop
                        current = new Loop(parser, f);
                        controlFlowStack.Push(current);
                        break;
                    case 0x04: // if
                        current = new If(parser, f);
                        controlFlowStack.Push(current);
                        break;
                    case 0x05: // else
                        if (controlFlowStack.Count() == 0)
                        {
                            throw new Exception("Else with no matching if.");
                        }
                        else
                        {
                            current = new Else(parser, f);
                            var match = controlFlowStack.Pop();
                            match.End(current); // notify of else
                            controlFlowStack.Push(current); // add back to find end
                            break;
                        }
                    case 0x0B: // end
                        if(controlFlowStack.Count() == 0)
                        {
                            current = new End(parser, f);
                            done = true;
                            break;
                        }
                        else
                        {
                            current = new End(parser, f);
                            var match = controlFlowStack.Pop();

                            match.End(current); // notify of end
                            break;
                        }
                    case 0x0C: // br
                        current = new Br(parser, f); break;
                    case 0x0D: // br_if
                        current = new BrIf(parser, f); break;
                    case 0x0E: // br_table
                        current = new BrTable(parser, f); break;
                    case 0x0F: // return
                        current = new Return(parser, f); break;
                    case 0x10: // call
                        current = new Call(parser, f); break;
                    case 0x11: // call_indirect
                        current = new CallIndirect(parser, f); break;

                    /* Parametric Instructions */

                    case 0x1A: // drop
                        current = new Drop(parser, f); break;
                    case 0x1B: // select
                        current = new Select(parser, f); break;

                    /* Variable Instructions */

                    case 0x20: // local.get
                        current = new LocalGet(parser, f); break;
                    case 0x21: // local.set
                        current = new LocalSet(parser, f); break;
                    case 0x22: // local.tee
                        current = new LocalTee(parser, f); break;
                    case 0x23: // global.get
                        current = new GlobalGet(parser, f); break;
                    case 0x24: // global.set
                        current = new GlobalSet(parser, f); break;

                    /* Memory Instructions */

                    case 0x28: // i32.load
                        current = new I32load(parser, f); break;
                    case 0x29: // i64.load
                        current = new I64load(parser, f); break;
                    case 0x2A: // f32.load
                        current = new F32load(parser, f); break;
                    case 0x2B: // f64.load
                        current = new F64load(parser, f); break;
                    case 0x2C: // i32.load8_s
                        current = new I32load8s(parser, f); break;
                    case 0x2D: // i32.load8_u
                        current = new I32load8u(parser, f); break;
                    case 0x2E: // i32.load16_s
                        current = new I32load16s(parser, f); break;
                    case 0x2F: // i32.load16_u
                        current = new I32load16u(parser, f); break;
                    case 0x30: // i64.load8_s
                        current = new I64load8s(parser, f); break;
                    case 0x31: // i64.load8_u
                        current = new I64load8u(parser, f); break;
                    case 0x32: // i64.load16_s
                        current = new I64load16s(parser, f); break;
                    case 0x33: // i64.load16_u
                        current = new I64load16u(parser, f); break;
                    case 0x34: // i64.load32_s
                        current = new I64load32s(parser, f); break;
                    case 0x35: // i64.load32_u
                        current = new I64load32u(parser, f); break;
                    case 0x36: // i32.store
                        current = new I32store(parser, f); break;
                    case 0x37: // i64.store
                        current = new I64store(parser, f); break;
                    case 0x38: // f32.store
                        current = new F32store(parser, f); break;
                    case 0x39: // f64.store
                        current = new F64store(parser, f); break;
                    case 0x3A: // i32.store8
                        current = new I32store8(parser, f); break;
                    case 0x3B: // i32.store16
                        current = new I32store16(parser, f); break;
                    case 0x3C: // i64.store8
                        current = new I64store8(parser, f); break;
                    case 0x3D: // i64.store16
                        current = new I64store16(parser, f); break;
                    case 0x3E: // i64.store32
                        current = new I64store32(parser, f); break;
                    case 0x3F: // memory.size
                        current = new MemorySize(parser, f); break;
                    case 0x40: // memory.grow
                        current = new MemoryGrow(parser, f); break;

                    /* Numeric Instructions */

                    case 0x41: // i32.const
                        current = new I32const(parser, f); break;
                    case 0x42: // i64.const
                        current = new I64const(parser, f); break;
                    case 0x43: // f32.const
                        current = new F32const(parser, f); break;
                    case 0x44: // f64.const
                        current = new F64const(parser, f); break;

                    case 0x45: // i32.eqz
                        current = new I32eqz(parser, f); break;
                    case 0x46: // i32.eq
                        current = new I32eq(parser, f); break;
                    case 0x47: // i32.ne
                        current = new I32ne(parser, f); break;
                    case 0x48: // i32.lt_s
                        current = new I32lts(parser, f); break;
                    case 0x49: // i32.lt_u
                        current = new I32ltu(parser, f); break;
                    case 0x4A: // i32.gt_s
                        current = new I32gts(parser, f); break;
                    case 0x4B: // i32.gt_u
                        current = new I32gtu(parser, f); break;
                    case 0x4C: // i32.le_s
                        current = new I32les(parser, f); break;
                    case 0x4D: // i32.le_u
                        current = new I32leu(parser, f); break;
                    case 0x4E: // i32.ge_s
                        current = new I32ges(parser, f); break;
                    case 0x4F: // i32.ge_u
                        current = new I32geu(parser, f); break;

                    case 0x50: // i64.eqz
                        current = new I64eqz(parser, f); break;
                    case 0x51: // i64.eq
                        current = new I64eq(parser, f); break;
                    case 0x52: // i64.ne
                        current = new I64ne(parser, f); break;
                    case 0x53: // i64.lt_s
                        current = new I64lts(parser, f); break;
                    case 0x54: // i64.lt_u
                        current = new I64ltu(parser, f); break;
                    case 0x55: // i64.gt_s
                        current = new I64gts(parser, f); break;
                    case 0x56: // i64.gt_u
                        current = new I64gtu(parser, f); break;
                    case 0x57: // i64.le_s
                        current = new I64les(parser, f); break;
                    case 0x58: // i64.le_u
                        current = new I64leu(parser, f); break;
                    case 0x59: // i64.ge_s
                        current = new I64ges(parser, f); break;
                    case 0x5A: // i64.ge_u
                        current = new I64geu(parser, f); break;

                    case 0x5B: // f32.eq
                        current = new F32eq(parser, f); break;
                    case 0x5C: // f32.ne
                        current = new F32ne(parser, f); break;
                    case 0x5D: // f32.lt
                        current = new F32lt(parser, f); break;
                    case 0x5E: // f32.gt
                        current = new F32gt(parser, f); break;
                    case 0x5F: // f32.le
                        current = new F32le(parser, f); break;
                    case 0x60: // f32.ge
                        current = new F32ge(parser, f); break;

                    case 0x61: // f64.eq
                        current = new F64eq(parser, f); break;
                    case 0x62: // f64.ne
                        current = new F64ne(parser, f); break;
                    case 0x63: // f64.lt
                        current = new F64lt(parser, f); break;
                    case 0x64: // f64.gt
                        current = new F64gt(parser, f); break;
                    case 0x65: // f64.le
                        current = new F64le(parser, f); break;
                    case 0x66: // f64.ge
                        current = new F64ge(parser, f); break;

                    case 0x67: // i32.clz
                        current = new I32clz(parser, f); break;
                    case 0x68: // i32.ctz
                        current = new I32ctz(parser, f); break;
                    case 0x69: // i32.popcnt
                        current = new I32popcnt(parser, f); break;
                    case 0x6A: // i32.add
                        current = new I32add(parser, f); break;
                    case 0x6B: // i32.sub
                        current = new I32sub(parser, f); break;
                    case 0x6C: // i32.mul
                        current = new I32mul(parser, f); break;
                    case 0x6D: // i32.div_s
                        current = new I32divs(parser, f); break;
                    case 0x6E: // i32.div_u
                        current = new I32divu(parser, f); break;
                    case 0x6F: // i32.rem_s
                        current = new I32rems(parser, f); break;
                    case 0x70: // i32.rem_u
                        current = new I32remu(parser, f); break;
                    case 0x71: // i32.and
                        current = new I32and(parser, f); break;
                    case 0x72: // i32.or
                        current = new I32or(parser, f); break;
                    case 0x73: // i32.xor
                        current = new I32xor(parser, f); break;
                    case 0x74: // i32.shl
                        current = new I32shl(parser, f); break;
                    case 0x75: // i32.shr_s
                        current = new I32shrs(parser, f); break;
                    case 0x76: // i32.shr_u
                        current = new I32shru(parser, f); break;
                    case 0x77: // i32.rotl
                        current = new I32rotl(parser, f); break;
                    case 0x78: // i32.rotr
                        current = new I32rotr(parser, f); break;

                    case 0x79: // i64.clz
                        current = new I64clz(parser, f); break;
                    case 0x7A: // i64.ctz
                        current = new I64ctz(parser, f); break;
                    case 0x7B: // i64.popcnt
                        current = new I64popcnt(parser, f); break;
                    case 0x7C: // i64.add
                        current = new I64add(parser, f); break;
                    case 0x7D: // i64.sub
                        current = new I64sub(parser, f); break;
                    case 0x7E: // i64.mul
                        current = new I64mul(parser, f); break;
                    case 0x7F: // i64.div_s
                        current = new I64divs(parser, f); break;
                    case 0x80: // i64.div_u
                        current = new I64divu(parser, f); break;
                    case 0x81: // i64.rem_s
                        current = new I64rems(parser, f); break;
                    case 0x82: // i64.rem_u
                        current = new I64remu(parser, f); break;
                    case 0x83: // i64.and
                        current = new I64and(parser, f); break;
                    case 0x84: // i64.or
                        current = new I64or(parser, f); break;
                    case 0x85: // i64.xor
                        current = new I64xor(parser, f); break;
                    case 0x86: // i64.shl
                        current = new I64shl(parser, f); break;
                    case 0x87: // i64.shr_s
                        current = new I64shrs(parser, f); break;
                    case 0x88: // i64.shr_u
                        current = new I64shru(parser, f); break;
                    case 0x89: // i64.rotl
                        current = new I64rotl(parser, f); break;
                    case 0x8A: // i64.rotr
                        current = new I64rotr(parser, f); break;

                    case 0x8B: // f32.abs
                        current = new F32abs(parser, f); break;
                    case 0x8C: // f32.neg
                        current = new F32neg(parser, f); break;
                    case 0x8D: // f32.ceil
                        current = new F32ceil(parser, f); break;
                    case 0x8E: // f32.floor
                        current = new F32floor(parser, f); break;
                    case 0x8F: // f32.trunc
                        current = new F32trunc(parser, f); break;
                    case 0x90: // f32.nearest
                        current = new F32nearest(parser, f); break;
                    case 0x91: // f32.sqrt
                        current = new F32sqrt(parser, f); break;
                    case 0x92: // f32.add
                        current = new F32add(parser, f); break;
                    case 0x93: // f32.sub
                        current = new F32sub(parser, f); break;
                    case 0x94: // f32.mul
                        current = new F32mul(parser, f); break;
                    case 0x95: // f32.div
                        current = new F32div(parser, f); break;
                    case 0x96: // f32.min
                        current = new F32min(parser, f); break;
                    case 0x97: // f32.max
                        current = new F32max(parser, f); break;
                    case 0x98: // f32.copysign
                        current = new F32copysign(parser, f); break;

                    case 0x99: // f64.abs
                        current = new F64abs(parser, f); break;
                    case 0x9A: // f64.neg
                        current = new F64neg(parser, f); break;
                    case 0x9B: // f64.ceil
                        current = new F64ceil(parser, f); break;
                    case 0x9C: // f64.floor
                        current = new F64floor(parser, f); break;
                    case 0x9D: // f64.trunc
                        current = new F64trunc(parser, f); break;
                    case 0x9E: // f64.nearest
                        current = new F64nearest(parser, f); break;
                    case 0x9F: // f64.sqrt
                        current = new F64sqrt(parser, f); break;
                    case 0xA0: // f64.add
                        current = new F64add(parser, f); break;
                    case 0xA1: // f64.sub
                        current = new F64sub(parser, f); break;
                    case 0xA2: // f64.mul
                        current = new F64mul(parser, f); break;
                    case 0xA3: // f64.div
                        current = new F64div(parser, f); break;
                    case 0xA4: // f64.min
                        current = new F64min(parser, f); break;
                    case 0xA5: // f64.max
                        current = new F64max(parser, f); break;
                    case 0xA6: // f64.copysign
                        current = new F64copysign(parser, f); break;

                    case 0xA7: // i32.wrap_i64
                        current = new I32wrapI64(parser, f); break;
                    case 0xA8: // i32.trunc_f32_s
                        current = new I32truncF32s(parser, f); break;
                    case 0xA9: // i32.trunc_f32_u
                        current = new I32truncF32u(parser, f); break;
                    case 0xAA: // i32.trunc_f64_s
                        current = new I32truncF64s(parser, f); break;
                    case 0xAB: // i32.trunc_f64_u
                        current = new I32truncF64u(parser, f); break;
                    case 0xAC: // i64.extend_i32_s
                        current = new I64extendI32s(parser, f); break;
                    case 0xAD: // i64.extend_i32_u
                        current = new I64extendI32u(parser, f); break;
                    case 0xAE: // i64.trunc_f32_s
                        current = new I64truncF32s(parser, f); break;
                    case 0xAF: // i64.trunc_f32_u
                        current = new I64truncF32u(parser, f); break;
                    case 0xB0: // i64.trunc_f64_s
                        current = new I64truncF64s(parser, f); break;
                    case 0xB1: // i64.trunc_f64_u
                        current = new I64truncF64u(parser, f); break;
                    case 0xB2: // f32.convert_i32_s
                        current = new F32convertI32s(parser, f); break;
                    case 0xB3: // f32.convert_i32_u
                        current = new F32convertI32u(parser, f); break;
                    case 0xB4: // f32.convert_i64_s
                        current = new F32convertI64s(parser, f); break;
                    case 0xB5: // f32.convert_i64_u
                        current = new F32convertI64u(parser, f); break;
                    case 0xB6: // f32.demote_f64
                        current = new F32demoteF64(parser, f); break;
                    case 0xB7: // f64.convert_i32_s
                        current = new F64convertI32s(parser, f); break;
                    case 0xB8: // f64.convert_i32_u
                        current = new F64convertI32u(parser, f); break;
                    case 0xB9: // f64.convert_i64_s
                        current = new F64convertI64s(parser, f); break;
                    case 0xBA: // f64.convert_i64.u
                        current = new F64convertI64u(parser, f); break;
                    case 0xBB: // f64.promote_f32
                        current = new F64promoteF32(parser, f); break;
                    case 0xBC: // i32.reinterpret_f32
                        current = new I32reinterpretF32(parser, f); break;
                    case 0xBD: // i64.reinterpret_i32
                        current = new I64reinterpretI32(parser, f); break;
                    case 0xBE: // f32.reinterpret_i32
                        current = new F32reinterpretI32(parser, f); break;
                    case 0xBF: // f64.reinterpret_i64
                        current = new F64reinterpretI64(parser, f); break;
                    default:
                        throw new Exception("Opcode not implemented: 0x" + code.ToString("X"));
                }

                if (start == null) start = current;
                if (current != null)
                {
                    current.Pointer = pointer;
                    if(debug)
                    {
                        Console.WriteLine(current);
                        Console.ReadKey();
                    }

                    if (last != null && !done && last.Next == null)
                    {
                        last.Next = current;
                    }

                    last = current;
                }
            }

            if (optimizer)
            {
                last = start;
                for (var c = start; c != null; c = c.Next)
                {
                    // Remove constant instructions
                    if (c.isConstantInstruction && c != start)
                    {
                        var q = new List<object>();
                        do
                        {
                            if ((c as I32const) != null)
                            {
                                q.Add((c as I32const).value);
                            }
                            if ((c as I64const) != null)
                            {
                                q.Add((c as I64const).value);
                            }
                            if ((c as F32const) != null)
                            {
                                q.Add((c as F32const).value);
                            }
                            if ((c as F64const) != null)
                            {
                                q.Add((c as F64const).value);
                            }

                            c = c.Next;
                        } while (c != null && c.isConstantInstruction);

                        if (c != null && (c as End) == null) // can't add the constants to an end since multiple things can go to an end.
                        {
                            c._constants = q.ToArray();
                            last.Next = c;
                            c = last;
                        }
                    }

                    last = c;
                    if (c == null) break;
                }
            }

            return start;
        }
    }
}
