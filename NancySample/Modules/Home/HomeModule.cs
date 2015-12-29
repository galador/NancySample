using Nancy.Linker;
using NancySample.Models;
using NancySample.Modules.Application;

namespace NancySample.Modules.Home
{
    public class HomeModule : ApplicationModule
    {
        public HomeModule(IAppDbContext dbCtx, IResourceLinker linker) : base(dbCtx, linker)
        {
            Get[RouteNames.GetHome, "/home"] = parameters =>
            {
                AppModel.PageTitle = "Home Page";
                var user = (User) Context.CurrentUser;
                var model = new HomeModel { UserId = user.Id, UserName = user.UserName };
                return View["index", model];
            };
        }
    }
}
