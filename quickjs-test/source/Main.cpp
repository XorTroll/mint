
#include <js>

JS_DEFINE_NATIVE_FN(buftest, (u32 size)
{
    u8 *buf = new u8[size]();
    randomGet(buf, size);
    
    return js::Buffer::Make(buf, size);
})

JS_MODULE_DECLARE_START(demo)

JS_MODULE_EXPORT_NATIVE_FN(buftest)

JS_MODULE_DECLARE_END(demo)

int main()
{
    consoleInit(NULL);
    js::Initialize();

    JS_PUSH_MODULE(demo)

    consoleUpdate(NULL);
    printf("\nEval result: %s\n", js::Evaluate("var test = demo.buftest(10); Array.prototype.map.call(new Uint8Array(test), x => ('00' + x.toString(16)).slice(-2)).join('')", "<test-eval>").c_str());
    consoleUpdate(NULL);
    
    while(true)
    {
        hidScanInput();
        if(hidKeysDown(CONTROLLER_P1_AUTO)) break;
    }

    consoleExit(NULL);
    js::Finalize();
}