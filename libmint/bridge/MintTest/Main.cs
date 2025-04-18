using Bridge;
using System;

namespace MintTest
{
    public class MintMain
    {
        public static void Main()
        {
            Switch.Console.Initialize();

            QuickJS.Std.PrintF("Hello world!\n");

            while (Switch.Applet.MainLoop())
            {
                var keys = Switch.Hid.GetKeysHeld(Switch.Controller.Auto);
                if((keys & Switch.Keys.Plus) != 0)
                {
                    QuickJS.Std.PrintF("Matches...\n");
                }
                Switch.Console.Flush();
            }

            Switch.Console.Exit();
        }
    }
}