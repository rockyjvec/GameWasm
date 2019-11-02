using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GameWasm.Webassembly.New
{
    public class Runtime
    {
        private Function[] functions;
        private List<Function> _functions;
        private Value[] vStack; // Value stack
        private State[] cStack; // Call stack
        private int cStackPtr = 0;
        private State s; // the current state

        
        // DEBUGGING STUFF
        Stopwatch timer = new Stopwatch();
        Dictionary<byte, TimeSpan> profile = new Dictionary<byte, TimeSpan>();
        public bool Debug = false;
        public bool Profile = false;
        private UInt64 counter = 0;

        public int AddFunction(Function f)
        {
            _functions.Add(f);
            functions = _functions.ToArray();
            return functions.Length - 1;
        }
        
        public Runtime()
        {
            // TODO: these could be set differently
            cStack = new State[1000];
            for (int i = 0; i < 1000; i++)
            {
                cStack[i] = new State();
                cStack[i].locals = new Value[2000];
                cStack[i].lStack = new Label[1000];
            }
            vStack = new Value[1000];
            functions = new Function[0];
            _functions = new List<Function>();
        }

        // This should not be called internally, only for external use.
        public Value ReturnValue()
        {
            return vStack[0];
        }

        // Native call function
        public void Call(int functionIndex, object[] parameters = null)
        {
            if(Debug)
                Console.WriteLine("\n\nNATIVE CALL");
            if (parameters == null)
                parameters = new object[] { };

            cStack[0].ip = 0;
            cStack[0].function = null;
            cStack[0].labelPtr = 0;
            cStack[0].vStackPtr = 0;
            cStackPtr = 1;
            cStack[1].ip = 0;
            cStack[1].function = functions[functionIndex];
            cStack[1].program = cStack[1].function.program;
            cStack[1].labelPtr = 0;
            cStack[1].vStackPtr = 0;
            s = cStack[1];

            // PushLabel
            s.lStack[s.labelPtr].ip = s.program.Length - 1;
            s.lStack[s.labelPtr++].vStackPtr = s.vStackPtr;

            int localPtr = 0;
            // TODO: check for matching type in FStat
            for(int i = 0; i < parameters.Length ; i++)
            {
                if (parameters[i] is UInt32 && s.function.Type.Parameters[i] == Type.i32)
                {
                    s.locals[localPtr].type = Type.i32;
                    s.locals[localPtr].i32 = (UInt32)parameters[i];
                }
                else if (parameters[i] is UInt64 && s.function.Type.Parameters[i] == Type.i64)
                {
                    s.locals[localPtr].type = Type.i64;
                    s.locals[localPtr].i64 = (UInt64)parameters[i];
                }
                else if (parameters[i] is float && s.function.Type.Parameters[i] == Type.f32)
                {
                    s.locals[localPtr].type = Type.f32;
                    s.locals[localPtr].f32 = (float)parameters[i];
                }
                else if (parameters[i] is double && s.function.Type.Parameters[i] == Type.f64)
                {
                    s.locals[localPtr].type = Type.f64;
                    s.locals[localPtr].f64 = (double)parameters[i];
                }
                else
                {
                    throw new Trap("argument type mismatch");
                }

                localPtr++;
            }

            if(s.function.LocalTypes.Length > 0)
                Array.Copy(s.function.LocalTypes, 0, s.locals, localPtr, s.function.LocalTypes.Length);
        }

        // Returns true while there is still work to be done.
        public bool Step(int steps = 1000)
        {
            for(; steps >= 0; --steps)
            {
/*
                if (Debug)
                {
                    if (s.vStackPtr > 0)
                        Console.Write(" => " + Type.Pretify(vStack[s.vStackPtr - 1]));

                    for (int i = 0; i < s.function.Type.Parameters.Length + s.function.LocalTypes.Length; i++)
                    {
                        if (i == 0)
                            Console.Write("\n");
                        if (i > 0)
                            Console.Write("\n");
                        Console.Write(" $var" + i + " = " + Type.Pretify(s.locals[i]));
                    }

                    Console.Write("\n" + s.function.Module.Name + "@" +
                                  s.function.Name + "[" + s.program[s.ip].pointer.ToString("X") +
                                  ", " + s.program[s.ip].i.Pos + "]: " + s.program[s.ip].i.ToString());

                    Console.ReadKey();
                }
                else if (Profile)
                {
                    timer.Stop();
                    var overhead = timer.Elapsed;
                    if (!profile.ContainsKey(0xFF))
                    {
                        profile.Add(0xFF, TimeSpan.Zero);
                    }
                    profile[0xFF] += timer.Elapsed;
                    timer.Reset();
                    timer.Start();
                }
*/
                switch (s.program[s.ip].opCode)
                {
                    case 0x00: // unreachable
                        throw new Trap("unreachable");
                    case 0x01: // nop
                        break;
                    case 0x02: // block
                        // PushLabel
                        s.lStack[s.labelPtr].ip = (int) s.program[s.ip].i32;
                        s.lStack[s.labelPtr].vStackPtr = s.vStackPtr;
                        ++s.labelPtr;
                        break;
                    case 0x03: // loop
                        // PushLabel
                        s.lStack[s.labelPtr].ip = (int) s.ip - 1;
                        s.lStack[s.labelPtr].vStackPtr = s.vStackPtr;
                        ++s.labelPtr;
                        break;
                    case 0x04: // if
                        if (vStack[--s.vStackPtr].i32 > 0)
                        {
                            if (s.program[(int) s.program[s.ip].i32].opCode == 0x05) // if it's an else
                            {
                                // PushLabel
                                s.lStack[s.labelPtr].ip = (int) s.program[(int) s.program[s.ip].i32].i32;
                                s.lStack[s.labelPtr].vStackPtr = s.vStackPtr;
                                ++s.labelPtr;
                            }
                            else
                            {
                                // PushLabel
                                s.lStack[s.labelPtr].ip = (int) s.program[s.ip].i32;
                                s.lStack[s.labelPtr].vStackPtr = s.vStackPtr;
                                ++s.labelPtr;
                            }
                        }
                        else
                        {
                            if (s.program[(int) s.program[s.ip].i32].opCode == 0x05) // if it's an else
                            {
                                // PushLabel
                                s.lStack[s.labelPtr].ip = (int) s.program[(int) s.program[s.ip].i32].i32;
                                s.lStack[s.labelPtr].vStackPtr = s.vStackPtr;
                                ++s.labelPtr;
                            }
                            s.ip = (int) s.program[s.ip].i32;
                        }

                        break;
                    case 0x05: // else
                        s.ip = (int) s.program[s.ip].i32 - 1;
                        break;
                    case 0x0B: // end
                        // If special case of end of a function, just get out of here.
                        if (s.ip + 1 == s.program.Length) break;
                        --s.labelPtr;
                        break;
                    case 0x0C: // br
                    {
                        var l = s.lStack[s.labelPtr - (int) s.program[s.ip].i32 + 1];
                        var len = s.vStackPtr - s.lStack[s.labelPtr - 1].vStackPtr;
                        for (int i = 0; i < len; i++)
                        {
                            vStack[l.vStackPtr++] = vStack[--s.vStackPtr];
                        }

                        s.labelPtr -= (int) s.program[s.ip].i32 + 1;
                        s.vStackPtr = s.lStack[s.labelPtr].vStackPtr;

                        s.ip = s.lStack[s.labelPtr].ip;
                        break;
                    }
                    case 0x0D: // br_if
                    {
                        if (vStack[--s.vStackPtr].i32 > 0)
                        {
                            var l = s.lStack[s.labelPtr - (int) s.program[s.ip].i32 + 1];
                            var len = s.vStackPtr - s.lStack[s.labelPtr - 1].vStackPtr;
                            for (int i = 0; i < len; i++)
                            {
                                vStack[l.vStackPtr++] = vStack[--s.vStackPtr];
                            }

                            s.labelPtr -= (int) s.program[s.ip].i32 + 1;
                            s.vStackPtr = s.lStack[s.labelPtr].vStackPtr;

                            s.ip = s.lStack[s.labelPtr].ip;
                        }

                        break;
                    }
                    case 0x0E: // br_table
                    {
                        UInt32 index = vStack[--s.vStackPtr].i32;

                        if (index >= s.program[s.ip].table.Length)
                        {
                            index = s.program[s.ip].i32;
                        }
                        else
                        {
                            index = s.program[s.ip].table[(int) index];
                        }

                        var l = s.lStack[s.labelPtr - (int) index + 1];
                        var len = s.vStackPtr - s.lStack[s.labelPtr - 1].vStackPtr;
                        for (int i = 0; i < len; i++)
                        {
                            vStack[l.vStackPtr++] = vStack[--s.vStackPtr];
                        }

                        s.labelPtr -= (int) index + 1;
                        s.vStackPtr = s.lStack[s.labelPtr].vStackPtr;

                        s.ip = s.lStack[s.labelPtr].ip;

                        break;
                    }
                    case 0x0F: // return

                        for (int i = 0; i < s.function.Type.Results.Length; i++)
                        {
                            vStack[cStack[cStackPtr - 1].vStackPtr++] = vStack[s.vStackPtr - 1 - i];
                        }

                        s = cStack[--cStackPtr];
                        if (cStackPtr == 0)
                            return false;
                        break;
                    case 0x10: // call
                    {
                        var funcIndex = s.function.Module.functions[(int) s.program[s.ip].i32].GlobalIndex; // TODO: this may need to be optimized

                        if (functions[funcIndex].program == null) // native
                        {
                            Value[] parameters = new Value[functions[funcIndex].Type.Parameters.Length];
                            for (int i = functions[funcIndex].Type.Parameters.Length - 1; i >= 0; i--)
                            {
                                parameters[i] = vStack[--s.vStackPtr];
                            }

                            Value[] returns = functions[funcIndex].native(parameters);

                            for (int i = 0; i < returns.Length; i++)
                            {
                                vStack[s.vStackPtr++] = returns[i];
                            }
                        }
                        else
                        {
                            s = cStack[++cStackPtr];
                            s.ip = -1;
                            s.function = functions[funcIndex];
                            s.program = s.function.program;
                            s.labelPtr = cStack[cStackPtr - 1].labelPtr;

                            // PushLabel
                            s.lStack[s.labelPtr].ip = s.program.Length - 1;
                            s.lStack[s.labelPtr++].vStackPtr = s.vStackPtr;

                            if (s.function.Type.Parameters.Length > 0)
                            {
                                cStack[cStackPtr - 1].vStackPtr -= s.function.Type.Parameters.Length;
                                Array.Copy(vStack, cStack[cStackPtr - 1].vStackPtr, s.locals, 0, s.function.Type.Parameters.Length);
                            }
                        
                            s.vStackPtr = cStack[cStackPtr - 1].vStackPtr;

                            if(s.function.LocalTypes.Length > 0)
                                Array.Copy(functions[funcIndex].LocalTypes, 0, s.locals, s.function.Type.Parameters.Length, functions[funcIndex].LocalTypes.Length);
                        }

                        break;
                    }
                    case 0x11: // call_indirect
                    {
                        var m = s.function.Module;
                        var ii = (int) m.Tables[(int) s.program[s.ip].i32].Get(vStack[--s.vStackPtr].i32);
                        var funcIndex = (int) m.functions[ii].GlobalIndex;

                        
                        s = cStack[++cStackPtr];

                        s.ip = -1;
                        s.function = functions[funcIndex];
                        s.program = s.function.program;
                        s.labelPtr = cStack[cStackPtr - 1].labelPtr;

                        // PushLabel
                        s.lStack[s.labelPtr].ip = s.program.Length - 1;
                        s.lStack[s.labelPtr++].vStackPtr = s.vStackPtr;

                        if (s.function.Type.Parameters.Length > 0)
                        {
                            cStack[cStackPtr - 1].vStackPtr -= s.function.Type.Parameters.Length;
                            Array.Copy(vStack, cStack[cStackPtr - 1].vStackPtr, s.locals, 0, s.function.Type.Parameters.Length);
                        }
                        
                        s.vStackPtr = cStack[cStackPtr - 1].vStackPtr;

                        if(s.function.LocalTypes.Length > 0)
                            Array.Copy(functions[funcIndex].LocalTypes, 0, s.locals, s.function.Type.Parameters.Length, functions[funcIndex].LocalTypes.Length);

                        break;
                    }

                    /* Parametric Instructions */

                    case 0x1A: // drop
                        --s.vStackPtr;
                        break;
                    case 0x1B: // select
                        if (vStack[s.vStackPtr - 1].i32 == 0)
                        {
                            vStack[s.vStackPtr - 3] = vStack[s.vStackPtr - 2];
                        }

                        --s.vStackPtr;
                        --s.vStackPtr;
                        break;

                    /* Variable Instructions */

                    case 0x20: // local.get
                        vStack[s.vStackPtr] = s.locals[s.program[s.ip].i32];
                        ++s.vStackPtr;
                        break;
                    case 0x21: // local.set
                        --s.vStackPtr;
                        s.locals[s.program[s.ip].i32] = vStack[s.vStackPtr];
                        break;
                    case 0x22: // local.tee
                        s.locals[s.program[s.ip].i32] = vStack[s.vStackPtr - 1];
                        break;
                    case 0x23: // global.get
                        vStack[s.vStackPtr++] = s.function.Module.globals[(int)s.program[s.ip].i32].GetValue();

                        break;
                    case 0x24: // global.set
                        s.function.Module.globals[(int)s.program[s.ip].i32].Set(vStack[--s.vStackPtr]);
//                            globals[s.program[s.ip].i32] = vStack[--s.vStackPtr];
                        break;

                    /* Memory Instructions */

                    case 0x28: // i32.load
                    {
                        var pos = s.vStackPtr - 1;
                        var offset = (UInt64) s.program[s.ip].i32 + vStack[pos].i32;
                        vStack[pos].b0 = s.function.Module.memory[0].Buffer[offset + 0];
                        vStack[pos].b1 = s.function.Module.memory[0].Buffer[offset + 1];
                        vStack[pos].b2 = s.function.Module.memory[0].Buffer[offset + 2];
                        vStack[pos].b3 = s.function.Module.memory[0].Buffer[offset + 3];
                        break;
                    }
                    case 0x29: // i64.load
                    {
                        var pos = s.vStackPtr - 1;
                        var offset = (UInt64) s.program[s.ip].i32 + vStack[pos].i32;
                        vStack[pos].b0 = s.function.Module.memory[0].Buffer[offset + 0];
                        vStack[pos].b1 = s.function.Module.memory[0].Buffer[offset + 1];
                        vStack[pos].b2 = s.function.Module.memory[0].Buffer[offset + 2];
                        vStack[pos].b3 = s.function.Module.memory[0].Buffer[offset + 3];
                        vStack[pos].b4 = s.function.Module.memory[0].Buffer[offset + 4];
                        vStack[pos].b5 = s.function.Module.memory[0].Buffer[offset + 5];
                        vStack[pos].b6 = s.function.Module.memory[0].Buffer[offset + 6];
                        vStack[pos].b7 = s.function.Module.memory[0].Buffer[offset + 7];
                        break;
                    }
                    case 0x2A: // f32.load
                        vStack[s.vStackPtr - 1].f32 = s.function
                            .Module.memory[0]
                            .GetF32((UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x2B: // f64.load
                        vStack[s.vStackPtr - 1].f64 = s.function
                            .Module.memory[0]
                            .GetF64((UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x2C: // i32.load8_s
                        vStack[s.vStackPtr - 1].i32 = (UInt32)(Int32)(sbyte)s.function.Module.memory[0].Buffer[(UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 1].i32];
                        break;
                    case 0x2D: // i32.load8_u
                        vStack[s.vStackPtr - 1].i32 = s.function.Module.memory[0].Buffer[(UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 1].i32];
                        break;
                    case 0x2E: // i32.load16_s
                        vStack[s.vStackPtr - 1].i32 = s.function
                            .Module.memory[0]
                            .GetI3216s((UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x2F: // i32.load16_u
                        vStack[s.vStackPtr - 1].i32 = s.function
                            .Module.memory[0]
                            .GetI3216u((UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x30: // i64.load8_s
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.memory[0]
                            .GetI648s((UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x31: // i64.load8_u
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.memory[0]
                            .GetI648u((UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x32: // i64.load16_s
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.memory[0]
                            .GetI6416s((UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x33: // i64.load16_u
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.memory[0]
                            .GetI6416u((UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x34: // i64.load32_s
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.memory[0]
                            .GetI6432s((UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x35: // i64.load32_u
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.memory[0]
                            .GetI6432u((UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 1].i32);
                        break;

                    /* TODO:THESE NEED TO BE OPTIMIZED TO NOT USE FUNCTION CALLS */
                    case 0x36: // i32.store
                    {
                        var pos = s.vStackPtr - 1;
                        var offset = (UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 2].i32;
                        s.function.Module.memory[0].Buffer[offset + 0] = vStack[pos].b0;
                        s.function.Module.memory[0].Buffer[offset + 1] = vStack[pos].b1;
                        s.function.Module.memory[0].Buffer[offset + 2] = vStack[pos].b2;
                        s.function.Module.memory[0].Buffer[offset + 3] = vStack[pos].b3;
                        s.vStackPtr -= 2;
                        break;
                    }
                    case 0x37: // i64.store
                    {
                        var pos = s.vStackPtr - 1;
                        var offset = (UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 2].i32;
                        s.function.Module.memory[0].Buffer[offset + 0] = vStack[pos].b0;
                        s.function.Module.memory[0].Buffer[offset + 1] = vStack[pos].b1;
                        s.function.Module.memory[0].Buffer[offset + 2] = vStack[pos].b2;
                        s.function.Module.memory[0].Buffer[offset + 3] = vStack[pos].b3;
                        s.function.Module.memory[0].Buffer[offset + 4] = vStack[pos].b4;
                        s.function.Module.memory[0].Buffer[offset + 5] = vStack[pos].b5;
                        s.function.Module.memory[0].Buffer[offset + 6] = vStack[pos].b6;
                        s.function.Module.memory[0].Buffer[offset + 7] = vStack[pos].b7;
                        s.vStackPtr -= 2;
                        break;
                    }
                    case 0x38: // f32.store
                        s.function.Module.memory[0].SetI32(
                            (UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 2].i32,
                            vStack[s.vStackPtr - 1]
                                .i32); // this may not work, but they point to the same location so ?
                        s.vStackPtr -= 2;
                        break;
                    case 0x39: // f64.store
                        s.function.Module.memory[0].SetI64(
                            (UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 2].i32,
                            vStack[s.vStackPtr - 1]
                                .i64); // this may not work, but they point to the same location so ?
                        s.vStackPtr -= 2;
                        break;
                    case 0x3A: // i32.store8
                        s.function.Module.memory[0].Buffer[(UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 2].i32] = vStack[s.vStackPtr - 1].b0; 
                        s.vStackPtr -= 2;
                        break;
                    case 0x3B: // i32.store16
                        s.function.Module.memory[0].SetI16(
                            (UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 2].i32,
                            (UInt16) vStack[s.vStackPtr - 1].i32);
                        s.vStackPtr -= 2;
                        break;
                    case 0x3C: // i64.store8
                        s.function.Module.memory[0].Buffer[(UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 2].i32] = vStack[s.vStackPtr - 1].b0; 
                        s.vStackPtr -= 2;
                        break;
                    case 0x3D: // i64.store16
                        s.function.Module.memory[0].SetI16(
                            (UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 2].i32,
                            (UInt16) vStack[s.vStackPtr - 1].i64);
                        s.vStackPtr -= 2;
                        break;
                    case 0x3E: // i64.store32
                        s.function.Module.memory[0].SetI32(
                            (UInt64) s.program[s.ip].i32 + vStack[s.vStackPtr - 2].i32,
                            (UInt32) vStack[s.vStackPtr - 1].i64);
                        s.vStackPtr -= 2;
                        break;
                    case 0x3F: // memory.size
                        vStack[s.vStackPtr++].type = Type.i32;
                        vStack[s.vStackPtr - 1].i32 =
                            (UInt32) s.function.Module.memory[0].CurrentPages;
                        break;
                    case 0x40: // memory.grow
                        vStack[s.vStackPtr - 1].type = Type.i32;
                        vStack[s.vStackPtr - 1].i32 = s.function
                            .Module.memory[0].Grow(vStack[s.vStackPtr - 1].i32);
                        break;

                    /* Numeric Instructions */

                    // These could be optimized by passing the const values as already created Value types
                    case 0x41: // i32.const
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr++].i32 = s.program[s.ip].i32; 
                        break;
                    case 0x42: // i64.const
                        vStack[s.vStackPtr].type = Type.i64;
                        vStack[s.vStackPtr++].i64 = s.program[s.ip].i64; 
                        break;
                    case 0x43: // f32.const
                    {
                        vStack[s.vStackPtr].type = Type.f32;
                        vStack[s.vStackPtr++].f32 = s.program[s.ip].f32;
                        break;
                    }
                    case 0x44: // f64.const
                    {
                        vStack[s.vStackPtr].type = Type.f64;
                        vStack[s.vStackPtr++].f64 = s.program[s.ip].f64;
                        break;
                    }
                    case 0x45: // i32.eqz
                        vStack[s.vStackPtr - 1].i32 =
                            (vStack[s.vStackPtr - 1].i32 == 0) ? 1 : (UInt32) 0;
                        break;
                    case 0x46: // i32.eq
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i32 ==
                             vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x47: // i32.ne
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i32 !=
                             vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x48: // i32.lt_s
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int32) vStack[s.vStackPtr - 2].i32 <
                             (Int32) vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x49: // i32.lt_u
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i32 <
                             vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x4A: // i32.gt_s
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int32) vStack[s.vStackPtr - 2].i32 >
                             (Int32) vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x4B: // i32.gt_u
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i32 >
                             vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x4C: // i32.le_s
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int32) vStack[s.vStackPtr - 2].i32 <=
                             (Int32) vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x4D: // i32.le_u
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i32 <=
                             vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x4E: // i32.ge_s
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int32) vStack[s.vStackPtr - 2].i32 >=
                             (Int32) vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x4F: // i32.ge_u
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i32 >=
                             vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;

                    case 0x50: // i64.eqz
                        vStack[s.vStackPtr - 1].type = Type.i32;
                        vStack[s.vStackPtr - 1].i32 =
                            (vStack[s.vStackPtr - 1].i64 == 0) ? 1 : (UInt32) 0;
                        break;
                    case 0x51: // i64.eq
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i64 ==
                             vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x52: // i64.ne
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i64 !=
                             vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x53: // i64.lt_s
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int64) vStack[s.vStackPtr - 2].i64 <
                             (Int64) vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x54: // i64.lt_u
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i64 <
                             vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x55: // i64.gt_s
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int64) vStack[s.vStackPtr - 2].i64 >
                             (Int64) vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x56: // i64.gt_u
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i64 >
                             vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x57: // i64.le_s
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int64) vStack[s.vStackPtr - 2].i64 <=
                             (Int64) vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x58: // i64.le_u
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i64 <=
                             vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x59: // i64.ge_s
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int64) vStack[s.vStackPtr - 2].i64 >=
                             (Int64) vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x5A: // i64.ge_u
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i64 >=
                             vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;

                    case 0x5B: // f32.eq
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f32 ==
                             vStack[s.vStackPtr - 1].f32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x5C: // f32.ne
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f32 !=
                             vStack[s.vStackPtr - 1].f32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x5D: // f32.lt
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f32 <
                             vStack[s.vStackPtr - 1].f32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x5E: // f32.gt
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f32 >
                             vStack[s.vStackPtr - 1].f32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x5F: // f32.le
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f32 <=
                             vStack[s.vStackPtr - 1].f32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x60: // f32.ge
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f32 >=
                             vStack[s.vStackPtr - 1].f32)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;

                    case 0x61: // f64.eq
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f64 ==
                             vStack[s.vStackPtr - 1].f64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x62: // f64.ne
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f64 !=
                             vStack[s.vStackPtr - 1].f64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x63: // f64.lt
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f64 <
                             vStack[s.vStackPtr - 1].f64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x64: // f64.gt
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f64 >
                             vStack[s.vStackPtr - 1].f64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x65: // f64.le
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f64 <=
                             vStack[s.vStackPtr - 1].f64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;
                    case 0x66: // f64.ge
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f64 >=
                             vStack[s.vStackPtr - 1].f64)
                                ? 1
                                : (UInt32) 0;
                        s.vStackPtr--;
                        break;

                    case 0x67: // i32.clz
                    {
                        // TODO: optimize this
                        var a = vStack[s.vStackPtr - 1].i32;

                        UInt32 bits = 0;
                        UInt32 compare = 0x80000000;
                        while (bits < 32)
                        {
                            if ((compare & a) == 0)
                            {
                                bits++;
                                compare >>= 1;
                            }
                            else
                            {
                                break;
                            }
                        }

                        vStack[s.vStackPtr - 1].i32 = bits;
                        break;
                    }
                    case 0x68: // i32.ctz
                    {
                        // TODO: optimize this
                        var a = vStack[s.vStackPtr - 1].i32;

                        UInt32 bits = 0;
                        UInt32 compare = 1;
                        while (bits < 32)
                        {
                            if ((compare & a) == 0)
                            {
                                bits++;
                                compare <<= 1;
                            }
                            else
                            {
                                break;
                            }
                        }

                        vStack[s.vStackPtr - 1].i32 = bits;

                        break;
                    }
                    case 0x69: // i32.popcnt
                    {
                        // TODO: optimize this
                        var a = vStack[s.vStackPtr - 1].i32;

                        UInt32 bits = 0;
                        UInt32 compare = 1;
                        while (true)
                        {
                            if ((compare & a) != 0)
                            {
                                bits++;
                            }

                            if (compare == 0x80000000) break;
                            compare <<= 1;
                        }

                        vStack[s.vStackPtr - 1].i32 = bits;

                        break;
                    }
                    case 0x6A: // i32.add
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 +
                            vStack[s.vStackPtr - 1].i32;
                        s.vStackPtr--;
                        break;
                    case 0x6B: // i32.sub
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 -
                            vStack[s.vStackPtr - 1].i32;
                        s.vStackPtr--;
                        break;
                    case 0x6C: // i32.mul
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 *
                            vStack[s.vStackPtr - 1].i32;
                        s.vStackPtr--;
                        break;
                    case 0x6D: // i32.div_s
                        vStack[s.vStackPtr - 2].i32 =
                            (UInt32) ((Int32) vStack[s.vStackPtr - 2].i32 /
                                      (Int32) vStack[s.vStackPtr - 1].i32);
                        s.vStackPtr--;
                        break;
                    case 0x6E: // i32.div_u
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 /
                            vStack[s.vStackPtr - 1].i32;
                        s.vStackPtr--;
                        break;
                    case 0x6F: // i32.rem_s
                        vStack[s.vStackPtr - 2].i32 =
                            (UInt32) ((vStack[s.vStackPtr - 2].i32 == 0x80000000 &&
                                       vStack[s.vStackPtr - 1].i32 == 0xFFFFFFFF)
                                ? 0
                                : ((Int32) vStack[s.vStackPtr - 2].i32 %
                                   (Int32) vStack[s.vStackPtr - 1].i32));
                        s.vStackPtr--;
                        break;
                    case 0x70: // i32.rem_u
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 %
                            vStack[s.vStackPtr - 1].i32;
                        s.vStackPtr--;
                        break;
                    case 0x71: // i32.and
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 &
                            vStack[s.vStackPtr - 1].i32;
                        s.vStackPtr--;
                        break;
                    case 0x72: // i32.or
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 |
                            vStack[s.vStackPtr - 1].i32;
                        s.vStackPtr--;
                        break;
                    case 0x73: // i32.xor
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 ^
                            vStack[s.vStackPtr - 1].i32;
                        s.vStackPtr--;
                        break;
                    case 0x74: // i32.shl
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 <<
                            (int) vStack[s.vStackPtr - 1].i32;
                        s.vStackPtr--;
                        break;
                    case 0x75: // i32.shr_s
                        vStack[s.vStackPtr - 2].i32 =
                            (UInt32) ((Int32) vStack[s.vStackPtr - 2].i32 >>
                                      (Int32) vStack[s.vStackPtr - 1].i32);
                        s.vStackPtr--;
                        break;
                    case 0x76: // i32.shr_u
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 >>
                            (int) vStack[s.vStackPtr - 1].i32;
                        s.vStackPtr--;
                        break;
                    case 0x77: // i32.rotl
                        vStack[s.vStackPtr - 2].i32 =
                            ((vStack[s.vStackPtr - 2].i32 <<
                              (int) vStack[s.vStackPtr - 1].i32) |
                             (vStack[s.vStackPtr - 2].i32 >>
                              (32 - (int) vStack[s.vStackPtr - 1].i32)));
                        s.vStackPtr--;
                        break;
                    case 0x78: // i32.rotr
                        vStack[s.vStackPtr - 2].i32 =
                            ((vStack[s.vStackPtr - 2].i32 >>
                              (int) vStack[s.vStackPtr - 1].i32) |
                             (vStack[s.vStackPtr - 2].i32 <<
                              (32 - (int) vStack[s.vStackPtr - 1].i32)));
                        s.vStackPtr--;
                        break;
                    case 0x79: // i64.clz
                    {
                        // TODO: optimize this
                        var a = vStack[s.vStackPtr - 1].i64;

                        UInt32 bits = 0;
                        UInt64 compare = 0x8000000000000000;
                        while (bits < 64)
                        {
                            if ((compare & a) == 0)
                            {
                                bits++;
                                compare >>= 1;
                            }
                            else
                            {
                                break;
                            }
                        }

                        vStack[s.vStackPtr - 1].i64 = bits;
                        break;
                    }
                    case 0x7A: // i64.ctz
                    {
                        // TODO: optimize this
                        var a = vStack[s.vStackPtr - 1].i64;

                        UInt64 bits = 0;
                        UInt64 compare = 1;
                        while (bits < 64)
                        {
                            if ((compare & a) == 0)
                            {
                                bits++;
                                compare <<= 1;
                            }
                            else
                            {
                                break;
                            }
                        }

                        vStack[s.vStackPtr - 1].i64 = bits;
                        break;
                    }
                    case 0x7B: // i64.popcnt
                    {
                        // TODO: optimize this
                        var a = vStack[s.vStackPtr - 1].i64;

                        UInt64 bits = 0;
                        UInt64 compare = 1;
                        while (true)
                        {
                            if ((compare & a) != 0)
                            {
                                bits++;
                            }

                            if (compare == 0x8000000000000000) break;
                            compare <<= 1;
                        }

                        vStack[s.vStackPtr - 1].i64 = bits;
                        break;
                    }
                    case 0x7C: // i64.add
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 +
                            vStack[s.vStackPtr - 1].i64;
                        s.vStackPtr--;
                        break;
                    case 0x7D: // i64.sub
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 -
                            vStack[s.vStackPtr - 1].i64;
                        s.vStackPtr--;
                        break;
                    case 0x7E: // i64.mul
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 *
                            vStack[s.vStackPtr - 1].i64;
                        s.vStackPtr--;
                        break;
                    case 0x7F: // i64.div_s
                        vStack[s.vStackPtr - 2].i64 =
                            (UInt64) ((Int64) vStack[s.vStackPtr - 2].i64 /
                                      (Int64) vStack[s.vStackPtr - 1].i64);
                        s.vStackPtr--;
                        break;
                    case 0x80: // i64.div_u
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 /
                            vStack[s.vStackPtr - 1].i64;
                        s.vStackPtr--;
                        break;
                    case 0x81: // i64.rem_s
                        vStack[s.vStackPtr - 2].i64 =
                            (UInt64) ((vStack[s.vStackPtr - 2].i64 == 0x8000000000000000 &&
                                       vStack[s.vStackPtr - 1].i64 == 0xFFFFFFFFFFFFFFFF)
                                ? 0
                                : ((Int64) vStack[s.vStackPtr - 2].i64 %
                                   (Int64) vStack[s.vStackPtr - 1].i64));
                        s.vStackPtr--;
                        break;
                    case 0x82: // i64.rem_u
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 %
                            vStack[s.vStackPtr - 1].i64;
                        s.vStackPtr--;
                        break;
                    case 0x83: // i64.and
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 &
                            vStack[s.vStackPtr - 1].i64;
                        s.vStackPtr--;
                        break;
                    case 0x84: // i64.or
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 |
                            vStack[s.vStackPtr - 1].i64;
                        s.vStackPtr--;
                        break;
                    case 0x85: // i64.xor
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 ^
                            vStack[s.vStackPtr - 1].i64;
                        s.vStackPtr--;
                        break;
                    case 0x86: // i64.shl
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 <<
                            (int) vStack[s.vStackPtr - 1].i64;
                        s.vStackPtr--;
                        break;
                    case 0x87: // i64.shr_s
                        vStack[s.vStackPtr - 2].i64 =
                            (UInt64) ((Int64) vStack[s.vStackPtr - 2].i64 >>
                                      (int) vStack[s.vStackPtr - 1].i64);
                        s.vStackPtr--;
                        break;
                    case 0x88: // i64.shr_u
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 >>
                            (int) vStack[s.vStackPtr - 1].i64;
                        s.vStackPtr--;
                        break;
                    case 0x89: // i64.rotl
                        vStack[s.vStackPtr - 2].i64 =
                            (UInt64) ((vStack[s.vStackPtr - 2].i64 <<
                                       (int) vStack[s.vStackPtr - 1].i64) |
                                      (vStack[s.vStackPtr - 2].i64 >>
                                       (64 - (int) vStack[s.vStackPtr - 1].i64)));
                        s.vStackPtr--;
                        break;
                    case 0x8A: // i64.rotr
                        vStack[s.vStackPtr - 2].i64 =
                            (UInt64) ((vStack[s.vStackPtr - 2].i64 >>
                                       (int) vStack[s.vStackPtr - 1].i64) |
                                      (vStack[s.vStackPtr - 2].i64 <<
                                       (64 - (int) vStack[s.vStackPtr - 1].i64)));
                        s.vStackPtr--;
                        break;

                    case 0x8B: // f32.abs
                        vStack[s.vStackPtr - 1].f32 =
                            Math.Abs(vStack[s.vStackPtr - 1].f32);
                        break;
                    case 0x8C: // f32.neg
                        vStack[s.vStackPtr - 1].f32 = -vStack[s.vStackPtr - 1].f32;
                        break;
                    case 0x8D: // f32.ceil
                        vStack[s.vStackPtr - 1].f32 =
                            (float) Math.Ceiling(vStack[s.vStackPtr - 1].f32);
                        break;
                    case 0x8E: // f32.floor
                        vStack[s.vStackPtr - 1].f32 =
                            (float) Math.Floor(vStack[s.vStackPtr - 1].f32);
                        break;
                    case 0x8F: // f32.trunc
                        vStack[s.vStackPtr - 1].f32 =
                            (float) Math.Truncate(vStack[s.vStackPtr - 1].f32);
                        break;
                    case 0x90: // f32.nearest
                        vStack[s.vStackPtr - 1].f32 =
                            (float) Math.Round(vStack[s.vStackPtr - 1].f32);
                        break;
                    case 0x91: // f32.sqrt
                        vStack[s.vStackPtr - 1].f32 =
                            (float) Math.Sqrt(vStack[s.vStackPtr - 1].f32);
                        break;
                    case 0x92: // f32.add
                        vStack[s.vStackPtr - 2].f32 =
                            vStack[s.vStackPtr - 2].f32 +
                            vStack[s.vStackPtr - 1].f32;
                        s.vStackPtr--;
                        break;
                    case 0x93: // f32.sub
                        vStack[s.vStackPtr - 2].f32 =
                            vStack[s.vStackPtr - 2].f32 -
                            vStack[s.vStackPtr - 1].f32;
                        s.vStackPtr--;
                        break;
                    case 0x94: // f32.mul
                        vStack[s.vStackPtr - 2].f32 =
                            vStack[s.vStackPtr - 2].f32 *
                            vStack[s.vStackPtr - 1].f32;
                        s.vStackPtr--;
                        break;
                    case 0x95: // f32.div
                        vStack[s.vStackPtr - 2].f32 =
                            vStack[s.vStackPtr - 2].f32 /
                            vStack[s.vStackPtr - 1].f32;
                        s.vStackPtr--;
                        break;
                    case 0x96: // f32.min
                        vStack[s.vStackPtr - 2].f32 = Math.Min(
                            vStack[s.vStackPtr - 2].f32,
                            vStack[s.vStackPtr - 1].f32);
                        s.vStackPtr--;
                        break;
                    case 0x97: // f32.max
                        vStack[s.vStackPtr - 2].f32 = Math.Max(
                            vStack[s.vStackPtr - 2].f32,
                            vStack[s.vStackPtr - 1].f32);
                        s.vStackPtr--;
                        break;
                    case 0x98: // f32.copysign
                        if (vStack[s.vStackPtr - 2].f32 >= 0 &&
                            vStack[s.vStackPtr - 1].f32 < 0)
                        {
                            vStack[s.vStackPtr - 1].f32 =
                                -vStack[s.vStackPtr - 1].f32;
                        }

                        if (vStack[s.vStackPtr - 2].f32 < 0 &&
                            vStack[s.vStackPtr - 1].f32 >= 0)
                        {
                            vStack[s.vStackPtr - 1].f32 =
                                -vStack[s.vStackPtr - 1].f32;
                        }

                        s.vStackPtr--;
                        break;

                    case 0x99: // f64.abs
                        vStack[s.vStackPtr - 1].f64 =
                            Math.Abs(vStack[s.vStackPtr - 1].f64);
                        break;
                    case 0x9A: // f64.neg
                        vStack[s.vStackPtr - 1].f64 = -vStack[s.vStackPtr - 1].f64;
                        break;
                    case 0x9B: // f64.ceil
                        vStack[s.vStackPtr - 1].f64 =
                            Math.Ceiling(vStack[s.vStackPtr - 1].f64);
                        break;
                    case 0x9C: // f64.floor
                        vStack[s.vStackPtr - 1].f64 =
                            Math.Floor(vStack[s.vStackPtr - 1].f64);
                        break;
                    case 0x9D: // f64.trunc
                        vStack[s.vStackPtr - 1].f64 =
                            Math.Truncate(vStack[s.vStackPtr - 1].f64);
                        break;
                    case 0x9E: // f64.nearest
                        vStack[s.vStackPtr - 1].f64 =
                            Math.Round(vStack[s.vStackPtr - 1].f64);
                        break;
                    case 0x9F: // f64.sqrt
                        vStack[s.vStackPtr - 1].f64 =
                            Math.Sqrt(vStack[s.vStackPtr - 1].f64);
                        break;
                    case 0xA0: // f64.add
                        vStack[s.vStackPtr - 2].f64 =
                            vStack[s.vStackPtr - 2].f64 +
                            vStack[s.vStackPtr - 1].f64;
                        s.vStackPtr--;
                        break;
                    case 0xA1: // f64.sub
                        vStack[s.vStackPtr - 2].f64 =
                            vStack[s.vStackPtr - 2].f64 -
                            vStack[s.vStackPtr - 1].f64;
                        s.vStackPtr--;
                        break;
                    case 0xA2: // f64.mul
                        vStack[s.vStackPtr - 2].f64 =
                            vStack[s.vStackPtr - 2].f64 *
                            vStack[s.vStackPtr - 1].f64;
                        s.vStackPtr--;
                        break;
                    case 0xA3: // f64.div
                        vStack[s.vStackPtr - 2].f64 =
                            vStack[s.vStackPtr - 2].f64 /
                            vStack[s.vStackPtr - 1].f64;
                        s.vStackPtr--;
                        break;
                    case 0xA4: // f64.min
                        vStack[s.vStackPtr - 2].f64 = Math.Min(
                            vStack[s.vStackPtr - 2].f64,
                            vStack[s.vStackPtr - 1].f64);
                        s.vStackPtr--;
                        break;
                    case 0xA5: // f64.max
                        vStack[s.vStackPtr - 2].f64 = Math.Max(
                            vStack[s.vStackPtr - 2].f64,
                            vStack[s.vStackPtr - 1].f64);
                        s.vStackPtr--;
                        break;
                    case 0xA6: // f64.copysign
                        if (vStack[s.vStackPtr - 2].f64 >= 0 &&
                            vStack[s.vStackPtr - 1].f64 < 0)
                        {
                            vStack[s.vStackPtr - 1].f64 =
                                -vStack[s.vStackPtr - 1].f64;
                        }

                        if (vStack[s.vStackPtr - 2].f64 < 0 &&
                            vStack[s.vStackPtr - 1].f64 >= 0)
                        {
                            vStack[s.vStackPtr - 1].f64 =
                                -vStack[s.vStackPtr - 1].f64;
                        }

                        s.vStackPtr--;
                        break;

                    case 0xA7: // i32.wrap_i64
                        vStack[s.vStackPtr - 1].type = Type.i32;
                        vStack[s.vStackPtr - 1].i32 =
                            (UInt32) vStack[s.vStackPtr - 1].i64;
                        break;
                    case 0xA8: // i32.trunc_f32_s
                        vStack[s.vStackPtr - 1].type = Type.i32;
                        vStack[s.vStackPtr - 1].i32 =
                            (UInt32) (Int32) Math.Truncate(vStack[s.vStackPtr - 1].f32);
                        break;
                    case 0xA9: // i32.trunc_f32_u
                        vStack[s.vStackPtr - 1].type = Type.i32;
                        vStack[s.vStackPtr - 1].i32 =
                            (UInt32) Math.Truncate(vStack[s.vStackPtr - 1].f32);
                        break;
                    case 0xAA: // i32.trunc_f64_s
                        vStack[s.vStackPtr - 1].type = Type.i32;
                        vStack[s.vStackPtr - 1].i32 =
                            (UInt32) (Int32) Math.Truncate(vStack[s.vStackPtr - 1].f64);
                        break;
                    case 0xAB: // i32.trunc_f64_u
                        vStack[s.vStackPtr - 1].type = Type.i32;
                        vStack[s.vStackPtr - 1].i32 =
                            (UInt32) Math.Truncate(vStack[s.vStackPtr - 1].f64);
                        break;
                    case 0xAC: // i64.extend_i32_s
                        vStack[s.vStackPtr - 1].type = Type.i64;
                        vStack[s.vStackPtr - 1].i64 =
                            (UInt64) (Int64) (Int32) vStack[s.vStackPtr - 1].i32;
                        break;
                    case 0xAD: // i64.extend_i32_u
                        vStack[s.vStackPtr - 1].type = Type.i64;
                        vStack[s.vStackPtr - 1].i64 =
                            (UInt64) vStack[s.vStackPtr - 1].i32;
                        break;
                    case 0xAE: // i64.trunc_f32_s
                        vStack[s.vStackPtr - 1].type = Type.i64;
                        vStack[s.vStackPtr - 1].i64 =
                            (UInt64) (Int64) Math.Truncate(vStack[s.vStackPtr - 1].f32);
                        break;
                    case 0xAF: // i64.trunc_f32_u
                        vStack[s.vStackPtr - 1].type = Type.i64;
                        vStack[s.vStackPtr - 1].i64 =
                            (UInt64) Math.Truncate(vStack[s.vStackPtr - 1].f32);
                        break;
                    case 0xB0: // i64.trunc_f64_s
                        vStack[s.vStackPtr - 1].type = Type.i64;
                        vStack[s.vStackPtr - 1].i64 =
                            (UInt64) (Int64) Math.Truncate(vStack[s.vStackPtr - 1].f64);
                        break;
                    case 0xB1: // i64.trunc_f64_u
                        vStack[s.vStackPtr - 1].type = Type.i64;
                        vStack[s.vStackPtr - 1].i64 =
                            (UInt64) Math.Truncate(vStack[s.vStackPtr - 1].f64);
                        break;
                    case 0xB2: // f32.convert_i32_s
                        vStack[s.vStackPtr - 1].type = Type.f32;
                        vStack[s.vStackPtr - 1].f32 =
                            (float) (Int32) vStack[s.vStackPtr - 1].i32;
                        break;
                    case 0xB3: // f32.convert_i32_u
                        vStack[s.vStackPtr - 1].type = Type.f32;
                        vStack[s.vStackPtr - 1].f32 =
                            (float) vStack[s.vStackPtr - 1].i32;
                        break;
                    case 0xB4: // f32.convert_i64_s
                        vStack[s.vStackPtr - 1].type = Type.f32;
                        vStack[s.vStackPtr - 1].f32 =
                            (float) (Int64) vStack[s.vStackPtr - 1].i64;
                        break;
                    case 0xB5: // f32.convert_i64_u
                        vStack[s.vStackPtr - 1].type = Type.f32;
                        vStack[s.vStackPtr - 1].f32 =
                            (float) vStack[s.vStackPtr - 1].i64;
                        break;
                    case 0xB6: // f32.demote_f64
                        vStack[s.vStackPtr - 1].type = Type.f32;
                        vStack[s.vStackPtr - 1].f32 =
                            (float) vStack[s.vStackPtr - 1].f64;
                        break;
                    case 0xB7: // f64.convert_i32_s
                        vStack[s.vStackPtr - 1].type = Type.f64;
                        vStack[s.vStackPtr - 1].f64 =
                            (double) (Int32) vStack[s.vStackPtr - 1].i32;
                        break;
                    case 0xB8: // f64.convert_i32_u
                        vStack[s.vStackPtr - 1].type = Type.f64;
                        vStack[s.vStackPtr - 1].f64 =
                            (double) vStack[s.vStackPtr - 1].i32;
                        break;
                    case 0xB9: // f64.convert_i64_s
                        vStack[s.vStackPtr - 1].type = Type.f64;
                        vStack[s.vStackPtr - 1].f64 =
                            (double) (Int64) vStack[s.vStackPtr - 1].i64;
                        break;
                    case 0xBA: // f64.convert_i64.u
                        vStack[s.vStackPtr - 1].type = Type.f64;
                        vStack[s.vStackPtr - 1].f64 =
                            (double) vStack[s.vStackPtr - 1].i64;
                        break;
                    case 0xBB: // f64.promote_f32
                        vStack[s.vStackPtr - 1].type = Type.f64;
                        vStack[s.vStackPtr - 1].f64 =
                            (double) vStack[s.vStackPtr - 1].f32;
                        break;
                    case 0xBC: // i32.reinterpret_f32
                        vStack[s.vStackPtr - 1].type = Type.i32;
                        break;
                    case 0xBD: // i64.reinterpret_f64
                        vStack[s.vStackPtr - 1].type = Type.i64;
                        break;
                    case 0xBE: // f32.reinterpret_i32
                        vStack[s.vStackPtr - 1].type = Type.f32;
                        break;
                    case 0xBF: // f64.reinterpret_i64
                        vStack[s.vStackPtr - 1].type = Type.f64;
                        break;
                }
/*
                if (Profile)
                {
                    timer.Stop();

                    if (!profile.ContainsKey(s.program[s.ip].opCode))
                    {
                        profile.Add(s.program[s.ip].opCode, TimeSpan.Zero);
                    }

                    profile[s.program[s.ip].opCode] += timer.Elapsed;
                    
                    if (counter % 10000000 == 0)
                    {
                        TimeSpan total = TimeSpan.Zero;
                        foreach (var keyValuePair in profile.OrderBy(x => x.Value))
                        {
                            var tss = keyValuePair.Value;
                            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                tss.Hours, tss.Minutes, tss.Seconds,
                                tss.Milliseconds / 10);
                            total += tss;
                            Console.WriteLine(Instruction.Instruction.Translate(keyValuePair.Key) + ": " + elapsedTime);
                        }
                        string elapsedTime2 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            total.Hours, total.Minutes, total.Seconds,
                            total.Milliseconds / 10);
                        Console.WriteLine("Total: " + elapsedTime2);
                    }

                    timer.Restart();
                    timer.Start();

                    ++counter;
                }
*/
                ++s.ip;
            }
 
            if (cStackPtr == 0 && s.ip >= s.program.Length)
                return false;
            return true;
        }
    }
}