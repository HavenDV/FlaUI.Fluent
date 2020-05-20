using System;
using System.Collections.Generic;
using System.Linq;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;

namespace FlaUI.Fluent
{
    /// <summary>
    /// Fluent class for AutomationElement
    /// </summary>
    public class FindBuilder
    {
        #region Properties

        private AutomationElement Element { get; }
        private TreeScope TreeScope { get; set; } = TreeScope.None;
        private RetrySettings? RetrySettings { get; set; }

        private List<string> Modifiers { get; } = new List<string>();
        private List<ConditionBase> Conditions { get; } = new List<ConditionBase>();
        private List<ActionType> Actions { get; } = new List<ActionType>();

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<IList<AutomationElement>>? FindOccurred;

        private void OnFindOccurred(IList<AutomationElement> value)
        {
            FindOccurred?.Invoke(this, value);
        }

        private void OnFindOccurred(AutomationElement? value)
        {
            OnFindOccurred(value != null
                ? new List<AutomationElement> { value }
                : new List<AutomationElement>());
        }

        #endregion

        #region Constructors

        internal FindBuilder(AutomationElement element)
        {
            Element = element ?? throw new ArgumentNullException(nameof(element));
        }

        #endregion

        #region Methods

        #region Type

        /// <summary>
        /// Sets scope.
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public FindBuilder Among(TreeScope scope)
        {
            TreeScope = scope;

            return this;
        }

        /// <summary>
        /// The scope includes the element itself.
        /// </summary>
        /// <returns></returns>
        public FindBuilder AmongElement()
        {
            return Among(TreeScope.Element);
        }

        /// <summary>
        /// The scope includes children of the element.
        /// </summary>
        /// <returns></returns>
        public FindBuilder AmongChildren()
        {
            return Among(TreeScope.Children);
        }

        /// <summary>
        /// The scope includes children and more distant descendants of the element.
        /// </summary>
        /// <returns></returns>
        public FindBuilder AmongDescendants()
        {
            return Among(TreeScope.Descendants);
        }

        /// <summary>
        /// The scope includes the element and all its descendants.
        /// </summary>
        /// <returns></returns>
        public FindBuilder AmongSubtree()
        {
            return Among(TreeScope.Subtree);
        }

        /// <summary>
        /// The scope includes the parent of the element.
        /// </summary>
        /// <returns></returns>
        public FindBuilder AmongParents()
        {
            return Among(TreeScope.Parent);
        }

        /// <summary>
        /// The scope includes the parent and more distant ancestors of the element.
        /// </summary>
        /// <returns></returns>
        public FindBuilder AmongAncestors()
        {
            return Among(TreeScope.Ancestors);
        }

        #endregion

        #region Modifiers

        #region By

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="modifierName"></param>
        /// <returns></returns>
        public FindBuilder By(Func<ConditionFactory, ConditionBase> func, string modifierName)
        {
            Modifiers.Add(modifierName);
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
            return By(factory => factory.ByName(value), $"{nameof(ByName)}: {value}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FindBuilder ByClassName(string value)
        {
            return By(factory => factory.ByClassName(value), $"{nameof(ByClassName)}: {value}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FindBuilder ByHelpText(string value)
        {
            return By(factory => factory.ByHelpText(value), $"{nameof(ByHelpText)}: {value}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FindBuilder ByValue(string value)
        {
            return By(factory => factory.ByValue(value), $"{nameof(ByValue)}: {value}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FindBuilder ByAutomationId(string value)
        {
            return By(factory => factory.ByAutomationId(value), $"{nameof(ByAutomationId)}: {value}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FindBuilder ByText(string value)
        {
            return By(factory => factory.ByText(value), $"{nameof(ByText)}: {value}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FindBuilder ByControlType(ControlType value)
        {
            return By(factory => factory.ByControlType(value), $"{nameof(ByControlType)}: {value}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FindBuilder ByLocalizedControlType(string value)
        {
            return By(factory => factory.ByLocalizedControlType(value), $"{nameof(ByLocalizedControlType)}: {value}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FindBuilder ByFrameworkId(string value)
        {
            return By(factory => factory.ByFrameworkId(value), $"{nameof(ByFrameworkId)}: {value}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FindBuilder ByProcessId(int value)
        {
            return By(factory => factory.ByProcessId(value), $"{nameof(ByProcessId)}: {value}");
        }

        #endregion

        #region Actions

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

        private AutomationElement[] AllInternal()
        {
            return TreeScope switch
            {
                TreeScope.Children => Element.FindAllNested(Conditions.ToArray()),
                TreeScope.Descendants => Element.FindAll(TreeScope, Conditions.Single()),
                TreeScope.Element => Element.FindAll(TreeScope, Conditions.Single()),
                TreeScope.Subtree => Element.FindAll(TreeScope, Conditions.Single()),
                TreeScope.Parent => Element.FindAll(TreeScope, Conditions.Single()),
                TreeScope.Ancestors => Element.FindAll(TreeScope, Conditions.Single()),
                TreeScope.None => new []{ Element },
                _ => throw new InvalidOperationException($"Unknown tree scope: {TreeScope}.")
            };
        }

        private AutomationElement? FirstInternal()
        {
            return TreeScope switch
            {
                TreeScope.Children => Element.FindFirstNested(Conditions.ToArray()),
                TreeScope.Descendants => Element.FindFirst(TreeScope, Conditions.Single()),
                TreeScope.Element => Element.FindFirst(TreeScope, Conditions.Single()),
                TreeScope.Subtree => Element.FindFirst(TreeScope, Conditions.Single()),
                TreeScope.Parent => Element.FindFirst(TreeScope, Conditions.Single()),
                TreeScope.Ancestors => Element.FindFirst(TreeScope, Conditions.Single()),
                TreeScope.None => Element,
                _ => throw new ArgumentOutOfRangeException($"Unknown tree scope: {TreeScope}.")
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AutomationElement[] ToArray()
        {
            var values = RetrySettings != null
                ? Core.Tools.Retry.Find(AllInternal, RetrySettings)
                : AllInternal();

            foreach (var type in Actions)
            {
                switch (type)
                {
                    case ActionType.Parent:
                        values = values.Select(value => value?.Parent).ToArray();
                        break;

                    case ActionType.Child:
                        values = values.Select(value => value?.FindFirstChild()).ToArray();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException($"Unknown action type: {type}.");
                }
            }

            OnFindOccurred(values);

            return values;
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
            
            OnFindOccurred(value);

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
