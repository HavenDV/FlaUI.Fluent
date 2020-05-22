# [FlaUI.Fluent](https://github.com/HavenDV/FlaUI.Fluent/) 

[![Language](https://img.shields.io/badge/language-C%23-blue.svg?style=flat-square)](https://github.com/HavenDV/FlaUI.Fluent/search?l=C%23&o=desc&s=&type=Code) 
[![Requirements](https://img.shields.io/badge/Requirements-.NET%20Standard%202.0-blue.svg)](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md) 
[![Requirements](https://img.shields.io/badge/Requirements-.NET%20Framework%204.5-blue.svg)]()

Fluent interface for FlaUI find queries

### Notes
- `Application.WaitMainWindow` works correctly without `Thread.Sleep` before this only for **.Net Framework**

### Example
```cs
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

```

### Used documentation
1. [FlaUI](https://github.com/FlaUI/FlaUI)
2. [FlaUI Examples](https://github.com/FlaUI/FlaUI/blob/master/src/FlaUI.Core.UITests)
3. [Accessing Standard UIA Properties using FlaUI](https://www.youtube.com/watch?v=EOKPiLykNVE)
4. [FlaUInspect binaries](https://github.com/FlaUI/FlaUInspect/releases)

### Contacts
* [mail](mailto:havendv@gmail.com)
