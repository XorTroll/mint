#include <quickjs/quickjs.h>
#include <quickjs/quickjs-libc.h>

#include <switch.h>

typedef JSValue JSCGetterFunction(JSContext *ctx, JSValueConst this_val);
typedef JSValue JSCSetterFunction(JSContext *ctx, JSValueConst this_val, JSValueConst val);

JSCFunctionListEntry __js_C_MakeNativeFunctionImpl(JSCFunction *fn, const char *name, int count)
{
    return (JSCFunctionListEntry)JS_CFUNC_DEF(name, count, fn);
}

JSCFunctionListEntry __js_C_MakeStringPropertyImpl(const char *name, const char *val, int flags)
{
    return (JSCFunctionListEntry)JS_PROP_STRING_DEF(name, val, flags);
}

JSCFunctionListEntry __js_C_MakeNumberPropertyImpl(const char *name, double val, int flags)
{
    return (JSCFunctionListEntry)JS_PROP_DOUBLE_DEF(name, val, flags);
}

JSCFunctionListEntry __js_C_MakeGetterSetterImpl(const char *name, JSCGetterFunction *getter, JSCSetterFunction *setter)
{
    return (JSCFunctionListEntry)JS_CGETSET_DEF(name, getter, setter);
}