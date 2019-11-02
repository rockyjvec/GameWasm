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
        private Label[] lStack; // Label stack
        private Value[] vStack; // Value stack
        private State[] cStack; // Call stack
        private int cStackPtr = 0;
        private Value[] locals;
        private int localPtr = 0;
        private State s; // the current state

        
        // DEBUGGING STUFF
        Stopwatch timer = new Stopwatch();
        Dictionary<int, TimeSpan> profile = new Dictionary<int, TimeSpan>();
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
            lStack = new Label[1000];
            cStack = new State[1000];
            for (int i = 0; i < 1000; i++) cStack[i] = new State();
            vStack = new Value[1000];
            locals = new Value[10000];
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
            cStack[0].labelBasePtr = 0;
            cStack[0].labelPtr = 0;
            cStack[0].localBasePtr = 0;
            cStack[0].vStackPtr = 0;
            cStackPtr = 1;
            cStack[1].ip = 0;
            cStack[1].function = functions[functionIndex];
            cStack[1].labelBasePtr = 0;
            cStack[1].labelPtr = 0;
            cStack[1].localBasePtr = localPtr;
            cStack[1].vStackPtr = 0;
            s = cStack[1];


            PushLabel(9999999);
            
            // TODO: check for matching type in FStat
            for(int i = 0; i < parameters.Length ; i++)
            {
                if (parameters[i] is UInt32 && functions[functionIndex].Type.Parameters[i] == Type.i32)
                {
                    locals[localPtr].type = Type.i32;
                    locals[localPtr].i32 = (UInt32)parameters[i];
                }
                else if (parameters[i] is UInt64 && functions[functionIndex].Type.Parameters[i] == Type.i64)
                {
                    locals[localPtr].type = Type.i64;
                    locals[localPtr].i64 = (UInt64)parameters[i];
                }
                else if (parameters[i] is float && functions[functionIndex].Type.Parameters[i] == Type.f32)
                {
                    locals[localPtr].type = Type.f32;
                    locals[localPtr].f32 = (float)parameters[i];
                }
                else if (parameters[i] is double && functions[functionIndex].Type.Parameters[i] == Type.f64)
                {
                    locals[localPtr].type = Type.f64;
                    locals[localPtr].f64 = (double)parameters[i];
                }
                else
                {
                    throw new Trap("argument type mismatch");
                }

                localPtr++;
            }

            for (int i = 0; i < functions[functionIndex].LocalTypes.Count; i++)
            {
                locals[localPtr].type = functions[functionIndex].LocalTypes[i];
                locals[localPtr].i64 = 0; // shared offsets means this updates all, yay
                localPtr++;
            }
        }

        private void PushLabel(int ip)
        {
            lStack[s.labelBasePtr + s.labelPtr].ip = ip;
            lStack[s.labelBasePtr + s.labelPtr].vStackPtr = s.vStackPtr;
            
            s.labelPtr++;
        }
        
        private Label PopLabel(int number, bool end = false)
        {
            Label old;
            Label l = old = lStack[s.labelBasePtr + --s.labelPtr];
            if (l.ip == 1480)
            {
                Debugger.Break();
            }
            for (; number > 1; number--)
            {
                l = lStack[s.labelBasePtr + --s.labelPtr];
                if (l.ip == 1480)
                {
                    Debugger.Break();
                }
            }

            if (!end)
            {
                for (int i = 0; i < s.vStackPtr - old.vStackPtr; i++)
                {
                    vStack[l.vStackPtr++] = vStack[--s.vStackPtr];
                }
                
                s.vStackPtr = l.vStackPtr;
            }

            return l;
        }
        
        // Returns true while there is still work to be done.
        public bool Step(int steps = 1000)
        {
            while (steps >= 0)
            {
                if (s.ip >= s.function.program.Length)
                {
                    if (cStackPtr == 1)
                        return false;

                    // TODO: this probably isn't very efficient to create a new value array for every return
                    Value[] returnValues = new Value[s.function.Type.Results.Length];
                    for (int i = 0; i < s.function.Type.Results.Length; i++)
                    {
                        returnValues[i] = vStack[s.vStackPtr - 1 - i];
                    }

                    for (int i = 0; i < s.function.Type.Results.Length; i++)
                    {
                        vStack[cStack[cStackPtr - 1].vStackPtr++] = returnValues[i];
                    }

                    localPtr = s.localBasePtr;
                    
                    s = cStack[--cStackPtr];
                    if (cStackPtr == 0)
                        return false;
                    steps--;
                    s.ip++;
                    continue;
                }

                var inst = s.function.program[s.ip];

                if (Debug)
                {
                    if (s.vStackPtr > 0)
                        Console.Write(" => " + Type.Pretify(vStack[s.vStackPtr - 1]));

                    for (int i = 0; i < s.function.Type.Parameters.Length + s.function.LocalTypes.Count; i++)
                    {
                        if (i == 0)
                            Console.Write("\n");
                        if (i > 0)
                            Console.Write("\n");
                        Console.Write(" $var" + i + " = " + Type.Pretify(locals[s.localBasePtr + i]));
                    }

                    Console.Write("\n" + s.function.Module.Name + "@" +
                                  s.function.Name + "[" + inst.pointer.ToString("X") +
                                  ", " + inst.i.Pos + "]: " + inst.i.ToString());

                    Console.ReadKey();
                }

                if (Profile)
                {
                    timer.Reset();
                    timer.Start();
                }

                switch (inst.opCode)
                {
                    case 0x00: // unreachable
                        throw new Trap("unreachable");
                    case 0x01: // nop
                        break;
                    case 0x02: // block
                        PushLabel((int) inst.i32);
                        break;
                    case 0x03: // loop
                        PushLabel(s.ip - 1);
                        break;
                    case 0x04: // if
                        if (vStack[--s.vStackPtr].i32 > 0)
                        {
                            if (s.function.program[(int) inst.i32].opCode == 0x05
                            ) // if it's an else
                                PushLabel((int) s.function.program[(int) inst.i32].i32);
                            else
                                PushLabel((int) inst.i32);
                        }
                        else
                        {
                            if (s.function.program[(int) inst.i32].opCode == 0x05
                            ) // if it's an else
                                PushLabel((int) s.function.program[(int) inst.i32].i32);
                            s.ip = (int) inst.i32;
                        }

                        break;
                    case 0x05: // else
                        s.ip = (int) inst.i32 - 1;
                        break;
                    case 0x0B: // end
                        // If special case of end of a function, just get out of here.
                        if (s.ip + 1 ==
                            s.function.program.Length) break;
                        PopLabel(1, true);
                        break;
                    case 0x0C: // br
                    {
                        Label l = PopLabel((int) inst.i32 + 1);
                        s.ip = l.ip;
                        break;
                    }
                    case 0x0D: // br_if
                    {
                        if (vStack[--s.vStackPtr].i32 > 0)
                        {
                            Label l = PopLabel((int) inst.i32 + 1);
                            s.ip = l.ip;
                        }

                        break;
                    }
                    case 0x0E: // br_table
                    {
                        UInt32 index = vStack[--s.vStackPtr].i32;

                        if (index >= inst.table.Length)
                        {
                            index = inst.i32;
                        }
                        else
                        {
                            index = inst.table[(int) index];
                        }

                        Label l = PopLabel((int) index + 1);

                        s.ip = l.ip;

                        break;
                    }
                    case 0x0F: // return

//                            if (cStackPtr == 1) return false;
                        
                        // TODO: this probably isn't very efficient to create a new value array for every return
                        Value[] returnValues =
                            new Value[s.function.Type.Results.Length];
                        for (int i = 0; i < s.function.Type.Results.Length; i++)
                        {
                            if (vStack[s.vStackPtr - 1 - i].type !=
                                s.function.Type.Results[i])
                            {
                                throw new Exception("return type mismatch");
                            }
                            returnValues[i] = vStack[s.vStackPtr - 1 - i];
                        }

                        for (int i = 0; i < s.function.Type.Results.Length; i++)
                        {
                            vStack[cStack[cStackPtr - 1].vStackPtr++] = returnValues[i];
                        }

                        localPtr = s.localBasePtr;
                        
                        s = cStack[--cStackPtr];
                        if (cStackPtr == 0)
                            return false;
                        break;
                    case 0x10: // call
                    {
                        var funcIndex = s.function.Module.Functions[(int) inst.i32]
                            .GlobalIndex; // TODO: this may need to be optimized

                        if (functions[funcIndex].program == null) // native
                        {
                            Value[] parameters = new Value[functions[funcIndex].Type.Parameters.Length];
                            for (int i = functions[funcIndex].Type.Parameters.Length - 1; i >= 0; i--)
                            {
                                //     if (functions[funcIndex].Type.Parameters[i] !=
                                //       vStack[cStack[cStackPtr - 1].vStackPtr - 1].type)
                                {
                                    //  throw new Trap("call type mismatch");
                                }

                                // TODO: should probably validate the types, but whatevs
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
                            s.labelPtr = cStack[cStackPtr - 1].labelPtr;
                            s.localBasePtr = localPtr;

                            PushLabel(9999999);
                        
                            // TODO: check for matching type in FStat
                            for (int i = functions[funcIndex].Type.Parameters.Length - 1; i >= 0; i--)
                            {
                                //     if (functions[funcIndex].Type.Parameters[i] !=
                                //       vStack[cStack[cStackPtr - 1].vStackPtr - 1].type)
                                {
                                    //  throw new Trap("call type mismatch");
                                }

                                // TODO: should probably validate the types, but whatevs
                                locals[localPtr] = vStack[cStack[cStackPtr - 1].vStackPtr - i - 1];
                                localPtr++;
                            }
                            cStack[cStackPtr - 1].vStackPtr -= functions[funcIndex].Type.Parameters.Length;
                            s.vStackPtr = cStack[cStackPtr - 1].vStackPtr;

                            for (int i = 0; i < functions[funcIndex].LocalTypes.Count; i++)
                            {
                                // TODO:  should I record the actual function local types here?
                                locals[localPtr].type = functions[funcIndex].LocalTypes[i];
                                locals[localPtr].i64 = 0; // shared offsets means this updates all, yay
                                localPtr++;
                            }
                        }

                        break;
                    }
                    case 0x11: // call_indirect
                    {
                        var m = s.function.Module;
                        var ii = (int) m.Tables[(int) inst.i32].Get(vStack[--s.vStackPtr].i32);
                        var funcIndex = (int) m.Functions[ii].GlobalIndex;

                        
                        s = cStack[++cStackPtr];

                        s.ip = -1;
                        s.function = functions[funcIndex];
                        s.labelPtr = cStack[cStackPtr - 1].labelPtr;
                        s.localBasePtr = localPtr;

                        PushLabel(9999999);

                        // TODO: check for matching type in FStat
                        for (int i = functions[funcIndex].Type.Parameters.Length - 1; i >= 0 ; i--)
                        {
//                              if (functions[funcIndex].Type.Parameters[i] !=
//                                    vStack[cStack[cStackPtr - 1].vStackPtr - 1].type)
                            {
                               // throw new Trap("indirect call type mismatch");
                            }

                            // TODO: should probably validate the types, but whatevs
                            locals[localPtr] = vStack[cStack[cStackPtr - 1].vStackPtr - i - 1];
                            localPtr++;
                        }
                        cStack[cStackPtr - 1].vStackPtr -= functions[funcIndex].Type.Parameters.Length;
                        s.vStackPtr = cStack[cStackPtr - 1].vStackPtr;

                        for (int i = 0; i < functions[funcIndex].LocalTypes.Count; i++)
                        {
                            // TODO:  should I record the actual function local types here?
                            locals[localPtr].type = functions[funcIndex].LocalTypes[i];
                            locals[localPtr].i64 = 0; // shared offsets means this updates all, yay
                            localPtr++;
                        }

                        break;
                    }

                    /* Parametric Instructions */

                    case 0x1A: // drop
                        s.vStackPtr--;
                        break;
                    case 0x1B: // select
                        if (vStack[s.vStackPtr - 1].i32 == 0)
                        {
                            vStack[s.vStackPtr - 3] = vStack[s.vStackPtr - 2];
                        }

                        s.vStackPtr -= 2;
                        break;

                    /* Variable Instructions */

                    case 0x20: // local.get
                        vStack[s.vStackPtr++] = locals[s.localBasePtr + inst.i32];
                        break;
                    case 0x21: // local.set
                        locals[s.localBasePtr + inst.i32] = vStack[--s.vStackPtr];
                        break;
                    case 0x22: // local.tee
                        locals[s.localBasePtr + inst.i32] = vStack[s.vStackPtr - 1];
                        break;
                    case 0x23: // global.get
                        vStack[s.vStackPtr++] = s.function.Module.Globals[(int)inst.i32].GetValue();

                        break;
                    case 0x24: // global.set
                        s.function.Module.Globals[(int)inst.i32].Set(vStack[--s.vStackPtr]);
//                            globals[inst.i32] = vStack[--s.vStackPtr];
                        break;

                    /* Memory Instructions */

                    case 0x28: // i32.load
                        vStack[s.vStackPtr - 1].i32 = s.function
                            .Module.Memory[0]
                            .GetI32((UInt64) inst.i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x29: // i64.load
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.Memory[0]
                            .GetI64((UInt64) inst.i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x2A: // f32.load
                        vStack[s.vStackPtr - 1].f32 = s.function
                            .Module.Memory[0]
                            .GetF32((UInt64) inst.i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x2B: // f64.load
                        vStack[s.vStackPtr - 1].f64 = s.function
                            .Module.Memory[0]
                            .GetF64((UInt64) inst.i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x2C: // i32.load8_s
                        vStack[s.vStackPtr - 1].i32 = s.function
                            .Module.Memory[0]
                            .GetI328s((UInt64) inst.i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x2D: // i32.load8_u
                        vStack[s.vStackPtr - 1].i32 = s.function
                            .Module.Memory[0]
                            .GetI328u((UInt64) inst.i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x2E: // i32.load16_s
                        vStack[s.vStackPtr - 1].i32 = s.function
                            .Module.Memory[0]
                            .GetI3216s((UInt64) inst.i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x2F: // i32.load16_u
                        vStack[s.vStackPtr - 1].i32 = s.function
                            .Module.Memory[0]
                            .GetI3216u((UInt64) inst.i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x30: // i64.load8_s
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.Memory[0]
                            .GetI648s((UInt64) inst.i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x31: // i64.load8_u
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.Memory[0]
                            .GetI648u((UInt64) inst.i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x32: // i64.load16_s
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.Memory[0]
                            .GetI6416s((UInt64) inst.i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x33: // i64.load16_u
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.Memory[0]
                            .GetI6416u((UInt64) inst.i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x34: // i64.load32_s
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.Memory[0]
                            .GetI6432s((UInt64) inst.i32 + vStack[s.vStackPtr - 1].i32);
                        break;
                    case 0x35: // i64.load32_u
                        vStack[s.vStackPtr - 1].i64 = s.function
                            .Module.Memory[0]
                            .GetI6432u((UInt64) inst.i32 + vStack[s.vStackPtr - 1].i32);
                        break;

                    /* TODO:THESE NEED TO BE OPTIMIZED TO NOT USE FUNCTION CALLS */
                    case 0x36: // i32.store
                        s.function.Module.Memory[0].SetI32(
                            (UInt64) inst.i32 + vStack[s.vStackPtr - 2].i32,
                            vStack[s.vStackPtr - 1].i32);
                        s.vStackPtr -= 2;
                        break;
                    case 0x37: // i64.store
                        s.function.Module.Memory[0].SetI64(
                            (UInt64) inst.i32 + vStack[s.vStackPtr - 2].i32,
                            vStack[s.vStackPtr - 1].i64);
                        s.vStackPtr -= 2;
                        break;
                    case 0x38: // f32.store
                        s.function.Module.Memory[0].SetI32(
                            (UInt64) inst.i32 + vStack[s.vStackPtr - 2].i32,
                            vStack[s.vStackPtr - 1]
                                .i32); // this may not work, but they point to the same location so ?
                        s.vStackPtr -= 2;
                        break;
                    case 0x39: // f64.store
                        s.function.Module.Memory[0].SetI64(
                            (UInt64) inst.i32 + vStack[s.vStackPtr - 2].i32,
                            vStack[s.vStackPtr - 1]
                                .i64); // this may not work, but they point to the same location so ?
                        s.vStackPtr -= 2;
                        break;
                    case 0x3A: // i32.store8
                        s.function.Module.Memory[0].Set(
                            (UInt64) inst.i32 + vStack[s.vStackPtr - 2].i32,
                            (byte) vStack[s.vStackPtr - 1].i32);
                        s.vStackPtr -= 2;
                        break;
                    case 0x3B: // i32.store16
                        s.function.Module.Memory[0].SetI16(
                            (UInt64) inst.i32 + vStack[s.vStackPtr - 2].i32,
                            (UInt16) vStack[s.vStackPtr - 1].i32);
                        s.vStackPtr -= 2;
                        break;
                    case 0x3C: // i64.store8
                        s.function.Module.Memory[0].Set(
                            (UInt64) inst.i32 + vStack[s.vStackPtr - 2].i32,
                            (byte) vStack[s.vStackPtr - 1].i64);
                        s.vStackPtr -= 2;
                        break;
                    case 0x3D: // i64.store16
                        s.function.Module.Memory[0].SetI16(
                            (UInt64) inst.i32 + vStack[s.vStackPtr - 2].i32,
                            (UInt16) vStack[s.vStackPtr - 1].i64);
                        s.vStackPtr -= 2;
                        break;
                    case 0x3E: // i64.store32
                        s.function.Module.Memory[0].SetI32(
                            (UInt64) inst.i32 + vStack[s.vStackPtr - 2].i32,
                            (UInt32) vStack[s.vStackPtr - 1].i64);
                        s.vStackPtr -= 2;
                        break;
                    case 0x3F: // memory.size
                        vStack[s.vStackPtr++].type = Type.i32;
                        vStack[s.vStackPtr - 1].i32 =
                            (UInt32) s.function.Module.Memory[0].CurrentPages;
                        break;
                    case 0x40: // memory.grow
                        vStack[s.vStackPtr - 1].type = Type.i32;
                        vStack[s.vStackPtr - 1].i32 = s.function
                            .Module.Memory[0].Grow(vStack[s.vStackPtr - 1].i32);
                        break;

                    /* Numeric Instructions */

                    // These could be optimized by passing the const values as already created Value types
                    case 0x41: // i32.const
                    {
                        Value v = new Value();
                        v.type = Type.i32;
                        v.i32 = inst.i32;
                        vStack[s.vStackPtr++] = v;
                        break;
                    }
                    case 0x42: // i64.const
                    {
                        Value v = new Value();
                        v.type = Type.i64;
                        v.i64 = inst.i64;
                        vStack[s.vStackPtr++] = v;
                        break;
                    }
                    case 0x43: // f32.const
                    {
                        Value v = new Value();
                        v.type = Type.f32;
                        v.f32 = inst.f32;
                        vStack[s.vStackPtr++] = v;
                        break;
                    }
                    case 0x44: // f64.const
                    {
                        Value v = new Value();
                        v.type = Type.f64;
                        v.f64 = inst.f64;
                        vStack[s.vStackPtr++] = v;
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

                if (Profile)
                {
                    timer.Stop();

                    if (!profile.ContainsKey(inst.opCode))
                    {
                        profile.Add(inst.opCode, TimeSpan.Zero);
                    }

                    profile[inst.opCode] += timer.Elapsed;
                    
                    if (counter % 10000000 == 0)
                    {
                        foreach (var keyValuePair in profile.OrderBy(x => x.Value))
                        {
                            var tss = keyValuePair.Value;
                            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                tss.Hours, tss.Minutes, tss.Seconds,
                                tss.Milliseconds / 10);
                            Console.WriteLine(keyValuePair.Key.ToString("X") + ": " + elapsedTime);
                        }
                    }
                }

                counter++;
                steps--;

                s.ip++;
            }
 
            if (cStackPtr == 0 && s.ip >= s.function.program.Length)
                return false;
            return true;
        }
    }
}