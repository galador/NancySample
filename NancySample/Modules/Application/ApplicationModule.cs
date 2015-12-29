using System.Diagnostics;
using Nancy;
using Nancy.Linker;
using Nancy.Security;
using NancySample.Models;
using StackExchange.Profiling;

namespace NancySample.Modules.Application
{
    public class ApplicationModule : NancyModule
    {
        //Private variables
        private readonly IAppDbContext _db;
        private readonly IResourceLinker _linker;

        //Public properties
        public ApplicationModel AppModel = new ApplicationModel();
        public const string MasterLayoutPath = "";

        //Constructor
        public ApplicationModule(IAppDbContext dbCtx, IResourceLinker linker) 
        {
            //All pages on the site should be viewed through HTTPS
            //IIS Express puts HTTPS on 44300
            this.RequiresHttps(true, AppModel.IsTest ? 44300 : 443);
            
            //Populate injected interfaces.
            _db = dbCtx;
            _linker = linker;

            //Setup filters
            Before += AddSiteConfiguration;
            After += AssignAppModelToViewBag;
        }

        /*
         * Before Methods
         */
        private Response AddSiteConfiguration(NancyContext ctx)
        {
            AppModel.Profiler = MiniProfiler.Current;
            AppModel.ProfilerIncludes = MiniProfiler.RenderIncludes().ToHtmlString();
            AppModel.Linker = _linker;
            return null;
        }

        /*
         * After Methods
         */
        private void AssignAppModelToViewBag(NancyContext ctx)
        {
            ViewBag.AppModel = AppModel;
        }
    }
}
