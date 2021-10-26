namespace CosmosStack.Reflection.ObjectVisitors
{
    /// <summary>
    /// Global verify provider names <br />
    /// 全局验证提供者程序名
    /// </summary>
    public static class GlobalVerifyProviderNames
    {
        /// <summary>
        /// Default <br />
        /// 默认
        /// </summary>
        public static string Default => string.Empty;

        /// <summary>
        /// For <br />
        /// 使用指定的名称
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static string For(string providerName) => providerName;
    }
}