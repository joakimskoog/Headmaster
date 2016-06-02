using System;
using System.Web.Http;

namespace Headmaster.Core
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetInnermostNamespaceName(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
          
            string[] namespaceSegments = type.Namespace?.Split(Type.Delimiter);
            var innermostNamespace = namespaceSegments?[namespaceSegments.Length - 1]; 

            return innermostNamespace ?? string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsInnermostNamespaceName(this Type type, string version)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (version == null) throw new ArgumentNullException(nameof(version));

            var supportedVersion = type.GetInnermostNamespaceName();

            return string.Equals(version, supportedVersion, StringComparison.OrdinalIgnoreCase);
        }
    }
}