#include <applet.hpp>

namespace applet
{
    JS_DEFINE_NATIVE_FN(getAppletType, ()
    {
        return (u32)appletGetAppletType();
    })

    JS_DEFINE_NATIVE_FN(mainLoop, ()
    {
        return appletMainLoop();
    })

    JS_MODULE_DECLARE_START(applet)
    JS_MODULE_EXPORT_NATIVE_FN(getAppletType)
    JS_MODULE_EXPORT_NATIVE_FN(mainLoop)
    JS_MODULE_DECLARE_END(applet)

    JS_MODULE_INIT_ROUTINE_START(applet)
    JS_MODULE_INIT_ROUTINE_END

    void InitializeModule()
    {
        JS_MODULE_INCLUDE_START(applet)
        JS_MODULE_INCLUDE_END(applet)
    }
}