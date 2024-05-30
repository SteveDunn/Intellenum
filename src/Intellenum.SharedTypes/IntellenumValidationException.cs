using System;

namespace Intellenum;

public class IntellenumException : Exception
{
    public IntellenumException()
    {
    }

    public IntellenumException(string message) : base(message)
    {
    }

    public IntellenumException(string message, Exception inner) : base(message, inner)
    {
    }
}

public class IntellenumUninitialisedException : IntellenumException
{
    public IntellenumUninitialisedException()
    {
    }

    public IntellenumUninitialisedException(string message) : base(message)
    {
    }

    public IntellenumUninitialisedException(string message, Exception inner) : base(message, inner)
    {
    }
}

public class IntellenumCreationFailedException : IntellenumException
{
    public IntellenumCreationFailedException()
    {
    }

    public IntellenumCreationFailedException(string message) : base(message)
    {
    }

    public IntellenumCreationFailedException(string message, Exception inner) : base(message, inner)
    {
    }
}

public class IntellenumMatchFailedException : IntellenumException
{
    public IntellenumMatchFailedException()
    {
    }

    public IntellenumMatchFailedException(string message) : base(message)
    {
    }

    public IntellenumMatchFailedException(string message, Exception inner) : base(message, inner)
    {
    }
}
