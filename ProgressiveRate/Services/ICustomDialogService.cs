using System.Windows;

namespace ProgressiveRate.Services
{
    public interface ICustomDialogService
    {
        string FilePath { get; }

        bool OpenFileDialog(string extensions = "");

        void ShowMessage(string caption, string message, MessageBoxImage boxImage);
    }
}