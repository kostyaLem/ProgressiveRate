using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ProgressiveRate.Services
{
    public interface IExcelReader
    {
        event EventHandler<double> FileProcessed;

        Task<DataTable> ReadTableAsync(string path, string sheetName, int columnsRange, CancellationToken token = default);
    }
}