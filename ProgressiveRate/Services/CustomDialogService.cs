using Microsoft.Win32;
using System.Windows;

namespace ProgressiveRate.Services
{
    public class CustomDialogService : ICustomDialogService
    {
        public string FilePath { get; private set; }

        public bool OpenFileDialog(string extensions = "")
        {
            var openFileDialog = new OpenFileDialog() { Filter = extensions };
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            else
            {
                FilePath = string.Empty;
                return false;
            }
        }

        public void ShowMessage(string caption, string message, MessageBoxImage boxImage)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, boxImage);
        }
    }
}
