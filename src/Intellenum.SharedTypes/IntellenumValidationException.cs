using System;
using System.Runtime.Serialization;

namespace Intellenum;

[Serializable]
public class IntellenumValidationException : Exception
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