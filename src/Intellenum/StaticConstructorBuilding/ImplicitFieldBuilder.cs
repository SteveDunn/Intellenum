using System.Collections.Generic;
using System.Text;

namespace Intellenum.StaticConstructorBuilding;

public class ImplicitFieldBuilder
{
    public static void GenerateNewExpressionsForDeclators(IEnumerable<MemberProperties> implicitlyNamedMembers,
        StringBuilder sb,
        string wrapperType)
    {
        foreach (MemberProperties eachMember in implicitlyNamedMembers)
        {
            sb.AppendLine($"{eachMember.FieldName} = new {wrapperType}(\"{eachMember.FieldName}\", {eachMember.ValueAsText});");
        }
    }

    public static void GenerateEachImplicitField(IEnumerable<MemberProperties> implicitlyNamedMembers, StringBuilder sb)
    {
        foreach (MemberProperties eachMember in implicitlyNamedMembers)
        {
            if (eachMember.Source is not(MemberSource.FromAttribute or MemberSource.FromMemberMethod or MemberSource.FromNewExpression))
            {
                continue;
            }
            if (!eachMember.WasExplicitlySetAName)
            {
                sb.AppendLine($"{eachMember.FieldName}._name = \"{eachMember.FieldName}\";");
            }

            if (!eachMember.WasExplicitlySetAValue)
            {
                sb.AppendLine($"{eachMember.FieldName}._value = {eachMember.ValueAsText};");
                sb.AppendLine($"{eachMember.FieldName}._isInitialized = true;");
            }
        }
    }
}