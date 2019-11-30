#include <console.hpp>

namespace console
{
    static bool initialized = false;

    JS_DEFINE_NATIVE_FN(initialize, ()
    {
        if(initialized) return false;
        consoleInit(NULL);
        initialized = true;
        return true;
    })

    JS_DEFINE_NATIVE_FN(isInitialized, ()
    {
        return initialized;
    })

    JS_DEFINE_NATIVE_FN(exit, ()
    {
        if(initialized)
        {
            consoleExit(NULL);
            initialized = false;
        }
    })

    JS_DEFINE_NATIVE_FN(flush, ()
    {
        if(initialized) consoleUpdate(NULL);
    })

    JS_DEFINE_NATIVE_FN(clear, ()
    {
        if(initialized) consoleClear();
    })

    JS_MODULE_DECLARE_START(console)
    JS_MODULE_EXPORT_NATIVE_FN(initialize)
    JS_MODULE_EXPORT_NATIVE_FN(exit)
    JS_MODULE_EXPORT_NATIVE_FN(isInitialized)
    JS_MODULE_EXPORT_NATIVE_FN(flush)
    JS_MODULE_EXPORT_NATIVE_FN(clear)
    JS_MODULE_DECLARE_END(console)

    void InitializeModule()
    {
        JS_PUSH_MODULE(console)
    }
}