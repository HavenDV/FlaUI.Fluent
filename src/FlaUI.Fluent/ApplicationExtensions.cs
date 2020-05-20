using System;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;

namespace FlaUI.Fluent
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="automation"></param>
        /// <param name="func"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="automation"></param>
        /// <param name="func"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
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
