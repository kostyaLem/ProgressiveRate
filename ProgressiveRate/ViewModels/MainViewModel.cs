using GalaSoft.MvvmLight.Command;
using ProgressiveRate.Helpers;
using ProgressiveRate.Models;
using ProgressiveRate.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;

namespace ProgressiveRate.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields
        private bool _isWaiting;
        private double _processScore;
        private string _selectedFileName;
        #endregion

        private CancellationTokenSource _tkn;

        IExcelReader _excelReader = new ExcelReader();
        ICustomDialogService _dialogService = new CustomDialogService();



        public bool IsWaiting { get => _isWaiting; private set { SetProperty(ref _isWaiting, value); } }
        public double ProcessScore { get => _processScore; private set { SetProperty(ref _processScore, value); } }
        public string SelectedFileName { get => _selectedFileName; private set { SetProperty(ref _selectedFileName, value); } }

        public Dictionary<string, string> ColumnHeaderNames { get; }
        public ObservableCollection<CargoStorageRecord> Records { get; private set; }

        public ICommand GenerateReport { get; set; }
        public ICommand OpenExcelFileCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public MainViewModel()
        {
            Records = new ObservableCollection<CargoStorageRecord>();
            ColumnHeaderNames = new Dictionary<string, string>();

            _excelReader.FileProcessed += (s, value) => ProcessScore = value;

            GenerateReport = new RelayCommand(Run, () => true);
            OpenExcelFileCommand = new RelayCommand(SelectFile, () => true);
            CancelCommand = new RelayCommand(CancelTask, () => true);

            DisplayNameHelper.FillNames(typeof(CargoStorageRecord), ColumnHeaderNames);
        }

        private async void Run()
        {
            IsWaiting = true;
            _tkn = new CancellationTokenSource();

            try
            {
                await _excelReader.ReadTableAsync(SelectedFileName, "Груз", 3, _tkn.Token);
            }
            catch (OperationCanceledException)
            {
                Records.Clear();
            }

            IsWaiting = false;
        }

        private void SelectFile()
        {
            if (_dialogService.OpenFileDialog(FileExtensions.ExcelExtensions))
            {
                SelectedFileName = _dialogService.FilePath;
            }
        }

        private void CancelTask()
        {
            _tkn?.Cancel();
        }
    }
}
