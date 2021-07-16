using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorPanzoom
{
    public interface IPanzoomHelper
    {
        public ValueTask<PanzoomInterop> CreateForElementReference(ElementReference elementReference,
            PanzoomOptions? panzoomOptions = default);

        public ValueTask<PanzoomInterop[]> CreateForSelector(string selector, PanzoomOptions? panzoomOptions = default);

        public ValueTask RegisterZoomWithWheel(
            PanzoomInterop panzoom, ElementReference? elementReference = null);

        public ValueTask RegisterWheelListener(PanzoomInterop panzoom, EventCallback<WheelEventArgs> onWheel,
            ElementReference? elementReference = null);

        public ValueTask RegisterWheelListener(PanzoomInterop panzoom, object receiver,
            Func<WheelEventArgs, Task> onWheel, ElementReference? elementReference = null);

        public ValueTask ResetAllFor(IEnumerable<PanzoomInterop> panzoomInterops);
    }
}