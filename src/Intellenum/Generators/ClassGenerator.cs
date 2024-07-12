using Intellenum.Generators.Snippets;
using Intellenum.MemberBuilding;
using Intellenum.StaticConstructorBuilding;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum.Generators;

public class ClassGenerator
{
    public string BuildClass(VoWorkItem item, TypeDeclarationSyntax tds, bool isNetFramework)
    {
        var className = tds.Identifier;

        string itemUnderlyingType = item.UnderlyingTypeFullName;
        
        return $@"
using Intellenum;
using System;
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
        {MemberGeneration.GenerateConstValuesIfPossible(item)}

        {Util.GenerateLazyLookupsIfNeeded(item)}

#if DEBUG    
        private readonly global::System.Diagnostics.StackTrace _stackTrace = null;
#endif
        private readonly global::System.Boolean _isInitialized;
        private readonly {itemUnderlyingType} _value;

        {StaticConstructorBuilder.BuildIfNeeded(item)}

        /// <summary>
        /// Gets the underlying <see cref=""{itemUnderlyingType}"" /> value if set, otherwise default
        /// </summary>
        public {itemUnderlyingType} Value => _value;

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
        public {className}({itemUnderlyingType} value)
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

        public void Deconstruct(out string Name, out {itemUnderlyingType} Value)
        {{
            Name = this.Name;
            Value = this.Value;
        }}

        {SnippetGenerationFactory.Generate(item, tds, SnippetType.FromValueRelateMethods, isNetFramework)}

        {SnippetGenerationFactory.Generate(item, tds, SnippetType.FromNameRelatedMethods, isNetFramework)}

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
        {EmitMemberMethods.Emit(item)}

        // only called internally when something has been deserialized into
        // its primitive type.
        private static {className} Deserialize({itemUnderlyingType} value)
        {{
            {GenerateNullCheckIfNeeded(item)}

            {Util.GenerateCallToValidateForDeserializing(item)}

            return FromValue(value);
        }}

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static global::System.Boolean operator ==({className} left, {className} right) => Equals(left, right);

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static global::System.Boolean operator !=({className} left, {className} right) => !Equals(left, right);

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static global::System.Boolean operator ==({className} left, {itemUnderlyingType} right) => Equals(left.Value, right);

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static global::System.Boolean operator !=({className} left, {itemUnderlyingType} right) => !Equals(left.Value, right);

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static global::System.Boolean operator ==({itemUnderlyingType} left, {className} right) => Equals(left, right.Value);

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static global::System.Boolean operator !=({itemUnderlyingType} left, {className} right) => !Equals(left, right.Value);
        
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static global::System.Boolean operator <({className} left, {className} right) => left.CompareTo(right) < 0;

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static global::System.Boolean operator <=({className} left, {className} right) => left.CompareTo(right) <= 0;

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static global::System.Boolean operator >({className} left, {className} right) => left.CompareTo(right) > 0;

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static global::System.Boolean operator >=({className} left, {className} right) => left.CompareTo(right) >= 0;

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static explicit operator {className}({itemUnderlyingType} value) => FromValue(value);

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static implicit operator {itemUnderlyingType}({className} value) => value.Value;

        {Util.GenerateIComparableImplementationIfNeeded(item, tds)}

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
