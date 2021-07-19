using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace BlazorPanzoom
{
    public interface IMiscOnlyOptions
    {
        public bool GetAnimateOrDefault(bool animate = default);
        public bool GetCanvasOrDefault(bool canvas = default);
        public double GetDurationOrDefault(double duration = default);
        public string GetEasingOrDefault(string easing = "");
        public IEnumerable<ElementReference> GetExcludeOrDefault(IEnumerable<ElementReference> excludedElements);
        public string GetExcludeClassOrDefault(string excludeClass = "");
        public bool GetForceOrDefault(bool force = default);
        public bool GetNoBindOrDefault(bool noBind = default);
        public string GetOriginOrDefault(string origin = "");
        public Overflow GetOverflowOrDefault(Overflow overflow = Overflow.Hidden);
        public bool GetSilentOrDefault(bool silent = default);
        public double GetStartScaleOrDefault(double startScale = default);
        public double GetStartXOrDefault(double startX = default);
        public double GetStartYOrDefault(double startY = default);
        public TouchAction GetTouchActionOrDefault(TouchAction touchAction = TouchAction.None);
    }
}