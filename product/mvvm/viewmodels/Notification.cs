using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using MVPtoMVVM.utility;

namespace MVPtoMVVM.mvvm.viewmodels
{
    public class Notification<T> : IDataErrorInfo
    {
        IDictionary<string, IList<IRule>> validationRules = new Dictionary<string, IList<IRule>>();

        public Notification<T> Register<Severity>(Expression<Func<T, object>> property, Func<bool> failCondition, Func<string> errorMessage) where Severity : ISeverity, new()
        {
            return Register(property, new AnonymousRule<Severity>(failCondition, errorMessage));
        }

        public Notification<T> Register(Expression<Func<T, object>> property, IRule rule)
        {
            EnsureRulesAreInitializeFor(property);
            validationRules[property.pick_property().Name].Add(rule);
            return this;
        }

        public string this[Expression<Func<T, object>> property] { get { return this[property. pick_property().Name]; } }
        public string this[string propertyName]
        {
            get
            {
                if (!validationRules.ContainsKey(propertyName)) return null;
                var validationRulesForProperty = validationRules[propertyName];
                return validationRulesForProperty.Any(x => x.IsViolated())
                           ? BuildErrorsFor(validationRulesForProperty)
                           : null;
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public bool AreAnyRulesViolatedAndMoreSevereThan<Severity>() where Severity : ISeverity, new()
        {
            return validationRules.Any(validationRule => validationRule.Value.Any(x => x.IsViolatedAndMoreSevereThan<Severity>()));
        }

        void EnsureRulesAreInitializeFor(Expression<Func<T, object>> property)
        {
            if (!validationRules.ContainsKey(property. pick_property().Name))
                validationRules[property.pick_property().Name] = new List<IRule>();
        }

        string BuildErrorsFor(IEnumerable<IRule> validationRulesForProperty)
        {
            var errors = new List<string>();
            validationRulesForProperty.each(x =>
            {
                if (x.IsViolated()) errors.Add(x.ErrorMessage);
            });
            return string.Join(Environment.NewLine, errors.ToArray());
        }
    }
    public interface IRule
    {
        bool IsViolated();
        string ErrorMessage { get; }
        bool IsViolatedAndMoreSevereThan<Severity>() where Severity : ISeverity, new();
    }

    public interface ISeverity
    {
        bool IsMoreSevereThan<OtherSeverity>(OtherSeverity otherSeverity) where OtherSeverity : ISeverity;
    }

    public class Warning : ISeverity
    {
        public bool IsMoreSevereThan<OtherSeverity>(OtherSeverity otherSeverity) where OtherSeverity : ISeverity
        {
            return !(otherSeverity is Error);
        }
    }

    public class Error : ISeverity
    {
        public bool IsMoreSevereThan<OtherSeverity>(OtherSeverity otherSeverity) where OtherSeverity : ISeverity
        {
            return true;
        }
    }

    public class AnonymousRule<Severity> : IRule where Severity : ISeverity, new()
    {
        readonly Func<bool> failCondition;
        readonly Func<string> errorMessage;

        public AnonymousRule(Func<bool> failCondition, Func<string> errorMessage)
        {
            this.failCondition = failCondition;
            this.errorMessage = errorMessage;
        }

        public string ErrorMessage
        {
            get { return errorMessage(); }
        }

        public bool IsViolatedAndMoreSevereThan<OtherSeverity>() where OtherSeverity : ISeverity, new()
        {
            return IsViolated() && new Severity().IsMoreSevereThan(new OtherSeverity());
        }

        public bool IsViolated()
        {
            return failCondition();
        }
    }
}