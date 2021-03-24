namespace CosmosProwessUT.OvUT.Helpers
{
    public abstract class Prepare
    {
        static Prepare()
        {
#if !NETFRAMEWORK
            NatashaInitializer.InitializeAndPreheating();
#endif
        }
    }
}