using System;

namespace GameWasm.Webassembly
{
    public class Memory
    {
        public byte[] Buffer;
        public int MinPages = 0, MaxPages = 0, CurrentPages = 0;

        private const int PAGE_CAP = 1024;
        public Memory(int minPages, int maxPages)
        {
            if (minPages > PAGE_CAP)
            {
                throw new Exception("Out of memory!");
            }
            MinPages = minPages;
            MaxPages = maxPages;
            CurrentPages = MinPages;

            Buffer = new byte[CurrentPages * 65536];
            Array.Clear(Buffer, 0, Buffer.Length);                
        }

        public bool CompatibleWith(Memory m)
        {
            return MinPages == m.MinPages && MaxPages == m.MaxPages;
        }

        public override string ToString()
        {
            return "<memory min: " + MinPages + ", max: " + MaxPages + ", cur: " + CurrentPages + ">";
        }
        
        public void Set(UInt64 offset, byte b)
        {
            if (offset >= 0 && offset < (UInt64)Buffer.Length)
            {
                Buffer[offset] = b;
            }
            else
            {
                throw new Trap("out of bounds memory access");
            }
        }

        public byte[] GetBytes(UInt64 offset, UInt64 bytes)
        {
            if (offset >= 0 && offset + bytes <= (UInt64)Buffer.Length)
            {
                byte[] buffer = new byte[bytes];
                Array.Copy(Buffer, (int)offset, buffer, 0, (int)bytes);

                return buffer;
            }
            
            throw new Trap("out of bounds memory access");
        }

        public void SetBytes(UInt64 offset, byte[] bytes)
        {
            if (offset >= 0 && offset + (UInt64)bytes.Length <= (UInt64)Buffer.Length)
            {
                Array.Copy(bytes, 0, Buffer, (int)offset, bytes.Length);
            }
            else
            {
                throw new Trap("out of bounds memory access");
            }
        }

        public float GetF32(UInt64 offset)
        {
            return BitConverter.ToSingle(GetBytes(offset, 4), 0);
        }

        public double GetF64(UInt64 offset)
        {
            return BitConverter.ToDouble(GetBytes(offset, 8), 0);
        }

        public void SetI16(UInt64 offset, UInt16 value)
        {
            if (offset >= 0 && offset + 1 < (UInt64)Buffer.Length)
            {
                Buffer[offset + 1] = (byte)((value & 0xFF00) >> 8);
                Buffer[offset + 0] = (byte)(value & 0xFF);
            }
            else
            {
                throw new Trap("out of bounds memory access");
            }
        }

        public void SetI32(UInt64 offset, UInt32 value)
        {
            if (offset >= 0 && offset + 3 < (UInt64)Buffer.Length)
            {
                Buffer[offset + 3] = (byte)((value & 0xFF000000) >> 24);
                Buffer[offset + 2] = (byte)((value & 0xFF0000) >> 16);
                Buffer[offset + 1] = (byte)((value & 0xFF00) >> 8);
                Buffer[offset + 0] = (byte)(value & 0xFF);
            }
            else
            {
                throw new Trap("out of bounds memory access");
            }
        }

        public void SetI64(UInt64 offset, UInt64 value)
        {
            if (offset >= 0 && offset + 7 < (UInt64)Buffer.Length)
            {
                Buffer[offset + 7] = (byte)((value & 0xFF00000000000000) >> 56);
                Buffer[offset + 6] = (byte)((value & 0xFF000000000000) >> 48);
                Buffer[offset + 5] = (byte)((value & 0xFF0000000000) >> 40);
                Buffer[offset + 4] = (byte)((value & 0xFF00000000) >> 32);
                Buffer[offset + 3] = (byte)((value & 0xFF000000) >> 24);
                Buffer[offset + 2] = (byte)((value & 0xFF0000) >> 16);
                Buffer[offset + 1] = (byte)((value & 0xFF00) >> 8);
                Buffer[offset + 0] = (byte)(value & 0xFF);
            }
            else
            {
                throw new Trap("out of bounds memory access");
            }
        }

        public UInt32 GetI32(UInt64 offset)
        {
            if (offset >= 0 && offset + 3 < (UInt64)Buffer.Length)
            {
                return (UInt32)Buffer[offset] | 
                       (UInt32)Buffer[offset + 1] << 8 | 
                       (UInt32)Buffer[offset + 2] << 16 | 
                       (UInt32)Buffer[offset + 3] << 24;
            }

            throw new Trap("out of bounds memory access");
        }

