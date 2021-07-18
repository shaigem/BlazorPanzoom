namespace BlazorPanzoom
{
    public class SetTransformEventArgs : IBlazorPanzoomEvent
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Scale { get; set; }
        public bool IsSvg { get; set; }
    }
}