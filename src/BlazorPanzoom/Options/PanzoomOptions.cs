using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

namespace BlazorPanzoom
{
    public record PanzoomOptions : IZoomOnlyOptions, IPanOnlyOptions, IMiscOnlyOptions
    {
        public const double DefaultStartScale = 1;
        public const double DefaultMinScale = 0.125;
        public const double DefaultMaxScale = 4;
        public const double DefaultStepScale = 0.3;
        public const double DefaultDuration = 200;
        public const string DefaultEasing = "ease-in-out";
        public const string DefaultOrigin = "50% 50%";
        public const Overflow DefaultOverflow = BlazorPanzoom.Overflow.Hidden;
        public const string DefaultExcludeClass = "panzoom-exclude";

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
        public IEnumerable<ElementReference>? Exclude { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ExcludeClass { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Force { get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? NoBind { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Origin { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Overflow? Overflow { private get; init; }

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
        public TouchAction? TouchAction { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableZoom { private get; init; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ReadOnlyFocalPoint? Focal { private get; init; }

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

        public IEnumerable<ElementReference> GetExcludeOrDefault(
            IEnumerable<ElementReference>? elementReferences = null)
        {
            return Exclude ?? elementReferences ?? EmptyExcludedReferences;
        }

        public string GetExcludeClassOrDefault(string excludeClass = DefaultExcludeClass)
        {
            return ExcludeClass ?? excludeClass;
        }

        public bool GetForceOrDefault(bool force = default)
        {
            return Force ?? force;
        }

        public bool GetNoBindOrDefault(bool noBind = default)
        {
            return NoBind ?? noBind;
        }

        public string GetOriginOrDefault(string origin = DefaultOrigin)
        {
            return Origin ?? origin;
        }

        public Overflow GetOverflowOrDefault(Overflow overflow = DefaultOverflow)
        {
            return Overflow ?? overflow;
        }

        public bool GetSilentOrDefault(bool silent = default)
        {
            return Silent ?? silent;
        }

        public double GetStartScaleOrDefault(double startScale = DefaultStartScale)
        {
            return StartScale ?? startScale;
        }

        public double GetStartXOrDefault(double startX = default)
        {
            return StartX ?? startX;
        }

        public double GetStartYOrDefault(double startY = default)
        {
            return StartY ?? startY;
        }

        public TouchAction GetTouchActionOrDefault(TouchAction touchAction = BlazorPanzoom.TouchAction.None)
        {
            return TouchAction ?? touchAction;
        }

        public bool GetAnimateOrDefault(bool animate = default)
        {
            return Animate ?? animate;
        }

        public bool GetCanvasOrDefault(bool canvas = default)
        {
            return Canvas ?? canvas;
        }

        public double GetDurationOrDefault(double duration = DefaultDuration)
        {
            return Duration ?? duration;
        }

        public string GetEasingOrDefault(string easing = DefaultEasing)
        {
            return Easing ?? easing;
        }

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

        public ReadOnlyFocalPoint GetFocalOrDefault(ReadOnlyFocalPoint focal = default)
        {
            return Focal ?? focal;
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
    }
}