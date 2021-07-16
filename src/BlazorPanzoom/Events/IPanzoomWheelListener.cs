using System.Threading.Tasks;

namespace BlazorPanzoom
{
    public interface IPanzoomWheelListener
    {
        public ValueTask OnCustomWheelEvent(PanzoomWheelEventArgs args);
        public ValueTask RemoveWheelListener();
    }
}