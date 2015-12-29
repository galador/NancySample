using System.Collections.Generic;
using Nancy.ViewEngines.Razor;

namespace NancySample
{
    public class RazorConfig : IRazorConfiguration
    {
        public IEnumerable<string> GetAssemblyNames()
        {
            yield return "MiniProfiler";
            yield return "System.Web";
            yield return "System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            yield return "StackExchange.Profiling";
        }

        public bool AutoIncludeModelNamespace => true;
    }
}
