using System.Configuration;
using System.Diagnostics;
using System.Dynamic;
using Nancy.Linker;
using StackExchange.Profiling;

namespace NancySample.Modules.Application
{
    public class ApplicationModel
    {
        public string SiteName => ConfigurationManager.AppSettings["siteName"];
        public string LayoutPath => "Modules/Application/Views/_Layout.cshtml";
        public bool IsTest => Debugger.IsAttached;
        public MiniProfiler Profiler { get; set; }
        public string ProfilerIncludes { get; set; }
        public string PageTitle { get; set; }

        public string Title
        {
            get
            {
                var subTitle = string.IsNullOrWhiteSpace(PageTitle) ? "" : (" - " + PageTitle);
                return $"{SiteName}{subTitle}";
            }
        }

        public IResourceLinker Linker { get; set; }
    }
}
