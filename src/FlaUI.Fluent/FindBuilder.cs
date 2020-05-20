using System;
using System.Collections.Generic;
using System.Linq;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;

namespace FlaUI.Fluent
{
    public class FindBuilder
    {
        #region Properties

        private AutomationElement Element { get; }
        private FindType Type { get; set; } = FindType.Nested;
        private RetrySettings? RetrySettings { get; set; }

        private List<string> Modifiers { get; } = new List<string>();
        private List<ConditionBase> Conditions { get; } = new List<ConditionBase>();
        private List<ActionType> Actions { get; } = new List<ActionType>();

        #endregion

        #region Events

        public event EventHandler<(FindBuilder builder, AutomationElement? element)>? FindOccurred;

        private void OnFindOccurred((FindBuilder builder, AutomationElement? element) value)
        {
            FindOccurred?.Invoke(this, value);
        }

        #endregion

        #region Constructors

        public FindBuilder(AutomationElement element)
        {
            Element = element ?? throw new ArgumentNullException(nameof(element));
        }

        #endregion

        #region Methods

        #region Type

        public FindBuilder Nested()
        {
            Type = FindType.Nested;

            return this;
        }

        public FindBuilder Descendant()
        {
            Type = FindType.Descendant;

            return this;
        }

        #endregion

        #region Modifiers

        #region By

        public FindBuilder By(Func<ConditionFactory, ConditionBase> func)
        {
            Conditions.Add(func(Element.ConditionFactory));

            return this;
        }

        public FindBuilder ByName(string value)
        {
            Modifiers.Add($"Name: {value}");

            return By(factory => factory.ByName(value));
        }

        public FindBuilder ByClassName(string value)
        {
            Modifiers.Add($"ClassName: {value}");

            return By(factory => factory.ByClassName(value));
        }

        public FindBuilder ByHelpText(string value)
        {
            Modifiers.Add($"HelpText: {value}");

            return By(factory => factory.ByHelpText(value));
        }

        public FindBuilder ByAutomationId(string value)
        {
            Modifiers.Add($"AutomationId: {value}");

            return By(factory => factory.ByAutomationId(value));
        }

        public FindBuilder ByText(string value)
        {
            Modifiers.Add($"Text: {value}");

            return By(factory => factory.ByText(value));
        }

        public FindBuilder ByControlType(ControlType value)
        {
            Modifiers.Add($"ControlType: {value}");

            return By(factory => factory.ByControlType(value));
        }

        #endregion

        public FindBuilder Parent()
        {
            Modifiers.Add("Action: Parent");

            Actions.Add(ActionType.Parent);

            return this;
        }

        public FindBuilder Child()
        {
            Modifiers.Add("Action: Child");

            Actions.Add(ActionType.Child);

            return this;
        }

        #endregion

        #region Retry

        public FindBuilder Retry(RetrySettings settings)
        {
            RetrySettings = settings;

            return this;
        }

        public FindBuilder Retry(TimeSpan timeout)
        {
            return Retry(new RetrySettings
            {
                Timeout = timeout,
            });
        }

        #endregion

        #region End

        private AutomationElement? FirstInternal()
        {
            return Type switch
            {
                FindType.Nested => Element.FindFirstNested(Conditions.ToArray()),
                FindType.Descendant => Element.FindFirstDescendant(Conditions.Single()),
                _ => throw new InvalidOperationException($"Unknown type: {Type}")
            };
        }

        public AutomationElement? FirstOrDefault()
        {
            var value = RetrySettings != null
                ? Core.Tools.Retry.Find(FirstInternal, RetrySettings)
                : FirstInternal();

            foreach (var type in Actions)
            {
                switch (type)
                {
                    case ActionType.Parent:
                        value = value?.Parent;
                        break;

                    case ActionType.Child:
                        value = value?.FindFirstChild();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            OnFindOccurred((this, value));

            return value;
        }

        public AutomationElement First()
        {
            return FirstOrDefault() ?? 
                   throw new InvalidOperationException($@"FindBuilder.First() returned null. {this}");
        }

        #endregion

        public override string ToString()
        {
            return $@"FindBuilder.Modifiers:
{string.Join(Environment.NewLine, Modifiers.Select((modifier, i) => $"{i + 1}. {modifier}"))}";
        }

        #endregion

        #region Enums

        public enum FindType
        {
            Nested,
            Descendant,
        }

        public enum ActionType
        {
            Parent,
            Child,
        }

        #endregion
    }
}
