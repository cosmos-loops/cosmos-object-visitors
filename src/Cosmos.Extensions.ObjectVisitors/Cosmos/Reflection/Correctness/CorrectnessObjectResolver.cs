using System;
using System.Collections.Generic;
using Cosmos.Reflection.Internals.Members;
using Cosmos.Validation.Objects;

namespace Cosmos.Reflection.Correctness
{
    internal class CorrectnessObjectResolver : AbstractVerifiableObjectResolver
    {
        public override VerifiableObjectContext Resolve<T>(T instance)
        {
            if (instance is VerifiableObjectContext objectContext) return objectContext;
            if (instance is VerifiableMemberContext memberContext) return memberContext.ConvertToObjectContext();

            var handler = MemberHandlerResolver.Resolve<T>();
            var impl = new MemberObjectContractImpl(handler);
            var contract = new VerifiableObjectContract(impl);
            return contract.WithInstance(instance);
        }

        public override VerifiableObjectContext Resolve<T>(T instance, string instanceName)
        {
            if (instance is VerifiableObjectContext objectContext) return objectContext;
            if (instance is VerifiableMemberContext memberContext) return memberContext.ConvertToObjectContext();

            var handler = MemberHandlerResolver.Resolve<T>();
            var impl = new MemberObjectContractImpl(handler);
            var contract = new VerifiableObjectContract(impl);
            return contract.WithInstance(instance, instanceName);
        }

        public override VerifiableObjectContext Resolve<T>(IDictionary<string, object> keyValueCollection)
        {
            var handler = MemberHandlerResolver.Resolve<T>();
            var impl = new MemberObjectContractImpl(handler);
            var contract = new VerifiableObjectContract(impl);
            return contract.WithDictionary(keyValueCollection);
        }

        public override VerifiableObjectContext Resolve<T>(IDictionary<string, object> keyValueCollection, string instanceName)
        {
            var handler = MemberHandlerResolver.Resolve<T>();
            var impl = new MemberObjectContractImpl(handler);
            var contract = new VerifiableObjectContract(impl);
            return contract.WithDictionary(keyValueCollection, instanceName);
        }

        public override VerifiableObjectContext Resolve(Type declaringType, object instance)
        {
            if (instance is VerifiableObjectContext objectContext) return objectContext;
            if (instance is VerifiableMemberContext memberContext) return memberContext.ConvertToObjectContext();

            var handler = MemberHandlerResolver.Resolve(declaringType);
            var impl = new MemberObjectContractImpl(handler);
            var contract = new VerifiableObjectContract(impl);
            return contract.WithInstance(instance);
        }

        public override VerifiableObjectContext Resolve(Type declaringType, object instance, string instanceName)
        {
            if (instance is VerifiableObjectContext objectContext) return objectContext;
            if (instance is VerifiableMemberContext memberContext) return memberContext.ConvertToObjectContext();

            var handler = MemberHandlerResolver.Resolve(declaringType);
            var impl = new MemberObjectContractImpl(handler);
            var contract = new VerifiableObjectContract(impl);
            return contract.WithInstance(instance, instanceName);
        }

        public override VerifiableObjectContext Resolve(Type declaringType, IDictionary<string, object> keyValueCollection)
        {
            var handler = MemberHandlerResolver.Resolve(declaringType);
            var impl = new MemberObjectContractImpl(handler);
            var contract = new VerifiableObjectContract(impl);
            return contract.WithDictionary(keyValueCollection);
        }

        public override VerifiableObjectContext Resolve(Type declaringType, IDictionary<string, object> keyValueCollection, string instanceName)
        {
            var handler = MemberHandlerResolver.Resolve(declaringType);
            var impl = new MemberObjectContractImpl(handler);
            var contract = new VerifiableObjectContract(impl);
            return contract.WithDictionary(keyValueCollection, instanceName);
        }
    }
}