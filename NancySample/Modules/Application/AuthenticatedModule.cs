using Nancy.Linker;
using Nancy.Security;
using NancySample.Models;

namespace NancySample.Modules.Application
{
    public class AuthenticatedModule : ApplicationModule
    {
        public AuthenticatedModule(IAppDbContext dbCtx, IResourceLinker linker) : base(dbCtx, linker)
        {
            this.RequiresAuthentication();
        }
    }
}