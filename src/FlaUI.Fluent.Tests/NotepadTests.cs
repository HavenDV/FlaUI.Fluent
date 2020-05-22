using System;
using System.Diagnostics;
using System.Threading;
using FlaUI.Core.AutomationElements;
using NUnit.Framework;

namespace FlaUI.Fluent.Tests
{
    [TestFixture]
    public class NotepadTests : BaseTests
    {
        #region Constructos

        public NotepadTests() : base("notepad.exe")
        {
            ProcessStartInfo.WindowStyle = ProcessWindowStyle.Maximized;
        }

        #endregion

        [Test]
        public void SimpleTest()
        {
#if !NETFRAMEWORK
            Thread.Sleep(TimeSpan.FromSeconds(1));
#endif

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
