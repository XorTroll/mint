using Bridge;
using System;

namespace mintnewdemo
{
    public class MintMain
    {
        public static void Main()
        {
            Switch.Console.Initialize();
            QuickJS.Std.PrintF("Hello from C#! Console initialized: %d\n", Switch.Console.IsInitialized());
            Switch.Console.Flush();
            var arr = new byte[] { 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }; // 0100000000001000
            QuickJS.Std.PrintF("Array to string: %s, type: %s\n", BitConverter.ToString(arr), arr.GetType().ToString());
            Switch.Console.Flush();

            while (true) ;
        }
    }
}