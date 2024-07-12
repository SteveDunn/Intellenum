using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Intellenum.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Intellenum.Rules;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DuplicateMembersAnalyzer : DiagnosticAnalyzer
{
    // ReSharper disable once ArrangeObjectCreationWhenTypeEvident - current bug in Roslyn analyzer means it
    // won't find this and will report:
    // "error RS2002: Rule 'XYZ123' is part of the next unshipped analyzer release, but is not a supported diagnostic for any analyzer"
    private static readonly DiagnosticDescriptor _rule = new DiagnosticDescriptor(
        RuleIdentifiers.DuplicateMembers,
        "Duplicate members",
        "The type '{0}' has duplicate members. {1}.",
        RuleCategories.Usage,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description:
        "Member declarations have resulted in duplicate values.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(compilationContext =>
        {
            compilationContext.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        });
    }

    private static void AnalyzeSymbol(SymbolAnalysisContext context)
    {
        var symbol = (INamedTypeSymbol) context.Symbol;

        if (symbol.TypeKind != TypeKind.Class)
        {
            return;
        }

        if (!IntellenumFilter.IsTarget(symbol))
        {
            return;
        }

        var attrs = IntellenumFilter.TryGetIntellenumAttributes(symbol).ToList();

        if (attrs.Count != 1)
        {
            return;
        }

        if (!TryBuildConfiguration(context, attrs, out var config))
        {
            return;
        }

        // by the time this is run, the attributes and the calls to synthetic Member and Members have already been
        // translated to new statements, so we only want to find duplicates in the new statements.

        var members = DiscoverMembers
            .Discover(symbol.GetAttributes(), symbol, config.UnderlyingType, context.Compilation)
            .ValidMembers
            .Where(m => m.Value.Source == MemberSource.FromNewExpression);

        MemberPropertiesCollection justFromNewStatements = new(members.ToList());

        foreach (var eachDuplicate in justFromNewStatements.DescribeAnyDuplicates())
        {
            context.ReportDiagnostic(DiagnosticsCatalogue.Create(_rule, symbol.Locations, symbol.Name, eachDuplicate));
        }
    }

    private static bool TryBuildConfiguration(SymbolAnalysisContext context,
        List<AttributeData> attrs,
        out CombinedIntellenumConfiguration combinedIntellenumConfiguration)
    {
        combinedIntellenumConfiguration = default;
        
        IntellenumConfigurationBuildResult localConfigBuildResult = ManageAttributes.TryBuildConfigurationFromAttribute(attrs[0]);

        if (localConfigBuildResult.ResultingConfiguration is null)
        {
            return false;
        }

        IntellenumConfigurationBuildResult globalConfigBuildResult = ManageAttributes.GetDefaultConfigFromGlobalAttribute(context.Compilation);
        IntellenumConfiguration? global = globalConfigBuildResult.ResultingConfiguration;
        IntellenumConfiguration local = localConfigBuildResult.ResultingConfiguration.Value;

        combinedIntellenumConfiguration = IntellenumConfiguration.Combine(local, global, () => context.Compilation.GetSpecialType(SpecialType.System_Int32));
        return true;
    }
}