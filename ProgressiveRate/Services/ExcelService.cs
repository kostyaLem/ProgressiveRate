using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace ProgressiveRate.Services
{
    public delegate void PositionHandler(object sender, PositionEventArgs e);
    public delegate void CompletedReadHandler(object sender, CompletedEventArgs e);

    public class ExcelService
    {
        private AsyncNotifyReader _asyncReader;

        public event PositionHandler PositionChanged;
        public event CompletedReadHandler Completed;

        private async Task<byte[]> Read(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                _asyncReader = new AsyncNotifyReader(fileStream);
                _asyncReader.PositionChanged += asyncReader_PositionChanged;
                _asyncReader.Completed += _asyncReader_Completed;
            }
        }

        private void _asyncReader_Completed(object sender, CompletedEventArgs e)
        {
            Completed?.Invoke(this, e);
        }

        private void asyncReader_PositionChanged(object sender, PositionEventArgs e)
        {
            PositionChanged?.Invoke(this, e);
        }
    }

    public class AsyncNotifyReader
    {
        private readonly Stream _input;

        private List<byte> _readedBytes = new List<byte>();
        private byte[] _buffer = new byte[4096];

        public event PositionHandler PositionChanged;
        public event CompletedReadHandler Completed;

        public bool IsReading { get; private set; }

        public AsyncNotifyReader(Stream input)
        {
            _input = input;
        }

        public async Task BeginRead()
        {
            await Task.Factory.StartNew(() =>
            {
                if (IsReading != true)
                {
                    Read();
                }
            });
        }

        private void Read()
        {
            _input.BeginRead(_buffer, 0, _buffer.Length, ReadComplete, null);
        }

        private void ReadComplete(IAsyncResult result)
        {
            int bytesRead = _input.EndRead(result);

            if (bytesRead == 0)
            {
                IsReading = false;
                Completed?.Invoke(this, new CompletedEventArgs(_readedBytes.ToArray()));
            }
            else
            {
                _readedBytes.AddRange(_buffer);

                PositionChanged?.Invoke(this, new PositionEventArgs(_input.Position, _input.Length));
                Read();
            }
        }
    }
}