using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Intellenum.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum;

internal static class ManageAttributes
{
    /// <summary>
    /// Gets global default configuration from any global (assembly) attribute.
    /// If none are specified, then the default configuration is used.
    /// If some are specified, then they are validated.
    /// If anything is invalid, a compilation error is raised.
    /// </summary>
    /// <param name="defaults"></param>
    /// <param name="compilation"></param>
    /// <returns></returns>
    public static IntellenumConfigurationBuildResult GetDefaultConfigFromGlobalAttribute(
        ImmutableArray<AttributeSyntax> defaults,
        Compilation compilation)
    {
        if (defaults.IsDefaultOrEmpty)
        {
            // No global defaults
            return IntellenumConfigurationBuildResult.Null;
        }

        var assemblyAttributes = compilation.Assembly.GetAttributes();
        if (assemblyAttributes.IsDefaultOrEmpty)
        {
            return IntellenumConfigurationBuildResult.Null;
        }

        INamedTypeSymbol? allThatMatchByName = compilation.GetTypeByMetadataName("Intellenum.IntellenumDefaultsAttribute");
        if (allThatMatchByName is null)
        {
            return IntellenumConfigurationBuildResult.Null;
        }

        AttributeData? matchingAttribute = assemblyAttributes.SingleOrDefault(aa =>
            allThatMatchByName.Equals(aa.AttributeClass, SymbolEqualityComparer.Default));

        if (matchingAttribute == null)
        {
            return IntellenumConfigurationBuildResult.Null;
        }

        IntellenumConfigurationBuildResult globalConfig = TryBuildConfigurationFromAttribute(matchingAttribute);

        return globalConfig;
    }

    public static IntellenumConfigurationBuildResult TryBuildConfigurationFromAttribute(AttributeData matchingAttribute)
    {
        IntellenumConfigurationBuildResult buildResult = new IntellenumConfigurationBuildResult();

        INamedTypeSymbol? invalidExceptionType = null;
        INamedTypeSymbol? underlyingType = null;
        Conversions conversions = Conversions.Default;
        Customizations customizations = Customizations.None;
        DebuggerAttributeGeneration debuggerAttributes = DebuggerAttributeGeneration.Default;

        bool hasErroredAttributes = false;

        var isBaseGenericType = matchingAttribute.AttributeClass!.BaseType!.IsGenericType;
        if (!matchingAttribute.ConstructorArguments.IsEmpty || isBaseGenericType)
        {
            // make sure we don't have any errors
            ImmutableArray<TypedConstant> args = matchingAttribute.ConstructorArguments;

            foreach (TypedConstant arg in args)
            {
                if (arg.Kind == TypedConstantKind.Error)
                {
                    hasErroredAttributes = true;
                }
            }

            // find which constructor to use, it could be the generic attribute (> C# 11), or the non-generic.
            if (matchingAttribute.AttributeClass!.IsGenericType || isBaseGenericType)
            {
                PopulateFromGenericAttribute(matchingAttribute, args);
            }
            else
            {
                PopulateFromNonGenericAttribute(args);
            }
        }

        if (!matchingAttribute.NamedArguments.IsEmpty)
        {
            foreach (KeyValuePair<string, TypedConstant> arg in matchingAttribute.NamedArguments)
            {
                TypedConstant typedConstant = arg.Value;
                if (typedConstant.Kind == TypedConstantKind.Error)
                {
                    hasErroredAttributes = true;
                }
                else
                {
                    switch (arg.Key)
                    {
                        case "underlyingType":
                            underlyingType = (INamedTypeSymbol?) typedConstant.Value!;
                            break;
                        case "invalidExceptionType":
                            invalidExceptionType = (INamedTypeSymbol?) typedConstant.Value!;
                            break;
                        case "conversions":
                            conversions = (Conversions) (typedConstant.Value ?? Conversions.Default);
                            break;
                        case "customizations":
                            customizations = (Customizations) (typedConstant.Value ?? Customizations.None);
                            break;
                        case "debuggerAttributes":
                            debuggerAttributes = (DebuggerAttributeGeneration) (typedConstant.Value ?? DebuggerAttributeGeneration.Full);
                            break;
                    }
                }
            }
        }

        if (hasErroredAttributes)
        {
            // skip further generator execution and let compiler generate the errors
            return IntellenumConfigurationBuildResult.Null;
        }

        if (!conversions.IsValidFlags())
        {
            var syntax = matchingAttribute.ApplicationSyntaxReference?.GetSyntax();
            if (syntax is not null)
            {
                buildResult.AddDiagnostic(DiagnosticsCatalogue.InvalidConversions(syntax.GetLocation()));
            }
        }

        if (!customizations.IsValidFlags())
        {
            var syntax = matchingAttribute.ApplicationSyntaxReference?.GetSyntax();
            if (syntax is not null)
            {
                buildResult.AddDiagnostic(DiagnosticsCatalogue.InvalidCustomizations(syntax.GetLocation()));
            }
        }

        buildResult.ResultingConfiguration = new IntellenumConfiguration(
            underlyingType,
            conversions,
            customizations,
            debuggerAttributes);

        return buildResult;

        void PopulateFromGenericAttribute(
            AttributeData attributeData,
            ImmutableArray<TypedConstant> args)
        {
            var isDerivedFromGenericAttribute =
                attributeData.AttributeClass!.BaseType!.FullName()!.StartsWith("Intellenum.IntellenumAttribute<");
            
            // Extracts the generic argument from the base type when the derived type isn't generic
            // e.g. MyCustomVoAttribute : IntellenumAttribute<long>
            var type = isDerivedFromGenericAttribute && attributeData.AttributeClass!.TypeArguments.IsEmpty
                ? attributeData.AttributeClass!.BaseType!.TypeArguments[0] as INamedTypeSymbol
                : attributeData.AttributeClass!.TypeArguments[0] as INamedTypeSymbol;
            switch (args.Length)
            {
                case 3:
                    if (args[2].Value != null)
                    {
                        debuggerAttributes = (DebuggerAttributeGeneration) args[2].Value!;
                    }

                    goto case 2;
                case 2:
                    if (args[1].Value != null)
                    {
                        customizations = (Customizations) args[1].Value!;
                    }

                    goto case 1;
                case 1:
                    if (args[0].Value != null)
                    {
                        conversions = (Conversions) args[0].Value!;
                    }

                    break;
            }

            underlyingType = type;
        }

        void PopulateFromNonGenericAttribute(ImmutableArray<TypedConstant> args)
        {
            switch (args.Length)
            {
                case 4:
                    if (args[3].Value != null)
                    {
                        debuggerAttributes = (DebuggerAttributeGeneration) args[3].Value!;
                    }

                    goto case 3;
                case 3:
                    if (args[2].Value != null)
                    {
                        customizations = (Customizations) args[2].Value!;
                    }

                    goto case 2;
                case 2:
                    if (args[1].Value != null)
                    {
                        conversions = (Conversions) args[1].Value!;
                    }

                    goto case 1;
                case 1:
                    underlyingType = (INamedTypeSymbol?) args[0].Value;
                    break;
            }
        }
    }

