using System.Configuration;
using System.Text;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Cryptography;
using Nancy.Hosting.Aspnet;
using Nancy.TinyIoc;
using Nancy.ViewEngines;
using NancySample.Models;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;

namespace NancySample
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            MiniProfilerEF6.Initialize();

            base.ApplicationStartup(container, pipelines);
            pipelines.BeforeRequest += PreRequest;
            pipelines.AfterRequest += PostRequest;

            //Make sure that the IoC container does not return singletons for EF.
            container.Register<IAppDbContext, AppDbContext>().AsPerRequestSingleton();

            //Setup Forms Auth
            var formsAuthConfig = new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/auth/login",
                UserMapper = container.Resolve<IUserMapper>(),
                RequiresSSL = true,
                CryptographyConfiguration = CreateCrypto()
            };
            FormsAuthentication.Enable(pipelines, formsAuthConfig);

            //Set up custom view resolution, looks in Modules/{ModuleName}/{ViewName}.cshtml
            Conventions.ViewLocationConventions.Add((viewName, model, context) => $"Modules/{context.ModuleName}/{viewName}");
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            container.Register<IUserMapper, AuthenticationUser>();
        }

        private static Response PreRequest(NancyContext ctx)
        {
            MiniProfiler.Start();
            return null;
        }

        private static void PostRequest(NancyContext ctx)
        {
            MiniProfiler.Stop();
        }

        private static CryptographyConfiguration CreateCrypto()
        {
            var passPhrase =  ConfigurationManager.AppSettings["cryptoPass"];
            var salt = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["cryptoSalt"]);
            var keygen = new PassphraseKeyGenerator(passPhrase, salt);
            return new CryptographyConfiguration(new RijndaelEncryptionProvider(keygen), new DefaultHmacProvider(keygen));
        }
    }
}
