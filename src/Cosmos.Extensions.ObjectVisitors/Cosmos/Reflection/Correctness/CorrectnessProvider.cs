using Cosmos.Validation;
using Cosmos.Validation.Objects;
using Cosmos.Validation.Projects;

namespace Cosmos.Reflection.Correctness
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