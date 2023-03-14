// ReSharper disable UnusedParameter.Local

using System;

namespace Intellenum
{
#if NETCOREAPP
    /// <summary>
    /// Marks a type as a Value Object. The type should be partial so that the
    /// source generator can augment the type with equality and validation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class IntellenumAttribute<T> : IntellenumAttribute
    {
        // keep this signature in-line with `IntellenumConfiguration`
        // as the syntax/semantics are read in the generator
        // using parameter indexes (i.e. it expected param 0 to be the underlying type etc.
        public IntellenumAttribute(
            Conversions conversions = Conversions.Default,
            Type? throws = null!,
            DebuggerAttributeGeneration debuggerAttributes = DebuggerAttributeGeneration.Default)
            : base(typeof(T), conversions, throws, debuggerAttributes)
        {
        }
    }
#endif

    /// <summary>
    /// Marks a type as a Value Object. The type that this is applied to should be partial so that the
    /// source generator can augment it with equality, creation barriers, and any conversions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class IntellenumAttribute : Attribute
    {
        // keep this signature in-line with `IntellenumConfiguration`
        // as the syntax/semantics are read in the generator
        // using parameter indexes (i.e. it expected param 0 to be the underlying type etc).
        public IntellenumAttribute(
            Type? underlyingType = null!,
            Conversions conversions = Conversions.Default,
            Type? throws = null!,
            DebuggerAttributeGeneration debuggerAttributes = DebuggerAttributeGeneration.Default)
        {
            // UnderlyingType = underlyingType;
            // Conversions = conversions;
            // ValidationExceptionType = throws;
            // Customizations = customizations;
            // DeserializationStrictness = deserializationStrictness;
            // OmitDebugAttributes = omitDebugAttributes;
        }

        // public Type? UnderlyingType { get; }
        //
        // public Type? ValidationExceptionType { get; }
        //
        // public Conversions Conversions { get; }
        //
        // public Customizations Customizations { get; }
        //
        // public DeserializationStrictness DeserializationStrictness { get; } =
        //     DeserializationStrictness.AllowValidAndKnownInstances;
        //
        // public bool OmitDebugAttributes { get; }
    }
}