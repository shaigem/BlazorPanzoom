using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorPanzoom
{
    public interface IPanzoomHelper
    {
        public ValueTask<PanzoomInterop> CreateForElementReferenceAsync(ElementReference elementReference,
            PanzoomOptions? panzoomOptions = default);

        public ValueTask<PanzoomInterop[]> CreateForSelectorAsync(string selector,
            PanzoomOptions? panzoomOptions = default);

        public ValueTask RegisterZoomWithWheelAsync(
            PanzoomInterop panzoom, ElementReference? elementReference = null);

        public ValueTask RegisterWheelListenerAsync(PanzoomInterop panzoom, EventCallback<WheelEventArgs> onWheel,
            ElementReference? elementReference = null);

        public ValueTask RegisterWheelListenerAsync(PanzoomInterop panzoom, object receiver,
            Func<WheelEventArgs, Task> onWheel, ElementReference? elementReference = null);

        public ValueTask ResetAllForAsync(IEnumerable<PanzoomInterop> panzoomInterops);
    }
}