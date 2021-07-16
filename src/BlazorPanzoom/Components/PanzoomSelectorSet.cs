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
        public IEnumerable<PanzoomInterop> PanzoomSet => _panzoomSet;
        [Parameter] public string Selector { private get; set; } = "";
        [Parameter] public PanzoomOptions PanzoomOptions { private get; set; } = PanzoomOptions.DefaultOptions;
        [Parameter] public EventCallback OnAfterCreate { get; set; }

        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            await _panzoomSet.DisposeAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var list = await PanzoomHelper.CreateForSelectorAsync(Selector, PanzoomOptions);

                foreach (var panzoom in list)
                {
                    _panzoomSet.Add(panzoom);
                }

                if (OnAfterCreate.HasDelegate)
                {
                    await OnAfterCreate.InvokeAsync();
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}