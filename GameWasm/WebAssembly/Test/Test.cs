using System;

namespace GameWasm.Webassembly.Test
{
    abstract public class Test
    {
        protected string path = "";
        public Test(string path)
        {
            this.path = path;
            test();
        }

        protected void assert(Value a, UInt32 b)
        {
            if(a.type == Type.i32 && a.i32 != b)
            {
                throw new Exception("Fail! " + a.i32 + " != " + b);
            } 
        }
        
        protected void assertF32(Value a, float b)
        {
            if (a.type == Type.f32 && a.f32 != b)
            {
                if(float.IsNaN(a.f32) && float.IsNaN(b))
                {
                    return;
                }
                throw new Exception("Fail!");
            }
        }

        protected void assertF64(Value a, double b)
        {
            if (a.type == Type.f64 && a.f64 != b)
            {
                if (double.IsNaN(a.f64) && double.IsNaN(b))
                {
                    return;
                }
                throw new Exception("Fail! " + a.f64 + " != " + b);
            }
        }

        protected void assert64(Value a, UInt64 b)
        {
            if (a.type == Type.i64 && a.i64 != b)
            {
                throw new Exception("Fail! " + a.i64 + " != " + b);
            }
        }

        protected void assert_trap(Action a, string message)
        {
            try
            {
                a();
                throw new Exception("Fail.");
            }
            catch (Trap e)
            {
                if (e.Message != message)
                {
                    throw new Exception("Fail.");
                }
            }
        }

        abstract public void test();
        /*

        */

        public static bool Run(string path)
        {
            Console.WriteLine("StoreTest");
            new StoreTest(path);
            Console.WriteLine("I32");
            new I32(path);
            Console.WriteLine("I64");
            new I64(path);
            Console.WriteLine("LoadI32");
            new LoadI32(path);
            Console.WriteLine("LoadI64");
            new LoadI64(path);
            Console.WriteLine("LoadF32");
            new LoadF32(path);
            Console.WriteLine("LoadF64");
            new LoadF64(path);
            Console.WriteLine("Load");
            new Load(path);
            Console.WriteLine("Address");
            new Address(path);
            Console.WriteLine("Align2");
            new Align2(path);
            Console.WriteLine("Memory");
            new Memory(path);
            Console.WriteLine("Stack");
            new Stack(path); 
            Console.WriteLine("BreakDrop");
            new BreakDrop(path);
            Console.WriteLine("Select");
            new Select(path);
            Console.WriteLine("FuncPtrs");
            new FuncPtrs(path);
            Console.WriteLine("Start");
            new Start(path);
            Console.WriteLine("Func2");
            new Func2(path);
            Console.WriteLine("Globals");
            new Globals(path);
            Console.WriteLine("Block");
            new Block(path);
            Console.WriteLine("If");
            new If(path);
            Console.WriteLine("Loop");
            new Loop(path);
            Console.WriteLine("Labels");
            new Labels(path);
            Console.WriteLine("Func1");
            new Func1(path);
            Console.WriteLine("Switch");
            new Switch(path);
            Console.WriteLine("Align1");
            new Align1(path);
            Console.WriteLine("Br");
            new Br(path);
            Console.WriteLine("MemoryRedundancy");
            new MemoryRedundancy(path);
            Console.WriteLine("LeftToRight");
            new LeftToRight(path);
            Console.WriteLine("Return");
            new Return(path);
            Console.WriteLine("Unwind");
            new Unwind(path);
            Console.WriteLine("BrIf");
            new BrIf(path);
            Console.WriteLine("CallIndirect"); //TODO: implement traps
            new CallIndirect(path);
            Console.WriteLine("BrTable");
            new BrTable(path);
            Console.WriteLine("LocalTee");
            new LocalTee(path);
            Console.WriteLine("Forward");
            new Forward(path);
            Console.WriteLine("Fac");
            new Fac(path);
            Console.WriteLine("LocalGet");
            new LocalGet(path);
            Console.WriteLine("LocalSet");
            new LocalSet(path);
            Console.WriteLine("MemoryGrow");
            new MemoryGrow(path);
            Console.WriteLine("Elem");
            new Elem(path);
            Console.WriteLine("Call");
            new Call(path);
            Console.WriteLine("Nop");
            new Nop(path);
            Console.WriteLine("Endianness");
            new Endianness(path);
            Console.WriteLine("Conversions");
            new Conversions(path);

            return true;
        }
    }
}
