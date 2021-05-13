namespace Cosmos.Reflection.ObjectVisitors
{
    public static class GlobalVerifyProviderName
    {
        public static string Default => string.Empty;

        public static string For(string providerName) => providerName;
    }
}