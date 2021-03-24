using Cosmos.Validation.Registrars;

namespace Cosmos.Reflection.Correctness
{
    internal static class CorrectnessVerifiableWallInitializer
    {
        private static bool _flag = false;
        
        public static void InitializeAndPreheating()
        {
            if (!_flag)
            {
                var projectManager = new CorrectnessProjectManager();
                var objectResolver = new CorrectnessObjectResolver();
                var options = CorrectnessOptions.Original();

                var provider = new CorrectnessProvider(projectManager, objectResolver, options);

                // Add the global correct provider into validation-me
                ValidationRegistrar.ForProvider(provider, CorrectnessVerifiableWall.GLOBAL_CORRECT_PROVIDER_KEY).Build();
            
                // Update flag
                _flag = true;
            }            
        }
    }
}