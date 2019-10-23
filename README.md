# GameWasm
An experimental implementation of a WebAssembly/WASI VM in C# to be used for modding support in games.

This implementation differs from others by allowing execution in the VM to be throttled, paused etc.  This allows games, such as Space Engineers, that allow in game scripting to have the scripts be sandboxed and limited so they don't take up too many cpu cycles, memory, etc.  It can also allow sandboxed access to the filesystem.