        public UInt64 GetI64(UInt64 offset)
        {
            if (offset >= 0 && offset + 7 < (UInt64)Buffer.Length)
            {
                return (UInt64)Buffer[offset] | 
                       (UInt64)Buffer[offset + 1] << 8 | 
                       (UInt64)Buffer[offset + 2] << 16 | 
                       (UInt64)Buffer[offset + 3] << 24 | 
                       (UInt64)Buffer[offset + 4] << 32 | 
                       (UInt64)Buffer[offset + 5] << 40 | 
                       (UInt64)Buffer[offset + 6] << 48 | 
                       (UInt64)Buffer[offset + 7] << 56;
            }

            throw new Trap("out of bounds memory access");
        }

        public UInt32 GetI3216s(UInt64 offset)
        {
            if (offset >= 0 && offset + 1 < (UInt64)Buffer.Length)
            {
                return (UInt32)(Int16)((UInt16)Buffer[offset] | 
                                       (UInt16)Buffer[offset + 1] << 8);
            }

            throw new Trap("out of bounds memory access");
        }

        public UInt32 GetI3216u(UInt64 offset)
        {
            if (offset >= 0 && offset + 1 < (UInt64)Buffer.Length)
            {
                return (UInt32)((UInt16)Buffer[offset] | 
                                (UInt16)Buffer[offset + 1] << 8);
            }

            throw new Trap("out of bounds memory access");
        }

        public UInt32 GetI328s(UInt64 offset)
        {
            if (offset >= 0 && offset < (UInt64)Buffer.Length)
            {
                return (UInt32) (sbyte)Buffer[offset];
            }

            throw new Trap("out of bounds memory access");
        }

        public UInt32 GetI328u(UInt64 offset)
        {
            if (offset >= 0 && offset < (UInt64)Buffer.Length)
            {
                return (UInt32) Buffer[offset];
            }

            throw new Trap("out of bounds memory access");
        }

        public UInt64 GetI6416s(UInt64 offset)
        {
            if (offset >= 0 && offset + 1 < (UInt64)Buffer.Length)
            {
                return (UInt64)(Int16)((UInt16)Buffer[offset] | 
                                       (UInt16)Buffer[offset + 1] << 8);
            }

            throw new Trap("out of bounds memory access");
        }

        public UInt64 GetI6416u(UInt64 offset)
        {
            if (offset >= 0 && offset + 1 < (UInt64)Buffer.Length)
            {
                return (UInt64)((UInt16)Buffer[offset] | 
                                (UInt16)Buffer[offset + 1] << 8);
            }

            throw new Trap("out of bounds memory access");
        }

        public UInt64 GetI6432s(UInt64 offset)
        {
            if (offset >= 0 && offset + 3 < (UInt64)Buffer.Length)
            {
                return (UInt64)(Int32)((UInt32)Buffer[offset] | 
                                       (UInt32)Buffer[offset + 1] << 8 | 
                                       (UInt32)Buffer[offset + 2] << 16 | 
                                       (UInt32)Buffer[offset + 3] << 24);
            }

            throw new Trap("out of bounds memory access");
        }

        public UInt64 GetI6432u(UInt64 offset)
        {
            if (offset >= 0 && offset + 3 < (UInt64)Buffer.Length)
            {
                return (UInt64)((UInt32)Buffer[offset] | 
                                (UInt32)Buffer[offset + 1] << 8 | 
                                (UInt32)Buffer[offset + 2] << 16 | 
                                (UInt32)Buffer[offset + 3] << 24);
            }

            throw new Trap("out of bounds memory access");
        }

        public UInt64 GetI648s(UInt64 offset)
        {
            if (offset >= 0 && offset < (UInt64)Buffer.Length)
            {
                return (UInt64) (sbyte)Buffer[offset];
            }

            throw new Trap("out of bounds memory access");
        }

        public UInt64 GetI648u(UInt64 offset)
        {
            if (offset >= 0 && offset < (UInt64)Buffer.Length)
            {
                return (UInt64) Buffer[offset];
            }

            throw new Trap("out of bounds memory access");
        }

        public UInt32 Grow(UInt32 size)
        {

            if ((MaxPages != 0 && (CurrentPages + size) > MaxPages) || ((CurrentPages + size) > PAGE_CAP))
            {
                return 0xFFFFFFFF;

            }
            else if ((CurrentPages + size) < CurrentPages)
            {
                return 0xFFFFFFFF;
            }
            else if ((CurrentPages + size) == CurrentPages)
            {
                return (UInt32)CurrentPages;
            }
            else
            {
                Array.Resize(ref Buffer, (int)(size + CurrentPages) * 65536);
                Array.Clear(Buffer, CurrentPages * 65536, 65536 * (int)size);

                CurrentPages += (int)size;
                return (UInt32)(CurrentPages - size);
            }
        }
    }
}
