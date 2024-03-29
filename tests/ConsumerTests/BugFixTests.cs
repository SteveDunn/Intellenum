using Newtonsoft.Json;

// ReSharper disable NullableWarningSuppressionIsUsed

namespace ConsumerTests.BugFixTests
{
    public class BugFixTests
    {
        /// <summary>
        /// Fixes bug https://github.com/SteveDunn/Vogen/issues/344 where a field that is a Intellenum and is null when
        /// deserialized by Newtonsoft.Json, throws an exception instead of returning null.
        /// </summary>
        [Fact]
        public void Bug344_Can_deserialize_a_null_field()
        {
            var p = new Person
            {
                AgeRange = AgeRange.Senior
            };

            var serialized = JsonConvert.SerializeObject(p);
            var deserialized = JsonConvert.DeserializeObject<Person>(serialized)!;

            deserialized.AgeRange.Should().Be(AgeRange.Senior);
            deserialized.Name.Should().BeNull();
            deserialized.Address.Should().BeNull();
        }
    }

    public class Person
    {
        public AgeRange AgeRange { get; init; } = AgeRange.Senior;
        
        public NameType? Name { get; set; }
        
        public Address? Address { get; set; }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson)]
    [Member("Junior", 1)]
    [Member("Adult", 2)]
    [Member("Senior", 3)]
    public partial class AgeRange
    {
    }

    [Intellenum(typeof(string), conversions: Conversions.NewtonsoftJson)]
    [Member("FirstAndLast", 1)]
    [Member("Nickname", 2)]
    public partial class NameType
    {
    }
    
    [Intellenum(typeof(string), conversions: Conversions.NewtonsoftJson)] 
    [Member("Full", 1)]
    [Member("JustThePostcode", 2)]
    public partial class Address { }
}