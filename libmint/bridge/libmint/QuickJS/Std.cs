using Bridge;

namespace QuickJS
{
    [External]
    [Name("std")]
    public static class Std
    {
        [Name("exit")]
        public static extern void Exit(int code);

        [Name("evalScript")]
        public static extern void EvaluateScript(string src);

        [Name("loadScript")]
        public static extern void LoadScript(string path);

        [Name("printf")]
        [ExpandParams]
        public static extern void PrintF(string fmt, params object[] args);
    }
}
