using System;

namespace ProgressiveRate.Services.CustomEventArgs
{
    public class CompletedEventArgs : EventArgs
    {
        public byte[] ReadedBytes { get; }

        public CompletedEventArgs(byte[] bytes)
        {
            ReadedBytes = bytes;
        }
    }
}