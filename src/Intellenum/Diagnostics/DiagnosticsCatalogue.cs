using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Intellenum.Diagnostics;

internal static class DiagnosticsCatalogue
{
    private static readonly DiagnosticDescriptor _typeCannotBeNested = CreateDescriptor(
        RuleIdentifiers.TypeCannotBeNested,
        "Types cannot be nested",
        "Type '{0}' cannot be nested - remove it from inside {1}");

    private static readonly DiagnosticDescriptor _typeCannotBeAbstract = CreateDescriptor(
        RuleIdentifiers.TypeCannotBeAbstract,
        "Types cannot be abstract",
        "Type '{0}' cannot be abstract");

    private static readonly DiagnosticDescriptor _recordToStringOverloadShouldBeSealed = CreateDescriptor(
        RuleIdentifiers.RecordToStringOverloadShouldBeSealed,
        "Overrides of ToString on records should be sealed to differentiate it from the C# compiler-generated method. See https://github.com/SteveDunn/Intellenum/wiki/Records#tostring for more information.",
        "ToString overrides should be sealed on records. See https://github.com/SteveDunn/Intellenum/wiki/Records#tostring for more information.");

    private static readonly DiagnosticDescriptor _typeShouldBePartial = CreateDescriptor(
        RuleIdentifiers.TypeShouldBePartial,
        "Intellenum types should be declared in partial types.",
        "Type {0} is decorated as an Intellenum and should be in a partial type.");

    private static readonly DiagnosticDescriptor _duplicateTypesFound = CreateDescriptor(
        RuleIdentifiers.DuplicateTypesFound,
        "Duplicate Intellenum found.",
        "Type {0} is decorated as an Intellenum but is declared multiple times. Remove the duplicate definition or differentiate with a namespace.");

    private static readonly DiagnosticDescriptor _cannotHaveUserConstructors = CreateDescriptor(
        RuleIdentifiers.CannotHaveUserConstructors,
        "Cannot have user defined constructors",
        "Cannot have user defined constructors, please use the From method for creation.");

    private static readonly DiagnosticDescriptor _underlyingTypeCannotBeCollection = CreateDescriptor(
        RuleIdentifiers.UnderlyingTypeCannotBeCollection,
        "Underlying type cannot be collection",
        "Type '{0}' has an underlying type of {1} which is not valid");

    private static readonly DiagnosticDescriptor _invalidConversions = CreateDescriptor(
        RuleIdentifiers.InvalidConversions,
        "Invalid Conversions",
        "The Conversions specified do not match any known conversions - see the Conversions type");

    private static readonly DiagnosticDescriptor _invalidCustomizations = CreateDescriptor(
        RuleIdentifiers.InvalidCustomizations,
        "Invalid Customizations",
        "The Customizations specified do not match any known customizations - see the Customizations type");

    private static readonly DiagnosticDescriptor _underlyingTypeMustNotBeSameAsEnum = CreateDescriptor(
        RuleIdentifiers.UnderlyingTypeMustNotBeSameAsEnum,
        "Invalid underlying type",
        "Type '{0}' has the same underlying type - must specify a primitive underlying type");

    private static readonly DiagnosticDescriptor _memberMethodCallCannotHaveNullArgumentName = CreateDescriptor(
        RuleIdentifiers.MemberMethodCallCannotHaveNullArgumentName,
        "Member attribute cannot have null name",
        "{0} cannot have a null name");

    private static readonly DiagnosticDescriptor _memberMethodCallCannotHaveNullArgumentValue = CreateDescriptor(
        RuleIdentifiers.MemberMethodCallCannotHaveNullArgumentValue,
        "Member attribute cannot have null value",
        "{0} cannot have a null value");

    private static readonly DiagnosticDescriptor _memberMethodCallCanOnlyOmitValuesForStringsAndInts = CreateDescriptor(
        RuleIdentifiers.MemberMethodCallCanOnlyOmitValuesForStringsAndInts,
        "Member method calls must specify a value",
        "{0} cannot omit a value to the Member method unless it is a string or int");

    private static readonly DiagnosticDescriptor _memberValueCannotBeConverted = CreateDescriptor(
        RuleIdentifiers.MemberValueCannotBeConverted,
        "Member attribute has value that cannot be converted",
        "{0} cannot be converted. {1}");

    private static readonly DiagnosticDescriptor _customExceptionMustDeriveFromException = CreateDescriptor(
        RuleIdentifiers.CustomExceptionMustDeriveFromException,
        "Invalid custom exception",
        "{0} must derive from System.Exception");

    private static readonly DiagnosticDescriptor _customExceptionMustHaveValidConstructor = CreateDescriptor(
        RuleIdentifiers.CustomExceptionMustHaveValidConstructor,
        "Invalid custom exception",
        "{0} must have at least 1 public constructor with 1 parameter of type System.String");

    private static readonly DiagnosticDescriptor _mustHaveMembers = CreateDescriptor(
        RuleIdentifiers.MustHaveMembers,
        "Must have members",
        "{0} must have at least 1 member");

    private static readonly DiagnosticDescriptor _membersAttributeCanOnlyBeUsedOnIntOrStringBasedEnums = CreateDescriptor(
        RuleIdentifiers.MembersAttributeCanOnlyBeUsedOnIntBasedEnums,
        "Members attribute can only be used on int or string based enums",
        "The type '{0}' cannot have a Members attribute because it is not based on int or string");

