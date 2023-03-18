// ReSharper disable MemberInitializerValueIgnored
// ReSharper disable UnusedType.Global

// ReSharper disable UnusedParameter.Local

using System;

namespace Intellenum
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class IntellenumDefaultsAttribute : Attribute
    {
        // Note that this is just a placeholder for the source generator. Nothing
        // is actually stored in fields, it's just read at compile time.
        
        /// <summary>
        /// Creates a new instance of a type that represents the default
        /// values used for value object generation.
        /// </summary>
        /// <param name="underlyingType">The primitive underlying type.</param>
        /// <param name="conversions">Any conversions that need to be done for this type, e.g. to be serialized etc.</param>
        /// <param name="customizations">Any customizations, for instance, treating numbers in [de]serialization as strings.</param>
        /// <param name="debuggerAttributes">Controls how debugger attributes are generated. This is useful in Rider where the attributes crash Rider's debugger.</param>
        public IntellenumDefaultsAttribute(
            Type? underlyingType = null,
            Conversions conversions = Conversions.Default,
            Customizations customizations = Customizations.None,
            DebuggerAttributeGeneration debuggerAttributes = DebuggerAttributeGeneration.Default)
        {
        }
    }
}

