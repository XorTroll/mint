using Bridge;
using System;

namespace MintTest
{
    public class MintMain
    {
        public static void Main()
        {
            Switch.Console.Initialize();
            var res = new Switch.Result(0x4A8);

            QuickJS.Std.PrintF("Hello from C#!\nConsole initialized: %d\n", Switch.Console.IsInitialized());
            Switch.Console.Flush();

            QuickJS.Std.PrintF($"Result info:\n- Module: {res.Module}\n- Description: {res.Description}\n- IsSuccess: {res.IsSuccess}\n- Hex value: {res.Value:X}");
            Switch.Console.Flush();

            while (true) ;
        }
    }
}