using GalaSoft.MvvmLight.Command;
using ProgressiveRate.Helpers;
using ProgressiveRate.Models;
using ProgressiveRate.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;

namespace ProgressiveRate.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private CancellationTokenSource _tkn;

        ICustomDialogService _dialogService = new CustomDialogService();

        public int ProcessValue { get; set; }


        public string SelectedFileName { get; set; }
        public CargoStorageRecord SelectedRecord { get; set; }

        public Dictionary<string, string> ColumnHeaderNames { get; } = new Dictionary<string, string>();
        public ObservableCollection<CargoStorageRecord> Records { get; set; } = new ObservableCollection<CargoStorageRecord>();

        public ICommand GenerateReport { get; set; }
        public ICommand OpenExcelFileCommand { get; set; }

        public MainViewModel()
        {
            DisplayNameHelper.FillNames(typeof(CargoStorageRecord), ColumnHeaderNames);

            GenerateReport = new RelayCommand(Run, () => true);
            OpenExcelFileCommand = new RelayCommand(SelectFile, () => true);
        }

        private object _sync = new object();

        private void Run()
        {
            _tkn = new CancellationTokenSource();


        }

        private void SelectFile()
        {
            if (_dialogService.OpenFileDialog())
            {
                SelectedFileName = _dialogService.FilePath;
                OnPropertyChanged(nameof(SelectedFileName));
            }
        }
    }
}
