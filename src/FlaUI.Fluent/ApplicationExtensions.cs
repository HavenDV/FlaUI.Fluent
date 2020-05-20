using System;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;

namespace FlaUI.Fluent
{
    public static class ApplicationExtensions
    {
        public static Window WaitMainWindow(
            this Application application, 
            AutomationBase automation, 
            Func<FindBuilder, FindBuilder> func, 
            RetrySettings settings)
        {
            Window? window = null;

            var element = Retry.Find(() =>
            {
                window = application.GetMainWindow(automation, Retry.DefaultInterval);

                return window == null
                    ? null
                    : func(window.BuildFind()).FirstOrDefault();
            }, settings);

            window = window ?? throw new InvalidOperationException("Main window is null");

            // Throws right exception
            if (element == null)
            {
                func(window.BuildFind()).First();
            }

            return window;
        }

        public static Window WaitMainWindow(
            this Application application, 
            AutomationBase automation, 
            Func<FindBuilder, FindBuilder> func, 
            TimeSpan? timeout = null)
        {
            return application.WaitMainWindow(automation, func, new RetrySettings
            {
                Timeout = timeout,
            });
        }
    }
}
