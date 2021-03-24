using Cosmos.Validation;

namespace Cosmos.Reflection.Internals
{
    internal static class VerifyResultExtensions
    {
        public const string COMMON_VERIFY_FAILURE_STRING = "Verification failed. Please see internal information for specific errors.";

        public static void Raise(this VerifyResult result)
        {
            if (result is not null && !result.IsValid)
            {
                result.Raise(COMMON_VERIFY_FAILURE_STRING);
            }
        }
    }
}