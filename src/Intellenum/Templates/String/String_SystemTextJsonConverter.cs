﻿
        /// <summary>
        /// Converts an Intellenum instance to or from JSON.
        /// </summary>
        public class VOTYPESystemTextJsonConverter : global::System.Text.Json.Serialization.JsonConverter<VOTYPE>
        {
            public override VOTYPE Read(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)
            {
                return VOTYPE.__Deserialize(reader.GetString());
            }

            public override void Write(System.Text.Json.Utf8JsonWriter writer, VOTYPE value, global::System.Text.Json.JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.Value);
            }

#if NET6_0_OR_GREATER
            public override VOTYPE ReadAsPropertyName(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)
            {
                return VOTYPE.__Deserialize(reader.GetString());
            }

            public override void WriteAsPropertyName(System.Text.Json.Utf8JsonWriter writer, VOTYPE value, global::System.Text.Json.JsonSerializerOptions options)
            {
                writer.WritePropertyName(value.Value);
            }
#endif            
        }