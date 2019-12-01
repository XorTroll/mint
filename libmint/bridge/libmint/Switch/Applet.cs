using Bridge;

namespace Switch
{
    [Enum(Emit.Value)]
    public enum AppletType
    {
        None = -2,
        Default = -1,
        Application = 0,
        SystemApplet = 1,
        LibraryApplet = 2,
        OverlayApplet = 3,
        SystemApplication = 4,
    }

    [External]
    [Name("applet")]
    public static class Applet
    {
        [Name("getAppletType")]
        public static extern AppletType GetAppletType();
    }
}
