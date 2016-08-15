using System;
using Flobot.Identity;
using Flobot.InversionOfControl;
using Flobot.Messages;
using Flobot.Messages.Handlers.Fuck;
using Flobot.Messages.Handlers.PsychoRaid;
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
                .Register<FoaasProxy>(Lifetime.PerResolve)
                .Register<GoogleDocProxy>(Lifetime.PerResolve)
                .Register<IMessageParser, RegexMessageParser>(Lifetime.PerResolve)
                .Register<IUserStore, UserStore>(Lifetime.PerResolve)
                .Register<IUserManager, UserManager>(Lifetime.PerResolve);
        }
    }
}
