using System;
using CosmosObjectVisitorUT.Model;
using Cosmos.Reflection.ObjectVisitors;
using Cosmos.Validation;
using Xunit;

namespace CosmosObjectVisitorUT
{
    [Trait("Validation Rules", "Scope")]
    public class ValidationRuleScopeTests
    {
        public ValidationRuleScopeTests()
        {
            Data = new NiceAct2(true);
            Type = typeof(NiceAct2);
        }

        public Type Type { get; set; }
        public NiceAct2 Data { get; set; }

        [Fact(DisplayName = "Normal model with one 'And' test.")]
        public void NormalModelWithAndTest()
        {
            var v = ObjectVisitor.Create(Type, Data);
            v.VerifiableEntry.ForMember("Str", c => c.MaxLength(7).And().MinLength(5));
            var r = v.Verify();
            Assert.True(r.IsValid);
        }

        [Fact(DisplayName = "Normal model with two 'And's test.")]
        public void NormalModelWith2AndsTest()
        {
            var v = ObjectVisitor.Create(Type, Data);
            v.VerifiableEntry.ForMember("Str", c => c.MaxLength(7).And().MinLength(5).And().NotEmpty());
            var r = v.Verify();
            Assert.True(r.IsValid);
        }

        [Fact(DisplayName = "Normal model with one 'Or' test.")]
        public void NormalModelWithOrTest()
        {
            var v = ObjectVisitor.Create(Type, Data);
            v.VerifiableEntry.ForMember("Str", c => c.MaxLength(7).Or().MinLength(5));
            var r = v.Verify();
            Assert.True(r.IsValid);
        }

        [Fact(DisplayName = "Normal model with two 'Or's test.")]
        public void NormalModelWith2OrsTest()
        {
            var v = ObjectVisitor.Create(Type, Data);
            v.VerifiableEntry.ForMember("Str", c => c.MaxLength(7).Or().MinLength(5).Or().Empty());
            var r = v.Verify();
            Assert.True(r.IsValid);

            var v2 = ObjectVisitor.Create(Type, new NiceAct2(true) {Str = ""});
            v2.VerifiableEntry.ForMember("Str", c => c.MaxLength(7).Or().MinLength(5).Or().Empty());
            var r2 = v2.Verify();
            Assert.True(r2.IsValid);
        }
    }
}