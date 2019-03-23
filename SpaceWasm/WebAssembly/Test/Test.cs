using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace WebAssembly.Test
{
    abstract public class Test
    {
        protected string path = "";
        public Test(string path)
        {
            this.path = path;
            this.test();
        }

        protected void assert(object a, object b)
        {
            if((UInt32)a != (UInt32)b)
            {
                throw new Exception("Fail!");
            } 
        }

        private string GetByteString(float a)
        {
            string ret = "0x";
            foreach(var i in BitConverter.GetBytes(a))
            {
                ret += i.ToString("X");
            }
            return ret;
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
            new Func1(path);
            new Func2(path);
            new LoadI32(path);
            new LoadI64(path);
            new LoadF32(path);
            new LoadF64(path);
            new Align2(path);
            new Align1(path);
            new Memory(path);
            new BrIf(path);
            new Stack(path);
            new Return(path);
            new Block(path);
            new Br(path);
            new BrTable(path);
            new BreakDrop(path);
            new Call(path);
            new CallIndirect(path);
            new Conversions(path);
            new Endianness(path);
            new Labels(path);
            new Switch(path);
            new If(path);
            new I32(path);
            new Globals(path);

            return true;
        }
    }
}
