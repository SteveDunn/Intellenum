using System.Linq;
using System.Text;

namespace Intellenum.StaticConstructorBuilding;

/// <summary>
/// Builds either a static constructor, or an inner static type.
/// The inner static type allows the user to have their own static constructor.
/// Sometimes though, we *have* to emit a static constructor, for instance,
/// when the user creates a member with just a declarator and no creation expression, e.g. `public static readonly MyEnum One, Two`.
/// The inner static type is set to set names and/or values to members, and also to declare ServiceStack.Text serializers.
/// </summary>
public static class StaticInitializationBuilder
{
    public static string BuildIfNeeded(VoWorkItem item)
    {
        bool hasSsdt = item.Conversions.HasFlag(Conversions.ServiceStackDotText);

        var membersThatNeedInitializing = item.MemberProperties.MembersThatNeedInitializing.ToList();
        
        bool hasMembersThatNeedInitializing = membersThatNeedInitializing.Any();

        if (!hasSsdt && !hasMembersThatNeedInitializing)
        {
            return string.Empty;
        }
        
        StringBuilder sb = new StringBuilder();

        var declators = item.MemberProperties.ValidMembers.Where(p => p.Source == MemberSource.FromFieldDeclator).ToList();
        if (declators.Count > 0)
        {
            sb.AppendLine(
                $$"""
                  static {{item.VoTypeName}}()
                  {
                  """);
            ImplicitFieldBuilder.GenerateNewExpressionsForDeclators(declators, sb, item.VoTypeName);
            sb.AppendLine(
                $$"""
                  }
                  """);
        }

        sb.AppendLine(
            """
            private static class __Inner
            {
                public static int __ComeAlive() => 42;  
                
                static __Inner()
                {
            """);

        ImplicitFieldBuilder.GenerateEachImplicitField(item.MemberProperties.ValidMembers, sb);

        ServiceStackDotTextBuilder.GenerateIfNeeded(item, sb);

        sb.AppendLine(
            """
                }
            }

            private static readonly int __discard = __Inner.__ComeAlive();
            """);
        return sb.ToString();
    }
}