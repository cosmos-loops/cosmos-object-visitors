using Cosmos.Validation;
using Cosmos.Validation.Validators;

namespace Cosmos.Reflection.ObjectVisitors.Correctness;

internal static class CorrectnessVerifiableWall
{
    public const string GLOBAL_CORRECT_PROVIDER_KEY = "__cosmos_ov";

    public static IValidationProvider ValidationProvider { get; set; }

    public static void InitValidationProvider(IValidationProvider provider)
    {
        if (provider is null) return;
        if (ValidationProvider is not null) return;
        ValidationProvider = provider;
    }

    #region Resolve

    public static IValidator Resolve(Type declaringType) => ValidationProvider.Resolve(declaringType);

    public static IValidator<T> Resolve<T>() => ValidationProvider.Resolve<T>();

    #endregion
}