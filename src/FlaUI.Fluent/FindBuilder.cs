using System;
using System.Collections.Generic;
using System.Linq;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;

namespace FlaUI.Fluent
{
    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<(FindBuilder builder, AutomationElement? element)>? FindOccurred;

        private void OnFindOccurred((FindBuilder builder, AutomationElement? element) value)
        {
            FindOccurred?.Invoke(this, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public FindBuilder(AutomationElement element)
        {
            Element = element ?? throw new ArgumentNullException(nameof(element));
        }

        #endregion

        #region Methods

        #region Type

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FindBuilder Nested()
        {
            Type = FindType.Nested;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FindBuilder Descendant()
        {
            Type = FindType.Descendant;

            return this;
        }

        #endregion

        #region Modifiers

        #region By

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public FindBuilder By(Func<ConditionFactory, ConditionBase> func)
        {
            Conditions.Add(func(Element.ConditionFactory));

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FindBuilder ByName(string value)
        {
            Modifiers.Add($"Name: {value}");

            return By(factory => factory.ByName(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FindBuilder ByClassName(string value)
        {
            Modifiers.Add($"ClassName: {value}");

            return By(factory => factory.ByClassName(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FindBuilder ByHelpText(string value)
        {
            Modifiers.Add($"HelpText: {value}");

            return By(factory => factory.ByHelpText(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FindBuilder ByAutomationId(string value)
        {
            Modifiers.Add($"AutomationId: {value}");

            return By(factory => factory.ByAutomationId(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FindBuilder ByText(string value)
        {
            Modifiers.Add($"Text: {value}");

            return By(factory => factory.ByText(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FindBuilder ByControlType(ControlType value)
        {
            Modifiers.Add($"ControlType: {value}");

            return By(factory => factory.ByControlType(value));
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FindBuilder Parent()
        {
            Modifiers.Add("Action: Parent");

            Actions.Add(ActionType.Parent);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FindBuilder Child()
        {
            Modifiers.Add("Action: Child");

            Actions.Add(ActionType.Child);

            return this;
        }

        #endregion

        #region Retry

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public FindBuilder Retry(RetrySettings settings)
        {
            RetrySettings = settings;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AutomationElement First()
        {
            return FirstOrDefault() ?? 
                   throw new InvalidOperationException($@"FindBuilder.First() returned null. {this}");
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $@"FindBuilder.Modifiers:
{string.Join(Environment.NewLine, Modifiers.Select((modifier, i) => $"{i + 1}. {modifier}"))}";
        }

        #endregion

        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum FindType
        {
            /// <summary>
            /// 
            /// </summary>
            Nested,

            /// <summary>
            /// 
            /// </summary>
            Descendant,
        }

        /// <summary>
        /// 
        /// </summary>
        public enum ActionType
        {
            /// <summary>
            /// 
            /// </summary>
            Parent,

            /// <summary>
            /// 
            /// </summary>
            Child,
        }

        #endregion
    }
}
