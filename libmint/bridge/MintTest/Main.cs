using Bridge;
using System;

namespace MintTest
{
    public class MintMain
    {
        public static void Main()
        {
            // Out variables work :)
            Switch.Applet.Test(out var res);

            Switch.Console.Initialize();

            QuickJS.Std.PrintF("Hello from C#!\nConsole initialized: %d\n", Switch.Console.IsInitialized());
            Switch.Console.Flush();

            QuickJS.Std.PrintF($"Got value: {res:X}");
            Switch.Console.Flush();

            while (true) ;
        }
    }
}