using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Cosmos.Reflection.ObjectVisitors.Internals;
using Cosmos.Validation;
using Cosmos.Validation.Strategies;

namespace Cosmos.Reflection.ObjectVisitors.Correctness
{
    internal class CorrectnessContext<T> : IValidationEntry<T>
    {
        private readonly ICoreVisitor<T> _visitor;

        public CorrectnessContext(ICoreVisitor<T> visitor, bool strictMode)
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            Options = CorrectnessOptions.Copy();
            StrictMode = strictMode;
            CorrectRuleChain = new();
        }

        public bool StrictMode { get; set; }

        public ValidationOptions Options { get; }

        #region CorrectRuleChain

        private RegisterRuleChain CorrectRuleChain { get; set; }

        internal RegisterRuleChain ExposeCorrectRuleChain() => CorrectRuleChain;

        #endregion

        #region Register

        public IValidationEntry<T> SetStrategy<TStrategy>(StrategyMode mode = StrategyMode.OverallOverwrite) where TStrategy : class, IValidationStrategy<T>, new()
        {
            CorrectRuleChain.RegisterStrategy<TStrategy, T>(mode);
            _needToBuild = true;
            return this;
        }

        public IValidationEntry<T> SetStrategy<TStrategy>(TStrategy strategy, StrategyMode mode = StrategyMode.OverallOverwrite) where TStrategy : class, IValidationStrategy<T>, new()
        {
            if (strategy is null) throw new ArgumentNullException(nameof(strategy));
            CorrectRuleChain.RegisterStrategy<TStrategy, T>(strategy, mode);
            _needToBuild = true;
            return this;
        }

        public IValidationEntry<T> SetRulePackage(VerifyRulePackage package, VerifyRuleMode mode = VerifyRuleMode.Append)
        {
            if (package is null)
                throw new ArgumentNullException(nameof(package));
            CorrectRuleChain.RegisterRulePackage<T>(package, mode);
            _needToBuild = true;
            return this;
        }

        public IValidationEntry<T> SetMemberRulePackage(string memberName, VerifyMemberRulePackage package, VerifyRuleMode mode = VerifyRuleMode.Append)
        {
            if (package is null)
                throw new ArgumentNullException(nameof(package));
            CorrectRuleChain.RegisterMemberRulePackage<T>(memberName, package, mode);
            _needToBuild = true;
            return this;
        }

        public IValidationEntry<T> SetMemberRulePackage<TVal>(Expression<Func<T, TVal>> expression, VerifyMemberRulePackage package, VerifyRuleMode mode = VerifyRuleMode.Append)
        {
            if (package is null)
                throw new ArgumentNullException(nameof(package));
            CorrectRuleChain.RegisterMemberRulePackage(expression, package, mode);
            _needToBuild = true;
            return this;
        }

        public IValidationEntry<T> ForMember(string name, Func<IValueRuleBuilder<T>, IValueRuleBuilder<T>> func)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            CorrectRuleChain.RegisterMember(name, func);
            _needToBuild = true;
            return this;
        }

        public IValidationEntry<T> ForMember(PropertyInfo propertyInfo, Func<IValueRuleBuilder<T>, IValueRuleBuilder<T>> func)
        {
            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));
            CorrectRuleChain.RegisterMember(propertyInfo, func);
            _needToBuild = true;
            return this;
        }

        public IValidationEntry<T> ForMember(FieldInfo fieldInfo, Func<IValueRuleBuilder<T>, IValueRuleBuilder<T>> func)
        {
            if (fieldInfo is null)
                throw new ArgumentNullException(nameof(fieldInfo));
            CorrectRuleChain.RegisterMember(fieldInfo, func);
            _needToBuild = true;
            return this;
        }

        public IValidationEntry<T> ForMember<TVal>(Expression<Func<T, TVal>> expression, Func<IValueRuleBuilder<T, TVal>, IValueRuleBuilder<T, TVal>> func)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));
            CorrectRuleChain.RegisterMember(expression, func);
            _needToBuild = true;
            return this;
        }

        #endregion

        #region Handler

        private bool _hasBeenBuilt;
        private bool _needToBuild;

        private ValidationHandler _handler;

        protected ValidationHandler ValidationHandler
        {
            get
            {
                if (_needToBuild || !_hasBeenBuilt)
                {
                    _handler = CorrectRuleChain.Build(Options);
                    _hasBeenBuilt = true;
                    _needToBuild = false;
                }

                return _handler;
            }
        }

        #endregion

        #region VerifyRulePackage

        public VerifyRulePackage ExposeRulePackage()
        {
            if (ValidationHandler is null)
                return VerifyRulePackage.Empty;
            return ValidationHandler.ExposeRulePackage(_visitor.SourceType);
        }

        public VerifyMemberRulePackage ExposeMemberRulePackage(string memberName)
        {
            if (ValidationHandler is null)
                return VerifyMemberRulePackage.Empty;
            return ValidationHandler.ExposeMemberRulePackage(_visitor.SourceType, memberName);
        }

        #endregion

        #region Verify

        public VerifyResult Verify(bool withGlobalRules, string withGlobalProviderName = "")
        {
            if (ValidationHandler is null)
                return VerifyResult.Success;
            var master = ValidationHandler.Verify(_visitor.SourceType, _visitor.Instance);
            var slave = withGlobalRules
                ? ValidationMe.Resolve<T>(withGlobalProviderName).Verify(_visitor.Instance)
                : VerifyResult.Success;
            return VerifyResult.Merge(master, slave);
        }

        public VerifyResult VerifyOne(string memberName, bool withGlobalRules, string withGlobalProviderName = "")
        {
            if (ValidationHandler is null)
                return VerifyResult.Success;
            var value = _visitor.ExposeLazyMemberHandler().Value.GetValueObject(memberName);
            var master = ValidationHandler.VerifyOne(_visitor.SourceType, value, memberName);
            var slave = withGlobalRules
                ? ValidationMe.Resolve<T>(withGlobalProviderName).VerifyOne(value, memberName)
                : VerifyResult.Success;
            return VerifyResult.Merge(master, slave);
        }

        public VerifyResult VerifyOne(string memberName, object value, bool withGlobalRules, string withGlobalProviderName = "")
        {
            if (ValidationHandler is null)
                return VerifyResult.Success;
            var master = ValidationHandler.VerifyOne(_visitor.SourceType, value, memberName);
            var slave = withGlobalRules
                ? ValidationMe.Resolve<T>(withGlobalProviderName).VerifyOne(value, memberName)
                : VerifyResult.Success;
            return VerifyResult.Merge(master, slave);
        }

        public VerifyResult VerifyMany(IDictionary<string, object> keyValueCollections, bool withGlobalRules, string withGlobalProviderName = "")
        {
            if (ValidationHandler is null)
                return VerifyResult.Success;
            var master = ValidationHandler.VerifyMany(_visitor.SourceType, keyValueCollections);
            var slave = withGlobalRules
                ? ValidationMe.Resolve<T>(withGlobalProviderName).VerifyMany(_visitor.SourceType, keyValueCollections)
                : VerifyResult.Success;
            return VerifyResult.Merge(master, slave);
        }

        #endregion
    }
}