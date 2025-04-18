using System;

namespace [mintc-dummy-project]
{
    public class Main
    {
        public static void Main()
        {
            Switch.Console.Initialize();

            QuickJS.Std.PrintF($"Hello from C#!\nConsole initialized: {Switch.Console.IsInitialized()}\n");
            

            while(true)
            {
            Switch.Console.Flush();
        }
        }
    }
}