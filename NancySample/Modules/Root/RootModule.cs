using Nancy.Linker;
using Nancy.Responses;
using NancySample.Models;
using NancySample.Modules.Application;

namespace NancySample.Modules.Root
{
    public class RootModule : ApplicationModule
    {
        public RootModule(IAppDbContext context, IResourceLinker linker) : base(context, linker)
        {
            //Redirect the root to the home page route
            Get[RouteNames.GetRoot, "/"] = parameters =>
            {
                if (Context.CurrentUser != null)
                {
                    return new RedirectResponse(
                        linker.BuildRelativeUri(Context, RouteNames.GetHome)
                            .ToString());
                }

                //Otherwise, show the logged out version
                return View["index", new { linker }];
            };
        }
    }
}
