using Bridge;

namespace Switch
{
    [External]
    [Name("console")]
    public static class Console
    {
        [Name("initialize")]
        public static extern bool Initialize();

        [Name("isInitialized")]
        public static extern bool IsInitialized();

        [Name("flush")]
        public static extern void Flush();

        [Name("clear")]
        public static extern void Clear();

        [Name("exit")]
        public static extern void Exit();
    }
}
