using Common.EntityFramework.Base;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Common.AssemblyUtility
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Gets resource name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetResourceName<T>(this Assembly assembly)
        {
            return assembly
                        .GetManifestResourceNames()
                        .Where(x => x.Contains(typeof(T).Name))
                        .FirstOrDefault() ?? string.Empty;
        }

        /// <summary>
        /// Gets resource name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static string GetResourceName(this Assembly assembly, string resourceName)
        {
            return assembly
                        .GetManifestResourceNames()
                        .Where(x => x.Contains(resourceName))
                        .FirstOrDefault() ?? string.Empty;
        }

        /// <summary>
        /// Reads embedded resource as json file from executing assembly and deserialize it to TEntity type.
        /// </summary>
        /// <typeparam name="TEntity">TEntity must be type of IBaseEntity</typeparam>
        /// <returns>Collection of TEntity types</returns>
        public static IEnumerable<TEntity> DeserializeResourceAsEntityCollection<TEntity>(this Assembly assembly) where TEntity : BaseEntity
        {
            var entityCollection = new List<TEntity>();

            var resourceName = assembly.GetResourceName<TEntity>();

            using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                string json = sr.ReadToEnd();
                entityCollection = JsonSerializer.Deserialize<List<TEntity>>(json);
            }

            return entityCollection;
        }

        /// <summary>
        /// Reads embedded resource as json file from executing assembly and deserialize it to class type.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns>Collection of class types</returns>
        public static IEnumerable<TSource> DeserializeResourceAsClassCollection<TSource>(this Assembly assembly, string resourceName) where TSource : class
        {
            var entityCollection = new List<TSource>();

            var rName = assembly.GetResourceName(resourceName);

            using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(rName)))
            {
                string json = sr.ReadToEnd();
                entityCollection = JsonSerializer.Deserialize<List<TSource>>(json);
            }

            return entityCollection;
        }
    }
}
