using System.Web;
using System.Web.Http;
using Flobot.Common.Container;
using Flobot.Handlers;
using Flobot.Handlers.DataSource;

namespace Flobot
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Bootstrapper.Initialize(IoC.Container);

            var metadataService = IoC.Container.Resolve<IHandlerMetadataService>();
            metadataService
               .AddDataSource(IoC.Container.Resolve<IMetadataDataSource>(nameof(LocalAssemblyDataSource)))
               .AddDataSource(IoC.Container.Resolve<IMetadataDataSource>(nameof(ExternalServicesDataSource)))
               .EnsureInitialized();
        }
    }
}
