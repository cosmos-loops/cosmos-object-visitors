using System;
using System.Reflection;
using Cosmos.Validation;
using Cosmos.Validation.Strategies;

namespace Cosmos.Reflection.ObjectVisitors.Correctness
{
    internal class LinkToCorrectnessContext<T> : IValidationEntry
    {
        private readonly CorrectnessContext<T> _linkToGenericContext;
        private readonly Type _declaringType;
        private RegisterRuleChain RuleChainRef { get; }

        public LinkToCorrectnessContext(CorrectnessContext<T> context)
        {
            _declaringType = typeof(T);
            _linkToGenericContext = context ?? throw new ArgumentNullException(nameof(context));
            RuleChainRef = _linkToGenericContext.ExposeCorrectRuleChain();
        }

        public IValidationEntry SetStrategy<TStrategy>(StrategyMode mode = StrategyMode.OverallOverwrite) where TStrategy : class, IValidationStrategy, new()
        {
            RuleChainRef.RegisterStrategy<TStrategy>(mode);
            return this;
        }

        public IValidationEntry SetStrategy<TStrategy>(TStrategy strategy, StrategyMode mode = StrategyMode.OverallOverwrite) where TStrategy : class, IValidationStrategy, new()
        {
            RuleChainRef.RegisterStrategy(strategy, mode);
            return this;
        }

        public IValidationEntry ForMember(string name, Func<IValueRuleBuilder, IValueRuleBuilder> func)
        {
            RuleChainRef.RegisterMember(_declaringType, name, func);
            return this;
        }

        public IValidationEntry ForMember(PropertyInfo propertyInfo, Func<IValueRuleBuilder, IValueRuleBuilder> func)
        {
            RuleChainRef.RegisterMember(_declaringType, propertyInfo, func);
            return this;
        }

        public IValidationEntry ForMember(FieldInfo fieldInfo, Func<IValueRuleBuilder, IValueRuleBuilder> func)
        {
            RuleChainRef.RegisterMember(_declaringType, fieldInfo, func);
            return this;
        }

        public bool StrictMode
        {
            get => _linkToGenericContext.StrictMode;
            set => _linkToGenericContext.StrictMode = value;
        }
    }
}