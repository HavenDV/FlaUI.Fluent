using System;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using FlaUI.Fluent;
using FlaUI.TestUtilities;
using FlaUI.UIA3;
using NUnit.Framework;

namespace RationalWill.UiTests
{
    [TestFixture]
    public class RationalWillTests : FlaUITestBase
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
            return Application.Launch(@"C:\Users\haven-desktop\Downloads\Rational Will - Debug\Rational Will.exe");
        }

        #endregion

        [Test]
        public void RunYoutubeTest()
        {
            var window = Application.WaitMainWindow(
                Automation, 
                builder => builder.Nested().ByAutomationId("MainWindowRibbon"), 
                Timeout);

            // Continue Trial If Required
            window
                .BuildFind().Nested().ByName("Trial Mode").Retry(Timeout).FirstOrDefault()?
                .BuildFind().Nested().ByName("Continue Trial").FirstOrDefault()?
                .AsButton()?
                .Invoke();

            // Select Decision Matrix
            window
                .BuildFind().Descendant().ByName("Decision Matrix").Retry(Timeout).First()
                .AsButton()
                .Invoke();

            // Select Identify Your Objectives
            window
                .BuildFind().Descendant().ByName("Identify your Objectives").Parent().Retry(Timeout).First()
                .AsButton()
                .Invoke();

            AddObjective(window, "Maximize", "Safety", true);
            AddObjective(window, "Maximize", "Comfort", true);
            AddObjective(window, "Minimize", "Cost", false);

            // Select Value By Slider
            {
                var control = window
                    .BuildFind().Descendant().ByAutomationId("ObjectiveTradeOffControl").Retry(Timeout).First();

                var completedCheckBox = control
                    .BuildFind().Nested().ByAutomationId("ChkPairCompletionStatus").First()
                    .AsCheckBox();
                var slider = control
                    .BuildFind().Nested().ByAutomationId("ControlValueSlider").First()
                    .AsSlider();
                slider.Value = 75;
                completedCheckBox.IsChecked = true;

                var transitivityCheckBox = control
                    .BuildFind().Nested().ByName("Enforce Transitivity Rule").First()
                    .AsCheckBox();
                transitivityCheckBox.IsChecked = false;
                // It seems you are processing the click incorrectly. Need to use the CheckedChanged event
                // These actions let you execute your code.
                transitivityCheckBox.Click();
                transitivityCheckBox.Click();

                var nextButton = control
                    .BuildFind().Nested().ByAutomationId("btnNext").First()
                    .AsButton();
                nextButton.Invoke();
                nextButton.Invoke();

                slider.Value = 12;
                completedCheckBox.IsChecked = true;

                control
                    .BuildFind().Nested().ByAutomationId("btnPrevious").First()
                    .AsButton()
                    .Invoke();

                slider.Value = 25;
                completedCheckBox.IsChecked = true;

                var comboBox = control
                    .BuildFind().Nested().ByAutomationId("cmbPairComparison").First()
                    .AsComboBox();
                comboBox.Select(2);

                var сonsistencyRatioTextBlock = window
                    .BuildFind().Descendant().ByAutomationId("aId_lblConsistencyRatio").Retry(Timeout).First();

                var columnChart = control
                    .BuildFind().Nested().ByAutomationId("columnChart").First();
                columnChart.RightClick();

                window
                    .BuildFind().Nested().ByControlType(ControlType.Window).Retry(Timeout).First()
                    .BuildFind().Descendant().ByName("Show Data Labels").First()
                    .AsMenuItem()
                    .Invoke();

                columnChart
                    .BuildFind().Descendant().ByName("45.4").Retry(Timeout).First();
                columnChart
                    .BuildFind().Descendant().ByName("22.54").First();
                columnChart
                    .BuildFind().Descendant().ByName("32.06").First();

                window
                    .BuildFind().Descendant().ByName("Proceed").First()
                    .AsButton()
                    .Invoke();
            }

            // Add options
            {
                var control = window
                    .BuildFind().Descendant().ByAutomationId("txtAddNew").Parent().Retry(Timeout).First();

                control
                    .BuildFind().Nested().ByAutomationId("txtAddNew").First()
                    .AsTextBox()
                    .Enter("Car 1");

                control
                    .BuildFind().Nested().ByControlType(ControlType.Button).First()
                    .AsButton()
                    .Invoke();

                control
                    .BuildFind().Nested().ByAutomationId("txtAddNew").First()
                    .AsTextBox()
                    .Enter("Car 2");

                control
                    .BuildFind().Nested().ByName("Finish").First()
                    .AsButton()
                    .Invoke();
            }

            // Select options values
            {
                var control = window
                    .BuildFind().Descendant().ByAutomationId("OptionPairComparisonControl").Parent().Retry(Timeout).First();

                var comparisonControl = control
                    .BuildFind().Nested().ByAutomationId("OptionPairComparisonControl").First();

                var slider = comparisonControl
                    .BuildFind().Nested().ByAutomationId("ControlValueSlider").First()
                    .AsSlider();
                slider.Value = 10;

                control
                    .BuildFind().Nested().ByName("Next").First()
                    .AsButton()
                    .Invoke();

                slider.Value = 12;

                control
                    .BuildFind().Nested().ByName("Next").First()
                    .AsButton()
                    .Invoke();

                slider.Value = 16;

                var columnChart = control
                    .BuildFind().Nested().ByAutomationId("columnChart").First();
                columnChart.RightClick();

                window
                    .BuildFind().Nested().ByControlType(ControlType.Window).Retry(Timeout).First()
                    .BuildFind().Descendant().ByName("Show Data Labels").First()
                    .AsMenuItem()
                    .Invoke();

                var thumb = slider
                    .BuildFind().Nested().ByControlType(ControlType.Thumb).First()
                    .AsThumb();
                thumb.SlideHorizontally(10);
                thumb.SlideHorizontally(-10);

                columnChart
                    .BuildFind().Descendant().ByName("16").Retry(Timeout).First();
                columnChart
                    .BuildFind().Descendant().ByName("84").First();

                control
                    .BuildFind().Nested().ByName("Finish").First()
                    .AsButton()
                    .Invoke();
            }

            // Check column chart
            {
                var control = window
                    .BuildFind().Descendant().ByAutomationId("ResultPanelTabControl").Retry(Timeout).First();

                var columnChart = control
                    .BuildFind().Descendant().ByAutomationId("columnChart").First();
                columnChart.RightClick();

                window
                    .BuildFind().Nested().ByControlType(ControlType.Window).Retry(Timeout).First()
                    .BuildFind().Descendant().ByName("Show Data Labels").First()
                    .AsMenuItem()
                    .Invoke();

                columnChart
                    .BuildFind().Descendant().ByName("60.9").Retry(Timeout).First();
                columnChart
                    .BuildFind().Descendant().ByName("39.1").First();
            }

            // Check recommendation
            {
                var recommendation = window
                    .BuildFind().Descendant().ByAutomationId("OptionsAnalyzerControl").Retry(Timeout).First()
                    .BuildFind().Descendant().ByName("Recommendation").First()
                    .BuildFind().Nested().ByControlType(ControlType.Text).First()
                    .Name;

                Assert.That(recommendation, Is.EqualTo("Car 1"));
            }

            // Check objectives
            {
                var ribbon = window
                    .BuildFind().Descendant().ByAutomationId("MainWindowRibbon").Retry(Timeout).First();

                ribbon
                    .BuildFind().Nested().ByName("View").First()
                    .Click();

                ribbon
                    .BuildFind().Descendant().ByAutomationId("objectives").Retry(Timeout).First()
                    .Click();

                var listView = window
                    .BuildFind().Descendant().ByAutomationId("LvObjectiveItems").Retry(Timeout).First();

                listView
                    .BuildFind().Descendant().ByName("45.4%").First();
                listView
                    .BuildFind().Descendant().ByName("22.54%").First();
                listView
                    .BuildFind().Descendant().ByName("32.06%").First();
            }

            //Thread.Sleep(TimeSpan.FromMinutes(11));

            // Close
            window
                .BuildFind().Nested().ByAutomationId("CloseButton").First()
                .AsButton()
                .Invoke();

            window
                .BuildFind().Nested().ByName("Rational Will").Retry(Timeout).First()
                .BuildFind().Nested().ByAutomationId("btnNo").First()
                .AsButton()
                .Invoke();
        }

        private void AddObjective(AutomationElement window, string action, string name, bool addAnother)
        {
            // Select action and enter name
            {
                var control = window
                    .BuildFind().Descendant().ByAutomationId("AddObjectiveUserControl").Child().Retry(Timeout).First();

                var comboBox = control
                    .BuildFind().Nested().ByAutomationId("CmbAttribute").First()
                    .AsComboBox();
                comboBox.Select(action);
                comboBox.Collapse();

                control
                    .BuildFind().Nested().ByAutomationId("TxtObjectiveName").First()
                    .AsTextBox()
                    .Enter(name);

                control
                    .BuildFind().Nested().ByName("Proceed").First()
                    .AsButton()
                    .Invoke();
            }

            // Select Subjective Type
            window
                .BuildFind().Descendant().ByName("Subjective Type").Parent().Retry(Timeout).First()
                .AsButton()
                .Invoke();

            // Select Yes or No in the next dialog: "Do you want to add another objective?"
            window
                .BuildFind().Descendant().ByName("Do you want to add another objective?").Parent().Retry(Timeout).First()
                .BuildFind().Nested().ByName(addAnother ? "Yes" : "No").First()
                .AsButton()
                .Invoke();
        }
    }
}
