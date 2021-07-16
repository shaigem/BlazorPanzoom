namespace BlazorPanzoom
{
    public readonly struct FocalPoint
    {
        public FocalPoint(double x = 0.0, double y = 0.0)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }
    }

    public class FocalPoint2
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}