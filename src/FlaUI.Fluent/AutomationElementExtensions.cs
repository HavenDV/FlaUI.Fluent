using FlaUI.Core.AutomationElements;

namespace FlaUI.Fluent
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutomationElementExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static FindBuilder BuildFind(this AutomationElement element)
        {
            return new FindBuilder(element);
        }
    }
}
