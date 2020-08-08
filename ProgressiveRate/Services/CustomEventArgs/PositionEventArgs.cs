using System;

namespace ProgressiveRate.Services.CustomEventArgs
{
    public class PositionEventArgs : EventArgs
    {
        public PositionEventArgs(long position, long length)
        {
            Position = position;
            Length = length;
        }

        public long Position { get; }
        public long Length { get; }
    }
}