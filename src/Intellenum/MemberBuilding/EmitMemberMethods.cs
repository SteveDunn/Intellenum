using System.Text;
using Microsoft.CodeAnalysis;

namespace Intellenum.MemberBuilding;

public static class EmitMemberMethods
{
    public static string Emit(VoWorkItem voItem)
    {
        StringBuilder sb = new();
        sb.AppendLine(
            $$"""

          // A placeholder method used by the source generator during compilation so that
          // users can 'Call' it. The source generator examines calls to this in order to 
          // generate physical members (e.g. public static readonly MyEnum Item1 = new...)
          private static void Member(string name, {{voItem.UnderlyingTypeFullName}} value)
          {
          }
  """);

        if (voItem.UnderlyingType.SpecialType.IsStringOrInt())
        {
            sb.AppendLine(
                $$"""
                  
                  // A placeholder method used by the source generator during compilation so that
                  // users can 'Call' it. The source generator examines calls to this in order to 
                  // generate physical members (e.g. public static readonly MyEnum Item1 = new...)
                  private static void Member(string name)
                  {
                  }
                  """);

            sb.AppendLine(
                $$"""

          // A placeholder method used by the source generator during compilation so that
          // users can 'Call' it. The source generator examines calls to this in order to 
          // generate physical members (e.g. public static readonly MyEnum Item1 = new...)
          /// <summary>
          /// Sets up members using just the name. For ints, the values are 0 to n, and for strings,
          /// the values are the names. 
          /// The input is a comma separated string, e.g. ""Silver, Gold, Diamond"". The individual
          /// items are trimmed of whitespace. 
          /// </summary>
          private static void Members(string csv)
          {
          }
  """);
        }

        return sb.ToString();
    }
}