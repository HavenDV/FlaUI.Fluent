# [FlaUI.Fluent](https://github.com/HavenDV/FlaUI.Fluent/) 

[![Language](https://img.shields.io/badge/language-C%23-blue.svg?style=flat-square)](https://github.com/HavenDV/FlaUI.Fluent/search?l=C%23&o=desc&s=&type=Code) 
[![Requirements](https://img.shields.io/badge/Requirements-.NET%20Standard%202.0-blue.svg)](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md) 
[![Requirements](https://img.shields.io/badge/Requirements-.NET%20Framework%204.5-blue.svg)]()

Fluent interface for FlaUI

### Example
```cs
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
}
```

### Used documentation
1. [FlaUI](https://github.com/FlaUI/FlaUI)
2. [FlaUI Examples](https://github.com/FlaUI/FlaUI/blob/master/src/FlaUI.Core.UITests)
3. [Accessing Standard UIA Properties using FlaUI](https://www.youtube.com/watch?v=EOKPiLykNVE)
4. [FlaUInspect binaries](https://github.com/FlaUI/FlaUInspect/releases)

### Contacts
* [mail](mailto:havendv@gmail.com)
