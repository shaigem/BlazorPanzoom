using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorPanzoom
{
    public interface IPanzoom
    {
        ValueTask PanAsync(double x, double y, IPanOnlyOptions? overridenOptions = default);
        ValueTask ZoomInAsync();
        ValueTask ZoomOutAsync();
        ValueTask ZoomAsync(double toScale);

        ValueTask ZoomToPointAsync(double toScale, double clientX, double clientY,
            IZoomOnlyOptions? overridenZoomOptions = default);

        ValueTask ZoomWithWheelAsync(WheelEventArgs args, IZoomOnlyOptions? overridenOptions = default);
        ValueTask ResetAsync(PanzoomOptions resetOptions);
        ValueTask ResetAsync();
        ValueTask SetOptionsAsync(PanzoomOptions options);
        ValueTask<PanzoomOptions> GetOptionsAsync();
        ValueTask<double> GetScaleAsync();
        ValueTask<FocalPoint2> GetPanAsync();
        ValueTask SetStyleAsync(string name, string value);
        ValueTask DestroyAsync();
    }
}