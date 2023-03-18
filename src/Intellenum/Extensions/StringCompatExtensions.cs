// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

#if !NETCOREAPP

using System;

namespace Intellenum.Extensions
{
    internal static class StringCompatExtensions
    {
        public static bool Contains(this string str, string value, StringComparison comparisonType)
        {
            return str.IndexOf(value, comparisonType) >= 0;
        }

        public static string Replace(this string str, string oldValue, string? newValue, StringComparison comparisonType)
        {
            if (comparisonType != StringComparison.Ordinal)
                throw new NotSupportedException();

            return str.Replace(oldValue, newValue);
        }
    }
}

#endif
