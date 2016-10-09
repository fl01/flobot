using Flobot.Common.Net;
using Flobot.Handlers;
using Flobot.Handlers.DataSource;
using Flobot.Handlers.Metadata;
using Flobot.Identity;
using Flobot.InversionOfControl;
using Flobot.Messages;
using Flobot.Messages.Handlers.ExternalHandler;
using Flobot.Messages.LocalHandlers.Fuck;
using Flobot.Messages.LocalHandlers.PictureStore;
using Flobot.Settings;

namespace Flobot
{
    public static class Bootstrapper
    {
        public static void Initialize(DependencyContainer container)
        {
            container
                .Register<IConfigSettings, ConfigSettings>(Lifetime.Singleton)
                .Register<ISettingsService, SettingsService>(Lifetime.Singleton)
                .Register<IMessageHandlerProvider, MessageHandlerProvider>(Lifetime.Singleton)
                .Register<IHandlerMetadataService, HandlerMetadataService>(Lifetime.Singleton)
                .Register<IMetadataConverter, MetadataConverter>(Lifetime.PerResolve)
                .Register<IPermissionsService, PermissionsService>(Lifetime.PerResolve)
                // TODO : should we use empty interfaces instead of nameof ? e.g. ILocalAssemblyDataSource : IMetadataDataSource 
                .Register<IMetadataDataSource, ExternalServicesDataSource>(nameof(ExternalServicesDataSource), Lifetime.PerResolve)
                .Register<IMetadataDataSource, LocalAssemblyDataSource>(nameof(LocalAssemblyDataSource), Lifetime.PerResolve)
                .Register<FoaasProxy>(Lifetime.PerResolve)
                .Register<IMessageParser, RegexMessageParser>(Lifetime.PerResolve)
                .Register<IUserStore, UserStore>(Lifetime.PerResolve)
                .Register<IUserManager, UserManager>(Lifetime.PerResolve)
                .Register<IPictureStore, LocalPictureStore>(Lifetime.PerResolve)
                .Register<IExternalSource, RestfulSource>(nameof(RestfulSource), Lifetime.PerResolve)
                .Register<HttpClient>(Lifetime.PerResolve);
        }
    }
}
