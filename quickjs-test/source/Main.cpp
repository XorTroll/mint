
#include <js>

JS_MODULE_DECLARE_START(demo)

JS_MODULE_EXPORT_VARIABLE(qwe, 42.0f)

JS_MODULE_DECLARE_END(demo)

int main()
{
    consoleInit(NULL);
    js::Initialize();

    JS_PUSH_MODULE(demo)

    consoleUpdate(NULL);
    printf("\nEval result: %s\n", js::Evaluate("demo.qwe", "<test-eval>").c_str());
    consoleUpdate(NULL);
    while(true);

    consoleExit(NULL);
    js::Finalize();
}