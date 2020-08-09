using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ProgressiveRate.Services
{
    /// <summary>
    /// Чтение таблиц из Excel-документов
    /// </summary>
    public interface IExcelReader
    {
        /// <summary>
        /// Событие изменения процесса загрузки
        /// </summary>
        event EventHandler<double> FileProcessed;

        /// <summary>
        /// Загрузка таблицы
        /// </summary>
        /// <param name="path"> Полный путь файла </param>
        /// <param name="sheetName"> Название страницы </param>
        /// <param name="columnsRange"> Количество столбцов от 0-го </param>
        /// <param name="token"> Токен отмены </param>
        /// <returns></returns>
        Task<DataTable> ReadTableAsync(string path, string sheetName, int columnsRange, CancellationToken token = default);
    }
}