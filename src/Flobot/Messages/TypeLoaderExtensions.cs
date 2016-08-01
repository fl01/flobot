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
            return assembly.GetLoadableTypes()
                .Where(typeof(T).IsAssignableFrom)
                .Where(h => h.GetCustomAttribute<PermissionsAttribute>()?.Role <= caller.Role);
        }
    }
}
