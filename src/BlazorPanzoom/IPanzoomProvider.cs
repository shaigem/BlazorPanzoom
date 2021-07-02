using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorPanzoom
{
    public interface IPanzoomProvider
    {
        public ValueTask<IJSObjectReference> CreatePanzoomAsync(ElementReference element);
        public ValueTask<IJSObjectReference> CreatePanzoomForIdAsync(string id);
        public ValueTask<IJSObjectReference> CreatePanzoomForSelectorAsync(string cssSelector);
    }
}