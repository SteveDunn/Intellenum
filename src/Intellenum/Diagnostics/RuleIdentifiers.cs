namespace Intellenum.Diagnostics;

public static class RuleIdentifiers
{
    public const string AddValidationMethod = "AddValidationMethod";
    public const string AddStaticToExistingValidationMethod = "AddStaticToExistingValidationMethod";
    public const string FixInputTypeOfValidationMethod = "FixInputTypeOfValidationMethod";

    public const string TypeCannotBeNested = "INTELLENUM001";
    public const string UnderlyingTypeMustNotBeSameAsEnum = "INTELLENUM002";
    public const string UnderlyingTypeCannotBeCollection = "INTELLENUM003";
    public const string MemberMethodCallCannotHaveNullArgumentName = "INTELLENUM006";
    public const string MemberMethodCallCannotHaveNullArgumentValue = "INTELLENUM007";
    public const string CannotHaveUserConstructors = "INTELLENUM008";
    public const string DoNotUseDefault = "INTELLENUM009";
    public const string DoNotUseNew = "INTELLENUM010";
    public const string InvalidConversions = "INTELLENUM011";
    public const string CustomExceptionMustDeriveFromException = "INTELLENUM012";
    public const string CustomExceptionMustHaveValidConstructor = "INTELLENUM013";
    public const string TypeCannotBeAbstract = "INTELLENUM017";
    public const string InvalidCustomizations = "INTELLENUM019";
    public const string RecordToStringOverloadShouldBeSealed = "INTELLENUM020";
    public const string TypeShouldBePartial = "INTELLENUM021";
    public const string MemberValueCannotBeConverted = "INTELLENUM023";
    public const string DuplicateTypesFound = "INTELLENUM024";
    public const string DoNotUseReflection = "INTELLENUM025";
    public const string MustHaveMembers = "INTELLENUM026";
    public const string MembersAttributeCanOnlyBeUsedOnIntBasedEnums = "INTELLENUM027";
}