using Bridge;

namespace Switch
{
    [External]
    [Name("Result")]
    public class Result
    {
        public extern Result(uint Value);

        [Name("value")]
        public extern uint Value { get; set; }

        [Name("module")]
        public extern uint Module { get; set; }

        [Name("description")]
        public extern uint Description { get; set; }

        [Name("isSuccess")]
        public extern bool IsSuccess { get; }

        [Name("isFailure")]
        public extern bool IsFailure { get; }
    }
}
