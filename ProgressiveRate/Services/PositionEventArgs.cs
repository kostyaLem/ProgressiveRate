using System;

namespace ProgressiveRate.Services
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