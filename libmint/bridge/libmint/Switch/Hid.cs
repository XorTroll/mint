using Bridge;
using System;

namespace Switch
{
    [Enum(Emit.Value)]
    [Flags]
    public enum Keys : ulong
    {
        A = 1 << 0,
        B = 1 << 1,
        X = 1 << 2,
        Y = 1 << 3,
        LStick = 1 << 4,
        RStick = 1 << 5,
        L = 1 << 6,
        R = 1 << 7,
        ZL = 1 << 8,
        ZR = 1 << 9,
        Plus = 1 << 10,
        Minus = 1 << 11,
        DPadLeft = 1 << 12,
        DPadUp = 1 << 13,
        DPadRight = 1 << 14,
        DPadDown = 1 << 15,
        LStickLeft = 1 << 16,
        LStickUp = 1 << 17,
        LStickRight = 1 << 18,
        LStickDown = 1 << 19,
        RStickLeft = 1 << 20,
        RStickUp = 1 << 21,
        RStickRight = 1 << 22,
        RStickDown = 1 << 23,
        SLLeft = 1 << 24,
        SRLeft = 1 << 25,
        SLRight = 1 << 26,
        SRRight = 1 << 27,
        Touch = 1 << 28,

        Up = DPadUp | LStickUp | RStickUp,
        Down = DPadDown | LStickDown | RStickDown,
        Left = DPadLeft | LStickLeft | RStickLeft,
        Right = DPadRight | LStickRight | RStickRight,
        SL = SLLeft | SLRight,
        SR = SRLeft | SRRight,
    }

    [Enum(Emit.Value)]
    public enum Controller
    {
        P1,
        P2,
        P3,
        P4,
        P5,
        P6,
        P7,
        P8,
        Handheld,
        Unknown,
        Auto
    }

    [External]
    [Name("hid")]
    public static class Hid
    {
        [Name("getKeysDown")]
        public static extern Keys GetKeysDown(Controller controller);

        [Name("getKeysUp")]
        public static extern Keys GetKeysUp(Controller controller);

        [Name("getKeysHeld")]
        public static extern Keys GetKeysHeld(Controller controller);
    }
}
