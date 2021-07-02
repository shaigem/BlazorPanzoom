using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

namespace BlazorPanzoom
{
    public readonly struct Point2D
    {
        internal Point2D(double x = 0.0, double y = 0.0)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }
    }

    public static class OptionExtensions
    {
        public static string? AsString(this Contain contain)
        {
            return contain switch
            {
                Contain.Inside => "inside",
                Contain.Outside => "outside",
                Contain.None => "none",
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
        public bool? DisableZoom { get; }
        public Point2D? Focal { get; }
        public double? MaxScale { get; }
        public double? MinScale { get; }
        public double? Step { get; }
    }

    public interface IPanOnlyOptions
    {
        public string? Contain { get; }
        public string? Cursor { get; }
        public bool? DisablePan { get; }
        public bool? DisableXAxis { get; }
        public bool? DisableYAxis { get; }
        public bool? PanOnlyWhenZoomed { get; }
        public bool? Relative { get; }
    }

    public interface IMiscOptions
    {
        public bool? Animate { get; }
        public bool? Canvas { get; }
        public double? Duration { get; }

        public string? Easing { get; }

        // public string[] Exclude { get; } // TODO
        public string? ExcludeClass { get; }
        public bool? Force { get; }
        public Action<EventArgs> HandleStartEvent { get; }
        public bool? NoBind { get; }
        public string? Origin { get; }
        public string? Overflow { get; } // TODO could use an enum

        // public Action<ElementReference> SetTransform { get; } TODO
        public bool? Silent { get; }
        public double? StartScale { get; }
        public double? StartX { get; }
        public double? StartY { get; }
        public string? TouchAction { get; }
    }

    public record PanzoomOptions : IMiscOptions, IPanOnlyOptions, IZoomOnlyOptions
    {
        private static readonly PanzoomOptions Default = new();


        public static ref readonly PanzoomOptions DefaultOptions => ref Default;

        // public PanzoomOptions(bool?animate = false, bool?canvas = false, double duration = 200,
        //     TransitionTimingFunction easing = TransitionTimingFunction.EaseInOut,
        //     string? excludeClass = "panzoom-exclude",
        //     Action<EventArgs>? handleStartEvent = null, bool?noBind = false, string? origin = "undefined",
        //     string? overflow = "hidden", bool?silent = false, double startScale = 1, double startX = 0,
        //     double startY = 0, string? touchAction = "none", Contain contain = BlazorPanzoom.Contain.None,
        //     string? cursor = "move",
        //     bool?disablePan = false, bool?disableXAxis = false, bool?disableYAxis = false,
        //     bool?panOnlyWhenZoomed = false, bool?relative = false, bool?disableZoom = false,
        //     (double x, double y) focal = default, double maxScale = 4, double minScale = 0.125,
        //     double step = 0.3)
        // {
        //     Animate = animate;
        //     Canvas = canvas;
        //     Duration = duration;
        //     TransitionTimingFunction = easing;
        //     Easing = TransitionTimingFunction.AsString();
        //     ExcludeClass = excludeClass;
        //     HandleStartEvent = handleStartEvent ?? (_ => { });
        //     NoBind = noBind;
        //     Origin = origin;
        //     Overflow = overflow;
        //     Silent = silent;
        //     StartScale = startScale;
        //     StartX = startX;
        //     StartY = startY;
        //     TouchAction = touchAction;
        //     Contain = contain.AsString();
        //     Cursor = cursor;
        //     DisablePan = disablePan;
        //     DisableXAxis = disableXAxis;
        //     DisableYAxis = disableYAxis;
        //     PanOnlyWhenZoomed = panOnlyWhenZoomed;
        //     Relative = relative;
        //     DisableZoom = disableZoom;
        //     var (x, y) = focal;
        //     Focal = new Point2D(x, y);
        public PanzoomOptions(bool? animate = default, bool? canvas = default, double? duration = default,
            TransitionTimingFunction transitionTimingFunction = default, string? easing = null,
            string? excludeClass = null, Action<EventArgs>? handleStartEvent = null, bool? noBind = default,
            string? origin = null, string? overflow = null, bool? silent = default, double? startScale = default,
            double? startX = default, double? startY = default, string? touchAction = null, string? contain = null,
            string? cursor = null, bool? disablePan = default, bool? disableXAxis = default,
            bool? disableYAxis = default, bool? panOnlyWhenZoomed = default, bool? relative = default,
            bool? disableZoom = default, Point2D? focal = default, double? maxScale = default,
            double? minScale = default, double? step = default)
        {
            Animate = animate;
            Canvas = canvas;
            Duration = duration;
            TransitionTimingFunction = transitionTimingFunction;
            Easing = easing;
            ExcludeClass = excludeClass;
            HandleStartEvent = handleStartEvent ?? (_ => { });
            NoBind = noBind;
            Origin = origin;
            Overflow = overflow;
            Silent = silent;
            StartScale = startScale;
            StartX = startX;
            StartY = startY;
            TouchAction = touchAction;
            Contain = contain;
            Cursor = cursor;
            DisablePan = disablePan;
            DisableXAxis = disableXAxis;
            DisableYAxis = disableYAxis;
            PanOnlyWhenZoomed = panOnlyWhenZoomed;
            Relative = relative;
            DisableZoom = disableZoom;
            Focal = focal;
            MaxScale = maxScale;
            MinScale = minScale;
            Step = step;
        }
        //     MaxScale = maxScale;
        //     MinScale = minScale;
        //     Step = step;
        // }


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Animate { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Canvas { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? Duration { get; }

        [JsonIgnore] private TransitionTimingFunction TransitionTimingFunction { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Easing { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ExcludeClass { get; }

        [JsonIgnore] public bool? Force => false;
        [JsonIgnore] public Action<EventArgs> HandleStartEvent { get; } // TODO

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? NoBind { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Origin { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Overflow { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Silent { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? StartScale { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? StartX { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? StartY { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TouchAction { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Contain { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Cursor { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisablePan { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableXAxis { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableYAxis { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PanOnlyWhenZoomed { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Relative { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableZoom { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Point2D? Focal { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? MaxScale { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? MinScale { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? Step { get; }
    }
}