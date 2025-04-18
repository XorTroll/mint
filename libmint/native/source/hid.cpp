#include <hid.hpp>

namespace hid
{
    #define _HID_KEYS_FN(type) \
    JS_DEFINE_NATIVE_FN(getKeys##type, (u32 controller_id) -> u64 \
    { \
        hidScanInput(); \
        return hidKeys##type((HidControllerID)controller_id); \
    })

    _HID_KEYS_FN(Down)
    _HID_KEYS_FN(Up)
    _HID_KEYS_FN(Held)

    JS_MODULE_DECLARE_START(hid)
    JS_MODULE_EXPORT_NATIVE_FN(getKeysDown)
    JS_MODULE_EXPORT_NATIVE_FN(getKeysUp)
    JS_MODULE_EXPORT_NATIVE_FN(getKeysHeld)
    JS_MODULE_DECLARE_END(hid)

    JS_MODULE_INIT_ROUTINE_START(hid)
    JS_MODULE_INIT_ROUTINE_END

    void InitializeModule()
    {
        JS_MODULE_INCLUDE_START(hid)
        JS_MODULE_INCLUDE_END(hid)
    }
}