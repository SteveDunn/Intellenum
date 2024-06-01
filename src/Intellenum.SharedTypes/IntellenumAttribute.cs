// ReSharper disable UnusedParameter.Local

// Note that these attributes are just placeholders for the source generator. Nothing
// is actually stored in fields, they're just read at compile time.

using System;

namespace Intellenum
{
    /// <summary>
    /// Marks a type as an Intellenum. The type should be a partial class so that the
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
            Customizations customizations = Customizations.None,
            DebuggerAttributeGeneration debuggerAttributes = DebuggerAttributeGeneration.Default)
            : base(typeof(T), conversions, customizations, debuggerAttributes)
        {
        }
    }

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
            Type? underlyingType = null,
            Conversions conversions = Conversions.Default,
            Customizations customizations = Customizations.None,
            DebuggerAttributeGeneration debuggerAttributes = DebuggerAttributeGeneration.Default)
        {
        }
    }
}