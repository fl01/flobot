using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Flobot.Identity;

namespace Flobot.Messages
{
    public static class TypeLoaderExtensions
    {
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public static IEnumerable<Type> GetPermittedTypes<T>(this Assembly assembly, User caller)
        {
            foreach (var type in assembly.GetLoadableTypes().Where(typeof(T).IsAssignableFrom))
            {
                var permissionsAttribute = type.GetCustomAttribute<PermissionsAttribute>();

                if (permissionsAttribute == null)
                {
                    continue;
                }

                if (permissionsAttribute.Role <= caller.Role && (caller.Group.HasFlag(permissionsAttribute.Group) || caller.Group.HasFlag(Group.Administrators)))
                {
                    yield return type;
                }
            }
        }
    }
}
