using System.Text.Json.Serialization;
using BlazorPanzoom.Converters;

namespace BlazorPanzoom
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Overflow
    {
        [JsonPropertyName("visible")] Visible,
        [JsonPropertyName("hidden")] Hidden,
        [JsonPropertyName("scroll")] Scroll,
        [JsonPropertyName("auto")] Auto,
        [JsonPropertyName("initial")] Initial,
        [JsonPropertyName("inherit")] Inherit
    }
}