using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
#pragma warning disable CS8618

namespace Intellenum;

public class VoWorkItem
{
    private readonly INamedTypeSymbol _underlyingType = null!;
    private readonly string _underlyingTypeFullName = null!;

    public INamedTypeSymbol UnderlyingType
    {
        get => _underlyingType;
        init
        {
            _underlyingType = value;
            _underlyingTypeFullName = value.FullName() ?? value.Name ?? throw new InvalidOperationException(
                "No underlying type specified - please file a bug at https://github.com/SteveDunn/Vogen/issues/new?assignees=&labels=bug&template=BUG_REPORT.yml");
            IsUnderlyingAString = typeof(string).IsAssignableFrom(Type.GetType(_underlyingTypeFullName));
        }
    }
    
    public bool IsUnderlyingAString { get; private set; }

    /// <summary>
    /// The syntax information for the type to augment.
    /// </summary>
    public TypeDeclarationSyntax TypeToAugment { get; init; }
    
    public bool IsValueType { get; init; }

    public List<MemberProperties> MemberProperties { get; init; } = new();

    public  string FullNamespace { get; init; } = string.Empty;

    public Conversions Conversions { get; init; }

    public Customizations Customizations { get; init; }

    public string VoTypeName => TypeToAugment.Identifier.ToString();

    public string UnderlyingTypeFullName =>  UnderlyingType.FullName() ?? UnderlyingType.Name ?? throw new InvalidOperationException(
        "No underlying type specified - please file a bug at https://github.com/SteveDunn/Intellenum/issues/new?assignees=&labels=bug&template=BUG_REPORT.yml");

    public bool HasToString { get; init; }

    public DebuggerAttributeGeneration DebuggerAttributes { get; init; }

    public bool IsConstant { get; init; }
}