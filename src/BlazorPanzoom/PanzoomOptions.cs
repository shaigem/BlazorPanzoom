using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

namespace BlazorPanzoom
{
    public readonly struct FocalPoint
    {
        public FocalPoint(double x = 0.0, double y = 0.0)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }
    }

    internal static class OptionExtensions
    {
        public static string? AsString(this Contain contain)
        {
            return contain switch
            {
                Contain.Inside => "inside",
                Contain.Outside => "outside",
                Contain.None => null,
                _ => throw new ArgumentOutOfRangeException(nameof(contain), contain, "Invalid Contain value")
            };
        }

        public static Contain AsContain(this string? contain)
        {
            return contain switch
            {
                "inside" => Contain.Inside,
                "outside" => Contain.Outside,
                null => Contain.None,
                _ => throw new ArgumentOutOfRangeException(nameof(contain), contain, "Invalid Contain value")
            };
        }

        public static string? AsString(this TransitionTimingFunction timingFunction)
        {
            return timingFunction switch
            {
                TransitionTimingFunction.Ease => "ease",
                TransitionTimingFunction.Linear => "linear",
                TransitionTimingFunction.EaseIn => "ease-in",
                TransitionTimingFunction.EaseOut => "ease-out",
                TransitionTimingFunction.EaseInOut => "ease-in-out",
                _ => throw new ArgumentOutOfRangeException(nameof(timingFunction), timingFunction,
                    "Invalid transition function")
            };
        }
    }

    public enum Contain
    {
        Inside,
        Outside,
        None
    }

    public enum TransitionTimingFunction
    {
        Ease,
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut
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
    }

    public record PanzoomOptions : IZoomOnlyOptions, IPanOnlyOptions
    {
        protected internal const double DefaultMinScale = 0.125;
        protected internal const double DefaultMaxScale = 4;
        protected internal const double DefaultStepScale = 0.3;

        private static readonly PanzoomOptions Default = new();
        public static ref readonly PanzoomOptions DefaultOptions => ref Default;

        private readonly Contain? _contain;

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
        [JsonConverter(typeof(JsonCamelCaseStringEnumConverter))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Contain? Contain
        {
            private get => _contain;
            init => _contain = value is not null && value.Equals(BlazorPanzoom.Contain.None) ? null : value;
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

        public Contain GetContainOrDefault(Contain contain = BlazorPanzoom.Contain.None)
        {
            return Contain ?? contain;
        }
    }
}