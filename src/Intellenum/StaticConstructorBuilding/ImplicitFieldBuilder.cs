using System.Collections.Generic;
using System.Text;

namespace Intellenum.StaticConstructorBuilding;

public class ImplicitFieldBuilder
{
    public static void GenerateEachImplicitField(IEnumerable<ValueOrDiagnostic<MemberProperties>> implicitlyNamedMembers, StringBuilder sb)
    {
        foreach (var eachMember in implicitlyNamedMembers)
        {
            if (eachMember.IsValue)
            {
                sb.AppendLine($"{eachMember.Value.FieldName}.Name = \"{eachMember.Value.FieldName}\";");
            }
        }
    }
}