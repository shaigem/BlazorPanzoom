using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorPanzoom
{
    public class PanzoomSelectorSet : ComponentBase, IAsyncDisposable
    {
        private readonly PanzoomSet _panzoomSet = new();
        [Inject] private IPanzoomHelper PanzoomHelper { get; set; } = null!;
        public IEnumerable<IPanzoom> PanzoomSet => _panzoomSet;
        [Parameter] public string Selector { private get; set; } = "";
        [Parameter] public PanzoomOptions PanzoomOptions { private get; set; } = PanzoomOptions.DefaultOptions;

        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            await _panzoomSet.DisposeAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var list = await PanzoomHelper.CreateForSelector(Selector, PanzoomOptions);

                foreach (var panzoom in list)
                {
                    _panzoomSet.Add((PanzoomInterop) panzoom);
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}