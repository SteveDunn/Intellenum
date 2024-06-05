using System.Text;

namespace Intellenum;

internal static class ServiceStackDotTextBuilder
{
    public static void GenerateIfNeeded(VoWorkItem item, StringBuilder sb)
    {
        if (!item.Conversions.HasFlag(Conversions.ServiceStackDotText))
        {
            return;
        }

        if (UnderlyingIsDateOrTimeRelated())
        {
            sb.Append($$"""
                        global::ServiceStack.Text.JsConfig<{{item.VoTypeName}}>.DeSerializeFn = v => {{item.VoTypeName}}.FromValue({{item.UnderlyingTypeFullName}}.Parse(v, global::System.Globalization.CultureInfo.InvariantCulture));
                        global::ServiceStack.Text.JsConfig<{{item.VoTypeName}}>.SerializeFn = v => v.Value.ToString("o", global::System.Globalization.CultureInfo.InvariantCulture);
                     """);

            return;
        }

        if (UnderlyingIsADateTime())
        {
            sb.Append($$"""
                        global::ServiceStack.Text.JsConfig<{{item.VoTypeName}}>.DeSerializeFn = v => FromValue(global::System.DateTime.ParseExact(v, "O", global::System.Globalization.CultureInfo.InvariantCulture, global::System.Globalization.DateTimeStyles.RoundtripKind));
                        global::ServiceStack.Text.JsConfig<{{item.VoTypeName}}>.SerializeFn = v => v.Value.ToUniversalTime().ToString("O", global::System.Globalization.CultureInfo.InvariantCulture);
                     """);
            return;
        }

        string deserialiseFn = $"v => {item.VoTypeName}.FromName(v)";
        // string deserialiseFn = item.IsUnderlyingAString
        //     ? $"{item.VoTypeName}.FromName"
        //     : $"v => {item.VoTypeName}.FromName({item.UnderlyingTypeFullName}.Parse(v))";

        sb.Append($$"""
                    global::ServiceStack.Text.JsConfig<{{item.VoTypeName}}>.DeSerializeFn = {{deserialiseFn}};
                    global::ServiceStack.Text.JsConfig<{{item.VoTypeName}}>.SerializeFn = v => v.Name;
                 """);
        
        return;

        bool UnderlyingIsDateOrTimeRelated()
        {
            var symbol = item.UnderlyingType;
            
            if (symbol.ContainingNamespace.ToDisplayString() != "System")
            {
                return false;
            }
            
            return symbol.Name is "DateTimeOffset" or "TimeOnly" or "DateOnly";
        }

        bool UnderlyingIsADateTime()
        {
            var symbol = item.UnderlyingType;
            
            if (symbol.ContainingNamespace.ToDisplayString() != "System")
            {
                return false;
            }
            
            return symbol.Name is "DateTime";
        }
    }
}