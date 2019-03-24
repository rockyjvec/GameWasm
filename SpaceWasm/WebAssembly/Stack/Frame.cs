using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Stack
{
    public class Frame
    {
        public Store Store;
        public Module.Module Module;
        public Function Function;

        public static Dictionary<string, string> UsedInstructions = new Dictionary<string, string>();

        public Instruction.Instruction Instruction;
        public List<object> Locals = new List<object>();

        public Stack<object> Results = new Stack<object>();

        public Frame(Store store, Module.Module module, Function function, Instruction.Instruction instruction)
        {
            this.Store = store;
            this.Module = module;
            this.Function = function;
            this.Instruction = instruction;
        }

        static uint inc = 0;

        public bool Step(bool debug = false)
        {
            if (this.Instruction != null)
            {
                if (debug)
                {
                    int num = 0;
                    foreach (var v in Locals)
                    {
                        Console.WriteLine("$var" + num + ": " + Type.Pretify(v));
                        num++;
                    }
                    num = 0;
                    foreach (var v in Module.Globals)
                    {
                        Console.WriteLine("$global" + num + ": " + Type.Pretify(v.GetValue()));
                        num++;
                    }

                    int numLabels = 0;

                    foreach(var i in this.Store.Stack.ToArray().Reverse())
                    {
                        if(i as Label != null)
                        {
                            numLabels++;
                        }

                        if (i as Frame != null)
                        {
                            break;
                        }
                    }

                    Console.Write(this.Instruction.Pointer.ToString("X").PadLeft(8, '0') + ": " + this.Module.Name + "@" + this.Store.CurrentFrame.Function.GetName() + " => " + new string(' ', numLabels * 2) + this.Instruction.ToString().Replace("WebAssembly.Instruction.", ""));
                }

                if (!UsedInstructions.ContainsKey(this.Instruction.ToString())) UsedInstructions[this.Instruction.ToString()] = this.Instruction.ToString();
                this.Instruction = this.Instruction.Run(this.Store);
                if (debug)
                {
                    if (this.Store.Stack.Size == 0)
                    {
                    }
                    else
                    {
                        Console.Write(" $ret: " + Type.Pretify(this.Store.Stack.Peek()));
                    }
                    Console.Write("\n");
                                    Console.ReadKey();
                }
            }

            return !(this.Store.CurrentFrame.Instruction == null);
        }
    }
}
