﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grapevine.Util;

namespace Grapevine.Server.Attributes
{
    /// <summary>
    /// <para>Method attribute for defining a RestRoute</para>
    /// <para>Targets: Method, AllowMultipe: true</para>
    /// <para>&#160;</para>
    /// <para>A method with the RestRoute attribute can have traffic routed to it by a RestServer if the request matches the assigned Method and PathInfo properties</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RestRoute : Attribute
    {
        /// <summary>
        /// The HttpMethod this route will respond to; defaults to HttpMethod.ALL
        /// </summary>
        public HttpMethod HttpMethod { get; set; }

        /// <summary>
        /// A parameterized pattern or regular expression string to match against the value of HttpContext.Request.PathInfo; defaults to an empty string
        /// </summary>
        public string PathInfo { get; set; }

        public RestRoute()
        {
            HttpMethod = HttpMethod.ALL;
            PathInfo = string.Empty;
        }
    }

    public static class RestRouteExtensions
    {
        /// <summary>
        /// Returns an enumerable of all RestRoute attributes on a method
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal static IEnumerable<RestRoute> GetRouteAttributes(this MethodInfo method)
        {
            return method.GetCustomAttributes(true).Where(a => a is RestRoute).Cast<RestRoute>();
        }

        /// <summary>
        /// Returns true if the method is a valid RestRoute
        /// </summary>
        internal static bool IsRestRoute(this MethodInfo method)
        {
            // Must be a public method
            if (!method.IsPublic) return false;

            // Can not be an abstract method
            if (method.IsAbstract) return false;

            // Should not be an overrideable method (sealed if needed)
            if (method.IsVirtual && !method.IsFinal) return false;

            // Can not have a special name (getters and setters)
            if (method.IsSpecialName) return false;

            // Can not be a method that was inherited
            if (method.DeclaringType != method.ReflectedType) return false;

            return method.GetCustomAttributes(true).Any(a => a is RestRoute);
        }
    }
}