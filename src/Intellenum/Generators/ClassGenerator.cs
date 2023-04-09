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
{Util.TryWriteNamespaceIfSpecified(item)}

{Util.WriteStartNamespace(item.FullNamespace)}
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage] 
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""{Util.GenerateYourAssemblyName()}"", ""{Util.GenerateYourAssemblyVersion()}"")]
    {Util.GenerateAnyConversionAttributes(tds, item)}
    {Util.GenerateDebugAttributes(item, className, itemUnderlyingType)}
    {Util.GenerateModifiersFor(tds)} class {className} : 
        global::System.IEquatable<{className}>, 
        global::System.IComparable, 
        global::System.IComparable<{className}> 
    {{
        {Util.GenerateLazyLookupsIfNeeded(item)}

#if DEBUG    
        private readonly global::System.Diagnostics.StackTrace _stackTrace = null;
#endif
        private readonly global::System.Boolean _isInitialized;
        private readonly {itemUnderlyingType} _value;

        {MemberGeneration.GeneratePrivateConstructionInitialisationIfNeeded(item)}

        /// <summary>
        /// Gets the underlying <see cref=""{itemUnderlyingType}"" /> value if set, otherwise default
        /// </summary>
        public {itemUnderlyingType} Value
        {{
            [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [global::System.Diagnostics.DebuggerStepThroughAttribute]
            get
            {{
                return _value;
            }}
        }}

private void Throw()
{{
#if DEBUG
                global::System.String message = ""Use of uninitialized Value Object at: "" + _stackTrace ?? """";
#else
                global::System.String message = ""Use of uninitialized Value Object."";
#endif

                throw new {nameof(IntellenumUninitialisedException)}(message);

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
        private {className}({itemUnderlyingType} value)
        {{
            _value = value;
            Name = ""[INFERRED-TO-BE-REPLACED!]"";
            _isInitialized = true;
        }}        

        [global::System.Diagnostics.DebuggerStepThroughAttribute]
        private {className}(string enumName, {itemUnderlyingType} value)
        {{
            _value = value;
            Name = enumName;
            _isInitialized = true;
        }}

        public string Name {{ get; private set; }}

        /// <summary>
        /// Builds a member from an enum value.
        /// </summary>
        /// <param name=""value"">The value.</param>
        /// <returns>The matching enum, or an exception.</returns>
        public static {className} FromValue({itemUnderlyingType} value)
        {{
            {Util.GenerateFromValueImplementation(item)}
        }}            

        /// <summary>
        /// Tries to get a member based on value.
        /// </summary>
        /// <param name=""value"">The value.</param>
        /// <returns>The matching enum, or an exception.</returns>
        public static bool TryFromValue({itemUnderlyingType} value, out {className} member)
        {{
            {Util.GenerateTryFromValueImplementation(item)}
        }}        

        public static bool IsDefined({itemUnderlyingType} value)
        {{
            {Util.GenerateIsDefinedBody(item)}
        }}

        public void Deconstruct(out string Name, out {itemUnderlyingType} Value)
        {{
            Name = this.Name;
            Value = this.Value;
        }}

        /// <summary>
        /// Gets the matching member based on name.
        /// </summary>
        /// <param name=""name"">The name.</param>
        /// <returns>The matching enum, or an exception.</returns>
        public static {className} FromName(string name)
        {{
            {Util.GenerateFromNameImplementation(item)}
        }}

        /// <summary>
        /// Tries to get the matching member from a name.
        /// </summary>
        /// <param name=""name"">The name.</param>
        /// <returns>The matching enum, or an exception.</returns>
        public static bool TryFromName(string name, out {className} member)
        {{
            {Util.GenerateTryFromNameImplementation(item)}
        }}

        public static bool IsNamedDefined(string name)
        {{
            {Util.GenerateIsNameDefinedImplementation(item)}
        }}


        /// <summary>
        /// Builds a member from the provided underlying type.
        /// </summary>
        /// <param name=""value"">The underlying type.</param>
        /// <returns>A member of this type.</returns>
        private static {className} From(string name, {itemUnderlyingType} value)
        {{
            {GenerateNullCheckIfNeeded(item)}

            {className} member = new {className}(name, value);

            return member;
        }}

        // placeholder method used by the source generator
        // to generate physical members (e.g. public static readonly MyEnum Item1 = new...)
        private static void Member(string name, {itemUnderlyingType} value)
        {{
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

            // It's possible to create uninitialized members via converters such as EfCore (HasDefaultValue), which call Equals.
            // We treat anything uninitialized as not equal to anything, even other uninitialized members of this type.
            if(!_isInitialized || !other._isInitialized) return false;
	    	
            if (ReferenceEquals(this, other))
            {{
                return true;
            }}

            return GetType() == other.GetType() && global::System.Collections.Generic.EqualityComparer<{itemUnderlyingType}>.Default.Equals(Value, other.Value);
        }}

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
        
        public static global::System.Boolean operator <({className} left, {className} right) => left.CompareTo(right) < 0;
        public static global::System.Boolean operator <=({className} left, {className} right) => left.CompareTo(right) <= 0;
        public static global::System.Boolean operator >({className} left, {className} right) => left.CompareTo(right) > 0;
        public static global::System.Boolean operator >=({className} left, {className} right) => left.CompareTo(right) >= 0;

        public static explicit operator {className}({itemUnderlyingType} value) => FromValue(value);
        public static implicit operator {itemUnderlyingType}({className} value) => value.Value;

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

        {MemberGeneration.GenerateAnyMembers(tds, item)}
        
        public static global::System.Collections.Generic.IEnumerable<{className}> List()
        {{
            {MemberGeneration.GenerateIEnumerableYields(item)}
        }}        

        {Util.GenerateToString(item)}

        {Util.GenerateAnyConversionBodies(tds, item)}
        
        {TryParseGeneration.GenerateTryParseIfNeeded(item)}

        {Util.GenerateDebuggerProxyForClasses(tds, item)}
    }}
{Util.WriteCloseNamespace(item.FullNamespace)}";
    }

    private static string GenerateNullCheckIfNeeded(VoWorkItem voWorkItem) =>
        voWorkItem.IsValueType ? string.Empty
            : $@"            if (value is null)
            {{
                throw new {nameof(IntellenumCreationFailedException)}(""Cannot create an Intellenum member with a null."");
            }}
";
}