# GameWasm
A WebAssembly/WASI interpreter to be used for adding modding support in games.

This implementation differs from others by allowing execution of wasm to be throttled, paused etc.  CPU resources can be "sandboxed" in addition to the other sandbox features of WebAssembly/WASI (filesystem access etc.).

I'm planning to use this in Space Engineers, initially, to add better, sandboxed, in game scripting support.

## Progress

[x] WebAssembly - passes the spec tests

[x] Throttling

[X] Runs [DOOM](https://github.com/rockyjvec/DOOMWASI) at full speed.  That special version of doom has been modified to write the screen output to a file that can be viewed externally.

[-] WASI - Partial support.