    private static readonly DiagnosticDescriptor _callToMembersMethodShouldOnlyBeCalledOnce = CreateDescriptor(
        RuleIdentifiers.CallToMembersMethodShouldOnlyBeCalledOnce,
        "The Members method can only be called once",
        "The type '{0}' cannot call the Members attribute more than once");

    public static Diagnostic TypeCannotBeNested(INamedTypeSymbol typeModel, INamedTypeSymbol container) => 
        Create(_typeCannotBeNested, typeModel.Locations, typeModel.Name, container.Name);

    public static Diagnostic TypeCannotBeAbstract(INamedTypeSymbol typeModel) => 
        Create(_typeCannotBeAbstract, typeModel.Locations, typeModel.Name);

    public static Diagnostic RecordToStringOverloadShouldBeSealed(Location location, string voClassName) => 
        BuildDiagnostic(_recordToStringOverloadShouldBeSealed, voClassName, location);

    public static Diagnostic TypeShouldBePartial(Location location, string voClassName) => 
        BuildDiagnostic(_typeShouldBePartial, voClassName, location);

    // Just in case the 'AllowMultiple' is taken off the attribute definition.
    public static Diagnostic DuplicateTypesFound(Location location, string voClassName) => 
        BuildDiagnostic(_duplicateTypesFound, voClassName, location);

    public static Diagnostic CannotHaveUserConstructors(IMethodSymbol constructor) => 
        Create(_cannotHaveUserConstructors, constructor.Locations);

    public static Diagnostic UnderlyingTypeMustNotBeSameAsEnumType(INamedTypeSymbol underlyingType) => 
        Create(_underlyingTypeMustNotBeSameAsEnum, underlyingType.Locations, underlyingType.Name);

    public static Diagnostic UnderlyingTypeCannotBeCollection(INamedTypeSymbol voClass, INamedTypeSymbol underlyingType) => 
        Create(_underlyingTypeCannotBeCollection, voClass.Locations, voClass.Name, underlyingType);

    public static Diagnostic InvalidConversions(Location location) => Create(_invalidConversions, location);
    
    public static Diagnostic InvalidCustomizations(Location location) => Create(_invalidCustomizations, location);
    
    public static Diagnostic MemberMethodCallCannotHaveNullArgumentName(INamedTypeSymbol ieClass) => 
        Create(_memberMethodCallCannotHaveNullArgumentName, ieClass.Locations, ieClass.Name);

    public static Diagnostic MemberMethodCallCannotHaveNullArgumentValue(INamedTypeSymbol ieClass) => 
        Create(_memberMethodCallCannotHaveNullArgumentValue, ieClass.Locations, ieClass.Name);

    public static Diagnostic MemberMethodCallCanOnlyOmitValuesForStringsAndInts(INamedTypeSymbol ieClass) => 
        Create(_memberMethodCallCanOnlyOmitValuesForStringsAndInts, ieClass.Locations, ieClass.Name);

    public static Diagnostic MemberValueCannotBeConverted(INamedTypeSymbol ieClass, string message) => 
        Create(_memberValueCannotBeConverted, ieClass.Locations, ieClass.Name, message);

    public static Diagnostic CustomExceptionMustDeriveFromException(INamedTypeSymbol symbol) => 
        Create(_customExceptionMustDeriveFromException, symbol.Locations, symbol.Name);

    public static Diagnostic CustomExceptionMustHaveValidConstructor(INamedTypeSymbol symbol) => 
        Create(_customExceptionMustHaveValidConstructor, symbol.Locations, symbol.Name);

    public static Diagnostic MustHaveMembers(INamedTypeSymbol symbol) => 
        Create(_mustHaveMembers, symbol.Locations, symbol.Name);

    public static Diagnostic MembersAttributeShouldOnlyBeOnIntOrStringBasedEnums(INamedTypeSymbol symbol) => 
        Create(_membersAttributeCanOnlyBeUsedOnIntOrStringBasedEnums, symbol.Locations, symbol.Name);

    // public static Diagnostic DuplicateMembersDeclared(INamedTypeSymbol symbol, string description) =>
    //     Create(_duplicateMembersDeclared, symbol.Locations, symbol.Name, description);
    
    public static Diagnostic CallToMembersMethodShouldOnlyBeCalledOnce(INamedTypeSymbol symbol) => 
        Create(_callToMembersMethodShouldOnlyBeCalledOnce, symbol.Locations, symbol.Name);

    private static DiagnosticDescriptor CreateDescriptor(string code, string title, string messageFormat, DiagnosticSeverity severity = DiagnosticSeverity.Error)
    {
        string[] tags = severity == DiagnosticSeverity.Error ? new[] { WellKnownDiagnosticTags.NotConfigurable } : Array.Empty<string>();

        return new DiagnosticDescriptor(code, title, messageFormat, "Intellenum", severity, isEnabledByDefault: true, customTags: tags);
    }

    public static Diagnostic BuildDiagnostic(DiagnosticDescriptor descriptor, string name, Location location) => 
        Diagnostic.Create(descriptor, location, name);

    public static Diagnostic Create(DiagnosticDescriptor descriptor, IEnumerable<Location> locations, params object?[] args)
    {
        var locationsList = (locations as IReadOnlyList<Location>) ?? locations.ToList();

        Diagnostic diagnostic = Diagnostic.Create(
            descriptor, 
            locationsList.Count == 0 ? Location.None : locationsList[0],
            locationsList.Skip(1), 
            args);

        return diagnostic;
    }

    private static Diagnostic Create(DiagnosticDescriptor descriptor, Location? location, params object?[] args) =>
        Diagnostic.Create(descriptor, location ?? Location.None, args);
}