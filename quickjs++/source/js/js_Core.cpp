#include <js>
#include <cstdio>

namespace js
{
    namespace impl
    {
        static JSRuntime *core_rt = NULL;
        static JSContext *core_ctx = NULL;
        static bool initialized = false;

        JSContext *GetCoreContextImpl()
        {
            return core_ctx;
        }

        void PushGlobalFunctionImpl(JSContext *ctx, JSCFunction fn, size_t arg_count, std::string name)
        {
            auto global = JS_GetGlobalObject(ctx);
            JS_SetPropertyStr(ctx, global, name.c_str(), JS_NewCFunction(ctx, fn, name.c_str(), arg_count));
        }

        std::string EvaluateImpl(std::string src, std::string eval_name, int mode)
        {
            if(!impl::initialized) return "";
            auto res = JS_Eval(impl::core_ctx, src.c_str(), src.length(), eval_name.c_str(), mode);
            if(JS_IsException(res)) res = JS_GetException(impl::core_ctx);
            return JS_ToCString(impl::core_ctx, res);
        }

        void ImportModuleImpl(std::string name)
        {
            std::string ipt = "import * as " + name + " from '" + name + "';\n";
            ipt += "globalThis." + name + " = " + name + ";";
            auto res = EvaluateImpl(ipt, "<module-import>", JS_EVAL_TYPE_MODULE);
            printf("Import of '%s': %s\n", name.c_str(), res.c_str());
        }
    }

    void Initialize()
    {
        if(!impl::initialized)
        {
            impl::core_rt = JS_NewRuntime();
            if(impl::core_rt != NULL)
            {
                impl::core_ctx = JS_NewContext(impl::core_rt);
                if(impl::core_ctx != NULL)
                {
                    JS_SetModuleLoaderFunc(impl::core_rt, NULL, js_module_loader, NULL);
                    impl::initialized = true;
                }
            }
        }
    }

    bool IsInitialized()
    {
        return impl::initialized;
    }

    void Finalize()
    {
        if(impl::initialized)
        {
            if(impl::core_ctx != NULL) JS_FreeContext(impl::core_ctx);
            if(impl::core_rt != NULL) JS_FreeRuntime(impl::core_rt);
            impl::initialized = false;
        }
    }

    void PushQuickJSModules()
    {
        js_init_module_std(impl::core_ctx, "std");
        impl::ImportModuleImpl("std");
        js_init_module_os(impl::core_ctx, "os");
        impl::ImportModuleImpl("os");
    }

    std::string Evaluate(std::string js_src, std::string eval_name)
    {
        return impl::EvaluateImpl(js_src, eval_name, JS_EVAL_TYPE_GLOBAL);
    }

    std::string EvaluateFromFile(std::string path)
    {
        size_t filesz = 0;
        auto filebuf = (const char*)js_load_file(impl::core_ctx, &filesz, path.c_str());
        if(filebuf != NULL)
        {
            if(filesz > 0)
            {
                auto rsp = Evaluate(filebuf, path);
                js_free(impl::core_ctx, (void*)filebuf);
                return rsp;
            }
        }
        return "FileErrorBaby";
    }
}