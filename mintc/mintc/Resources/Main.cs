using Bridge;
using System;

namespace [mintc-dummy-project]
{
    [Namespace(false)]
    public class MintMain
    {
        [External]
        [Template("demo_test({txt})")]
        public extern static void DemoTest(string txt);

        public static void Main()
        {
            DemoTest("Welcome to Bridge.NET!");

            while (true) ;
        }
    }
}