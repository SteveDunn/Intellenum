using System;
using System.Runtime.Serialization;

namespace Intellenum;

[Serializable]
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

    protected IntellenumException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}

[Serializable]
public class IntellenumValidationException : IntellenumException
{
    public IntellenumValidationException()
    {
    }

    public IntellenumValidationException(string message) : base(message)
    {
    }

    public IntellenumValidationException(string message, Exception inner) : base(message, inner)
    {
    }

    protected IntellenumValidationException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}

[Serializable]
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

    protected IntellenumUninitialisedException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}
[Serializable]
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

    protected IntellenumCreationFailedException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}

[Serializable]
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

    protected IntellenumMatchFailedException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}