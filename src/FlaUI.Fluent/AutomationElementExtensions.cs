using FlaUI.Core.AutomationElements;

namespace FlaUI.Fluent
{
    public static class AutomationElementExtensions
    {
        public static FindBuilder BuildFind(this AutomationElement element)
        {
            return new FindBuilder(element);
        }
    }
}
