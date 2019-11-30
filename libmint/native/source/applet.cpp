#include <applet.hpp>

namespace applet
{
    JS_DEFINE_NATIVE_FN(getAppletType, ()
    {
        return (u32)appletGetAppletType();
    })

    JS_DEFINE_NATIVE_FN(outTest, (js::Reference out_res)
    {
        out_res.Set<u32>(0x4A8);
    })

    JS_MODULE_DECLARE_START(applet)
    JS_MODULE_EXPORT_NATIVE_FN(getAppletType)
    JS_MODULE_EXPORT_NATIVE_FN(outTest)
    JS_MODULE_DECLARE_END(applet)

    void InitializeModule()
    {
        JS_PUSH_MODULE(applet)
    }
}