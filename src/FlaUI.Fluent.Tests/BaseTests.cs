using System;
using System.Diagnostics;
using FlaUI.Core;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using NUnit.Framework;

namespace FlaUI.Fluent.Tests
{
    public class BaseTests
    {
        #region Properties

        private AutomationBase? AutomationPrivate { get; set; }
        private Application? ApplicationPrivate { get; set; }

        protected TimeSpan Timeout { get; } = TimeSpan.FromSeconds(3);

        protected AutomationBase Automation => AutomationPrivate ?? 
                                               throw new InvalidOperationException("Automation is null");

        protected Application Application => ApplicationPrivate ??
                                             throw new InvalidOperationException("Application is null");

        protected ProcessStartInfo ProcessStartInfo { get; }

        #endregion

        #region Constructors

        protected BaseTests(ProcessStartInfo processStartInfo)
        {
            ProcessStartInfo = processStartInfo ?? throw new ArgumentNullException(nameof(processStartInfo));
        }

        protected BaseTests(string fileName)
        {
            fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

            ProcessStartInfo = new ProcessStartInfo(fileName)
            {
                UseShellExecute = true,
            };
        }

        #endregion

        #region SetUp/TearDown

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            AutomationPrivate = GetAutomation();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Automation.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            ApplicationPrivate = StartApplication();
        }

        /// <summary>
        /// Teardown method for each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            CloseApplication();
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        private void CloseApplication()
        {
            if (Application == null)
            {
                return;
            }

            Application.Close();
            // ReSharper disable once AccessToDisposedClosure
            Retry.WhileFalse(() => Application.HasExited, TimeSpan.FromSeconds(2), ignoreException: true);

            Application.Dispose();
        }

        #endregion

        #region Virtual

        protected virtual AutomationBase GetAutomation()
        {
            return new UIA3Automation();
        }

        protected virtual Application StartApplication()
        {
            return Application.Launch(ProcessStartInfo);
        }

        #endregion
    }
}
