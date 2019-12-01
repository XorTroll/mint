#include <global.hpp>

namespace global
{
    class Result
    {
        public:
            Result(u32 value) : inner_val(value)
            {
            }

            u32 value_get()
            {
                return this->inner_val;
            }

            void value_set(u32 value)
            {
                this->inner_val = value;
            }

            u32 module_get()
            {
                return R_MODULE(this->inner_val);
            }

            void module_set(u32 module)
            {
                this->inner_val = MAKERESULT(module, this->description_get());
            }

            u32 description_get()
            {
                return R_DESCRIPTION(this->inner_val);
            }

            void description_set(u32 description)
            {
                this->inner_val = MAKERESULT(this->module_get(), description);
            }

            bool isSuccess_get()
            {
                return R_SUCCEEDED(this->inner_val);
            }

            bool isFailure_get()
            {
                return R_FAILED(this->inner_val);
            }
        private:
            u32 inner_val;
    };

    JS_CLASS_DECLARE(Result, u32)

    JS_CLASS_DECLARE_PROPERTY_GS(Result, value, u32)
    JS_CLASS_DECLARE_PROPERTY_GS(Result, module, u32)
    JS_CLASS_DECLARE_PROPERTY_GS(Result, description, u32)
    JS_CLASS_DECLARE_PROPERTY_G(Result, isSuccess, bool)
    JS_CLASS_DECLARE_PROPERTY_G(Result, isFailure, bool)

    JS_CLASS_PROTOTYPE_START(Result)
    JS_CLASS_PROTOTYPE_EXPORT_PROPERTY_GS(Result, value)
    JS_CLASS_PROTOTYPE_EXPORT_PROPERTY_GS(Result, module)
    JS_CLASS_PROTOTYPE_EXPORT_PROPERTY_GS(Result, description)
    JS_CLASS_PROTOTYPE_EXPORT_PROPERTY_G(Result, isSuccess)
    JS_CLASS_PROTOTYPE_EXPORT_PROPERTY_G(Result, isFailure)
    JS_CLASS_PROTOTYPE_END

    void Initialize()
    {
        JS_GLOBAL_EXPORT_CLASS(Result)
    }
}