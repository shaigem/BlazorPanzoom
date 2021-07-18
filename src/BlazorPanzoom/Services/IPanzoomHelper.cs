using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorPanzoom
{
    public interface IPanzoomHelper
    {
        public ValueTask<PanzoomInterop> CreateForElementReferenceAsync(ElementReference elementReference,
            PanzoomOptions? panzoomOptions = default);

        public ValueTask<PanzoomInterop[]> CreateForSelectorAsync(string selector,
            PanzoomOptions? panzoomOptions = default);

        public ValueTask SetTransformAsync(PanzoomInterop panzoom,
            EventCallback<SetTransformEventArgs> onSetTransform);


        public ValueTask RegisterZoomWithWheelAsync(
            PanzoomInterop panzoom, ElementReference? elementReference = null);

        public ValueTask RegisterWheelListenerAsync(PanzoomInterop panzoom, EventCallback<CustomWheelEventArgs> onWheel,
            ElementReference? elementReference = null);

        public ValueTask RegisterWheelListenerAsync(PanzoomInterop panzoom, object receiver,
            Func<CustomWheelEventArgs, Task> onWheel, ElementReference? elementReference = null);


        public ValueTask ResetAllForAsync(IEnumerable<PanzoomInterop> panzoomInterops);
    }
}