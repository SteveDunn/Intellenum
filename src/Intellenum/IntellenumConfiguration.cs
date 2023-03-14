using System;
using Microsoft.CodeAnalysis;

namespace Intellenum;

public readonly struct IntellenumConfiguration
{
    public IntellenumConfiguration(INamedTypeSymbol? underlyingType,
        Conversions conversions,
        Customizations customizations,
        DebuggerAttributeGeneration debuggerAttributes)
    {
        UnderlyingType = underlyingType;
        Conversions = conversions;
        Customizations = customizations;
        DebuggerAttributes = debuggerAttributes;
    }

    public static IntellenumConfiguration Combine(
        IntellenumConfiguration localValues,
        IntellenumConfiguration? globalValues,
        Func<INamedTypeSymbol>? funcForDefaultUnderlyingType = null)
    {
        var conversions = (localValues.Conversions, globalValues?.Conversions) switch
        {
            (Conversions.Default, null) => DefaultInstance.Conversions,
            (Conversions.Default, Conversions.Default) => DefaultInstance.Conversions,
            (Conversions.Default, var globalDefault) => globalDefault.Value,
            (var specificValue, _) => specificValue
        };

        var customizations = (localValues.Customizations, globalValues?.Customizations) switch
        {
            (Customizations.None, null) => DefaultInstance.Customizations,
            (Customizations.None, Customizations.None) => DefaultInstance.Customizations,
            (Customizations.None, var globalDefault) => globalDefault.Value,
            (var specificValue, _) => specificValue
        };

        var debuggerAttributes = (localValues.DebuggerAttributes, globalValues?.DebuggerAttributes) switch
        {
            (DebuggerAttributeGeneration.Default, null) => DefaultInstance.DebuggerAttributes,
            (DebuggerAttributeGeneration.Default, DebuggerAttributeGeneration.Default) => DefaultInstance.DebuggerAttributes,
            (DebuggerAttributeGeneration.Default, var globalDefault) => globalDefault.Value,
            (var specificValue, _) => specificValue
        };

        var underlyingType = localValues.UnderlyingType ?? globalValues?.UnderlyingType ?? funcForDefaultUnderlyingType?.Invoke();

        return new IntellenumConfiguration(underlyingType, conversions, customizations, debuggerAttributes);
    }

    public INamedTypeSymbol? UnderlyingType { get; }
    
    public Conversions Conversions { get; }
    
    public Customizations Customizations { get; }
    
    public DebuggerAttributeGeneration DebuggerAttributes { get; }

    // the issue here is that without a physical 'symbol' in the source, we can't
    // get the namedtypesymbol
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly IntellenumConfiguration DefaultInstance = new(
        underlyingType: null,
        // ReSharper disable once RedundantNameQualifier
        conversions: Conversions.Default,
        customizations: Customizations.None,
        debuggerAttributes: DebuggerAttributeGeneration.Full);
}
