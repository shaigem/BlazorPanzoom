using System;
using System.Text.Json.Serialization;
using BlazorPanzoom.Converters;
using Microsoft.AspNetCore.Components;

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

    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Contain
    {
        [JsonPropertyName("inside")] Inside,
        [JsonPropertyName("outside")] Outside,
        [JsonPropertyName("none")] None
    }

    public interface IZoomOnlyOptions
    {
        public bool GetDisableZoomOrDefault(bool disableZoom = default);
        public double GetMinScaleOrDefault(double minScale = PanzoomOptions.DefaultMinScale);
        public double GetMaxScaleOrDefault(double maxScale = PanzoomOptions.DefaultMaxScale);
        public double GetStepOrDefault(double step = PanzoomOptions.DefaultStepScale);
    }

    public interface IPanOnlyOptions
    {
        public Contain GetContainOrDefault(Contain contain = Contain.None);
        public bool GetDisablePanOrDefault(bool disablePan = default);
        public bool GetDisableXAxisOrDefault(bool disableXAxis = default);
        public bool GetDisableYAxisOrDefault(bool disableYAxis = default);
        public Cursor GetCursorOrDefault(Cursor cursor = Cursor.Move);
        public bool GetPanOnlyWhenZoomedOrDefault(bool panOnlyWhenZoomed = default);
        public bool GetRelativeOrDefault(bool relative = default);
    }

    public record PanzoomOptions : IZoomOnlyOptions, IPanOnlyOptions
    {
        protected internal const double DefaultMinScale = 0.125;
        protected internal const double DefaultMaxScale = 4;
        protected internal const double DefaultStepScale = 0.3;

        private static readonly ElementReference[] EmptyExcludedReferences = Array.Empty<ElementReference>();

        private static readonly PanzoomOptions Default = new();

        private readonly Contain? _contain;
        public static ref readonly PanzoomOptions DefaultOptions => ref Default;

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Animate { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Canvas { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? Duration { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Easing { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ElementReference[]? Exclude { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ExcludeClass { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Force { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? NoBind { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Origin { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Overflow { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Silent { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? StartScale { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? StartX { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? StartY { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TouchAction { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableZoom { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public FocalPoint? Focal { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? MaxScale { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? MinScale { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? Step { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Contain? Contain
        {
            private get => _contain;
            init => _contain = value is not null && value.Equals(BlazorPanzoom.Contain.None) ? null : value;
        }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisablePan { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableXAxis { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableYAxis { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Cursor? Cursor { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PanOnlyWhenZoomed { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Relative { private get; init; }

        public Contain GetContainOrDefault(Contain contain = BlazorPanzoom.Contain.None)
        {
            return Contain ?? contain;
        }

        public bool GetDisablePanOrDefault(bool disablePan = default)
        {
            return DisablePan ?? disablePan;
        }

        public bool GetDisableXAxisOrDefault(bool disableXAxis = default)
        {
            return DisableXAxis ?? disableXAxis;
        }

        public bool GetDisableYAxisOrDefault(bool disableYAxis = default)
        {
            return DisableYAxis ?? disableYAxis;
        }

        public Cursor GetCursorOrDefault(Cursor cursor = BlazorPanzoom.Cursor.Move)
        {
            return Cursor ?? cursor;
        }

        public bool GetPanOnlyWhenZoomedOrDefault(bool panOnlyWhenZoomed = default)
        {
            return PanOnlyWhenZoomed ?? panOnlyWhenZoomed;
        }

        public bool GetRelativeOrDefault(bool relative = default)
        {
            return Relative ?? relative;
        }

        public bool GetDisableZoomOrDefault(bool disableZoom = false)
        {
            return DisableZoom ?? disableZoom;
        }

        public double GetMinScaleOrDefault(double minScale = DefaultMinScale)
        {
            return MinScale ?? minScale;
        }

        public double GetMaxScaleOrDefault(double maxScale = DefaultMaxScale)
        {
            return MaxScale ?? maxScale;
        }

        public double GetStepOrDefault(double step = DefaultStepScale)
        {
            return Step ?? step;
        }

        public ElementReference[] GetExcludeOrDefault(ElementReference[]? elementReferences)
        {
            return Exclude ?? elementReferences ?? EmptyExcludedReferences;
        }

        public ElementReference[] GetExcludeOrDefault()
        {
            return Exclude ?? EmptyExcludedReferences;
        }
    }
}