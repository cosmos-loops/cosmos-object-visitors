using Cosmos.Validation;
using Cosmos.Validation.Strategies;

namespace CosmosObjectVisitorUT.Model
{
    public class NormalNiceActValidationStrategy : ValidationStrategy
    {
        public NormalNiceActValidationStrategy() : base(typeof(NiceAct))
        {
            ForMember("Name").NotEmpty().MinLength(4).MaxLength(15);
        }
    }

    public class GenericNiceActValidationStrategy : ValidationStrategy<NiceAct>
    {
        public GenericNiceActValidationStrategy()
        {
            ForMember(x => x.Name).NotEmpty().MinLength(4).MaxLength(15);
        }
    }
}