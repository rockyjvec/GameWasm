using System;
using System.Collections.Generic;
using System.IO;

namespace GameWasm.Webassembly
{
    public class Store
    {
        public Dictionary<string, Module.Module> Modules = new Dictionary<string, Module.Module>();
        public Stack.Stack Stack; 
        public Stack.Frame CurrentFrame = null;
        
        public Store()
        {
            Stack = new Stack.Stack(this);
            init();
        }

        public Module.Module LoadModule(string name, string fileName)
        {         
            return LoadModule(name, File.ReadAllBytes(fileName));
        }

        public Module.Module LoadModule(string name, byte[] bytes)
        {
            return LoadModule(new Module.Module(name, this, bytes));
        }

        public Module.Module LoadModule(Module.Module module)
        {
            Modules.Add(module.Name, module);
            return module;
        }

        private void init()
        {
            LoadModule(new Module.Global.Global(this));
            LoadModule(new Module.Global.Math(this));
            LoadModule(new Module.Env(this));
            LoadModule(new Module.Asm2Wasm(this));
            LoadModule(new Module.SpecTest(this));
            LoadModule(new Module.Wasi(this));
        }

        public bool Step(int count = 1, bool debug = false)
        {
            bool exception = true;

            try
            {
                for (int step = 0; step < count; step++)
                {

                    var frame = CurrentFrame;

                    if (frame == null)
                    {
                        exception = false;
                        return false;
                    }
                    else
                    {
                        if (frame.Instruction == null)
                        {
                            if(frame.Function == null)
                            {
                                exception = false;
                                return false;
                            }
                            else
                            {
                                frame.Function.HandleReturn(this);

                                if (!Stack.PopFrame())
                                {
                                    exception = false;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            if (debug)
                            {
                                int num = 0;
                                foreach (var v in frame.Locals)
                                {
                                    Console.WriteLine("$var" + num + ": " + Type.Pretify(v));
                                    num++;
                                }
                                num = 0;
                                foreach (var v in frame.Module.Globals)
                                {
                                    Console.WriteLine("$global" + num + ": " + Type.Pretify(v.GetValue()));
                                    num++;
                                }

                                int numLabels = 0;

/*                                foreach (var i in Stack.ToArray()
                                {
                                    if (i as Stack.Label != null)
                                    {
                                        numLabels++;
                                    }

                                    if (i as Stack.Frame != null)
                                    {
                                        break;
                                    }
                                }
                                */
                                Console.Write(frame.Instruction.Pointer.ToString("X").PadLeft(8, '0') + ": " + frame.Module.Name + "@" + ((frame.Function != null)?frame.Function.GetName():"null") + " => " + new string(' ', numLabels * 2) + frame.Instruction.ToString().Replace("WebAssembly.Instruction.", ""));
                            }

                            frame.Instruction = frame.Instruction.Run(this);

                            if (debug)
                            {
                                if (Stack.Size == 0)
                                {
                                }
                                else
                                {
                                    Console.Write(" $ret: " + Type.Pretify(Stack.Peek()));
                                }
                                Console.Write("\n");
                                Console.ReadKey();
                            }
                        }
                    }
                }

                exception = false;
            }
            finally
            {
                if (exception)
                {
                    CurrentFrame = null;
                    Stack = new Stack.Stack(this);
                }
            }

            return true;
        }
    }
}
