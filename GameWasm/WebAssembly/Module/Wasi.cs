using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GameWasm.Webassembly.Module
{

    class Wasi : Module
    {
        const UInt32 WASI_ESUCCESS = 0;
        const UInt32 WASI_E2BIG = 1;
        const UInt32 WASI_EACCES = 2;
        const UInt32 WASI_EADDRINUSE = 3;
        const UInt32 WASI_EADDRNOTAVAIL = 4;
        const UInt32 WASI_EAFNOSUPPORT = 5;
        const UInt32 WASI_EAGAIN = 6;
        const UInt32 WASI_EALREADY = 7;
        const UInt32 WASI_EBADF = 8;
        const UInt32 WASI_EBADMSG = 9;
        const UInt32 WASI_EBUSY = 10;
        const UInt32 WASI_ECANCELED = 11;
        const UInt32 WASI_ECHILD = 12;
        const UInt32 WASI_ECONNABORTED = 13;
        const UInt32 WASI_ECONNREFUSED = 14;
        const UInt32 WASI_ECONNRESET = 15;
        const UInt32 WASI_EDEADLK = 16;
        const UInt32 WASI_EDESTADDRREQ = 17;
        const UInt32 WASI_EDOM = 18;
        const UInt32 WASI_EDQUOT = 19;
        const UInt32 WASI_EEXIST = 20;
        const UInt32 WASI_EFAULT = 21;
        const UInt32 WASI_EFBIG = 22;
        const UInt32 WASI_EHOSTUNREACH = 23;
        const UInt32 WASI_EIDRM = 24;
        const UInt32 WASI_EILSEQ = 25;
        const UInt32 WASI_EINPROGRESS = 26;
        const UInt32 WASI_EINTR = 27;
        const UInt32 WASI_EINVAL = 28;
        const UInt32 WASI_EIO = 29;
        const UInt32 WASI_EISCONN = 30;
        const UInt32 WASI_EISDIR = 31;
        const UInt32 WASI_ELOOP = 32;
        const UInt32 WASI_EMFILE = 33;
        const UInt32 WASI_EMLINK = 34;
        const UInt32 WASI_EMSGSIZE = 35;
        const UInt32 WASI_EMULTIHOP = 36;
        const UInt32 WASI_ENAMETOOLONG = 37;
        const UInt32 WASI_ENETDOWN = 38;
        const UInt32 WASI_ENETRESET = 39;
        const UInt32 WASI_ENETUNREACH = 40;
        const UInt32 WASI_ENFILE = 41;
        const UInt32 WASI_ENOBUFS = 42;
        const UInt32 WASI_ENODEV = 43;
        const UInt32 WASI_ENOENT = 44;
        const UInt32 WASI_ENOEXEC = 45;
        const UInt32 WASI_ENOLCK = 46;
        const UInt32 WASI_ENOLINK = 47;
        const UInt32 WASI_ENOMEM = 48;
        const UInt32 WASI_ENOMSG = 49;
        const UInt32 WASI_ENOPROTOOPT = 50;
        const UInt32 WASI_ENOSPC = 51;
        const UInt32 WASI_ENOSYS = 52;
        const UInt32 WASI_ENOTCONN = 53;
        const UInt32 WASI_ENOTDIR = 54;
        const UInt32 WASI_ENOTEMPTY = 55;
        const UInt32 WASI_ENOTRECOVERABLE = 56;
        const UInt32 WASI_ENOTSOCK = 57;
        const UInt32 WASI_ENOTSUP = 58;
        const UInt32 WASI_ENOTTY = 59;
        const UInt32 WASI_ENXIO = 60;
        const UInt32 WASI_EOVERFLOW = 61;
        const UInt32 WASI_EOWNERDEAD = 62;
        const UInt32 WASI_EPERM = 63;
        const UInt32 WASI_EPIPE = 64;
        const UInt32 WASI_EPROTO = 65;
        const UInt32 WASI_EPROTONOSUPPORT = 66;
        const UInt32 WASI_EPROTOTYPE = 67;
        const UInt32 WASI_ERANGE = 68;
        const UInt32 WASI_EROFS = 69;
        const UInt32 WASI_ESPIPE = 70;
        const UInt32 WASI_ESRCH = 71;
        const UInt32 WASI_ESTALE = 72;
        const UInt32 WASI_ETIMEDOUT = 73;
        const UInt32 WASI_ETXTBSY = 74;
        const UInt32 WASI_EXDEV = 75;
        const UInt32 WASI_ENOTCAPABLE = 76;

        const UInt32 WASI_SIGABRT = 0;
        const UInt32 WASI_SIGALRM = 1;
        const UInt32 WASI_SIGBUS = 2;
        const UInt32 WASI_SIGCHLD = 3;
        const UInt32 WASI_SIGCONT = 4;
        const UInt32 WASI_SIGFPE = 5;
        const UInt32 WASI_SIGHUP = 6;
        const UInt32 WASI_SIGILL = 7;
        const UInt32 WASI_SIGINT = 8;
        const UInt32 WASI_SIGKILL = 9;
        const UInt32 WASI_SIGPIPE = 10;
        const UInt32 WASI_SIGQUIT = 11;
        const UInt32 WASI_SIGSEGV = 12;
        const UInt32 WASI_SIGSTOP = 13;
        const UInt32 WASI_SIGTERM = 14;
        const UInt32 WASI_SIGTRAP = 15;
        const UInt32 WASI_SIGTSTP = 16;
        const UInt32 WASI_SIGTTIN = 17;
        const UInt32 WASI_SIGTTOU = 18;
        const UInt32 WASI_SIGURG = 19;
        const UInt32 WASI_SIGUSR1 = 20;
        const UInt32 WASI_SIGUSR2 = 21;
        const UInt32 WASI_SIGVTALRM = 22;
        const UInt32 WASI_SIGXCPU = 23;
        const UInt32 WASI_SIGXFSZ = 24;

        const byte WASI_FILETYPE_UNKNOWN = 0;
        const byte WASI_FILETYPE_BLOCK_DEVICE = 1;
        const byte WASI_FILETYPE_CHARACTER_DEVICE = 2;
        const byte WASI_FILETYPE_DIRECTORY = 3;
        const byte WASI_FILETYPE_REGULAR_FILE = 4;
        const byte WASI_FILETYPE_SOCKET_DGRAM = 5;
        const byte WASI_FILETYPE_SOCKET_STREAM = 6;
        const byte WASI_FILETYPE_SYMBOLIC_LINK = 7;

        const UInt16 WASI_FDFLAG_APPEND = 0x0001;
        const UInt16 WASI_FDFLAG_DSYNC = 0x0002;
        const UInt16 WASI_FDFLAG_NONBLOCK = 0x0004;
        const UInt16 WASI_FDFLAG_RSYNC = 0x0008;
        const UInt16 WASI_FDFLAG_SYNC = 0x0010;

        const UInt64 WASI_RIGHT_FD_DATASYNC = 0x0000000000000001;
        const UInt64 WASI_RIGHT_FD_READ = 0x0000000000000002;
        const UInt64 WASI_RIGHT_FD_SEEK = 0x0000000000000004;
        const UInt64 WASI_RIGHT_FD_FDSTAT_SET_FLAGS = 0x0000000000000008;
        const UInt64 WASI_RIGHT_FD_SYNC = 0x0000000000000010;
        const UInt64 WASI_RIGHT_FD_TELL = 0x0000000000000020;
        const UInt64 WASI_RIGHT_FD_WRITE = 0x0000000000000040;
        const UInt64 WASI_RIGHT_FD_ADVISE = 0x0000000000000080;
        const UInt64 WASI_RIGHT_FD_ALLOCATE = 0x0000000000000100;
        const UInt64 WASI_RIGHT_PATH_CREATE_DIRECTORY = 0x0000000000000200;
        const UInt64 WASI_RIGHT_PATH_CREATE_FILE = 0x0000000000000400;
        const UInt64 WASI_RIGHT_PATH_LINK_SOURCE = 0x0000000000000800;
        const UInt64 WASI_RIGHT_PATH_LINK_TARGET = 0x0000000000001000;
        const UInt64 WASI_RIGHT_PATH_OPEN = 0x0000000000002000;
        const UInt64 WASI_RIGHT_FD_READDIR = 0x0000000000004000;
        const UInt64 WASI_RIGHT_PATH_READLINK = 0x0000000000008000;
        const UInt64 WASI_RIGHT_PATH_RENAME_SOURCE = 0x0000000000010000;
        const UInt64 WASI_RIGHT_PATH_RENAME_TARGET = 0x0000000000020000;
        const UInt64 WASI_RIGHT_PATH_FILESTAT_GET = 0x0000000000040000;
        const UInt64 WASI_RIGHT_PATH_FILESTAT_SET_SIZE = 0x0000000000080000;
        const UInt64 WASI_RIGHT_PATH_FILESTAT_SET_TIMES = 0x0000000000100000;
        const UInt64 WASI_RIGHT_FD_FILESTAT_GET = 0x0000000000200000;
        const UInt64 WASI_RIGHT_FD_FILESTAT_SET_SIZE = 0x0000000000400000;
        const UInt64 WASI_RIGHT_FD_FILESTAT_SET_TIMES = 0x0000000000800000;
        const UInt64 WASI_RIGHT_PATH_SYMLINK = 0x0000000001000000;
        const UInt64 WASI_RIGHT_PATH_REMOVE_DIRECTORY = 0x0000000002000000;
        const UInt64 WASI_RIGHT_PATH_UNLINK_FILE = 0x0000000004000000;
        const UInt64 WASI_RIGHT_POLL_FD_READWRITE = 0x0000000008000000;
        const UInt64 WASI_RIGHT_SOCK_SHUTDOWN = 0x0000000010000000;

        const UInt64 RIGHTS_ALL = WASI_RIGHT_FD_DATASYNC | WASI_RIGHT_FD_READ
            | WASI_RIGHT_FD_SEEK | WASI_RIGHT_FD_FDSTAT_SET_FLAGS | WASI_RIGHT_FD_SYNC
            | WASI_RIGHT_FD_TELL | WASI_RIGHT_FD_WRITE | WASI_RIGHT_FD_ADVISE
            | WASI_RIGHT_FD_ALLOCATE | WASI_RIGHT_PATH_CREATE_DIRECTORY
            | WASI_RIGHT_PATH_CREATE_FILE | WASI_RIGHT_PATH_LINK_SOURCE
            | WASI_RIGHT_PATH_LINK_TARGET | WASI_RIGHT_PATH_OPEN | WASI_RIGHT_FD_READDIR
            | WASI_RIGHT_PATH_READLINK | WASI_RIGHT_PATH_RENAME_SOURCE
            | WASI_RIGHT_PATH_RENAME_TARGET | WASI_RIGHT_PATH_FILESTAT_GET
            | WASI_RIGHT_PATH_FILESTAT_SET_SIZE | WASI_RIGHT_PATH_FILESTAT_SET_TIMES
            | WASI_RIGHT_FD_FILESTAT_GET | WASI_RIGHT_FD_FILESTAT_SET_TIMES
            | WASI_RIGHT_FD_FILESTAT_SET_SIZE | WASI_RIGHT_PATH_SYMLINK
            | WASI_RIGHT_PATH_UNLINK_FILE | WASI_RIGHT_PATH_REMOVE_DIRECTORY
            | WASI_RIGHT_POLL_FD_READWRITE | WASI_RIGHT_SOCK_SHUTDOWN;

        const UInt64 RIGHTS_BLOCK_DEVICE_BASE = RIGHTS_ALL;
        const UInt64 RIGHTS_BLOCK_DEVICE_INHERITING = RIGHTS_ALL;

        const UInt64 RIGHTS_CHARACTER_DEVICE_BASE = RIGHTS_ALL;
        const UInt64 RIGHTS_CHARACTER_DEVICE_INHERITING = RIGHTS_ALL;

        const UInt64 RIGHTS_REGULAR_FILE_BASE = WASI_RIGHT_FD_DATASYNC | WASI_RIGHT_FD_READ
            | WASI_RIGHT_FD_SEEK | WASI_RIGHT_FD_FDSTAT_SET_FLAGS | WASI_RIGHT_FD_SYNC
            | WASI_RIGHT_FD_TELL | WASI_RIGHT_FD_WRITE | WASI_RIGHT_FD_ADVISE
            | WASI_RIGHT_FD_ALLOCATE | WASI_RIGHT_FD_FILESTAT_GET
            | WASI_RIGHT_FD_FILESTAT_SET_SIZE | WASI_RIGHT_FD_FILESTAT_SET_TIMES
            | WASI_RIGHT_POLL_FD_READWRITE;
        const UInt64 RIGHTS_REGULAR_FILE_INHERITING = 0;

        const UInt64 RIGHTS_DIRECTORY_BASE = WASI_RIGHT_FD_FDSTAT_SET_FLAGS
            | WASI_RIGHT_FD_SYNC | WASI_RIGHT_FD_ADVISE | WASI_RIGHT_PATH_CREATE_DIRECTORY
            | WASI_RIGHT_PATH_CREATE_FILE | WASI_RIGHT_PATH_LINK_SOURCE
            | WASI_RIGHT_PATH_LINK_TARGET | WASI_RIGHT_PATH_OPEN | WASI_RIGHT_FD_READDIR
            | WASI_RIGHT_PATH_READLINK | WASI_RIGHT_PATH_RENAME_SOURCE
            | WASI_RIGHT_PATH_RENAME_TARGET | WASI_RIGHT_PATH_FILESTAT_GET
            | WASI_RIGHT_PATH_FILESTAT_SET_SIZE | WASI_RIGHT_PATH_FILESTAT_SET_TIMES
            | WASI_RIGHT_FD_FILESTAT_GET | WASI_RIGHT_FD_FILESTAT_SET_TIMES
            | WASI_RIGHT_PATH_SYMLINK | WASI_RIGHT_PATH_UNLINK_FILE
            | WASI_RIGHT_PATH_REMOVE_DIRECTORY | WASI_RIGHT_POLL_FD_READWRITE;
        const UInt64 RIGHTS_DIRECTORY_INHERITING = RIGHTS_DIRECTORY_BASE
            | RIGHTS_REGULAR_FILE_BASE;

        const UInt64 RIGHTS_SOCKET_BASE = WASI_RIGHT_FD_READ | WASI_RIGHT_FD_FDSTAT_SET_FLAGS
            | WASI_RIGHT_FD_WRITE | WASI_RIGHT_FD_FILESTAT_GET
            | WASI_RIGHT_POLL_FD_READWRITE | WASI_RIGHT_SOCK_SHUTDOWN;
        const UInt64 RIGHTS_SOCKET_INHERITING = RIGHTS_ALL;

        const UInt64 RIGHTS_TTY_BASE = WASI_RIGHT_FD_READ | WASI_RIGHT_FD_FDSTAT_SET_FLAGS
            | WASI_RIGHT_FD_WRITE | WASI_RIGHT_FD_FILESTAT_GET
            | WASI_RIGHT_POLL_FD_READWRITE;
        const UInt64 RIGHTS_TTY_INHERITING = 0;
        
        const byte WASI_PREOPENTYPE_DIR = 0;
            
        const UInt16 WASI_O_CREAT = 1 << 0;
        const UInt16 WASI_O_DIRECTORY = 1 << 1;
        const UInt16 WASI_O_EXCL = 1 << 2;
        const UInt16 WASI_O_TRUNC = 1 << 3;
        
        const byte WASI_WHENCE_CUR = 0;
        const byte WASI_WHENCE_END = 1;
        const byte WASI_WHENCE_SET = 2;
            
        private string Directory = "/home/rocky";
        private string[] EnvVars = new string[] { "HOME=/home/rocky", "DOOMWADDIR=/home/rocky" };
        private string[] Args = new string[] { };
        private Dictionary<UInt32, Stream> FileDescriptors = new Dictionary<UInt32, Stream>();
        
        private bool Debug = false;
            
        public Wasi(Store store) : base("wasi_unstable", store)
        {
            AddExportFunc("args_get", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, ArgsGet );
            AddExportFunc("args_sizes_get", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, ArgsSizesGet );

            AddExportFunc("clock_time_get", new byte[] { Type.i32, Type.i64, Type.i32 }, new byte[] { Type.i32 }, ClockTimeGet );

            AddExportFunc("environ_get", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, EnvironGet );
            AddExportFunc("environ_sizes_get", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, EnvironSizesGet );

            AddExportFunc("fd_close", new byte[] { Type.i32 }, new byte[] { Type.i32 }, FdClose );
            AddExportFunc("fd_fdstat_get", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, FdFdStatGet );
            AddExportFunc("fd_fdstat_set_flags", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 } );
            AddExportFunc("fd_filestat_get", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, FdFilestatGet );
            AddExportFunc("fd_prestat_dir_name", new byte[] { Type.i32, Type.i32, Type.i32 }, new byte[] { Type.i32 }, FdPrestatDirName );
            AddExportFunc("fd_prestat_get", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, FdPrestatGet );
            AddExportFunc("fd_read", new byte[] { Type.i32, Type.i32, Type.i32, Type.i32 }, new byte[] { Type.i32 }, FdRead );
            AddExportFunc("fd_seek", new byte[] { Type.i32, Type.i64, Type.i32, Type.i32 }, new byte[] { Type.i32 }, FdSeek );
            AddExportFunc("fd_tell", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 } );
            AddExportFunc("fd_write", new byte[] { Type.i32, Type.i32, Type.i32, Type.i32 }, new byte[] { Type.i32 }, FdWrite );

            AddExportFunc("path_open", new byte[] { Type.i32, Type.i32, Type.i32, Type.i32, Type.i32, Type.i64, Type.i64, Type.i32, Type.i32 }, new byte[] { Type.i32 }, PathOpen );
            AddExportFunc("path_create_directory", new byte[] { Type.i32, Type.i32, Type.i32 }, new byte[] { Type.i32 } );
            AddExportFunc("path_filestat_get", new byte[] { Type.i32, Type.i32, Type.i32, Type.i32, Type.i32 }, new byte[] { Type.i32 }, PathFilestatGet );
            
            AddExportFunc("poll_oneoff", new byte[] { Type.i32, Type.i32, Type.i32, Type.i32 }, new byte[] { Type.i32 } );
            
            AddExportFunc("proc_exit", new byte[] { Type.i32 }, null, ProcExit );

            FileDescriptors[0] = Console.OpenStandardInput();
            FileDescriptors[1] = Console.OpenStandardOutput();
            FileDescriptors[2] = Console.OpenStandardError();
            Debug = false;
        }
        
        public Value[] ArgsGet(Value[] parameters)
        {
            if (Debug)
            {
                Console.WriteLine("args_get()");
            }

            UInt32 offset = parameters[1].i32;
            for (int i = 0; i < Args.Length; i++)
            {
                Memory[0].SetI32((UInt32)(parameters[0].i32 + (4*i)), offset);
                Memory[0].SetBytes(offset, Encoding.UTF8.GetBytes(Args[i] + "\0"));
                offset += (UInt32)EnvVars[i].Length + 1;
            }
            
            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }

        public Value[] ArgsSizesGet(Value[] parameters)
        {
            if (Debug)
            {
                Console.WriteLine("args_sizes_get()");
            }

            UInt32 length = 0;
            foreach(var s in Args)
            {
                length += (UInt32)s.Length;
            }
            Memory[0].SetI32(parameters[0].i32, (UInt32)Args.Length);
            Memory[0].SetI32(parameters[1].i32, length + (UInt32)Args.Length); // add room for null termination of each arg
            
            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }
      
        public Value[] EnvironGet(Value[] parameters)
        {
            if (Debug)
            {
                Console.WriteLine("environ_get()");
            }

            UInt32 offset = parameters[1].i32;
            for (int i = 0; i < EnvVars.Length; i++)
            {
                Memory[0].SetI32((UInt32)(parameters[0].i32 + (4*i)), offset);
                Memory[0].SetBytes(offset, Encoding.UTF8.GetBytes(EnvVars[i] + "\0"));
                offset += (UInt32)EnvVars[i].Length + 1;
            }
            
            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }

        public Value[] EnvironSizesGet(Value[] parameters)
        {
            if (Debug)
            {
                Console.WriteLine("environ_sizes_get()");
            }

            UInt32 length = 0;
            foreach(var s in EnvVars)
            {
                length += (UInt32)s.Length;
            }
            Memory[0].SetI32(parameters[0].i32, (UInt32)EnvVars.Length);
            Memory[0].SetI32(parameters[1].i32, length + (UInt32)EnvVars.Length); // add room for null termination of each variable
            
            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }

        public Value[] FdClose(Value[] parameters)
        {
            if (Debug)
            {
                Console.WriteLine("fd_close("+parameters[0].i32+")");
            }

            switch (parameters[0].i32)
            {
                case 1:
                case 2:
                case 3:
                    return new Value[] { Value.GetI32(WASI_EBADF) };
                default:
                    if (FileDescriptors.ContainsKey(parameters[0].i32))
                    {
                        var fd = FileDescriptors[parameters[0].i32];
                        fd.Close();
                        FileDescriptors.Remove(parameters[0].i32);
                    }
                    break;
            }

            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }

        public Value[] FdPrestatDirName(Value[] parameters)
        {
            if (Debug)
            {
                Console.WriteLine("fd_prestat_dir_name("+parameters[0].i32+")");
            }
            switch (parameters[0].i32)
            {
                case 0: // stdin
                case 1: // stdout
                case 2: // stderr
                    // not sure what to do for other cases
                    break;
                case 3: // --dir
                    if (Directory.Length <= parameters[2].i32)
                    {
                        Memory[0].SetBytes(parameters[1].i32, Encoding.UTF8.GetBytes(Directory)); // Directory name length
                    }
                    else
                    {
                        throw new Exception("WASI Error (fd_prestat_dir_name): Directory name does not fit in buffer.");
                    }
                    break;
                default:
                    return new Value[] { Value.GetI32(WASI_EBADF) };
            }

            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }

        public Value[] FdPrestatGet(Value[] parameters)
        {
            if (Debug)
            {
                Console.WriteLine("fd_prestat_get("+parameters[0].i32+")");
            }
            switch (parameters[0].i32)
            {
                case 0: // stdin
                case 1: // stdout
                case 2: // stderr
                    // not sure what to do for other cases
                    break;
                case 3: // --dir
                    Memory[0].SetBytes(parameters[1].i32, new byte[] {WASI_PREOPENTYPE_DIR}); // Type Directory?
                    Memory[0].SetI32(parameters[1].i32+4, (UInt32)Encoding.UTF8.GetBytes(Directory).Length); // Directory name length
                    break;
                default:
                    return new Value[] { Value.GetI32(WASI_EBADF) };
            }
            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }

        public Value[] FdFdStatGet(Value[] parameters)
        {
            if (Debug)
            {
                Console.WriteLine("fd_fdstat_get("+parameters[0].i32+")");
            }

            switch (parameters[0].i32)
            {
                case 0: // stdin
                case 1: // stdout
                case 2: // stderr
                    // not sure what to do for other cases
                    break;
                case 3: // --dir
                    Memory[0].SetBytes(parameters[1].i32, new byte[] {WASI_FILETYPE_DIRECTORY}); // File Type Directory
                    Memory[0].SetI32(parameters[1].i32+2, WASI_FDFLAG_SYNC); // Flags
                    Memory[0].SetBytes(parameters[1].i32+8, BitConverter.GetBytes(RIGHTS_DIRECTORY_BASE));
                    Memory[0].SetBytes(parameters[1].i32+16, BitConverter.GetBytes(RIGHTS_DIRECTORY_INHERITING));
                    
                    break;
                
                default:
                    if (FileDescriptors.ContainsKey(parameters[0].i32))
                    {
                        var fd = FileDescriptors[parameters[0].i32];
                        Memory[0].SetBytes(parameters[1].i32, new byte[] {WASI_FILETYPE_REGULAR_FILE}); // File Type Directory
                        Memory[0].SetI32(parameters[1].i32+2, WASI_FDFLAG_SYNC); // Flags
                        Memory[0].SetBytes(parameters[1].i32+8, BitConverter.GetBytes(RIGHTS_REGULAR_FILE_BASE));
                        Memory[0].SetBytes(parameters[1].i32+16, BitConverter.GetBytes(RIGHTS_REGULAR_FILE_INHERITING));
                    }
                    break;
            }

            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }

        public Value[] FdFilestatGet(Value[] parameters)
        {
            if (Debug)
            {
                Console.WriteLine("fd_filestat_get("+parameters[0].i32+") !!NOT IMPLEMENTED!!");
            }

            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }

        public Value[] FdRead(Value[] parameters)
        {
            if (!FileDescriptors.ContainsKey(parameters[0].i32))
            {
                return new Value[] { Value.GetI32(WASI_EBADF) };
            }

            var fd = FileDescriptors[parameters[0].i32];
            
            UInt32 read = 0;
            for (int i = 0; i < parameters[2].i32; i++)
            {
                UInt32 bytes = Memory[0].GetI32((UInt32) (parameters[1].i32 + i * 8));
                UInt32 length = Memory[0].GetI32((UInt32) (parameters[1].i32 + i * 8 + 4));

                if (bytes + length - 1 < Memory[0].Buffer.Length)
                {
                    read += (UInt32)fd.Read(Memory[0].Buffer, (int)bytes, (int)length);
                }
                else
                {
                    throw new Trap("out of bounds memory access");
                }
            }
            Memory[0].SetI32(parameters[3].i32, read);
            
            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }

        public Value[] FdSeek(Value[] parameters)
        {
            UInt64 offset = parameters[1].i64;
            byte whence = (byte) parameters[2].i32;

            if (!FileDescriptors.ContainsKey(parameters[0].i32))
            {
                return new Value[] { Value.GetI32(WASI_EBADF) };
            }

            var fd = FileDescriptors[parameters[0].i32];

            UInt64 pos;
            if(whence == WASI_WHENCE_CUR) 
                pos = (UInt64) fd.Seek((long)offset, SeekOrigin.Current);
            else if(whence == WASI_WHENCE_END)
                pos = (UInt64) fd.Seek((long)offset, SeekOrigin.End);
            else
                pos = (UInt64) fd.Seek((long)offset, SeekOrigin.Begin);

            Memory[0].SetBytes(parameters[3].i32, BitConverter.GetBytes(pos));
            
            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }

        public Value[] FdWrite(Value[] parameters)
        {
            if (Debug && parameters[0].i32 > 3)
            {
                Console.WriteLine("fd_write(" + parameters[0].i32 + ")");
            }

            UInt32 written = 0;
            if (FileDescriptors.ContainsKey(parameters[0].i32))
            {
                var fd = FileDescriptors[parameters[0].i32];
                for (int i = 0; i < parameters[2].i32; i++)
                {
                    UInt32 bytes = Memory[0].GetI32((UInt32) (parameters[1].i32 + i * 8));
                    UInt32 length = Memory[0].GetI32((UInt32) (parameters[1].i32 + i * 8 + 4));

                    if (length > 0)
                    {
                        if (bytes + length - 1 < Memory[0].Buffer.Length)
                        {
                            fd.Write(Memory[0].Buffer, (int) bytes, (int) length);
                        }
                        else
                        {
                            throw new Trap("out of bounds memory access");
                        }
                    }

                    written += length;
                }
            }
            else
            {
                return new Value[] { Value.GetI32(WASI_EBADF) };
            }

            Memory[0].SetI32(parameters[3].i32, written);
            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }
        
        public Value[] PathFilestatGet(Value[] parameters)
        {
            string path = Encoding.UTF8.GetString(Memory[0].GetBytes(parameters[2].i32, parameters[3].i32));
            if (Debug)
            {
                Console.WriteLine("path_filestat_get("+parameters[0].i32+", "+parameters[1].i32+", \""+path+"\")");
            }

            //TODO: Check follow symlink flag

            var f = new FileInfo(path);
            
            if (!f.Exists)
            {
                return new Value[] { Value.GetI32(WASI_EBADF) };
            }

            // Device
            Memory[0].SetBytes(parameters[4].i32, BitConverter.GetBytes((UInt64) 0));
            // INode
            Memory[0].SetBytes(parameters[4].i32 + 8, BitConverter.GetBytes((UInt64) 0));
            // Type
            byte type = 0;
            if (f.Attributes.HasFlag(FileAttributes.Device))
            {
                type = WASI_FILETYPE_BLOCK_DEVICE;
            }
            else if (f.Attributes.HasFlag(FileAttributes.Directory))
            {
                type = WASI_FILETYPE_DIRECTORY;
            }
            else if (f.Attributes.HasFlag(FileAttributes.Normal))
            {
                type = WASI_FILETYPE_REGULAR_FILE;
            }
            else // TODO: implement other types
            {
                type = WASI_FILETYPE_UNKNOWN;
            }
            Memory[0].SetBytes(parameters[4].i32 + 16, new byte[] { type });
            // # Hardlinks to file
            Memory[0].SetBytes(parameters[4].i32 + 20, BitConverter.GetBytes((UInt32) 0));
            // FileSize
            Memory[0].SetBytes(parameters[4].i32 + 24, BitConverter.GetBytes((UInt64) f.Length));
            // AccessTime
            Memory[0].SetBytes(parameters[4].i32 + 32, BitConverter.GetBytes((UInt64) f.LastAccessTime.ToFileTime()));
            // ModificationTime
            Memory[0].SetBytes(parameters[4].i32 + 40, BitConverter.GetBytes((UInt64) f.LastWriteTime.ToFileTime()));
            // CreationTime
            Memory[0].SetBytes(parameters[4].i32 + 48, BitConverter.GetBytes((UInt64) f.CreationTime.ToFileTime()));

            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }
        
        public Value[] PathOpen(Value[] parameters)
        {
            string path = Encoding.UTF8.GetString(Memory[0].GetBytes(parameters[2].i32, parameters[3].i32));

            if (Debug)
            {
                Console.WriteLine("path_open("+parameters[0].i32+", "+path+")");
            }

            var f = new FileInfo(path);

            UInt32 flags = parameters[4].i32;
            FileStream fd;
            // TODO: handle permissions, modes, and other attributes (currently just opens readOnly
            if ((flags & WASI_O_CREAT) > 0)
            {
                fd = f.Open(FileMode.OpenOrCreate);
            }
            else
            {
                if (!f.Exists)
                {
                    return new Value[] { Value.GetI32(WASI_ENFILE) };
                }
                fd = f.Open(FileMode.Open);
            }
            
            FileDescriptors.Add((UInt32)fd.SafeFileHandle.DangerousGetHandle().ToInt32() + 3, fd);
            Memory[0].SetI32(parameters[8].i32, (UInt32)fd.SafeFileHandle.DangerousGetHandle().ToInt32() + 3);

            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }
        
        public Value[] ProcExit(Value[] parameters)
        {
            if (Debug)
            {
                Console.WriteLine("proc_exit("+parameters[0].i32+")");
            }
            
            throw new Exception("ProcExit Called");
//            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }

        public Value[] ClockTimeGet(Value[] parameters)
        {
            long nano = Stopwatch.GetTimestamp() * 1000000000 / Stopwatch.Frequency;
            Memory[0].SetI64(parameters[2].i32, (UInt64)nano);

            return new Value[] { Value.GetI32(WASI_ESUCCESS) };
        }
    }
}