    private static void BuildAnyIssuesWithTheException(
        INamedTypeSymbol? invalidExceptionType, 
        IntellenumConfigurationBuildResult buildResult)
    {
        if (invalidExceptionType == null)
        {
            return;
        }

        if (!invalidExceptionType.ImplementsInterfaceOrBaseClass(typeof(Exception)))
        {
            buildResult.AddDiagnostic(DiagnosticsCatalogue.CustomExceptionMustDeriveFromException(invalidExceptionType));
        }

        var allConstructors = invalidExceptionType.Constructors.Where(c => c.DeclaredAccessibility == Accessibility.Public);

        var singleParameterConstructors = allConstructors.Where(c => c.Parameters.Length == 1);

        if (singleParameterConstructors.Any(c => c.Parameters.Single().Type.Name == "String"))
        {
            return;
        }

        buildResult.AddDiagnostic(DiagnosticsCatalogue.CustomExceptionMustHaveValidConstructor(invalidExceptionType));
    }

    /// <summary>
    /// Tries to get the syntax element for any matching attribute that might exist in the provided context.
    /// </summary>
    /// <param name="context"></param>
    /// <returns>The syntax of the attribute if it matches the global defaults attribute, otherwise null.</returns>
    public static AttributeSyntax? TryGetAssemblyLevelDefaultsAttribute(GeneratorSyntaxContext context)
    {
        // we know the node is a AttributeListSyntax thanks to IsSyntaxTargetForGeneration
        var attributeListSyntax = (AttributeListSyntax) context.Node;

        foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
        {
            if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
            {
                continue;
            }

            INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
            string fullName = attributeContainingTypeSymbol.ToDisplayString();

            if (fullName == "Intellenum.IntellenumDefaultsAttribute")
            {
                return attributeSyntax;
            }
        }

        return null;
    }
}