using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switch
{
    public class Result
    {
        public uint Module
        {
            get => UnpackValueModule(Value);

            set => Value = PackValue(value, Description);
        }

        public uint Description
        {
            get => UnpackValueDescription(Value);

            set => Value = PackValue(Module, value);
        }

        public uint Value { get; set; }

        public bool IsSuccess
        {
            get => Value == 0;
        }

        public bool IsFailure
        {
            get => !IsSuccess;
        }

        public Result(uint full_value)
        {
            Value = full_value;
        }

        public static implicit operator Result(uint val) => new Result(val);

        public override string ToString()
        {
            return $"{Module + 2000:0000}-{Description:0000}";
        }

        public static uint UnpackValueModule(uint val) => (val) & 0x1FF;

        public static uint UnpackValueDescription(uint val) => ((val) >> 9) & 0x1FFF;

        public static uint PackValue(uint mod, uint desc) => ((mod) & 0x1FF) | ((desc) & 0x1FFF) << 9;
    }
}
