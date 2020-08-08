using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProgressiveRate.Services
{
    public class ExcelReader : IExcelReader
    {
        private const int BufferSize = 256;

        private CancellationToken _token;
        private List<byte> _data;
        private byte[] _buffer = new byte[BufferSize];
        private string _path;

        public event EventHandler<double> FileProcessed;

        public ExcelReader()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task<DataTable> ReadTableAsync(string path, string sheetName, int columnsRange, CancellationToken token = default)
        {
            _token = token;

            if (columnsRange > 0)
            {
                byte[] file =_path != path ? await ReadFile(path) : _data.ToArray();

                using (var sReader = new MemoryStream(file))
                {
                    using (var excelPackage = new ExcelPackage(sReader))
                    {
                        var book = excelPackage.Workbook;

                        if (book.Worksheets.FirstOrDefault(s => s.Name == sheetName) != null)
                        {
                            var sheet = book.Worksheets[sheetName];
                            var table = new DataTable();

                            foreach (var header in sheet.Cells[1, 1, 1, columnsRange])
                                table.Columns.Add(header.Text);

                            for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                            {
                                var newRow = table.NewRow();

                                for (int column = 1; column <= columnsRange; column++)
                                    newRow[column - 1] = sheet.Cells[row, column].Text;

                                table.Rows.Add(newRow);
                            }

                            return table;
                        }

                        throw new ArgumentException("Документ не содержит страницу с заданным именем", nameof(sheetName));
                    }
                }
            }

            throw new ArgumentException("Кол-во столбцов не может быть отрицательным", nameof(columnsRange));
        }

        private async Task<byte[]> ReadFile(string path)
        {
            using (var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _path = path;
                _data = new List<byte>();

                for (int i = 0; i <= file.Length; i += BufferSize)
                {
                    if (_token.IsCancellationRequested)
                        

                        _token.ThrowIfCancellationRequested();

                    await file.ReadAsync(_buffer, 0, BufferSize);

                    await Task.Delay(50);

                    _data.AddRange(_buffer);

                    FileProcessed?.Invoke(this, (double)file.Position / file.Length);
                }
            }

            return _data.ToArray();
        }
    }
}