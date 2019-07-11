namespace Cockpit.Core.Plugins.MemoryMapping
{
    public static class ArgumentExtension
    {
        public static string Quote(this string input)
        {
            return "\"" + input + "\"";
        }
    }
}