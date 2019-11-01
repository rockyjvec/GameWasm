using System;

namespace GameWasm.Webassembly.Test
{
    class LeftToRight : Test
    {
        public LeftToRight(string path) : base(path)
        {

        }

        public override void test()
        {
            var filename = "left-to-right.wasm";

            var store = new Store();
            var test = store.LoadModule("test", path + '/' + filename);

            assert(test.Call("i32_add"), (UInt32) 0x0102);     
            assert(test.Call("i64_add"), (UInt32) 0x0102);
            assert(test.Call("i32_sub"), (UInt32) 0x0102);     
            assert(test.Call("i64_sub"), (UInt32) 0x0102);
            assert(test.Call("i32_mul"), (UInt32) 0x0102);     
            assert(test.Call("i64_mul"), (UInt32) 0x0102);
            assert(test.Call("i32_div_s"), (UInt32) 0x0102);   
            assert(test.Call("i64_div_s"), (UInt32) 0x0102);
            assert(test.Call("i32_div_u"), (UInt32) 0x0102);   
            assert(test.Call("i64_div_u"), (UInt32) 0x0102);
            assert(test.Call("i32_rem_s"), (UInt32) 0x0102);   
            assert(test.Call("i64_rem_s"), (UInt32) 0x0102);
            assert(test.Call("i32_rem_u"), (UInt32) 0x0102);   
            assert(test.Call("i64_rem_u"), (UInt32) 0x0102);
            assert(test.Call("i32_and"), (UInt32) 0x0102);     
            assert(test.Call("i64_and"), (UInt32) 0x0102);
            assert(test.Call("i32_or"), (UInt32) 0x0102);      
            assert(test.Call("i64_or"), (UInt32) 0x0102);
            assert(test.Call("i32_xor"), (UInt32) 0x0102);     
            assert(test.Call("i64_xor"), (UInt32) 0x0102);
            assert(test.Call("i32_shl"), (UInt32) 0x0102);     
            assert(test.Call("i64_shl"), (UInt32) 0x0102);
            assert(test.Call("i32_shr_u"), (UInt32) 0x0102);   
            assert(test.Call("i64_shr_u"), (UInt32) 0x0102);
            assert(test.Call("i32_shr_s"), (UInt32) 0x0102);   
            assert(test.Call("i64_shr_s"), (UInt32) 0x0102);
            assert(test.Call("i32_eq"), (UInt32) 0x0102);      
            assert(test.Call("i64_eq"), (UInt32) 0x0102);
            assert(test.Call("i32_ne"), (UInt32) 0x0102);      
            assert(test.Call("i64_ne"), (UInt32) 0x0102);
            assert(test.Call("i32_lt_s"), (UInt32) 0x0102);    
            assert(test.Call("i64_lt_s"), (UInt32) 0x0102);
            assert(test.Call("i32_le_s"), (UInt32) 0x0102);    
            assert(test.Call("i64_le_s"), (UInt32) 0x0102);
            assert(test.Call("i32_lt_u"), (UInt32) 0x0102);    
            assert(test.Call("i64_lt_u"), (UInt32) 0x0102);
            assert(test.Call("i32_le_u"), (UInt32) 0x0102);    
            assert(test.Call("i64_le_u"), (UInt32) 0x0102);
            assert(test.Call("i32_gt_s"), (UInt32) 0x0102);    
            assert(test.Call("i64_gt_s"), (UInt32) 0x0102);
            assert(test.Call("i32_ge_s"), (UInt32) 0x0102);    
            assert(test.Call("i64_ge_s"), (UInt32) 0x0102);
            assert(test.Call("i32_gt_u"), (UInt32) 0x0102);    
            assert(test.Call("i64_gt_u"), (UInt32) 0x0102);
            assert(test.Call("i32_ge_u"), (UInt32) 0x0102);    
            assert(test.Call("i64_ge_u"), (UInt32) 0x0102);
            assert(test.Call("i32_store"), (UInt32) 0x0102);   
            assert(test.Call("i64_store"), (UInt32) 0x0102);
            assert(test.Call("i32_store8"), (UInt32) 0x0102);  
            assert(test.Call("i64_store8"), (UInt32) 0x0102);
            assert(test.Call("i32_store16"), (UInt32) 0x0102); 
            assert(test.Call("i64_store16"), (UInt32) 0x0102);
            assert(test.Call("i64_store32"), (UInt32) 0x0102);
            assert(test.Call("i32_call"), (UInt32) 0x0102);    
            assert(test.Call("i64_call"), (UInt32) 0x0102);
            assert(test.Call("i32_call_indirect"), (UInt32) 0x010204);
            assert(test.Call("i64_call_indirect"), (UInt32) 0x010204);
            assert(test.Call("i32_select"), (UInt32) 0x010205);
            assert(test.Call("i64_select"), (UInt32) 0x010205);
            assert(test.Call("f32_add"), (UInt32) 0x0102);     
            assert(test.Call("f64_add"), (UInt32) 0x0102);
            assert(test.Call("f32_sub"), (UInt32) 0x0102);     
            assert(test.Call("f64_sub"), (UInt32) 0x0102);
            assert(test.Call("f32_mul"), (UInt32) 0x0102);     
            assert(test.Call("f64_mul"), (UInt32) 0x0102);
            assert(test.Call("f32_div"), (UInt32) 0x0102);     
            assert(test.Call("f64_div"), (UInt32) 0x0102);
            assert(test.Call("f32_copysign"), (UInt32) 0x0102);
            assert(test.Call("f64_copysign"), (UInt32) 0x0102);
            assert(test.Call("f32_eq"), (UInt32) 0x0102);      
            assert(test.Call("f64_eq"), (UInt32) 0x0102);
            assert(test.Call("f32_ne"), (UInt32) 0x0102);      
            assert(test.Call("f64_ne"), (UInt32) 0x0102);
            assert(test.Call("f32_lt"), (UInt32) 0x0102);      
            assert(test.Call("f64_lt"), (UInt32) 0x0102);
            assert(test.Call("f32_le"), (UInt32) 0x0102);      
            assert(test.Call("f64_le"), (UInt32) 0x0102);
            assert(test.Call("f32_gt"), (UInt32) 0x0102);      
            assert(test.Call("f64_gt"), (UInt32) 0x0102);
            assert(test.Call("f32_ge"), (UInt32) 0x0102);      
            assert(test.Call("f64_ge"), (UInt32) 0x0102);
            assert(test.Call("f32_min"), (UInt32) 0x0102);     
            assert(test.Call("f64_min"), (UInt32) 0x0102);
            assert(test.Call("f32_max"), (UInt32) 0x0102);     
            assert(test.Call("f64_max"), (UInt32) 0x0102);
            assert(test.Call("f32_store"), (UInt32) 0x0102);   
            assert(test.Call("f64_store"), (UInt32) 0x0102);
            assert(test.Call("f32_call"), (UInt32) 0x0102);    
            assert(test.Call("f64_call"), (UInt32) 0x0102);
            assert(test.Call("f32_call_indirect"), (UInt32) 0x010204);
            assert(test.Call("f64_call_indirect"), (UInt32) 0x010204);
            assert(test.Call("f32_select"), (UInt32) 0x010205);
            assert(test.Call("f64_select"), (UInt32) 0x010205);
            assert(test.Call("br_if"), (UInt32) 0x0102);
            assert(test.Call("br_table"), (UInt32) 0x0102);
        }
    }
}
