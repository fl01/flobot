using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Flobot.Handlers.Metadata;
using Flobot.Identity;
using Flobot.Messages;

namespace Flobot.Handlers.DataSource
{
    public class LocalAssemblyDataSource : IMetadataDataSource
    {
        public IEnumerable<IHandlerMetadata> GetAll()
        {
            var local = GetLoadableTypes().Where(t => typeof(IMessageHandler).IsAssignableFrom(t));

            return Create(local);
        }

        private IEnumerable<IHandlerMetadata> Create(IEnumerable<Type> rawLocalHandlerTypes)
        {
            foreach (var type in rawLocalHandlerTypes)
            {
                var permissions = type.GetCustomAttribute<PermissionsAttribute>();
                if (permissions == null)
                {
                    continue;
                }

                var commandData = type.GetCustomAttribute<MessageAttribute>();
                if (commandData == null)
                {
                    continue;
                }

                // TODO : rework this to avoid creation of instance to get sub commands, unfortunately it is the only one way to get them
                var instance = Activator.CreateInstance(type) as MessageHandlerBase;

                var localMetaData = new LocalHandlerMetadata()
                {
                    Group = permissions.Group,
                    Role = permissions.Role,
                    MessageHandlerType = type,
                    SubCommands = instance.SubCommands.Select(x => x.Key).ToList(),
                    Section = commandData.Section,
                    Description = commandData.Description,
                    SupportedCommands = commandData.SupportedCommands.ToArray()
                };

                yield return localMetaData;
            }
        }

        private IEnumerable<Type> GetLoadableTypes()
        {
            try
            {
                return Assembly.GetExecutingAssembly().GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}