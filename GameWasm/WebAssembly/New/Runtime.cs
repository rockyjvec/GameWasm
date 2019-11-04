//#define PROFILE
//#define DEBUG

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
        #if (PROFILE)
            Stopwatch timer = new Stopwatch();
            Dictionary<UInt32, TimeSpan> profile = new Dictionary<UInt32, TimeSpan>();
            Dictionary<UInt32, UInt64> followers = new Dictionary<UInt32, UInt64>();

            private UInt64 counter = 0;
            private UInt32 timed;
            private UInt32 lastOpCode = 0x00;
        #endif

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
            #if (DEBUG)
                Console.WriteLine("\n\nNATIVE CALL");
            #endif
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
            s.memory = s.function.Module.Memory[0]; 

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
            // reusable variables for optimization:
            Label label;
            int length;
            int index;
            UInt64 offset;
            int i;
            
            for(; steps != 0; --steps)
            {
                #if (DEBUG)
                    if (s.vStackPtr > 0)
                        Console.Write(" => " + Type.Pretify(vStack[s.vStackPtr - 1]));

                    for (i = 0; i < s.function.Type.Parameters.Length + s.function.LocalTypes.Length; i++)
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
                #endif
                #if (PROFILE)
                    timer.Stop();
                    var overhead = timer.Elapsed;
                    if (!profile.ContainsKey(0xFF))
                    {
                        profile.Add(0xFF, TimeSpan.Zero);
                    }
                    profile[0xFF] += timer.Elapsed;
                    timed = s.program[s.ip].opCode;
/*
                    if (s.program[s.ip].opCode == 0x202821 && s.ip + 1 < s.program.Length)
                    {
                        if (!followers.ContainsKey(lastOpCode))
                        {
                            followers.Add(lastOpCode, 0);
                        }

                        ++followers[lastOpCode];
                    }*/
                    timer.Reset();
                    timer.Start();
                #endif

                switch (s.program[s.ip].opCode)
                {
                    case 0x00: // unreachable
                        throw new Trap("unreachable");
                    case 0x01: // nop
                        break;
                    case 0x02: // block
                        // PushLabel
                        s.lStack[s.labelPtr].ip = s.program[s.ip].pos;
                        s.lStack[s.labelPtr].vStackPtr = s.vStackPtr;
                        ++s.labelPtr;
                        break;
                    case 0x03: // loop
                        // PushLabel
                        s.lStack[s.labelPtr].ip = s.ip - 1;
                        s.lStack[s.labelPtr].vStackPtr = s.vStackPtr;
                        ++s.labelPtr;
                        break;
                    case 0x04: // if
                        if (vStack[--s.vStackPtr].i32 > 0)
                        {
                            if (s.program[s.program[s.ip].pos].opCode == 0x05) // if it's an else
                            {
                                // PushLabel
                                s.lStack[s.labelPtr].ip = s.program[s.program[s.ip].pos].pos;
                                s.lStack[s.labelPtr].vStackPtr = s.vStackPtr;
                                ++s.labelPtr;
                            }
                            else
                            {
                                // PushLabel
                                s.lStack[s.labelPtr].ip = s.program[s.ip].pos;
                                s.lStack[s.labelPtr].vStackPtr = s.vStackPtr;
                                ++s.labelPtr;
                            }
                        }
                        else
                        {
                            if (s.program[s.program[s.ip].pos].opCode == 0x05) // if it's an else
                            {
                                // PushLabel
                                s.lStack[s.labelPtr].ip = s.program[s.program[s.ip].pos].pos;
                                s.lStack[s.labelPtr].vStackPtr = s.vStackPtr;
                                ++s.labelPtr;
                            }
                            s.ip = s.program[s.ip].pos;
                        }

                        break;
                    case 0x05: // else
                        s.ip = s.program[s.ip].pos;
                        break;
                    case 0x0B: // end
                        // If special case of end of a function, just get out of here.
                        if (s.ip + 1 == s.program.Length) break;
                        --s.labelPtr;
                        break;
                    case 0x0C: // br
                        label = s.lStack[s.labelPtr - s.program[s.ip].pos];
                        length = s.vStackPtr - s.lStack[s.labelPtr - 1].vStackPtr;
                        for (i = 0; i < length; ++i)
                        {
                            vStack[label.vStackPtr] = vStack[--s.vStackPtr];
                            label.vStackPtr++;
                        }

                        s.labelPtr -= s.program[s.ip].pos;
                        s.vStackPtr = s.lStack[s.labelPtr].vStackPtr;

                        s.ip = s.lStack[s.labelPtr].ip;
                        break;
                    case 0x0D: // br_if
                        --s.vStackPtr;
                        if (vStack[s.vStackPtr].i32 > 0)
                        {
                            label = s.lStack[s.labelPtr - s.program[s.ip].pos];
                            length = s.vStackPtr - s.lStack[s.labelPtr - 1].vStackPtr;
                            for (i = 0; i < length; ++i)
                            {
                                vStack[label.vStackPtr] = vStack[--s.vStackPtr];
                                ++label.vStackPtr;
                            }

                            s.labelPtr -= s.program[s.ip].pos;
                            s.vStackPtr = s.lStack[s.labelPtr].vStackPtr;

                            s.ip = s.lStack[s.labelPtr].ip;
                        }
                        break;
                    case 0x0E: // br_table
                        --s.vStackPtr;
                        index = (int)vStack[s.vStackPtr].i32;

                        if (index >= s.program[s.ip].table.Length)
                        {
                            index = s.program[s.ip].pos;
                        }
                        else
                        {
                            index = s.program[s.ip].table[index];
                        }

                        label = s.lStack[s.labelPtr - (int) index];
                        length = s.vStackPtr - s.lStack[s.labelPtr - 1].vStackPtr;
                        for (i = 0; i < length; ++i)
                        {
                            vStack[label.vStackPtr] = vStack[--s.vStackPtr];
                            ++label.vStackPtr;
                        }

                        s.labelPtr -= (int) index;
                        s.vStackPtr = s.lStack[s.labelPtr].vStackPtr;

                        s.ip = s.lStack[s.labelPtr].ip;

                        break;
                    case 0x0F: // return

                        for (i = 0; i < s.function.Type.Results.Length; ++i)
                        {
                            vStack[cStack[cStackPtr - 1].vStackPtr++] = vStack[s.vStackPtr - 1 - i];
                        }

                        s = cStack[--cStackPtr];
                        if (cStackPtr == 0)
                            return false;
                        break;
                    case 0x10: // call
                        index = s.function.Module.functions[s.program[s.ip].pos].GlobalIndex; // TODO: this may need to be optimized

                        if (functions[index].program == null) // native
                        {
                            Value[] parameters = new Value[functions[index].Type.Parameters.Length];
                            for (i = functions[index].Type.Parameters.Length - 1; i >= 0; --i)
                            {
                                parameters[i] = vStack[--s.vStackPtr];
                            }

                            Value[] returns = functions[index].native(parameters);

                            for (i = 0; i < returns.Length; ++i)
                            {
                                vStack[s.vStackPtr++] = returns[i];
                            }
                        }
                        else
                        {
                            s = cStack[++cStackPtr];
                            s.ip = -1;
                            s.function = functions[index];
                            s.program = s.function.program;
                            s.labelPtr = cStack[cStackPtr - 1].labelPtr;
                            s.memory = s.function.Module.Memory[0]; 

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
                                Array.Copy(functions[index].LocalTypes, 0, s.locals, s.function.Type.Parameters.Length, functions[index].LocalTypes.Length);
                        }

                        break;
                    case 0x11: // call_indirect
                        index = s.function.Module.functions[s.function.Module.Tables[s.program[s.ip].pos].Get(vStack[--s.vStackPtr].i32)].GlobalIndex;

                        s = cStack[++cStackPtr];

                        s.ip = -1;
                        s.function = functions[index];
                        s.program = s.function.program;
                        s.labelPtr = cStack[cStackPtr - 1].labelPtr;
                        s.memory = s.function.Module.Memory[0]; 

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
                            Array.Copy(functions[index].LocalTypes, 0, s.locals, s.function.Type.Parameters.Length, functions[index].LocalTypes.Length);

                        break;

                    /* Parametric Instructions */

                    case 0x1A: // drop
                        --s.vStackPtr;
                        break;
                    case 0x1B: // select
                        --s.vStackPtr;
                        if (vStack[s.vStackPtr].i32 == 0)
                        {
                            --s.vStackPtr;
                            vStack[s.vStackPtr - 1] = vStack[s.vStackPtr];
                        }
                        else
                        {
                            --s.vStackPtr;
                        }
                        break;

                    /* Variable Instructions */

                    case 0x20: // local.get
                        vStack[s.vStackPtr] = s.locals[s.program[s.ip].a];
                        ++s.vStackPtr;
                        break;
                    case 0x21: // local.set
                        --s.vStackPtr;
                        s.locals[s.program[s.ip].a] = vStack[s.vStackPtr];
                        break;
                    case 0x22: // local.tee
                        --s.vStackPtr;
                        s.locals[s.program[s.ip].a] = vStack[s.vStackPtr];
                        ++s.vStackPtr;
                        break;
                    case 0x23: // global.get
                        vStack[s.vStackPtr] = s.function.Module.globals[s.program[s.ip].a].GetValue();
                        ++s.vStackPtr;
                        break;
                    case 0x24: // global.set
                        --s.vStackPtr;
                        s.function.Module.globals[s.program[s.ip].a].Set(vStack[s.vStackPtr]);
                        break;

                    /* Memory Instructions */

                    case 0x28: // i32.load
                        --s.vStackPtr;
                        offset = s.program[s.ip].pos64 + vStack[s.vStackPtr].i32;
                        vStack[s.vStackPtr].b0 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b1 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b2 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b3 = s.memory.Buffer[offset];
                        ++s.vStackPtr;
                        break;
                    case 0x29: // i64.load
                        --s.vStackPtr;
                        offset = s.program[s.ip].pos64 + vStack[s.vStackPtr].i32;
                        vStack[s.vStackPtr].b0 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b1 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b2 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b3 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b4 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b5 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b6 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b7 = s.memory.Buffer[offset];
                        ++s.vStackPtr;
                        break;
                    case 0x2A: // f32.load
                        vStack[s.vStackPtr - 1].f32 = s.function
                            .Module.memory[0]
                            .GetF32(s.program[s.ip].pos64 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x2B: // f64.load
                        vStack[s.vStackPtr - 1].f64 = s.function
                            .Module.memory[0]
                            .GetF64(s.program[s.ip].pos64 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x2C: // i32.load8_s
                        vStack[s.vStackPtr - 1].i32 = (UInt32)(Int32)(sbyte)s.memory.Buffer[s.program[s.ip].pos64 + vStack[s.vStackPtr - 1].i32];
                        break;
                    case 0x2D: // i32.load8_u
                        vStack[s.vStackPtr - 1].i32 = s.memory.Buffer[s.program[s.ip].pos64 + vStack[s.vStackPtr - 1].i32];
                        break;
                    case 0x2E: // i32.load16_s
                        vStack[s.vStackPtr - 1].i32 = s.function
                            .Module.memory[0]
                            .GetI3216s(s.program[s.ip].pos64 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x2F: // i32.load16_u
                        --s.vStackPtr;
                        offset = s.program[s.ip].pos64 + vStack[s.vStackPtr].i32;
                        vStack[s.vStackPtr].b0 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b1 = s.memory.Buffer[offset];
                        vStack[s.vStackPtr].b2 = 0;
                        vStack[s.vStackPtr].b3 = 0;
                        ++s.vStackPtr;
                        break;
                    case 0x30: // i64.load8_s
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.memory[0]
                            .GetI648s(s.program[s.ip].pos64 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x31: // i64.load8_u
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.memory[0]
                            .GetI648u(s.program[s.ip].pos64 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x32: // i64.load16_s
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.memory[0]
                            .GetI6416s(s.program[s.ip].pos64 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x33: // i64.load16_u
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.memory[0]
                            .GetI6416u(s.program[s.ip].pos64 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x34: // i64.load32_s
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.memory[0]
                            .GetI6432s(s.program[s.ip].pos64 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x35: // i64.load32_u
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.memory[0]
                            .GetI6432u(s.program[s.ip].pos64 + vStack[s.vStackPtr - 1].i32);
                        break;

                    /* TODO:THESE NEED TO BE OPTIMIZED TO NOT USE FUNCTION CALLS */
                    case 0x36: // i32.store
                        --s.vStackPtr;
                        --s.vStackPtr;
                        offset = s.program[s.ip].pos64 + vStack[s.vStackPtr].i32;
                        ++s.vStackPtr;
                        s.memory.Buffer[offset] = vStack[s.vStackPtr].b0;
                        ++offset;
                        s.memory.Buffer[offset] = vStack[s.vStackPtr].b1;
                        ++offset;
                        s.memory.Buffer[offset] = vStack[s.vStackPtr].b2;
                        ++offset;
                        s.memory.Buffer[offset] = vStack[s.vStackPtr].b3;
                        --s.vStackPtr;
                        break;

                    case 0x37: // i64.store
                        --s.vStackPtr;
                        --s.vStackPtr;
                        offset = s.program[s.ip].pos64 + vStack[s.vStackPtr].i32;
                        ++s.vStackPtr;
                        s.memory.Buffer[offset] = vStack[s.vStackPtr].b0;
                        ++offset;
                        s.memory.Buffer[offset] = vStack[s.vStackPtr].b1;
                        ++offset;
                        s.memory.Buffer[offset] = vStack[s.vStackPtr].b2;
                        ++offset;
                        s.memory.Buffer[offset] = vStack[s.vStackPtr].b3;
                        ++offset;
                        s.memory.Buffer[offset] = vStack[s.vStackPtr].b4;
                        ++offset;
                        s.memory.Buffer[offset] = vStack[s.vStackPtr].b5;
                        ++offset;
                        s.memory.Buffer[offset] = vStack[s.vStackPtr].b6;
                        ++offset;
                        s.memory.Buffer[offset] = vStack[s.vStackPtr].b7;
                        --s.vStackPtr;
                        break;
                    case 0x38: // f32.store
                        s.function.Module.memory[0].SetI32(
                            s.program[s.ip].pos64 + vStack[s.vStackPtr - 2].i32,
                            vStack[s.vStackPtr - 1]
                                .i32); // this may not work, but they point to the same location so ?
                        s.vStackPtr -= 2;
                        break;
                    case 0x39: // f64.store
                        s.function.Module.memory[0].SetI64(
                            s.program[s.ip].pos64 + vStack[s.vStackPtr - 2].i32,
                            vStack[s.vStackPtr - 1]
                                .i64); // this may not work, but they point to the same location so ?
                        s.vStackPtr -= 2;
                        break;
                    case 0x3A: // i32.store8
                        s.memory.Buffer[s.program[s.ip].pos64 + vStack[s.vStackPtr - 2].i32] = vStack[s.vStackPtr - 1].b0; 
                        s.vStackPtr -= 2;
                        break;
                    case 0x3B: // i32.store16
                        s.function.Module.memory[0].SetI16(
                            s.program[s.ip].pos64 + vStack[s.vStackPtr - 2].i32,
                            (UInt16) vStack[s.vStackPtr - 1].i32);
                        s.vStackPtr -= 2;
                        break;
                    case 0x3C: // i64.store8
                        s.memory.Buffer[s.program[s.ip].pos64 + vStack[s.vStackPtr - 2].i32] = vStack[s.vStackPtr - 1].b0; 
                        s.vStackPtr -= 2;
                        break;
                    case 0x3D: // i64.store16
                        s.function.Module.memory[0].SetI16(
                            s.program[s.ip].pos64 + vStack[s.vStackPtr - 2].i32,
                            (UInt16) vStack[s.vStackPtr - 1].i64);
                        s.vStackPtr -= 2;
                        break;
                    case 0x3E: // i64.store32
                        s.function.Module.memory[0].SetI32(
                            s.program[s.ip].pos64 + vStack[s.vStackPtr - 2].i32,
                            (UInt32) vStack[s.vStackPtr - 1].i64);
                        s.vStackPtr -= 2;
                        break;
                    case 0x3F: // memory.size
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = (UInt32) s.function.Module.memory[0].CurrentPages;
                        ++s.vStackPtr;
                        break;
                    case 0x40: // memory.grow
                        --s.vStackPtr;
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = s.function.Module.memory[0].Grow(vStack[s.vStackPtr].i32);
                        ++s.vStackPtr;
                        break;

                    /* Numeric Instructions */

                    // These could be optimized by passing the const values as already created Value types
                    case 0x41: // i32.const
                        vStack[s.vStackPtr] = s.program[s.ip].value;
                        ++s.vStackPtr;
                        break;
                    case 0x42: // i64.const
                        vStack[s.vStackPtr] = s.program[s.ip].value;
                        ++s.vStackPtr;
                        break;
                    case 0x43: // f32.const
                    {
                        vStack[s.vStackPtr] = s.program[s.ip].value;
                        ++s.vStackPtr;
                        break;
                    }
                    case 0x44: // f64.const
                    {
                        vStack[s.vStackPtr] = s.program[s.ip].value;
                        ++s.vStackPtr;
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
                        --s.vStackPtr;
                        break;
                    case 0x47: // i32.ne
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i32 !=
                             vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x48: // i32.lt_s
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int32) vStack[s.vStackPtr - 2].i32 <
                             (Int32) vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x49: // i32.lt_u
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i32 <
                             vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x4A: // i32.gt_s
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int32) vStack[s.vStackPtr - 2].i32 >
                             (Int32) vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x4B: // i32.gt_u
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i32 >
                             vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x4C: // i32.le_s
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int32) vStack[s.vStackPtr - 2].i32 <=
                             (Int32) vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x4D: // i32.le_u
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i32 <=
                             vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x4E: // i32.ge_s
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int32) vStack[s.vStackPtr - 2].i32 >=
                             (Int32) vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x4F: // i32.ge_u
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i32 >=
                             vStack[s.vStackPtr - 1].i32)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
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
                        --s.vStackPtr;
                        break;
                    case 0x52: // i64.ne
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i64 !=
                             vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x53: // i64.lt_s
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int64) vStack[s.vStackPtr - 2].i64 <
                             (Int64) vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x54: // i64.lt_u
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i64 <
                             vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x55: // i64.gt_s
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int64) vStack[s.vStackPtr - 2].i64 >
                             (Int64) vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x56: // i64.gt_u
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i64 >
                             vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x57: // i64.le_s
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int64) vStack[s.vStackPtr - 2].i64 <=
                             (Int64) vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x58: // i64.le_u
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i64 <=
                             vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x59: // i64.ge_s
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            ((Int64) vStack[s.vStackPtr - 2].i64 >=
                             (Int64) vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x5A: // i64.ge_u
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].i64 >=
                             vStack[s.vStackPtr - 1].i64)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;

                    case 0x5B: // f32.eq
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f32 ==
                             vStack[s.vStackPtr - 1].f32)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x5C: // f32.ne
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f32 !=
                             vStack[s.vStackPtr - 1].f32)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x5D: // f32.lt
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f32 <
                             vStack[s.vStackPtr - 1].f32)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x5E: // f32.gt
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f32 >
                             vStack[s.vStackPtr - 1].f32)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x5F: // f32.le
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f32 <=
                             vStack[s.vStackPtr - 1].f32)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x60: // f32.ge
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f32 >=
                             vStack[s.vStackPtr - 1].f32)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;

                    case 0x61: // f64.eq
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f64 ==
                             vStack[s.vStackPtr - 1].f64)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x62: // f64.ne
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f64 !=
                             vStack[s.vStackPtr - 1].f64)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x63: // f64.lt
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f64 <
                             vStack[s.vStackPtr - 1].f64)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x64: // f64.gt
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f64 >
                             vStack[s.vStackPtr - 1].f64)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x65: // f64.le
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f64 <=
                             vStack[s.vStackPtr - 1].f64)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
                        break;
                    case 0x66: // f64.ge
                        vStack[s.vStackPtr - 2].type = Type.i32;
                        vStack[s.vStackPtr - 2].i32 =
                            (vStack[s.vStackPtr - 2].f64 >=
                             vStack[s.vStackPtr - 1].f64)
                                ? 1
                                : (UInt32) 0;
                        --s.vStackPtr;
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
                        --s.vStackPtr;
                        break;
                    case 0x6B: // i32.sub
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 -
                            vStack[s.vStackPtr - 1].i32;
                        --s.vStackPtr;
                        break;
                    case 0x6C: // i32.mul
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 *
                            vStack[s.vStackPtr - 1].i32;
                        --s.vStackPtr;
                        break;
                    case 0x6D: // i32.div_s
                        vStack[s.vStackPtr - 2].i32 =
                            (UInt32) ((Int32) vStack[s.vStackPtr - 2].i32 /
                                      (Int32) vStack[s.vStackPtr - 1].i32);
                        --s.vStackPtr;
                        break;
                    case 0x6E: // i32.div_u
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 /
                            vStack[s.vStackPtr - 1].i32;
                        --s.vStackPtr;
                        break;
                    case 0x6F: // i32.rem_s
                        vStack[s.vStackPtr - 2].i32 =
                            (UInt32) ((vStack[s.vStackPtr - 2].i32 == 0x80000000 &&
                                       vStack[s.vStackPtr - 1].i32 == 0xFFFFFFFF)
                                ? 0
                                : ((Int32) vStack[s.vStackPtr - 2].i32 %
                                   (Int32) vStack[s.vStackPtr - 1].i32));
                        --s.vStackPtr;
                        break;
                    case 0x70: // i32.rem_u
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 %
                            vStack[s.vStackPtr - 1].i32;
                        --s.vStackPtr;
                        break;
                    case 0x71: // i32.and
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 &
                            vStack[s.vStackPtr - 1].i32;
                        --s.vStackPtr;
                        break;
                    case 0x72: // i32.or
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 |
                            vStack[s.vStackPtr - 1].i32;
                        --s.vStackPtr;
                        break;
                    case 0x73: // i32.xor
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 ^
                            vStack[s.vStackPtr - 1].i32;
                        --s.vStackPtr;
                        break;
                    case 0x74: // i32.shl
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 <<
                            (int) vStack[s.vStackPtr - 1].i32;
                        --s.vStackPtr;
                        break;
                    case 0x75: // i32.shr_s
                        vStack[s.vStackPtr - 2].i32 =
                            (UInt32) ((Int32) vStack[s.vStackPtr - 2].i32 >>
                                      (Int32) vStack[s.vStackPtr - 1].i32);
                        --s.vStackPtr;
                        break;
                    case 0x76: // i32.shr_u
                        vStack[s.vStackPtr - 2].i32 =
                            vStack[s.vStackPtr - 2].i32 >>
                            (int) vStack[s.vStackPtr - 1].i32;
                        --s.vStackPtr;
                        break;
                    case 0x77: // i32.rotl
                        vStack[s.vStackPtr - 2].i32 = ((vStack[s.vStackPtr - 2].i32 << (int) vStack[s.vStackPtr - 1].i32) | (vStack[s.vStackPtr - 2].i32 >> (32 - (int) vStack[s.vStackPtr - 1].i32)));
                        --s.vStackPtr;
                        break;
                    case 0x78: // i32.rotr
                        vStack[s.vStackPtr - 2].i32 = ((vStack[s.vStackPtr - 2].i32 >> (int) vStack[s.vStackPtr - 1].i32) | (vStack[s.vStackPtr - 2].i32 << (32 - (int) vStack[s.vStackPtr - 1].i32)));
                        --s.vStackPtr;
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
                        --s.vStackPtr;
                        break;
                    case 0x7D: // i64.sub
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 -
                            vStack[s.vStackPtr - 1].i64;
                        --s.vStackPtr;
                        break;
                    case 0x7E: // i64.mul
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 *
                            vStack[s.vStackPtr - 1].i64;
                        --s.vStackPtr;
                        break;
                    case 0x7F: // i64.div_s
                        vStack[s.vStackPtr - 2].i64 =
                            (UInt64) ((Int64) vStack[s.vStackPtr - 2].i64 /
                                      (Int64) vStack[s.vStackPtr - 1].i64);
                        --s.vStackPtr;
                        break;
                    case 0x80: // i64.div_u
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 /
                            vStack[s.vStackPtr - 1].i64;
                        --s.vStackPtr;
                        break;
                    case 0x81: // i64.rem_s
                        vStack[s.vStackPtr - 2].i64 =
                            (UInt64) ((vStack[s.vStackPtr - 2].i64 == 0x8000000000000000 &&
                                       vStack[s.vStackPtr - 1].i64 == 0xFFFFFFFFFFFFFFFF)
                                ? 0
                                : ((Int64) vStack[s.vStackPtr - 2].i64 %
                                   (Int64) vStack[s.vStackPtr - 1].i64));
                        --s.vStackPtr;
                        break;
                    case 0x82: // i64.rem_u
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 %
                            vStack[s.vStackPtr - 1].i64;
                        --s.vStackPtr;
                        break;
                    case 0x83: // i64.and
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 &
                            vStack[s.vStackPtr - 1].i64;
                        --s.vStackPtr;
                        break;
                    case 0x84: // i64.or
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 |
                            vStack[s.vStackPtr - 1].i64;
                        --s.vStackPtr;
                        break;
                    case 0x85: // i64.xor
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 ^
                            vStack[s.vStackPtr - 1].i64;
                        --s.vStackPtr;
                        break;
                    case 0x86: // i64.shl
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 <<
                            (int) vStack[s.vStackPtr - 1].i64;
                        --s.vStackPtr;
                        break;
                    case 0x87: // i64.shr_s
                        vStack[s.vStackPtr - 2].i64 =
                            (UInt64) ((Int64) vStack[s.vStackPtr - 2].i64 >>
                                      (int) vStack[s.vStackPtr - 1].i64);
                        --s.vStackPtr;
                        break;
                    case 0x88: // i64.shr_u
                        vStack[s.vStackPtr - 2].i64 =
                            vStack[s.vStackPtr - 2].i64 >>
                            (int) vStack[s.vStackPtr - 1].i64;
                        --s.vStackPtr;
                        break;
                    case 0x89: // i64.rotl
                        vStack[s.vStackPtr - 2].i64 =
                            (UInt64) ((vStack[s.vStackPtr - 2].i64 <<
                                       (int) vStack[s.vStackPtr - 1].i64) |
                                      (vStack[s.vStackPtr - 2].i64 >>
                                       (64 - (int) vStack[s.vStackPtr - 1].i64)));
                        --s.vStackPtr;
                        break;
                    case 0x8A: // i64.rotr
                        vStack[s.vStackPtr - 2].i64 =
                            (UInt64) ((vStack[s.vStackPtr - 2].i64 >>
                                       (int) vStack[s.vStackPtr - 1].i64) |
                                      (vStack[s.vStackPtr - 2].i64 <<
                                       (64 - (int) vStack[s.vStackPtr - 1].i64)));
                        --s.vStackPtr;
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
                        --s.vStackPtr;
                        break;
                    case 0x93: // f32.sub
                        vStack[s.vStackPtr - 2].f32 =
                            vStack[s.vStackPtr - 2].f32 -
                            vStack[s.vStackPtr - 1].f32;
                        --s.vStackPtr;
                        break;
                    case 0x94: // f32.mul
                        vStack[s.vStackPtr - 2].f32 =
                            vStack[s.vStackPtr - 2].f32 *
                            vStack[s.vStackPtr - 1].f32;
                        --s.vStackPtr;
                        break;
                    case 0x95: // f32.div
                        vStack[s.vStackPtr - 2].f32 =
                            vStack[s.vStackPtr - 2].f32 /
                            vStack[s.vStackPtr - 1].f32;
                        --s.vStackPtr;
                        break;
                    case 0x96: // f32.min
                        vStack[s.vStackPtr - 2].f32 = Math.Min(
                            vStack[s.vStackPtr - 2].f32,
                            vStack[s.vStackPtr - 1].f32);
                        --s.vStackPtr;
                        break;
                    case 0x97: // f32.max
                        vStack[s.vStackPtr - 2].f32 = Math.Max(
                            vStack[s.vStackPtr - 2].f32,
                            vStack[s.vStackPtr - 1].f32);
                        --s.vStackPtr;
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

                        --s.vStackPtr;
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
                        --s.vStackPtr;
                        break;
                    case 0xA1: // f64.sub
                        vStack[s.vStackPtr - 2].f64 =
                            vStack[s.vStackPtr - 2].f64 -
                            vStack[s.vStackPtr - 1].f64;
                        --s.vStackPtr;
                        break;
                    case 0xA2: // f64.mul
                        vStack[s.vStackPtr - 2].f64 =
                            vStack[s.vStackPtr - 2].f64 *
                            vStack[s.vStackPtr - 1].f64;
                        --s.vStackPtr;
                        break;
                    case 0xA3: // f64.div
                        vStack[s.vStackPtr - 2].f64 =
                            vStack[s.vStackPtr - 2].f64 /
                            vStack[s.vStackPtr - 1].f64;
                        --s.vStackPtr;
                        break;
                    case 0xA4: // f64.min
                        vStack[s.vStackPtr - 2].f64 = Math.Min(
                            vStack[s.vStackPtr - 2].f64,
                            vStack[s.vStackPtr - 1].f64);
                        --s.vStackPtr;
                        break;
                    case 0xA5: // f64.max
                        vStack[s.vStackPtr - 2].f64 = Math.Max(
                            vStack[s.vStackPtr - 2].f64,
                            vStack[s.vStackPtr - 1].f64);
                        --s.vStackPtr;
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

                        --s.vStackPtr;
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
                    
                    
                    /* OPTIMIZED OPCODES */
                    
                    case 0x200D: // local.br_if
                        if (s.locals[s.program[s.ip].a].i32 > 0)
                        {
                            label = s.lStack[s.labelPtr - s.program[s.ip].pos];
                            length = s.vStackPtr - s.lStack[s.labelPtr - 1].vStackPtr;
                            for (i = 0; i < length; ++i)
                            {
                                vStack[label.vStackPtr] = vStack[--s.vStackPtr];
                                ++label.vStackPtr;
                            }

                            s.labelPtr -= s.program[s.ip].pos;
                            s.vStackPtr = s.lStack[s.labelPtr].vStackPtr;

                            s.ip = s.lStack[s.labelPtr].ip;
                        }
                        else
                        {
                            ++s.ip;
                        }
                        break;
                    case 0x2021: // local.copy
                        s.locals[s.program[s.ip].b] = s.locals[s.program[s.ip].a];
                        ++s.ip;
                        break;
                    case 0x2028: // local.i32.load 
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        vStack[s.vStackPtr].b0 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b1 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b2 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b3 = s.memory.Buffer[offset];
                        ++s.ip;
                        s.vStackPtr++;
                        break;
                    case 0x2029: // local.i64.load 
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        vStack[s.vStackPtr].b0 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b1 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b2 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b3 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b4 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b5 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b6 = s.memory.Buffer[offset];
                        ++offset;
                        vStack[s.vStackPtr].b7 = s.memory.Buffer[offset];
                        ++s.ip;
                        s.vStackPtr++;
                        break;
                    case 0x202C: // local.i32.load8_s
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        vStack[s.vStackPtr].i32 = (UInt32)(Int32)(sbyte)s.memory.Buffer[offset];
                        ++s.ip;
                        s.vStackPtr++;
                        break;
                    case 0x202D: // local.i32.load8_u
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        vStack[s.vStackPtr].i32 = (UInt32)s.memory.Buffer[offset];
                        ++s.ip;
                        s.vStackPtr++;
                        break;
                    case 0x202E: // local.i32.load16_s
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        vStack[s.vStackPtr].i32 = (UInt32)(Int32)(Int16)((UInt16)s.memory.Buffer[offset] | (UInt16)(s.memory.Buffer[offset + 1] << 8));
                        ++s.ip;
                        s.vStackPtr++;
                        break;
                    case 0x202F: // local.i32.load16_u
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        vStack[s.vStackPtr].i32 = (UInt32)s.memory.Buffer[offset] | (UInt32)(s.memory.Buffer[offset + 1] << 8);
                        ++s.ip;
                        s.vStackPtr++;
                        break;
                    
                    case 0x2036: // local.i32.store 
                        --s.vStackPtr;
                        offset = s.program[s.ip].pos64 + vStack[s.vStackPtr].i32;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].a].b0;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].a].b1;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].a].b2;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].a].b3;
                        ++s.ip;
                        break;
                    case 0x2037: // local.i64.store
                        --s.vStackPtr;
                        offset = s.program[s.ip].pos64 + vStack[s.vStackPtr].i32;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].a].b0;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].a].b1;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].a].b2;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].a].b3;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].a].b4;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].a].b5;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].a].b6;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].a].b7;
                        ++s.ip;
                        break;
                    
                    case 0x203A: // local.i32.store8
                        --s.vStackPtr;
                        offset = s.program[s.ip].pos64 + vStack[s.vStackPtr].i32;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].a].b0;
                        ++s.ip;
                        break;
                    case 0x203B: // local.i32.store16
                        --s.vStackPtr;
                        offset = s.program[s.ip].pos64 + vStack[s.vStackPtr].i32;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].a].b0;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].a].b1;
                        ++s.ip;
                        break;
                    
                    case 0x2045: // local.i32.eqz
                        vStack[s.vStackPtr].i32 = (s.locals[s.program[s.ip].a].i32 == 0)?(UInt32)1:(UInt32)0;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x2046: // local.i32.eq
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = (s.locals[s.program[s.ip].a].i32 == vStack[s.vStackPtr].i32)?(UInt32)1:(UInt32)0;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x2047: // local.i32.ne
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = (s.locals[s.program[s.ip].a].i32 != vStack[s.vStackPtr].i32)?(UInt32)1:(UInt32)0;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x2048: // local.i32.lt_s
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = ((Int32)vStack[s.vStackPtr].i32 < (Int32)s.locals[s.program[s.ip].a].i32)?(UInt32)1:(UInt32)0;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x2049: // local.i32.lt_u
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = (vStack[s.vStackPtr].i32 < s.locals[s.program[s.ip].a].i32)?(UInt32)1:(UInt32)0;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x204A: // local.i32.gt_s
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = ((Int32)vStack[s.vStackPtr].i32 > (Int32)s.locals[s.program[s.ip].a].i32)?(UInt32)1:(UInt32)0;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x204B: // local.i32.gt_u
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = (vStack[s.vStackPtr].i32 > s.locals[s.program[s.ip].a].i32)?(UInt32)1:(UInt32)0;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x206A: // local.i32.add
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 + vStack[s.vStackPtr].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x206B: // local.i32.sub
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = vStack[s.vStackPtr].i32 - s.locals[s.program[s.ip].a].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x206C: // local.i32.mul
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 * vStack[s.vStackPtr].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x206D: // local.i32.div_s
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = (UInt32)((Int32)vStack[s.vStackPtr].i32 / (Int32)s.locals[s.program[s.ip].a].i32);
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x206E: // local.i32.div_u
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = vStack[s.vStackPtr].i32 / s.locals[s.program[s.ip].a].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x206F: // local.i32.rem_s
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = (UInt32) ((vStack[s.vStackPtr].i32 == 0x80000000 & s.locals[s.program[s.ip].a].i32 == 0xFFFFFFFF)?0:((Int32)vStack[s.vStackPtr].i32 % (Int32)s.locals[s.program[s.ip].a].i32));
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x2070: // local.i32.rem_u
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = vStack[s.vStackPtr].i32 % s.locals[s.program[s.ip].a].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x2071: // local.i32.and
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 & vStack[s.vStackPtr].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x2072: // local.i32.or
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 | vStack[s.vStackPtr].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x2073: // local.i32.xor
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 ^ vStack[s.vStackPtr].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x2074: // local.i32.shl
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = vStack[s.vStackPtr].i32 << (int)s.locals[s.program[s.ip].a].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x2075: // local.i32.shr_s
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = (UInt32)((Int32)vStack[s.vStackPtr].i32 >> (Int32)s.locals[s.program[s.ip].a].i32);
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x2076: // local.i32.shr_u
                        --s.vStackPtr;
                        vStack[s.vStackPtr].i32 = vStack[s.vStackPtr].i32 >> (int)s.locals[s.program[s.ip].a].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        break;
                    case 0x4121: // i32.const.local
                        s.locals[s.program[s.ip].a].i32 = s.program[s.ip].i32; 
                        ++s.ip;
                        break;
                    case 0x4221: // i64.const.local
                        s.locals[s.program[s.ip].a].i64 = s.program[s.ip].i64; 
                        ++s.ip;
                        break;
                    case 0x202036: // local.local.i32.store 
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].b].b0;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].b].b1;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].b].b2;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].b].b3;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202037: // local.local.i64.store
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].b].b0;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].b].b1;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].b].b2;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].b].b3;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].b].b4;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].b].b5;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].b].b6;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].b].b7;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20203A: // local.local.i32.store8 
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].b].b0;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20203B: // local.local.i32.store16
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].b].b0;
                        ++offset;
                        s.memory.Buffer[offset] = s.locals[s.program[s.ip].b].b1;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202046: // local.local.132.eq
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = (UInt32)((s.locals[s.program[s.ip].a].i32 == s.locals[s.program[s.ip].b].i32)?1:0);
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202047: // local.local.132.ne
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = (UInt32)((s.locals[s.program[s.ip].a].i32 != s.locals[s.program[s.ip].b].i32)?1:0);
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202048: // local.local.i32.lt_s
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = (UInt32)(((Int32)s.locals[s.program[s.ip].a].i32 < (Int32)s.locals[s.program[s.ip].b].i32)?1:0);
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202049: // local.local.i32.lt_u
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = (s.locals[s.program[s.ip].a].i32 < s.locals[s.program[s.ip].b].i32)?(UInt32)1:(UInt32)0;
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20204A: // local.local.i32.gt_s
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = (UInt32)(((Int32)s.locals[s.program[s.ip].a].i32 > (Int32)s.locals[s.program[s.ip].b].i32)?1:0);
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20204B: // local.local.i32.gt_u
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 > s.locals[s.program[s.ip].b].i32?(UInt32)1:(UInt32)0;
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20206A: // local.local.132.add
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 + s.locals[s.program[s.ip].b].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20206B: // local.local.132.sub
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 - s.locals[s.program[s.ip].b].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20206C: // local.local.132.mul
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 * s.locals[s.program[s.ip].b].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20206D: // local.local.132.div_s
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = (UInt32)((Int32)s.locals[s.program[s.ip].a].i32 / (Int32)s.locals[s.program[s.ip].b].i32);
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20206E: // local.local.132.div_u
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 / s.locals[s.program[s.ip].b].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20206F: // local.local.132.rem_s
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = (UInt32)((s.locals[s.program[s.ip].a].i32 == 0x80000000 && s.locals[s.program[s.ip].b].i32 == 0xFFFFFFFF)?0:((Int32)s.locals[s.program[s.ip].a].i32 % (Int32)s.locals[s.program[s.ip].b].i32));
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202070: // local.local.132.rem_u
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 % s.locals[s.program[s.ip].b].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202071: // local.local.i32.and
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 & s.locals[s.program[s.ip].b].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202072: // local.local.i32.or
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 | s.locals[s.program[s.ip].b].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202073: // local.local.i32.xor
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 ^ s.locals[s.program[s.ip].b].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202074: // local.local.i32.shl
                        vStack[s.vStackPtr].type = Type.i32;
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 << (int)s.locals[s.program[s.ip].b].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202075: // local.local.i32.shr_s
                        vStack[s.vStackPtr].i32 = (UInt32)((Int32)s.locals[s.program[s.ip].a].i32 >> (Int32) s.locals[s.program[s.ip].b].i32);
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202076: // local.local.i32.shr_u
                        vStack[s.vStackPtr].i32 = s.locals[s.program[s.ip].a].i32 >> (int) s.locals[s.program[s.ip].b].i32;
                        ++s.vStackPtr;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202821: // local.i32.load.local
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        index = s.program[s.ip].b;
                        s.locals[index].b0 = s.memory.Buffer[offset];
                        ++offset;
                        s.locals[index].b1 = s.memory.Buffer[offset];
                        ++offset;
                        s.locals[index].b2 = s.memory.Buffer[offset];
                        ++offset;
                        s.locals[index].b3 = s.memory.Buffer[offset];
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202921: // local.i64.load.local
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        s.locals[s.program[s.ip].b].b0 = s.memory.Buffer[offset];
                        ++offset;
                        s.locals[s.program[s.ip].b].b1 = s.memory.Buffer[offset];
                        ++offset;
                        s.locals[s.program[s.ip].b].b2 = s.memory.Buffer[offset];
                        ++offset;
                        s.locals[s.program[s.ip].b].b3 = s.memory.Buffer[offset];
                        ++offset;
                        s.locals[s.program[s.ip].b].b4 = s.memory.Buffer[offset];
                        ++offset;
                        s.locals[s.program[s.ip].b].b5 = s.memory.Buffer[offset];
                        ++offset;
                        s.locals[s.program[s.ip].b].b6 = s.memory.Buffer[offset];
                        ++offset;
                        s.locals[s.program[s.ip].b].b7 = s.memory.Buffer[offset];
                        ++s.ip;
                        ++s.ip;
                        break;
                    
                    case 0x202C21: // local.i32.load8_s.local
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        s.locals[s.program[s.ip].b].i32 = (UInt32)(Int32)(sbyte)s.memory.Buffer[offset];
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202D21: // local.i32.load8_u.local
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        s.locals[s.program[s.ip].b].i32 = (UInt32)s.memory.Buffer[offset];
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202E21: // local.i32.load16_s.local
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        s.locals[s.program[s.ip].b].i32 = (UInt32)(Int32)(Int16)((UInt16)s.memory.Buffer[offset] | (UInt16)(s.memory.Buffer[offset + 1] << 8));
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x202F21: // local.i32.load16_u.local
                        offset = s.program[s.ip].pos64 + s.locals[s.program[s.ip].a].i32;
                        s.locals[s.program[s.ip].b].i32 = (UInt32)s.memory.Buffer[offset] | (UInt32)(s.memory.Buffer[offset + 1] << 8);
                        ++s.ip;
                        ++s.ip;
                        break;

                    
                    /* 32-bit */
                    
                    case 0x20204621: // local.local.i32.eq.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = (UInt32)((s.locals[s.program[s.ip].a].i32 == s.locals[s.program[s.ip].b].i32)?1:0);
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20204721: // local.local.i32.ne.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = (UInt32)((s.locals[s.program[s.ip].a].i32 != s.locals[s.program[s.ip].b].i32)?1:0);
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20204821: // local.local.i32.lt_s.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = (UInt32)(((Int32)s.locals[s.program[s.ip].a].i32 < (Int32)s.locals[s.program[s.ip].b].i32)?1:0);
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20204921: // local.local.i32.lt_u.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = (s.locals[s.program[s.ip].a].i32 < s.locals[s.program[s.ip].b].i32)?(UInt32)1:(UInt32)0;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20204A21: // local.local.i32.gt_s.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = (UInt32)(((Int32)s.locals[s.program[s.ip].a].i32 > (Int32)s.locals[s.program[s.ip].b].i32)?1:0);
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20204B21: // local.local.i32.gt_u.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = s.locals[s.program[s.ip].a].i32 > s.locals[s.program[s.ip].b].i32?(UInt32)1:(UInt32)0;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;                        
                        
                    case 0x20206A21: // local.local.i32.add.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = s.locals[s.program[s.ip].a].i32 + s.locals[s.program[s.ip].b].i32;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20206B21: // local.local.i32.sub.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = s.locals[s.program[s.ip].a].i32 - s.locals[s.program[s.ip].b].i32;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20206C21: // local.local.i32.mul.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = s.locals[s.program[s.ip].a].i32 * s.locals[s.program[s.ip].b].i32;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20206D21: // local.local.i32.div_s.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = (UInt32)((Int32)s.locals[s.program[s.ip].a].i32 / (Int32)s.locals[s.program[s.ip].b].i32);
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20206E21: // local.local.i32.div_u.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = s.locals[s.program[s.ip].a].i32 / s.locals[s.program[s.ip].b].i32;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20206F21: // local.local.i32.rem_s.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = (UInt32) ((s.locals[s.program[s.ip].a].i32 == 0x80000000 && s.locals[s.program[s.ip].b].i32 == 0xFFFFFFFF)?0:((Int32)s.locals[s.program[s.ip].a].i32 % (Int32)s.locals[s.program[s.ip].b].i32));
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20207021: // local.local.i32.rem_u.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = s.locals[s.program[s.ip].a].i32 - s.locals[s.program[s.ip].b].i32;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20207121: // local.local.i32.and.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = s.locals[s.program[s.ip].a].i32 & s.locals[s.program[s.ip].b].i32;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20207221: // local.local.i32.or.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = s.locals[s.program[s.ip].a].i32 | s.locals[s.program[s.ip].b].i32;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20207321: // local.local.i32.xor.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = s.locals[s.program[s.ip].a].i32 ^ s.locals[s.program[s.ip].b].i32;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20207421: // local.local.i32.shl.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = s.locals[s.program[s.ip].a].i32 << (int)s.locals[s.program[s.ip].b].i32;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20207521: // local.local.i32.shr_s.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = (UInt32)((Int32)s.locals[s.program[s.ip].a].i32 >> (Int32)s.locals[s.program[s.ip].b].i32);
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20207621: // local.local.i32.shr_u.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = s.locals[s.program[s.ip].a].i32 >> (int)s.locals[s.program[s.ip].b].i32;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20207721: // local.local.i32.rotl.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = s.locals[s.program[s.ip].a].i32 << (int) s.locals[s.program[s.ip].b].i32 | (s.locals[s.program[s.ip].a].i32 >> (32 - (int) s.locals[s.program[s.ip].b].i32)); 
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20207821: // local.local.i32.rotr.local
                        s.locals[s.program[s.ip].c].type = Type.i32;
                        s.locals[s.program[s.ip].c].i32 = s.locals[s.program[s.ip].a].i32 >> (int) s.locals[s.program[s.ip].b].i32 | (s.locals[s.program[s.ip].a].i32 << (32 - (int) s.locals[s.program[s.ip].b].i32)); 
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    
                    /* 64-bit */
                    
                    case 0x20207C21: // local.local.i64.add.local
                        s.locals[s.program[s.ip].c].type = Type.i64;
                        s.locals[s.program[s.ip].c].i64 = s.locals[s.program[s.ip].a].i64 + s.locals[s.program[s.ip].b].i64;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20207D21: // local.local.i64.sub.local
                        s.locals[s.program[s.ip].c].type = Type.i64;
                        s.locals[s.program[s.ip].c].i64 = s.locals[s.program[s.ip].a].i64 - s.locals[s.program[s.ip].b].i64;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20208321: // local.local.i64.and.local
                        s.locals[s.program[s.ip].c].type = Type.i64;
                        s.locals[s.program[s.ip].c].i64 = s.locals[s.program[s.ip].a].i64 & s.locals[s.program[s.ip].b].i64;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20208421: // local.local.i64.or.local
                        s.locals[s.program[s.ip].c].type = Type.i64;
                        s.locals[s.program[s.ip].c].i64 = s.locals[s.program[s.ip].a].i64 | s.locals[s.program[s.ip].b].i64;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0x20208521: // local.local.i64.xor.local
                        s.locals[s.program[s.ip].c].type = Type.i64;
                        s.locals[s.program[s.ip].c].i64 = s.locals[s.program[s.ip].a].i64 ^ s.locals[s.program[s.ip].b].i64;
                        ++s.ip;
                        ++s.ip;
                        ++s.ip;
                        break;
                    case 0xFF000000: // loop of local.i32.load.local
                        length = s.ip;
                        for (i = 0; i < s.program[length].optimalProgram.Length; ++i)
                        {
                            offset = s.program[length].optimalProgram[i].pos64 + s.locals[s.program[length].optimalProgram[i].a].i32;
                            index = s.program[length].optimalProgram[i].b;
                            s.locals[index].b0 = s.memory.Buffer[offset];
                            ++offset;
                            s.locals[index].b1 = s.memory.Buffer[offset];
                            ++offset;
                            s.locals[index].b2 = s.memory.Buffer[offset];
                            ++offset;
                            s.locals[index].b3 = s.memory.Buffer[offset];
                            ++s.ip;
                            ++s.ip;
                            ++s.ip;
                        }
                        --s.ip;
                        break;
                    default:
                        throw new Exception("Invalid opCode: " + s.program[s.ip].opCode.ToString("X"));
                }


                #if (PROFILE)
                    timer.Stop();

                    if (!profile.ContainsKey(timed))
                    {
                        profile.Add(timed, TimeSpan.Zero);
                    }

                    profile[timed] += timer.Elapsed;
                    lastOpCode = timed;
                    
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

/*
                        foreach (var keyValuePair in followers.OrderBy(x => x.Value))
                        {
                            Console.WriteLine(Instruction.Instruction.Translate(keyValuePair.Key) + ": " + keyValuePair.Value);
                        }*/
                    }

                    timer.Restart();
                    timer.Start();

                    ++counter;
                #endif
                
                ++s.ip;
            }
 
            if (cStackPtr == 0 && s.ip >= s.program.Length)
                return false;
            return true;
        }
    }
}