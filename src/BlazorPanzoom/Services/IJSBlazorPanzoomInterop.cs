using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorPanzoom
{
    public interface IJSBlazorPanzoomInterop
    {
        public ValueTask<IJSObjectReference>
            CreatePanzoomAsync(ElementReference elementReference, PanzoomOptions? options = null);

        public ValueTask<IJSObjectReference[]> CreatePanzoomAsync(string selector, PanzoomOptions? options = null);

        public ValueTask RegisterDefaultWheelZoom(ElementReference elementReference,
            IJSObjectReference jsPanzoomReference);

        public ValueTask RegisterWheelListenerAsync(
            DotNetObjectReference<IPanzoomWheelListener> dotNetObjectReference,
            ElementReference elementReference, IJSObjectReference jsPanzoomReference);

        public ValueTask RemoveZoomWithWheelListenerAsync(
            ElementReference elementReference,
            IJSObjectReference jsPanzoomReference);
    }
}