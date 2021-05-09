namespace CosmosObjectVisitorUT.Helpers
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