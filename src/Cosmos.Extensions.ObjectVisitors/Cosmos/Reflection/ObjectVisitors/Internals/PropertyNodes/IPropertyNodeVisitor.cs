using Cosmos.Reflection.ObjectVisitors.Correctness;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation;

namespace Cosmos.Reflection.ObjectVisitors.Internals.PropertyNodes;

internal interface IPropertyNodeVisitor
{
    string Name { get; }
        
    Type SourceType { get; }

    PropertyNodeVisitor Root { get; }

    PropertyNodeVisitor Parent { get; }

    int Deep { get; }

    int Signature { get; }

    bool HasChildren();

    bool IsEnd();

    bool Repeatable { get; }

    bool StrictMode { get; }

    AlgorithmKind AlgorithmKind { get; }

    IValidationEntry VerifiableEntry { get; }

    VerifyResult Verify();

    VerifyResult Verify(string globalVerifyProviderName);

    void VerifyAndThrow();

    void VerifyAndThrow(string globalVerifyProviderName);

    void ReNew();

    void Sync(object instance);

    public void SetValue(List<string> pathSegments, object value, int startIndex);

    public void SetValue(List<string> pathSegments, object value, int startIndex, string globalVerifyProviderName);

    object GetValue(List<string> pathSegments, int startIndex);

    IEnumerable<string> GetMemberNames();

    ObjectMember GetMember(string memberName);

    bool Contains(string memberName);

    IPropertyValueAccessor ToValueAccessor();
}