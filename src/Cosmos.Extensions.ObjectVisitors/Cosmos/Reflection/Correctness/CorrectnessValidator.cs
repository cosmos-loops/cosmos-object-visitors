using System;
using System.Collections.Generic;
using Cosmos.Validation;
using Cosmos.Validation.Validators;

namespace Cosmos.Reflection.Correctness
{
    public class CorrectnessValidator: IValidator
    {
        public VerifyResult Verify(Type declaringType, object instance)
        {
            throw new NotImplementedException();
        }

        public VerifyResult VerifyOne(Type declaringType, object memberValue, string memberName)
        {
            throw new NotImplementedException();
        }

        public VerifyResult VerifyMany(Type declaringType, IDictionary<string, object> keyValueCollections)
        {
            throw new NotImplementedException();
        }

        public string Name { get; }
        public bool IsAnonymous { get; }
    }
}