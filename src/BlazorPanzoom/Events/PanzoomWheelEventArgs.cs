using System;

namespace BlazorPanzoom
{
    public class PanzoomWheelEventArgs : EventArgs
    {
        public double DeltaX { get; set; }
        public double DeltaY { get; set; }
        public double ClientX { get; set; }
        public double ClientY { get; set; }
        public bool ShiftKey { get; set; }
        public bool AltKey { get; set; }

        // TODO add more args such as MetaKey

        public override string ToString()
        {
            return $"ClientX: {ClientX}, ClientY: {ClientY}, ShiftKey: {ShiftKey}, AltKey: {AltKey}";
        }
    }
}