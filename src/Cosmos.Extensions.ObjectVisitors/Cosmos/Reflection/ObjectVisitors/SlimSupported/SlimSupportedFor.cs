namespace Cosmos.Reflection.ObjectVisitors.SlimSupported
{
    public enum SlimSupportedFor
    {
#if !NETFRAMEWORK
        AnonymousObject,
#endif
        DynamicObject,
        ExpandoObject,
        Tuple
    }
}