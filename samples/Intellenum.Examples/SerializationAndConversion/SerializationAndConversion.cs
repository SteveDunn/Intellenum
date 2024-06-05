// ReSharper disable UnusedVariable
#pragma warning disable CS0219
using System.Diagnostics;
using System.Threading.Tasks;
using Intellenum.Examples.Types;
using JetBrains.Annotations;
using ServiceStack.Text;

using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;

namespace Intellenum.Examples.SerializationAndConversion
{
    [UsedImplicitly]
    public class SerializationAndConversionExamples : IScenario
    {
        public Task Run()
        {
            SerializeWithNewtonsoftJson();
            SerializeWithSystemTextJson();
            SerializeWithServiceStackTextJson();


            return Task.CompletedTask;
        }
        
        public static void SerializeWithServiceStackTextJson()
        {
            ServiceStackDotTextIntEnum orig = ServiceStackDotTextIntEnum.Item1;

            string json = JsonSerializer.SerializeToString(orig);
            
            var deserialised = JsonSerializer.DeserializeFromString<ServiceStackDotTextIntEnum>(json);
            
            Debug.Assert(deserialised.Value == orig.Value); 
        }


        public static void SerializeWithNewtonsoftJson()
        {
            var g1 = NewtonsoftJsonDateTimeOffsetEnum.Item1;

            string serialized = NewtonsoftJsonSerializer.SerializeObject(g1);
            string serializedString = NewtonsoftJsonSerializer.SerializeObject(g1.Value);

            var deserializedVo =
                NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonDateTimeOffsetEnum>(serializedString);
        }

        public static void SerializeWithSystemTextJson()
        {
            var foo = SystemTextJsonDateTimeOffsetEnum.Item1;

            string serializedFoo = SystemTextJsonSerializer.Serialize(foo);
            string serializedString = SystemTextJsonSerializer.Serialize(foo.Value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDateTimeOffsetEnum>(serializedString);
        }
    }
}