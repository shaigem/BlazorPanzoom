using System.Text.Json.Serialization;
using BlazorPanzoom.Converters;

namespace BlazorPanzoom
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum TouchAction
    {
        [JsonPropertyName("auto")] Auto,
        [JsonPropertyName("none")] None,
        [JsonPropertyName("pan-x")] PanX,
        [JsonPropertyName("pan-left")] PanLeft,
        [JsonPropertyName("pan-right")] PanRight,
        [JsonPropertyName("pan-y")] PanY,
        [JsonPropertyName("pan-up")] PanUp,
        [JsonPropertyName("pan-down")] PanDown,
        [JsonPropertyName("pinch-zoom")] PinchZoom,
        [JsonPropertyName("manipulation")] Manipulation,
        [JsonPropertyName("inherit")] Inherit,
        [JsonPropertyName("revert")] Revert,
        [JsonPropertyName("unset")] Unset
    }
}