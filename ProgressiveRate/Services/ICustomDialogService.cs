using System.Windows;

namespace ProgressiveRate.Services
{
    /// <summary>
    /// Диалоговые окна
    /// </summary>
    public interface ICustomDialog
    {
        /// <summary>
        /// Файл
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extensions"> Расширения файлов </param>
        /// <returns> Выбран ли файл </returns>
        bool OpenFileDialog(string extensions = "");

        /// <summary>
        /// Диалоговое окно сообщения
        /// </summary>
        /// <param name="caption"> Название окна</param>
        /// <param name="message"> Сообщение </param>
        /// <param name="boxImage"> Иконка </param>
        void ShowMessage(string caption, string message, MessageBoxImage boxImage);
    }
}