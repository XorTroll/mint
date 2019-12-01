
#include <js>

JS_MODULE_DECLARE_START(applet)
JS_MODULE_DECLARE_END(applet)

JS_CLASS_REF(Demo)

class Demo
{
    public:
        Demo(u32 a, u32 b) : ia(a), ib(b) {}

        u32 a_get() { return ia; }
        void a_set(u32 a) { ia = a; }

        u32 demo() { return ia * ib; }

        auto makeDemo() { return JS_FN_CREATE_INSTANCE(Demo, ia / 2, ib / 2); }
    private:
        u32 ia;
        u32 ib;
};

JS_CLASS_DECLARE(Demo, u32, u32)
JS_CLASS_DECLARE_PROPERTY_GS(Demo, a, u32)
JS_CLASS_DECLARE_MEMBER_FN(Demo, demo)
JS_CLASS_DECLARE_MEMBER_FN(Demo, makeDemo)

JS_CLASS_PROTOTYPE_START(Demo)
JS_CLASS_PROTOTYPE_EXPORT_PROPERTY_GS(Demo, a)
JS_CLASS_PROTOTYPE_EXPORT_MEMBER_FN(Demo, demo)
JS_CLASS_PROTOTYPE_EXPORT_MEMBER_FN(Demo, makeDemo)
JS_CLASS_PROTOTYPE_END

JS_MODULE_INIT_ROUTINE_START(applet)
// JS_MODULE_INIT_ROUTINE_EXPORT_CLASS(Demo)
JS_MODULE_INIT_ROUTINE_EXPORT_PROPERTY(gay, (bool)true)
JS_MODULE_INIT_ROUTINE_END

void InitializeModule()
{
    JS_GLOBAL_EXPORT_PROPERTY(bruh, "moment")
    JS_GLOBAL_EXPORT_CLASS(Demo)
    JS_MODULE_INCLUDE_START(applet)
    // JS_MODULE_INCLUDE_CLASS(applet, Demo)
    JS_MODULE_INCLUDE_PROPERTY(applet, gay)
    JS_MODULE_INCLUDE_END(applet)
}

int main()
{
    consoleInit(NULL);
    js::Initialize();

    InitializeModule();

    consoleUpdate(NULL);
    printf("\nEval result: %s\n", js::Evaluate("bruh", "<test-eval>").c_str());
    consoleUpdate(NULL);
    
    while(true)
    {
        hidScanInput();
        if(hidKeysDown(CONTROLLER_P1_AUTO)) break;
    }

    consoleExit(NULL);
    js::Finalize();
}