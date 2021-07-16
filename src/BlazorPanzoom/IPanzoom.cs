using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorPanzoom
{
    public interface IPanzoom
    {
        ValueTask ZoomInAsync();
        ValueTask ZoomOutAsync();
        ValueTask ZoomAsync(double toScale);

        ValueTask ZoomToPointAsync(double toScale, double clientX, double clientY,
            IZoomOnlyOptions? overridenZoomOptions);

        ValueTask ZoomWithWheel(WheelEventArgs args, IZoomOnlyOptions? overridenOptions = default);
        ValueTask ResetAsync(PanzoomOptions resetOptions);
        ValueTask ResetAsync();
        ValueTask SetOptionsAsync(PanzoomOptions options);
        ValueTask<PanzoomOptions> GetOptionsAsync();
        ValueTask<double> GetScaleAsync();
        ValueTask DestroyAsync();
    }
}