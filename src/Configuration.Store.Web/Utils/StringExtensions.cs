using System;

namespace Configuration.Store.Web.Utils
{
    public static class StringExtensions
    {
        public static Version ToVersion(this string version)
        {
            return Version.Parse(version);
        }
    }
}