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

        protected void assert(object a, object b)
        {
            if((UInt32)a != (UInt32)b)
            {
                throw new Exception("Fail!");
            } 
        }
        
        protected void assertF32(object a, object b)
        {
            if ((float)a != (float)b)
            {
                if(float.IsNaN((float)a) && float.IsNaN((float)b))
                {
                    return;
                }
                throw new Exception("Fail!");
            }
        }

        protected void assertF64(object a, object b)
        {
            if ((double)a != (double)b)
            {
                if (double.IsNaN((double)a) && double.IsNaN((double)b))
                {
                    return;
                }
                throw new Exception("Fail!");
            }
        }

        protected void assert64(object a, object b)
        {
            if ((UInt64)a != (UInt64)b)
            {
                throw new Exception("Fail!");
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
            Console.WriteLine("I64");
            new I64(path);
            Console.WriteLine("Func1");
            new Func1(path);
            Console.WriteLine("Func2");
            new Func2(path);
            Console.WriteLine("LoadI32");
            new LoadI32(path);
            Console.WriteLine("LoadI64");
            new LoadI64(path);
            Console.WriteLine("LoadF32");
            new LoadF32(path);
            Console.WriteLine("LoadF64");
            new LoadF64(path);
            Console.WriteLine("Align2");
            new Align2(path);
            Console.WriteLine("Align1");
            new Align1(path);
            Console.WriteLine("Memory");
            new Memory(path);
            Console.WriteLine("BrIf");
            new BrIf(path);
            Console.WriteLine("Stack");
            new Stack(path);
            Console.WriteLine("Return");
            new Return(path);
            Console.WriteLine("Block");
            new Block(path);
            Console.WriteLine("Br");
            new Br(path);
            Console.WriteLine("BrTable");
            new BrTable(path);
            Console.WriteLine("BreakDrop");
            new BreakDrop(path);
            Console.WriteLine("Call");
            new Call(path);
            Console.WriteLine("CallIndirect");
            new CallIndirect(path);
            Console.WriteLine("Conversions");
            new Conversions(path);
            Console.WriteLine("Endianness");
            new Endianness(path);
            Console.WriteLine("Labels");
            new Labels(path);
            Console.WriteLine("Switch");
            new Switch(path);
            Console.WriteLine("If");
            new If(path);
            Console.WriteLine("I32");
            new I32(path);
            Console.WriteLine("Globals");
            new Globals(path);
            Console.WriteLine("StoreTest");
            new StoreTest(path);
            Console.WriteLine("Load");
            new Load(path);
            Console.WriteLine("Address");
            new Address(path);
            Console.WriteLine("Unwind");
            new Unwind(path);
            Console.WriteLine("Select");
            new Select(path);
            Console.WriteLine("MemoryRedundancy");
            new MemoryRedundancy(path);
            Console.WriteLine("LocalTee");
            new LocalTee(path);
            Console.WriteLine("Forward");
            new Forward(path);
            Console.WriteLine("Fac");
            new Fac(path);
            Console.WriteLine("LeftToRight");
            new LeftToRight(path);
            Console.WriteLine("LocalGet");
            new LocalGet(path);
            Console.WriteLine("Loop");
            new Loop(path);
            Console.WriteLine("LocalSet");
            new LocalSet(path);
            Console.WriteLine("MemoryGrow");
            new MemoryGrow(path);
            Console.WriteLine("Nop");
            new Nop(path);
            Console.WriteLine("FuncPtrs");
            new FuncPtrs(path);
            Console.WriteLine("Start");
            new Start(path);
            Console.WriteLine("Elem");
            new Elem(path);
            
            return true;
        }
    }
}
