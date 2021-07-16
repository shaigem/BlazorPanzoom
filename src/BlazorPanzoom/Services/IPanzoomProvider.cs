using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorPanzoom
{
    public interface IPanzoomProvider
    {
        public ValueTask RegisterZooming(ElementReference elementReference,
            PanzoomInterop panzoomInterop, WheelHandler wheelHandler,
            EventCallback<PanzoomWheelEventArgs> handler);

        public ValueTask<IPanzoom> CreateForElementReference(ElementReference elementReference,
            PanzoomOptions? panzoomOptions = default);

        public ValueTask<IPanzoom[]> CreateForSelector(string selector, PanzoomOptions? panzoomOptions = default);

        public ValueTask ResetAllFor(IEnumerable<IPanzoom> panzoomEnumerable);
    }
}