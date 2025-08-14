using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
#pragma warning disable CS8618

namespace Intellenum;

public class VoWorkItem
{
    public required INamedTypeSymbol UnderlyingType { get; init; }

    public bool IsUnderlyingIsIComparableOfT { get; init; }
    public bool IsUnderlyingIComparable { get; init; }
    

    /// <summary>
    /// The syntax information for the type to augment.
    /// </summary>
    public TypeDeclarationSyntax TypeToAugment { get; init; }
    
    public bool IsValueType { get; init; }

    public required MemberPropertiesCollection MemberProperties { get; init; }

    public  string FullNamespace { get; init; } = string.Empty;

    public Conversions Conversions { get; init; }

    public Customizations Customizations { get; init; }

    public string VoTypeName => TypeToAugment.Identifier.ToString();

    public string UnderlyingTypeFullName =>  UnderlyingType.FullName() ?? UnderlyingType.Name ?? throw new InvalidOperationException(
        "No underlying type specified - please file a bug at https://github.com/SteveDunn/Intellenum/issues/new?assignees=&labels=bug&template=BUG_REPORT.yml");

    public bool HasToString { get; init; }

    public DebuggerAttributeGeneration DebuggerAttributes { get; init; }

    public bool IsConstant { get; init; }
    
    public required bool SupportsSpans { get; init; }
    
    public required bool TargetsDotNetFramework { get; set; }
}