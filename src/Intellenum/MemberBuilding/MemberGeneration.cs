using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum.MemberBuilding;

public static class MemberGeneration
{
    public static string GenerateIEnumerableYields(VoWorkItem item)
    {
        if (item.MemberProperties.Count == 0)
        {
            return "yield break;";
        }

        StringBuilder sb = new StringBuilder();

        foreach (MemberProperties each in item.MemberProperties)
        {
            sb.AppendLine($"yield return {each.FieldName};");
        }

        return sb.ToString();
    }

    public static string GenerateAnyMembers(TypeDeclarationSyntax classDeclarationSyntax, VoWorkItem item)
    {
        if (item.MemberProperties.Count == 0)
        {
            return string.Empty;
        }

        StringBuilder sb = new StringBuilder();

        foreach (MemberProperties each in item.MemberProperties.Where(i => i.Source is not MemberSource.FromNewExpression))
        {
            sb.AppendLine(GenerateMember(each, classDeclarationSyntax, item.FullNamespace));
        }

        return sb.ToString();
    }

    private static string GenerateMember(
        MemberProperties memberProperties,
        TypeDeclarationSyntax classDeclarationSyntax, 
        string itemFullNamespace)
    {
        var memberValue = memberProperties.ValueAsText;

        return $@"
// member...

{BuildMemberComment(classDeclarationSyntax.Identifier, memberProperties.TripleSlashComments, itemFullNamespace)}public static readonly {classDeclarationSyntax.Identifier} {Util.EscapeIfRequired(memberProperties.FieldName)} = new {classDeclarationSyntax.Identifier}(""{memberProperties.FieldName}"",{memberValue});";
    }

    private static string BuildMemberComment(SyntaxToken syntaxToken, string? commentText, string fullNamespace)
    {
        if (string.IsNullOrEmpty(commentText))
        {
            return string.Empty;
        }

        var x = new XElement("summary", commentText);
        var y = new XElement("returns", $"The \"T:{fullNamespace}.{syntaxToken}\" member.");

        return $@"    
/// {x}
/// {y}
";
    }

    public record BuildResult(bool Success, string Value, string ErrorMessage = "");

    // We don't need to consider a propertyValue of decimal here, as it cannot be passed in
    // via an attribute in C#
    public static BuildResult TryBuildMemberValueAsText(string propertyName, object propertyValue, string? underlyingType)
    {
        try
        {
            if (underlyingType == typeof(DateTime).FullName)
            {
                if(propertyValue is string s)
                {
                    var parsed = DateTime.Parse(s, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

                    return new(true,
                        $@"global::System.DateTime.Parse(""{parsed:O}"", global::System.Globalization.CultureInfo.InvariantCulture, global::System.Globalization.DateTimeStyles.RoundtripKind)");
                }

                if(propertyValue is long l)
                {
                    _ = new DateTime(l, DateTimeKind.Utc);

                    return new(true, $@"new global::System.DateTime({l},  global::System.DateTimeKind.Utc)");
                }

                if(propertyValue is int i)
                {
                    _ = new DateTime(i, DateTimeKind.Utc);

                    return new(true, $@"new global::System.DateTime({i},  global::System.DateTimeKind.Utc)");
                }
            }

            if (underlyingType == typeof(DateTimeOffset).FullName)
            {
                if(propertyValue is string s)
                {
                    var parsed = DateTimeOffset.Parse(s, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
                    var utcParsed = new DateTimeOffset(parsed.DateTime, TimeSpan.Zero);

                    return new(true,
                        $@"global::System.DateTimeOffset.Parse(""{utcParsed:O}"", null, global::System.Globalization.DateTimeStyles.RoundtripKind)");
                }

                if(propertyValue is long l)
                {
                    _ = new DateTimeOffset(l, TimeSpan.Zero);
                    return new(true, $@"new global::System.DateTimeOffset({l},  global::System.TimeSpan.Zero)");
                }

                if(propertyValue is int i)
                {
                    _ = new DateTimeOffset(i, TimeSpan.Zero);
                    return new(true, $@"new global::System.DateTimeOffset({i},  global::System.TimeSpan.Zero)");
                }
            }

            if (underlyingType == typeof(string).FullName)
            {
                return new(true, $@"""{propertyValue}""");
            }

            if (underlyingType == typeof(decimal).FullName)
            {
                if (propertyValue is char c)
                {
                    return new(true, $@"{c}m");
                }

                return new(true,
                    Convert.ToDecimal(propertyValue).ToString(CultureInfo.InvariantCulture) + "m");
            }

            if (underlyingType == typeof(double).FullName)
            {
                if (propertyValue is char c)
                {
                    return new(true, $@"{c}d");
                }

                return new(true,
                    Convert.ToDecimal(propertyValue).ToString(CultureInfo.InvariantCulture) + "d");
            }

            if (underlyingType == typeof(float).FullName)
            {
                if (propertyValue is char c)
                {
                    return new(true, $@"{c}f");
                }

                return new(true,
                    Convert.ToSingle(propertyValue).ToString(CultureInfo.InvariantCulture) + "f");
            }

            if (underlyingType == typeof(char).FullName)
            {
                if(propertyValue is char c)
                    return new(true, $@"'{c}'");

                var converted = Convert.ToChar(propertyValue);
                return new(true, $@"'{converted}'");
            }

            if (underlyingType == typeof(byte).FullName)
            {
                var converted = Convert.ToByte(propertyValue);

                return new(true, $@"{converted}");
            }

            if (underlyingType == typeof(bool).FullName)
            {
                var converted = propertyValue.ToString()?.ToLower();
            
                return new(true, $@"{converted}");
            }

            return new(true, propertyValue.ToString()!);
        }
        catch (Exception e)
        {
            return new(false, string.Empty,
                $"Member '{propertyName}' has a value type '{propertyValue.GetType()}' of '{propertyValue}' which cannot be converted to the underlying type of '{underlyingType}' - {e.Message}");
        }
    }
    
    public static string GenerateConstValuesIfPossible(VoWorkItem item)
    {
        if (!item.IsConstant || item.MemberProperties.Count == 0)
        {
            return string.Empty;
        }
        
        StringBuilder sb = new StringBuilder("// const fields...");
        sb.AppendLine();
        foreach (var memberProperties in item.MemberProperties)
        {
            sb.AppendLine($"public const {item.UnderlyingTypeFullName} {memberProperties.FieldName}Value = {memberProperties.ValueAsText};");
        }
        
        return sb.ToString();
    }


}
