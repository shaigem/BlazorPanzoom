using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorPanzoom
{
    public class JsonCamelCaseStringEnumConverter : JsonConverterFactory
    {
        private readonly JsonStringEnumConverter _jsonStringEnumConverter;
        
        public JsonCamelCaseStringEnumConverter()
        {
            _jsonStringEnumConverter = new JsonStringEnumConverter(
                JsonNamingPolicy.CamelCase);
        }
        public override bool CanConvert(Type typeToConvert)
        {
            return _jsonStringEnumConverter.CanConvert(typeToConvert);
        }
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return _jsonStringEnumConverter.CreateConverter(typeToConvert, options);
        }
    }

}