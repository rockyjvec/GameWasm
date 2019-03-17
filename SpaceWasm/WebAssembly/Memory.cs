using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly
{
    public class Memory
    {
        public byte[] Buffer;
        public UInt32 MinPages = 0, MaxPages = 0, CurrentPages = 0;


        public Memory(UInt32 minPages, UInt32 maxPages)
        {
            this.MinPages = minPages;
            this.MaxPages = maxPages;
            this.CurrentPages = this.MinPages;

            this.Buffer = new byte[65535 * this.CurrentPages];
        }

        public bool CompatibleWith(Memory m)
        {
            return this.MinPages == m.MinPages && this.MaxPages == m.MaxPages;
        }

        public override string ToString()
        {
            return "<memory min: " + this.MinPages + ", max: " + this.MaxPages + ", cur: " + this.CurrentPages + ">";
        }

        public void Set(UInt32 offset, byte b)
        {
            if (offset >= this.Buffer.Length)
            {
                throw new Exception("Invalid memory offset");
            }

            this.Buffer[offset] = b;
        }
    }
}
