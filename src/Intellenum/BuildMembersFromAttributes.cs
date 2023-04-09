using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Intellenum.Diagnostics;
using Microsoft.CodeAnalysis;

namespace Intellenum
{
    internal static class BuildMembersFromAttributes
    {
        public static IEnumerable<MemberProperties?> Build(
            IEnumerable<AttributeData> allAttributes,
            SourceProductionContext context, 
            INamedTypeSymbol voClass, 
            INamedTypeSymbol? underlyingType)
        {
            return allAttributes.Select(a => Build(a, context, voClass, underlyingType));
        }

        // ReSharper disable once CognitiveComplexity
        private static MemberProperties? Build(AttributeData matchingAttribute, SourceProductionContext context, INamedTypeSymbol voClass, INamedTypeSymbol? underlyingType)
        {
            // try build it from non-named arguments

            var constructorArgs = matchingAttribute.ConstructorArguments;
            
            if (!constructorArgs.IsEmpty)
            {
                // make sure we don't have any errors
                ImmutableArray<TypedConstant> args = constructorArgs;

                if (HasAnyErrors(args))
                {
                    return null;
                }

                return TryBuild(args[0], args[1], args[2], voClass, context, underlyingType);
            }

            // try build it from named arguments
            var namedArgs = matchingAttribute.NamedArguments;
            
            if (namedArgs.IsEmpty)
            {
                return null;
            }

            if (namedArgs.Any(a => a.Value.Kind == TypedConstantKind.Error))
            {
                return null;
            }

            TypedConstant nameConstant = default;
            TypedConstant valueConstant = default;
            TypedConstant commentConstant = default;

            foreach (KeyValuePair<string, TypedConstant> arg in namedArgs)
            {
                TypedConstant typedConstant = arg.Value;

                switch (arg.Key)
                {
                    case "name":
                        nameConstant = typedConstant;
                        break;
                    case "value":
                        valueConstant = typedConstant;
                        break;
                    case "tripleSlashComments":
                        commentConstant = typedConstant;
                        break;
                }
            }

            return TryBuild(nameConstant, valueConstant, commentConstant, voClass, context, underlyingType);
        }

        private static bool HasAnyErrors(ImmutableArray<TypedConstant> args)
        {
            if (args.Any(arg => arg.Kind == TypedConstantKind.Error))
            {
                return true;
            }

            return false;
        }

        private static MemberProperties? TryBuild(
            TypedConstant nameConstant, 
            TypedConstant valueConstant, 
            TypedConstant commentConstant,
            INamedTypeSymbol voClass,
            SourceProductionContext context,
            INamedTypeSymbol? underlyingType)
        {
            bool hasErrors = false;
            if (nameConstant.Value is null)
            {
                context.ReportDiagnostic(DiagnosticsCatalogue.MemberMethodCallCannotHaveNullArgumentName(voClass));
                hasErrors = true;
            }

            if (valueConstant.Value is null)
            {
                context.ReportDiagnostic(DiagnosticsCatalogue.MemberMethodCallCannotHaveNullArgumentValue(voClass));
                hasErrors = true;
            }

            if (hasErrors)
            {
                return null;
            }

            var r = MemberGeneration.TryBuildMemberValueAsText(
                (string) nameConstant.Value!,
                valueConstant.Value!,
                underlyingType?.FullName());

            if (!r.Success)
            {
                context.ReportDiagnostic(DiagnosticsCatalogue.MemberValueCannotBeConverted(voClass, r.ErrorMessage));
                return null;
            }

            return new MemberProperties(
                MemberSource.FromAttribute,
                (string)nameConstant.Value!, 
                (string)nameConstant.Value!, 
                r.Value,
                valueConstant.Value!, 
                (string) (commentConstant.Value ?? string.Empty));
        }
    }
}