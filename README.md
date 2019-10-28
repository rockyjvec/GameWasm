# GameWasm
A WebAssembly/WASI VM to be used for adding modding support in games.

This implementation differs from others by allowing execution in the VM to be throttled, paused etc.  So CPU resources can be "sandboxed" in addition to the other sandbox features of WebAssembly/WASI (filesystem access etc.).

I'm planning to use this in Space Engineers, initially, to add better, sandboxed, in game scripting support.

## Progress

[x] WebAssembly

[x] Throttling

[X] Runs [DOOMWASI](/rockyjvec/DOOMWASI)

[-] WASI - Partial support.

