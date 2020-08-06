using System.Windows;

namespace ProgressiveRate.Services
{
    public interface ICustomDialogService
    {
        string FilePath { get; }

        bool OpenFileDialog(string extensions = "");
        bool SaveFileDialog();
        void ShowMessage(string caption, string message, MessageBoxImage boxImage);
    }
}