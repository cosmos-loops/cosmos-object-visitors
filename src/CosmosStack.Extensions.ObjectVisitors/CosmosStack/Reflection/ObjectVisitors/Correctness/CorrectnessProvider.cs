using CosmosStack.Validation;
using CosmosStack.Validation.Objects;
using CosmosStack.Validation.Projects;

namespace CosmosStack.Reflection.ObjectVisitors.Correctness
{
    internal class CorrectnessProvider : AbstractValidationProvider
    {
        public CorrectnessProvider(
            IValidationProjectManager projectManager,
            IVerifiableObjectResolver objectResolver,
            ValidationOptions options)
            : base(projectManager, objectResolver, options) { }
    }
}