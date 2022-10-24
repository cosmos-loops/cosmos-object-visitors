namespace CosmosObjectVisitorUT.Helpers
{
    public abstract class Prepare
    {
        static Prepare()
        {
#if !NETFRAMEWORK
            NatashaInitializer.Preheating();
#endif
        }
    }
}