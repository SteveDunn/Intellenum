using System.Collections.Generic;
using System.Text;

namespace Intellenum.StaticConstructorBuilding;

public class ImplicitFieldBuilder
{
    public static void GenerateEachImplicitField(List<MemberProperties> implicitlyNamedMembers, StringBuilder sb)
    {
        foreach (var eachMember in implicitlyNamedMembers)
        {
            sb.AppendLine($"{eachMember.FieldName}.Name = \"{eachMember.FieldName}\";");
        }
    }
  
}