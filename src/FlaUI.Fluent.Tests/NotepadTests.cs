using System;
using System.Diagnostics;
using System.Threading;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.TestUtilities;
using FlaUI.UIA3;
using NUnit.Framework;

namespace FlaUI.Fluent.Tests
{
    [TestFixture]
    public class NotepadTests : FlaUITestBase
    {
        #region Properties

        public TimeSpan Timeout { get; } = TimeSpan.FromSeconds(3);

        #endregion

        #region Overrides

        protected override AutomationBase GetAutomation()
        {
            return new UIA3Automation();
        }

        protected override Application StartApplication()
        {
            return Application.Launch(new ProcessStartInfo("notepad.exe")
            {
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Maximized,
        });
        }

        #endregion

        [Test]
        public void SimpleTest()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            var window = Application.WaitMainWindow(
                Automation, 
                builder => builder, 
                Timeout);

            // Set text
            window
                .BuildFind().AmongChildren().ByAutomationId("15").Retry(Timeout).First()
                .Patterns
                .Value
                .Pattern
                .SetValue("Hello, World!");

            // Open File menu
            window
                .BuildFind().AmongChildren().ByAutomationId("MenuBar").Retry(Timeout).Child().First()
                .AsMenuItem()
                .Click();

            // Invoke Exit
            window
                .BuildFind().AmongDescendants().ByAutomationId("7").Retry(Timeout).First()
                .AsMenuItem()
                .Invoke();

            // Discard Changes
            window
                .BuildFind().AmongDescendants().ByAutomationId("CommandButton_7").Retry(Timeout).First()
                .AsButton()
                .Invoke();
        }
    }
}
