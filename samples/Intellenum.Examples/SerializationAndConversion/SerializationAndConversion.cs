// ReSharper disable UnusedVariable
#pragma warning disable CS0219
using System.Threading.Tasks;
using Intellenum.Examples.Types;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;

namespace Intellenum.Examples.SerializationAndConversion
{
    public class SerializationAndConversionExamples : IScenario
    {
        public Task Run()
        {
            SerializeWithNewtonsoftJson();
            SerializeWithSystemTextJson();

            return Task.CompletedTask;
        }

        public void SerializeWithNewtonsoftJson()
        {
            var g1 = NewtonsoftJsonDateTimeOffsetVo.Item1;

            string serialized = NewtonsoftJsonSerializer.SerializeObject(g1);
            string serializedString = NewtonsoftJsonSerializer.SerializeObject(g1.Value);

            var deserializedVo =
                NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonDateTimeOffsetVo>(serializedString);
        }

        public void SerializeWithSystemTextJson()
        {
            var foo = SystemTextJsonDateTimeOffsetVo.Item1;

            string serializedFoo = SystemTextJsonSerializer.Serialize(foo);
            string serializedString = SystemTextJsonSerializer.Serialize(foo.Value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDateTimeOffsetVo>(serializedString);
        }
    }
}