using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum.Generators;

public class ClassGenerator : IGenerateSourceCode
{
    public string BuildClass(VoWorkItem item, TypeDeclarationSyntax tds)
    {
        var className = tds.Identifier;

        var itemUnderlyingType = item.UnderlyingTypeFullName;
        
        return $@"
using Intellenum;

{Util.WriteStartNamespace(item.FullNamespace)}
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage] 
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""{Util.GenerateYourAssemblyName()}"", ""{Util.GenerateYourAssemblyVersion()}"")]
    {Util.GenerateAnyConversionAttributes(tds, item)}
    {Util.GenerateDebugAttributes(item, className, itemUnderlyingType)}
    {Util.GenerateModifiersFor(tds)} class {className} : global::System.IEquatable<{className}>, global::System.IEquatable<{itemUnderlyingType}> {Util.GenerateIComparableHeaderIfNeeded(", ", item, tds)}
    {{
#if DEBUG    
        private readonly global::System.Diagnostics.StackTrace _stackTrace = null;
#endif
        private readonly global::System.Boolean _isInitialized;
        private readonly {itemUnderlyingType} _value;
        
/// <summary>
/// Gets the underlying <see cref=""{itemUnderlyingType}"" /> value if set, otherwise a <see cref=""{nameof(IntellenumValidationException)}"" /> is thrown.
/// </summary>
public {itemUnderlyingType} Value
        {{
            [global::System.Diagnostics.DebuggerStepThroughAttribute]
            get
            {{
                EnsureInitialized();
                return _value;
            }}
        }}

        [global::System.Diagnostics.DebuggerStepThroughAttribute]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public {className}()
        {{
#if DEBUG
            _stackTrace = new global::System.Diagnostics.StackTrace();
#endif
            _isInitialized = false;
            _value = default;
            Name = ""[UNDEFINED]"";
        }}

        [global::System.Diagnostics.DebuggerStepThroughAttribute]
        private {className}(string name, {itemUnderlyingType} value)
        {{
            _value = value;
            Name = name;
            _isInitialized = true;
        }}

        public string Name {{ get; }}

        /// <summary>
        /// Builds an instance from an enum value.
        /// </summary>
        /// <param name=""value"">The value.</param>
        /// <returns>The matching enum, or an exception.</returns>
        public static {className} FromValue({itemUnderlyingType} value)
        {{
            {Util.GenerateFromValueImplementation(item)}
        }}            

        /// <summary>
        /// Tries to get an instance based on value.
        /// </summary>
        /// <param name=""value"">The value.</param>
        /// <returns>The matching enum, or an exception.</returns>
        public static bool TryFromValue({itemUnderlyingType} value, out {className} instance)
        {{
            {Util.GenerateTryFromValueImplementation(item)}
        }}        

        public static bool IsDefined({itemUnderlyingType} value)
        {{
            {Util.GenerateIsDefinedImplementation(item)}
        }}        

        /// <summary>
        /// Gets the matching instance based on name.
        /// </summary>
        /// <param name=""name"">The name.</param>
        /// <returns>The matching enum, or an exception.</returns>
        public static {className} FromName(string name)
        {{
            {Util.GenerateFromNameImplementation(item)}
        }}

        /// <summary>
        /// Tries to get the matching instance from a name.
        /// </summary>
        /// <param name=""name"">The name.</param>
        /// <returns>The matching enum, or an exception.</returns>
        public static bool TryFromName(string name, out {className} instance)
        {{
            {Util.GenerateTryFromNameImplementation(item)}
        }}

        public static bool IsNamedDefined(string name)
        {{
            {Util.GenerateIsNameDefinedImplementation(item)}
        }}


        /// <summary>
        /// Builds an instance from the provided underlying type.
        /// </summary>
        /// <param name=""value"">The underlying type.</param>
        /// <returns>An instance of this type.</returns>
        private static {className} From(string name, {itemUnderlyingType} value)
        {{
            {GenerateNullCheckIfNeeded(item)}

            {className} instance = new {className}(name, value);

            {Util.GenerateValidation(item)}

            return instance;
        }}

        // only called internally when something has been deserialized into
        // its primitive type.
        private static {className} Deserialize({itemUnderlyingType} value)
        {{
            {GenerateNullCheckIfNeeded(item)}

            {Util.GenerateCallToValidateForDeserializing(item)}

            return FromValue(value);
        }}

        public global::System.Boolean Equals({className} other)
        {{
            if (ReferenceEquals(null, other))
            {{
                return false;
            }}

            // It's possible to create uninitialized instances via converters such as EfCore (HasDefaultValue), which call Equals.
            // We treat anything uninitialized as not equal to anything, even other uninitialized instances of this type.
            if(!_isInitialized || !other._isInitialized) return false;
	    	
            if (ReferenceEquals(this, other))
            {{
                return true;
            }}

            return GetType() == other.GetType() && global::System.Collections.Generic.EqualityComparer<{itemUnderlyingType}>.Default.Equals(Value, other.Value);
        }}

        public global::System.Boolean Equals({itemUnderlyingType} primitive) => Value.Equals(primitive);

        public override global::System.Boolean Equals(global::System.Object obj)
        {{
            if (ReferenceEquals(null, obj))
            {{
                return false;
            }}

            if (ReferenceEquals(this, obj))
            {{
                return true;
            }}

            if (obj.GetType() != GetType())
            {{
                return false;
            }}

            return Equals(({className}) obj);
        }}

        public static global::System.Boolean operator ==({className} left, {className} right) => Equals(left, right);
        public static global::System.Boolean operator !=({className} left, {className} right) => !Equals(left, right);

        public static global::System.Boolean operator ==({className} left, {itemUnderlyingType} right) => Equals(left.Value, right);
        public static global::System.Boolean operator !=({className} left, {itemUnderlyingType} right) => !Equals(left.Value, right);

        public static global::System.Boolean operator ==({itemUnderlyingType} left, {className} right) => Equals(left, right.Value);
        public static global::System.Boolean operator !=({itemUnderlyingType} left, {className} right) => !Equals(left, right.Value);

        public static explicit operator {className}({itemUnderlyingType} value) => FromValue(value);
        public static explicit operator {itemUnderlyingType}({className} value) => value.Value;

        {Util.GenerateIComparableImplementationIfNeeded(item, tds)}

        public override global::System.Int32 GetHashCode()
        {{
            unchecked // Overflow is fine, just wrap
            {{
                global::System.Int32 hash = (global::System.Int32) 2166136261;
                hash = (hash * 16777619) ^ Value.GetHashCode();
                hash = (hash * 16777619) ^ GetType().GetHashCode();
                hash = (hash * 16777619) ^ global::System.Collections.Generic.EqualityComparer<{itemUnderlyingType}>.Default.GetHashCode();
                return hash;
            }}
        }}

        private void EnsureInitialized()
        {{
            if (!_isInitialized)
            {{
#if DEBUG
                global::System.String message = ""Use of uninitialized Value Object at: "" + _stackTrace ?? """";
#else
                global::System.String message = ""Use of uninitialized Value Object."";
#endif

                throw new {item.ValidationExceptionFullName}(message);
            }}
        }}


        {InstanceGeneration.GenerateAnyInstances(tds, item)}

        {Util.GenerateToString(item)}

        {Util.GenerateAnyConversionBodies(tds, item)}

        {Util.GenerateDebuggerProxyForClasses(tds, item)}
    }}
{Util.WriteCloseNamespace(item.FullNamespace)}";
    }

    private static string GenerateNullCheckIfNeeded(VoWorkItem voWorkItem) =>
        voWorkItem.IsValueType ? string.Empty
            : $@"            if (value is null)
            {{
                throw new {voWorkItem.ValidationExceptionFullName}(""Cannot create a value object with null."");
            }}
";
}