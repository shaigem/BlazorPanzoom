using System.Text.Json.Serialization;
using BlazorPanzoom.Converters;

namespace BlazorPanzoom
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Cursor
    {
        [JsonPropertyName("alias")] Alias,
        [JsonPropertyName("all-scroll")] AllScroll,
        [JsonPropertyName("auto")] Auto,
        [JsonPropertyName("cell")] Cell,
        [JsonPropertyName("context-menu")] ContextMenu,
        [JsonPropertyName("col-resize")] ColResize,
        [JsonPropertyName("copy")] Copy,
        [JsonPropertyName("crosshair")] CrossHair,
        [JsonPropertyName("default")] Default,
        [JsonPropertyName("e-resize")] EResize,
        [JsonPropertyName("ew-resize")] EwResize,
        [JsonPropertyName("grab")] Grab,
        [JsonPropertyName("grabbing")] Grabbing,
        [JsonPropertyName("help")] Help,
        [JsonPropertyName("move")] Move,
        [JsonPropertyName("n-resize")] NResize,
        [JsonPropertyName("ne-resize")] NeResize,
        [JsonPropertyName("nesw-resize")] NeswResize,
        [JsonPropertyName("ns-resize")] NsResize,
        [JsonPropertyName("nw-resize")] NwResize,
        [JsonPropertyName("nwse-resize")] NwseResize,
        [JsonPropertyName("no-drop")] NoDrop,
        [JsonPropertyName("none")] None,
        [JsonPropertyName("not-allowed")] NotAllowed,
        [JsonPropertyName("pointer")] Pointer,
        [JsonPropertyName("progress")] Progress,
        [JsonPropertyName("row-resize")] RowResize,
        [JsonPropertyName("s-resize")] SResize,
        [JsonPropertyName("se-resize")] SeResize,
        [JsonPropertyName("sw-resize")] SwResize,
        [JsonPropertyName("text")] Text,
        [JsonPropertyName("url")] Url,
        [JsonPropertyName("w-resize")] WResize,
        [JsonPropertyName("wait")] Wait,
        [JsonPropertyName("zoom-in")] ZoomIn,
        [JsonPropertyName("zoom-out")] ZoomOut,
    }
}