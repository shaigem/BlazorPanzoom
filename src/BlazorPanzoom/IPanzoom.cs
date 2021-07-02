using System;
using System.Threading.Tasks;

namespace BlazorPanzoom
{
    public interface IPanzoom
    {
        ValueTask ZoomInAsync();
        ValueTask ZoomOutAsync();
        ValueTask ZoomAsync(double toScale);
        ValueTask ResetAsync(PanzoomOptions resetOptions);
        ValueTask ResetAsync();
        ValueTask SetOptionsAsync(PanzoomOptions options);
        ValueTask<PanzoomOptions> GetOptionsAsync();
        ValueTask<double> GetScaleAsync();
        ValueTask DestroyAsync();
    }
}