using System.Linq;
using System.Text;

namespace Intellenum.StaticConstructorBuilding;

public static class StaticConstructorBuilder
{
    public static string BuildIfNeeded(VoWorkItem item)
    {
        bool hasSsdt = item.Conversions.HasFlag(Conversions.ServiceStackDotText);

        var implicitlyNamedMembers = item.MemberProperties.ImplicitlyNamedMembers.ToList();
        
        bool hasImplicitMembers = implicitlyNamedMembers.Any();

        if (!hasSsdt && !hasImplicitMembers)
        {
            return string.Empty;
        }
        
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(
            $$"""
              private static class __Inner
              {
                  public static int __ComeAlive() => 42;  
                  
                  static __Inner()
                  {
              """);

        ImplicitFieldBuilder.GenerateEachImplicitField(implicitlyNamedMembers, sb);

        ServiceStackDotTextBuilder.GenerateIfNeeded(item, sb);

        sb.AppendLine(
            $$"""
                }
              }

              private static readonly int __discard = __Inner.__ComeAlive();
              """);
        return sb.ToString();
    }
}