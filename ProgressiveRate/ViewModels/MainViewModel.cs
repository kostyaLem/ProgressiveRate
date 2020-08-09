using DevExpress.Mvvm;
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
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private DateTime _startOfDate = DateTime.Now.AddDays(-1);
        private DateTime _endOfDate = DateTime.Now;
        #endregion

        private CancellationTokenSource _tkn;

        IExcelReader _excelReader = new ExcelReader();
        ICustomDialogService _dialogService = new CustomDialogService();

        public DateTime StartOfDate { get { return _startOfDate; } set { SetValue(ref _startOfDate, value); } }
        public DateTime EndOfDate { get { return _endOfDate; } set { SetValue(ref _endOfDate, value); } }
        public string SelectedFileName { get { return GetValue<string>(); } set { SetValue(value); } }

        public bool IsWaiting { get { return GetValue<bool>(); } set { SetValue(value); } }
        public double ProcessScore { get { return GetValue<double>(); } set { SetValue(value); } }

        public Dictionary<string, string> ColumnHeaderNames { get; }
        public ObservableCollection<CargoStorageRecord> Records { get; private set; }

        #region Commands
        public ICommand GenerateReportCommand { get; set; }
        public ICommand ClearSelectedFileCommand { get; set; }
        public ICommand OpenExcelFileCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            DisplayNameHelper.FillNames(typeof(CargoStorageRecord), ColumnHeaderNames = new Dictionary<string, string>());
            Records = new ObservableCollection<CargoStorageRecord>();

            _excelReader.FileProcessed += (s, value) => ProcessScore = value;

            GenerateReportCommand = new DelegateCommand(Run, () => StartOfDate < EndOfDate && !string.IsNullOrEmpty(SelectedFileName));
            ClearSelectedFileCommand = new DelegateCommand(() => SelectedFileName = string.Empty, () => !string.IsNullOrEmpty(SelectedFileName));
            OpenExcelFileCommand = new DelegateCommand(SelectFile);
            CancelCommand = new DelegateCommand(() => _tkn.Cancel());
        }

        private async void Run()
        {
            IsWaiting = true;
            _tkn = new CancellationTokenSource();

            try
            {
                var dataTable = await _excelReader.ReadTableAsync(SelectedFileName, "Груз", 3, _tkn.Token);

                new CargoManager().DataTableToCargos(dataTable);
            }
            catch (OperationCanceledException)
            {
                Records.Clear();
            }
            finally
            {
                IsWaiting = false;
            }
        }

        private void SelectFile()
        {
            if (_dialogService.OpenFileDialog(FileExtensions.ExcelExtensions))
            {
                SelectedFileName = _dialogService.FilePath;
            }
        }
    }
}