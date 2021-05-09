using System;
using Cosmos.Validation;

namespace Cosmos.Reflection.ObjectVisitors.Correctness
{
    internal static class CorrectnessOptions
    {
        static CorrectnessOptions()
        {
            Options = new ValidationOptions();
        }

        private static ValidationOptions Options { get; set; }

        public static void Update(Action<ValidationOptions> updateAct)
        {
            updateAct?.Invoke(Options);
        }

        public static ValidationOptions Copy()
        {
            return Options.DeepCopy(DeepCopyOptions.ExpressionCopier);
        }

        public static ValidationOptions Original()
        {
            return Options;
        }
    }
}