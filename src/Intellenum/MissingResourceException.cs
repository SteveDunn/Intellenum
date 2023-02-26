using System;

namespace Intellenum;

internal class MissingResourceException : Exception
{
    public MissingResourceException(string message) : base(message)
    {
    }
}